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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using System.Xml;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReliableSqlConnectionTest
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

        [Description("F4.1.2")]
        [Priority(1)]
        [TestMethod]
        public void TestOpen()
        {
            connection.Open();
            connection.Close();
        }

        [Description("F3.2.2; F4.1.1")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // Unstable test
        public void TestConnectionString()
        {
            string str = connection.ConnectionString;
            StringAssert.StartsWith(connectionString, str, "Unexpected connection string");

            str = connection.Current.ConnectionString;
            StringAssert.StartsWith(connectionString, str, "Unexpected connection string");
        }

        [Description("F4.1.3; F4.2.1")]
        [Priority(2)]
        [TestMethod]
        public void TestSessionTracingId()
        {
            Guid guid = connection.SessionTracingId;
            Trace.WriteLine(string.Format("Session Tracing Id: {0}", guid.ToString()));
        }

        [Description("F4.1.4")]
        [Priority(1)]
        [TestMethod]
        public void TestExecuteSimpleCommand()
        {
            SqlCommand command = new SqlCommand("SELECT 1");
            connection.ExecuteCommand(command);
        }

        [Description("F4.1.5")]
        [Priority(1)]
        [TestMethod]
        public void TestExecuteSimpleCommandWithResult()
        {
            SqlCommand command = new SqlCommand("SELECT 1");
            int result = connection.ExecuteCommand<int>(command);

            Assert.AreEqual(1, result, "Unexpected result");
        }

        [Description("F4.1.6")]
        [Priority(1)]
        [TestMethod]
        public void TextExecuteSelectCommandToGetDataReader()
        {
            SqlCommand command = new SqlCommand("SELECT [ProductCategoryID], [Name] FROM [SalesLT].[ProductCategory]");
            using (IDataReader reader = connection.ExecuteCommand<IDataReader>(command))
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

        [Description("F4.1.7")]
        [Priority(2)]
        [TestMethod]
        public void TextExecuteSelectCommandToGetXmlReader()
        {
            SqlCommand command = new SqlCommand("SELECT [ProductCategoryID], [Name] FROM [SalesLT].[ProductCategory] FOR XML AUTO");
            using (XmlReader reader = connection.ExecuteCommand<XmlReader>(command))
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

        [Description("F4.1.8")]
        [Priority(2)]
        [TestMethod]
        public void TestConnectionTimeout()
        {
            int expectedTimeout = 30;

            connection.ConnectionString += string.Format("Connection Timeout={0};", expectedTimeout);
            int timeout = connection.ConnectionTimeout;

            Assert.AreEqual(expectedTimeout, timeout, "Unexpected timeout");
            connection.ConnectionString = TestSqlSupport.SqlDatabaseConnectionString;
        }

        [Description("F4.1.9")]
        [Priority(1)]
        [TestMethod]
        public void TestDatabaseName()
        {
            connection.Open();
            string database = connection.Database;
            connection.Close();

            Assert.AreEqual("AdventureWorksLTAZ2008R2", database, "Unexpected database");
            connection.ConnectionString = TestSqlSupport.SqlDatabaseConnectionString;
        }

        [Description("F4.1.10")]
        [Priority(2)]
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void TestChangeDatabase()
        {
            string expectedDatabase = "master";

            connection.Open();
            connection.ChangeDatabase(expectedDatabase);
            string database = connection.Database;
            connection.Close();

            Assert.AreEqual(expectedDatabase, database, "Unexpected database");
            connection.ConnectionString = TestSqlSupport.SqlDatabaseConnectionString;
        }

        [Ignore]
        [TestMethod]
        public void TestSuccessfulTransaction()
        {
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            SqlCommand command = new SqlCommand("SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]");
            command.Transaction = (SqlTransaction)transaction;
            int customerId = connection.ExecuteCommand<int>(command);

            command = new SqlCommand("SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]");
            command.Transaction = (SqlTransaction)transaction;
            int addressId = connection.ExecuteCommand<int>(command);

            command = new SqlCommand("INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)");
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";

            connection.ExecuteCommand(command);
            transaction.Commit();

            Assert.IsTrue(VerifyCustomerAddress(customerId, addressId), "Insert was failed");
            DeleteCustomerAddress(customerId, addressId);

            connection.Close();
        }

        [Description("F4.1.11")]
        [Priority(1)]
        [TestMethod]
        public void TestSuccessfulTransactionWithWorkaround()
        {
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            SqlCommand command = new SqlCommand("SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]");
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;
            int customerId = connection.ExecuteCommand<int>(command);

            command = new SqlCommand("SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]");
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;
            int addressId = connection.ExecuteCommand<int>(command);

            command = new SqlCommand("INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)");
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;

            connection.ExecuteCommand(command);
            transaction.Commit();

            Assert.IsTrue(VerifyCustomerAddress(customerId, addressId), "Insert was failed");
            DeleteCustomerAddress(customerId, addressId);

            connection.Close();
        }

        [Description("F4.1.12")]
        [Priority(1)]
        [TestMethod]
        public void TestFailedTransaction()
        {
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [SalesLT].[CustomerAddress]");
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;
            int expectedCount = connection.ExecuteCommand(command);

            command = new SqlCommand("SELECT TOP 1 [CustomerID] FROM [SalesLT].[Customer] ORDER BY [CustomerID]");
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;
            int customerId = connection.ExecuteCommand<int>(command);

            command = new SqlCommand("SELECT TOP 1 [AddressID] FROM [SalesLT].[Address] ORDER BY [AddressID]");
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;
            int addressId = connection.ExecuteCommand<int>(command);

            command = new SqlCommand("INSERT INTO [SalesLT].[CustomerAddress] ([CustomerID], [AddressID], [AddressType]) VALUES (@CustomerID, @AddressID, @AddressType)");
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Parameters.Add("@AddressType", SqlDbType.NVarChar, 100).Value = "Custom Address";
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;

            connection.ExecuteCommand(command);
            transaction.Rollback();

            command = new SqlCommand("SELECT COUNT(*) FROM [SalesLT].[CustomerAddress]");
            command.Connection = connection.Current;
            command.Transaction = (SqlTransaction)transaction;
            int actualCount = connection.ExecuteCommand(command);

            connection.Close();

            Assert.AreEqual(expectedCount, actualCount, "Rollback failed");
        }

        [TestMethod]
        [Ignore]    // Unstable test
        public void TestServerNameSubstitutionWithIPAddress()
        {
            using (var conn = new ReliableSqlConnection(connectionString))
            {
                Assert.AreEqual(connectionString, conn.Current.ConnectionString, "Connection string managed by ReliableSqlConnection class must not be modified at this point.");
                Assert.AreNotEqual<Guid>(conn.SessionTracingId, Guid.Empty, "Unable to resolve the connection's session ID.");
            }

            Thread.Sleep(1000);

            using (var conn = new ReliableSqlConnection(connectionString))
            {
                Assert.AreNotEqual(connectionString, conn.Current.ConnectionString, "Connection string managed by ReliableSqlConnection class has not been modified.");
                Assert.AreNotEqual<Guid>(conn.SessionTracingId, Guid.Empty, "Unable to resolve the connection's session ID.");

                IPAddress hostAddress = null;
                var conStringBuilder = new SqlConnectionStringBuilder(conn.Current.ConnectionString);
                string hostName = conStringBuilder.DataSource.StartsWith("tcp:") ? conStringBuilder.DataSource.Remove(0, "tcp:".Length) : conStringBuilder.DataSource;

                Assert.IsTrue(IPAddress.TryParse(hostName, out hostAddress), "The data source doesn't seem to be represented by an IP address.");

                conn.Current.Close();
                conn.Current.ConnectionString = conn.Current.ConnectionString.Replace("0", "1").Replace("2", "3").Replace("4", "5").Replace("6", "7");

                conn.Open();

                Assert.AreNotEqual<Guid>(conn.SessionTracingId, Guid.Empty, "Unable to resolve the connection's session ID.");
            }
        }

        private bool VerifyCustomerAddress(int customerId, int addressId)
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID");
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Connection = connection.Current;

            int count = connection.ExecuteCommand<int>(command);
            return count > 0;
        }

        private void DeleteCustomerAddress(int customerId, int addressId)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID");
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@AddressID", SqlDbType.Int).Value = addressId;
            command.Connection = connection.Current;

            connection.ExecuteCommand(command);
        }
    }
}
