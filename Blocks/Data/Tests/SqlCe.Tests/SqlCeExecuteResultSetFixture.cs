//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests
{
    [TestClass]
    public class SqlCeExecuteResultSetFixture
    {
        TestConnectionString testConnection;
        const string insertString = "Insert into Region values (99, 'Midwest')";
        const string queryString = "Select * from Region";
        SqlCeDatabase db;

        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
        }

        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
        }

        [TestMethod]
        public void ExecuteResultSet_ShouldCloseConnection()
        {
            DbConnection connection;
            using (DbCommand command = db.GetSqlStringCommand(queryString))
            {
                using (SqlCeResultSet reader = db.ExecuteResultSet(command))
                {
                    connection = command.Connection;
                }

                // Force shared pool closed, this should close out shared connection used by the reader.
                SqlCeConnectionPool.CloseSharedConnections();
                Assert.AreEqual(ConnectionState.Closed, connection.State);
            }
        }

        [TestMethod]
        public void CanExecuteResultSetWithCommand()
        {
            using (DbCommand command = db.GetSqlStringCommand(queryString))
            {
                string accumulator = "";
                using (SqlCeResultSet reader = db.ExecuteResultSet(command))
                {
                    while (reader.Read())
                    {
                        accumulator += ((string)reader["RegionDescription"]).Trim();
                    }

                }
                Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            }
        }

        [TestMethod]
        public void ExecuteResultSetWithBadCommandThrowsAndClosesConnection()
        {
            DbCommand badCommand = db.GetSqlStringCommand("select * from invalid");
            try
            {
                db.ExecuteResultSet(badCommand);
            }
            catch (SqlCeException)
            {
            }

            Assert.IsNotNull(badCommand.Connection); // Held open by pool
            // Force shared connection closed
            SqlCeConnectionPool.CloseSharedConnections();
            Assert.IsNull(badCommand.Connection);
        }

        [TestMethod]
        public void ShouldHaveCorrectRowsAffectedAfterInsertCommand()
        {
            int count = -1;
            try
            {
                using (DbCommand command = db.GetSqlStringCommand(insertString))
                {
                    using (SqlCeResultSet reader = db.ExecuteResultSet(command))
                    {
                        count = reader.RecordsAffected;
                    }
                }
            }
            finally
            {
                string deleteString = "Delete from Region where RegionId = 99";
                using (DbCommand cleanupCommand = db.GetSqlStringCommand(deleteString))
                {
                    db.ExecuteNonQuery(cleanupCommand);
                }
            }

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbCommand command = db.GetSqlStringCommand(insertString))
                {
                    using (var transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                    {
                        using (SqlCeResultSet reader = db.ExecuteResultSet(command, transaction.Transaction))
                        {
                            Assert.AreEqual(1, reader.RecordsAffected);
                            reader.Close();
                        }
                    }
                    Assert.AreEqual(ConnectionState.Open, connection.State);

                    command.Connection.Close();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteResultSetUsingNullCommandThrows()
        {
            using (SqlCeResultSet reader = db.ExecuteResultSet((DbCommand)null)) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            using (SqlCeResultSet reader = db.ExecuteResultSet(null, (DbTransaction)null)) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                try
                {
                    connection.Open();

                    using (DbCommand command = db.GetSqlStringCommand(queryString))
                    {
                        using (db.ExecuteReader(command, null)) { }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteResultSetWithNullCommandThrows()
        {
            db.ExecuteResultSet(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullQueryStringTest()
        {
            using (DbCommand myCommand = db.GetSqlStringCommand(null))
            {
                using (db.ExecuteResultSet(myCommand))
                {
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyQueryStringTest()
        {
            using (DbCommand myCommand = db.GetSqlStringCommand(String.Empty))
            {
                using (db.ExecuteResultSet(myCommand))
                {
                }
            }
        }
    }
}
