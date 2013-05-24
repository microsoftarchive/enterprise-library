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

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    [TestClass]
    public class SqlCommandExtensionsTest
    {
        private string connectionString;
        private ReliableSqlConnection connection;

        [TestInitialize]
        public void Initialize()
        {
            connectionString = TestSqlSupport.SqlDatabaseConnectionString;
            RetryPolicyFactory.CreateDefault();

            connection = new ReliableSqlConnection(connectionString);

            connection.ConnectionRetryPolicy.Retrying += (sender, args) =>
                Trace.WriteLine(string.Format("[Connection Retry] Current Retry Count: {0}, Last Exception: {0}, Delay (ms): {0}", args.CurrentRetryCount, args.LastException.Message, args.Delay.TotalMilliseconds));

            connection.CommandRetryPolicy.Retrying += (sender, args) =>
                Trace.WriteLine(string.Format("[Command Retry] Current Retry Count: {0}, Last Exception: {0}, Delay (ms): {0}", args.CurrentRetryCount, args.LastException.Message, args.Delay.TotalMilliseconds));
        }

        [TestCleanup]
        public void Cleanup()
        {
            RetryPolicyFactory.SetRetryManager(null, false);

            if (connection != null)
            {
                // Work around, close the connection manually.
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Dispose();
            }
        }

        [Description("F5.2.1")]
        [Priority(1)]
        [TestMethod]
        public void TextExecuteSimpleCommand()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            command.ExecuteNonQueryWithRetry();
        }

        [Description("F5.2.2")]
        [Priority(1)]
        [TestMethod]
        public void TestExecuteSimpleCommandWithResult()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            int result = (int)command.ExecuteScalarWithRetry();

            Assert.AreEqual(1, result, "Unexpected result");
        }

        [Description("F5.2.3")]
        [Priority(1)]
        [TestMethod]
        public void TextExecuteSelectCommandToGetDataReader()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT [ProductCategoryID], [Name] FROM [SalesLT].[ProductCategory]";

            using (IDataReader reader = command.ExecuteReaderWithRetry())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);

                    Trace.WriteLine(string.Format("{0}: {1}", id, name));
                }

                reader.Close();
            }
        }

        [Description("F5.2.4")]
        [Priority(1)]
        [TestMethod]
        public void TextExecuteSelectCommandToGetXmlReader()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT [ProductCategoryID], [Name] FROM [SalesLT].[ProductCategory] FOR XML AUTO";

            using (XmlReader reader = command.ExecuteXmlReaderWithRetry())
            {
                while (reader.Read())
                {
                    reader.MoveToFirstAttribute();
                    reader.ReadAttributeValue();
                    int id = reader.ReadContentAsInt();

                    reader.MoveToNextAttribute();
                    reader.ReadAttributeValue();
                    string name = reader.ReadContentAsString();

                    reader.MoveToElement();

                    Trace.WriteLine(string.Format("{0}: {1}", id, name));
                }

                reader.Close();
            }
        }

        [Description("F5.2.5")]
        [Priority(1)]
        [TestMethod]
        public void TestSuccessfulTransaction()
        {
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]";
            command.Transaction = (SqlTransaction)transaction;
            int customerId = (int)command.ExecuteScalarWithRetry();

            command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]";
            command.Transaction = (SqlTransaction)transaction;
            int addressId = (int)command.ExecuteScalarWithRetry();

            command = connection.CreateCommand();
            command.CommandText = "INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)";
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
            command.Transaction = (SqlTransaction)transaction;

            command.ExecuteNonQueryWithRetry();
            transaction.Commit();

            Assert.IsTrue(VerifyCustomerAddress(customerId, addressId), "Insert was failed");
            DeleteCustomerAddress(customerId, addressId);

            connection.Close();
        }

        [Description("F5.2.6")]
        [Priority(1)]
        [TestMethod]
        public void TestFailedTransaction()
        {
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM [SalesLT].[CustomerAddress]";
            command.Transaction = (SqlTransaction)transaction;
            int expectedCount = connection.ExecuteCommand(command);

            command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]";
            command.Transaction = (SqlTransaction)transaction;
            int customerId = (int)command.ExecuteScalarWithRetry();

            command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]";
            command.Transaction = (SqlTransaction)transaction;
            int addressId = (int)command.ExecuteScalarWithRetry();

            command = connection.CreateCommand();
            command.CommandText = "INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)";
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
            command.Transaction = (SqlTransaction)transaction;

            command.ExecuteNonQueryWithRetry();
            transaction.Rollback();

            command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM [SalesLT].[CustomerAddress]";
            command.Transaction = (SqlTransaction)transaction;
            int actualCount = connection.ExecuteCommand(command);

            connection.Close();

            Assert.AreEqual(expectedCount, actualCount, "Rollback failed");
        }

        private bool VerifyCustomerAddress(int customerId, int addressId)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID";
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;

            int count = (int)command.ExecuteScalarWithRetry();
            return count > 0;
        }

        private void DeleteCustomerAddress(int customerId, int addressId)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID";
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;

            command.ExecuteNonQueryWithRetry();
        }
    }
}
