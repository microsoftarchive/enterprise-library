#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    [TestClass]
    public class TransactionRetryScopeTest
    {
        private string connectionString;

        [TestInitialize]
        public void Setup()
        {
            connectionString = TestSqlSupport.SqlDatabaseConnectionString;
            RetryPolicyFactory.CreateDefault();
        }

        [TestCleanup]
        public void Cleanup()
        {
            RetryPolicyFactory.SetRetryManager(null, false);
        }

        private ReliableSqlConnection InitializeConnection()
        {
            ReliableSqlConnection connection = new ReliableSqlConnection(connectionString);

            connection.ConnectionRetryPolicy.Retrying += (sender, args) =>
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "[Connection Retry] Current Retry Count: {0}, Last Exception: {0}, Delay (ms): {0}", args.CurrentRetryCount, args.LastException.Message, args.Delay.TotalMilliseconds));

            connection.CommandRetryPolicy.Retrying += (sender, args) =>
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "[Command Retry] Current Retry Count: {0}, Last Exception: {0}, Delay (ms): {0}", args.CurrentRetryCount, args.LastException.Message, args.Delay.TotalMilliseconds));

            connection.Current.StateChange += (sender, e) =>
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "OriginalState: {0}, CurrentState {1}", e.OriginalState, e.CurrentState));

            return connection;
        }

        [Description("F4.3.1")]
        [Priority(1)]
        [TestMethod]
        public void TestSuccessfulTransaction()
        {
            int customerId = 0;
            int addressId = 0;

            Action action = new Action(() =>
            {
                using (ReliableSqlConnection connection = InitializeConnection())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]";
                        customerId = (int)command.ExecuteScalarWithRetry();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]";
                        addressId = (int)command.ExecuteScalarWithRetry();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)";
                        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                        command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
                        command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
                        command.ExecuteNonQueryWithRetry();
                    }
                }
            });

            using (TransactionRetryScope scope = new TransactionRetryScope(RetryPolicy.NoRetry, action))
            {
                try
                {
                    scope.InvokeUnitOfWork();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            Assert.IsTrue(VerifyCustomerAddress(customerId, addressId));
            DeleteCustomerAddress(customerId, addressId);
        }

        [Description("F4.3.2")]
        [Priority(1)]
        [TestMethod]
        public void TestSuccessfulTransactionWithRetryableError()
        {
            int customerId = 0;
            int addressId = 0;

            int executeCount = 0;

            Action action = new Action(() =>
            {
                using (ReliableSqlConnection connection = InitializeConnection())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]";
                        customerId = (int)command.ExecuteScalarWithRetry();
                    }

                    if (executeCount == 0)
                    {
                        executeCount++;
                        throw new TimeoutException();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]";
                        addressId = (int)command.ExecuteScalarWithRetry();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)";
                        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                        command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
                        command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
                        command.ExecuteNonQueryWithRetry();
                    }
                }
            });

            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("FixedIntervalDefault");

            using (TransactionRetryScope scope = new TransactionRetryScope(retryPolicy, action))
            {
                try
                {
                    scope.InvokeUnitOfWork();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            Assert.IsTrue(VerifyCustomerAddress(customerId, addressId));
            DeleteCustomerAddress(customerId, addressId);
        }

        [Description("F4.3.3")]
        [Priority(1)]
        [TestMethod]
        public void TestFailedTransaction()
        {
            int expectedCount = RetrieveCountofTable();

            int customerId = 0;
            int addressId = 0;

            Action action = new Action(() =>
            {
                using (ReliableSqlConnection connection = InitializeConnection())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]";
                        customerId = (int)command.ExecuteScalarWithRetry();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]";
                        addressId = (int)command.ExecuteScalarWithRetry();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)";
                        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                        command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
                        command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
                        command.ExecuteNonQueryWithRetry();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "RAISEERROR('ERROR', 16, 1)";
                        command.ExecuteNonQueryWithRetry();
                    }
                }
            });

            using (TransactionRetryScope scope = new TransactionRetryScope(action))
            {
                try
                {
                    scope.InvokeUnitOfWork();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            int actualCount = RetrieveCountofTable();

            Assert.AreEqual(expectedCount, actualCount, "Rollback failed");
        }

        private int RetrieveCountofTable()
        {
            int count = 0;

            using (ReliableSqlConnection connection = new ReliableSqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM [SalesLT].[CustomerAddress]";
                    count = (int)command.ExecuteScalarWithRetry();
                }

                connection.Close();
            }

            return count;
        }

        private bool VerifyCustomerAddress(int customerId, int addressId)
        {
            int count = 0;

            using (ReliableSqlConnection connection = new ReliableSqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
                    count = (int)command.ExecuteScalarWithRetry();
                }

                connection.Close();
            }

            return count > 0;
        }

        private void DeleteCustomerAddress(int customerId, int addressId)
        {
            using (ReliableSqlConnection connection = new ReliableSqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
                    command.ExecuteNonQueryWithRetry();
                }

                connection.Close();
            }
        }
    }
}
