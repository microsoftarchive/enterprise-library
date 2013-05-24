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

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Sql
{
    [TestClass]
    public class TransactionConnectionsFixture
    {
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
        }

        [TestMethod]
        public void GetConnection_ShouldReturnNullWhenNoTransactionActive()
        {
            Assert.IsNull(TransactionScopeConnections.GetConnection(db));
        }

        [TestMethod]
        public void GetConnection_ShouldReturnConnectionWhenTransactionActive()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var connection = TransactionScopeConnections.GetConnection(db);
                Assert.IsNotNull(connection.Connection);
            }
        }

        [TestMethod]
        public void GetConnection_ShouldReturnSameConnectionDuringTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var connection1 = TransactionScopeConnections.GetConnection(db);
                Assert.IsNotNull(connection1.Connection);

                var connection2 = TransactionScopeConnections.GetConnection(db);
                Assert.AreSame(connection1, connection2);
            }
        }

        [TestMethod]
        public void GetConnection_ShouldReturnNullAfterTransactionScopeClosed()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using(var connection = TransactionScopeConnections.GetConnection(db))
                {
                    Assert.IsNotNull(connection);
                }
            }
            Assert.IsNull(TransactionScopeConnections.GetConnection(db));
        }

        [TestMethod]
        public void Dispose_ShouldCloseConnection()
        {
            DatabaseConnectionWrapper connection;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using(connection = TransactionScopeConnections.GetConnection(db))
                {
                    Assert.IsNotNull(connection);
                }
            }
            Assert.IsTrue(connection.IsDisposed);
        }

        [TestMethod]
        public void Complete_ShouldNotCloseConnectionBeforeScopeIsDisposed()
        {
            DatabaseConnectionWrapper connection;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using(connection = TransactionScopeConnections.GetConnection(db))
                {
                    Assert.IsNotNull(connection);
                    scope.Complete();
                    Assert.AreEqual(ConnectionState.Open, connection.Connection.State);
                }
            }
        }

        [TestMethod]
        public void Supress_ShouldReturnNullConnection()
        {
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DatabaseConnectionWrapper connection = TransactionScopeConnections.GetConnection(db);
                Assert.IsNotNull(connection);

                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    Assert.IsNull(TransactionScopeConnections.GetConnection(db));
                }
            }
        }

        [TestMethod]
        public void Required_ShouldReturnSameConnection()
        {
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DatabaseConnectionWrapper connection1 = TransactionScopeConnections.GetConnection(db);
                Transaction transaction1 = Transaction.Current;

                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Required))
                {
                    DatabaseConnectionWrapper connection2 = TransactionScopeConnections.GetConnection(db);
                    Transaction transaction2 = Transaction.Current;

                    Assert.AreSame(transaction1, transaction2);
                    Assert.AreSame(connection1, connection2);
                }
            }
        }

        [TestMethod]
        public void RequiresNew_ShouldReturnNewConnection()
        {
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DatabaseConnectionWrapper connection1 = TransactionScopeConnections.GetConnection(db);
                Transaction transaction1 = Transaction.Current;

                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    DatabaseConnectionWrapper connection2 = TransactionScopeConnections.GetConnection(db);
                    Transaction transaction2 = Transaction.Current;

                    Assert.AreNotSame(transaction1, transaction2);
                    Assert.AreNotSame(connection1, connection2);
                }
            }
        }

        [TestMethod]
        public void GetConnection_ShouldReturnDifferentConnectionForDifferentConnectionStrings()
        {
            Database db2 = new SqlDatabase(db.ConnectionString.ToString() + ";Persist Security Info=false;");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    DatabaseConnectionWrapper connection1 = TransactionScopeConnections.GetConnection(db);
                    DatabaseConnectionWrapper connection2 = TransactionScopeConnections.GetConnection(db2);
                    Assert.AreNotSame(connection1, connection2);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("MSDTC"))
                {
                    Assert.Inconclusive("In order to run the test, enable the Distributed Transaction Coordinator service (MSDTC).\r\n{0}", ex.ToString());
                }

                throw;
            }
        }

        [TestMethod]
        public void Current_ShouldBeDifferentTransactionInOtherThread()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ThreadTests tests = new ThreadTests();
                Thread thread = new Thread(tests.GetTransactionScopeConnection);
                thread.Start();
                thread.Join();
                Assert.IsNotNull(tests.Current);
                Assert.IsNotNull(Transaction.Current);
                Assert.AreNotSame(tests.Current, Transaction.Current);
            }
        }

        [TestMethod]
        public void GetConnection_ShouldGetDifferentConnectionOnDifferentThreads()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DatabaseConnectionWrapper connection = TransactionScopeConnections.GetConnection(db);
                ThreadTests tests = new ThreadTests();
                Thread thread = new Thread(tests.GetTransactionScopeConnection);
                thread.Start();
                thread.Join();
                Assert.AreNotSame(connection, tests.Connection);
            }
        }

        [TestMethod]
        public void GetConnection_ShouldGetSameConnectionWhenOtherThreadUsesSameTransaction()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DatabaseConnectionWrapper connection = TransactionScopeConnections.GetConnection(db);
                ThreadTests tests = new ThreadTests();
                Thread thread = new Thread(tests.GetConnection);
                thread.Start(Transaction.Current);
                thread.Join();
                Assert.AreSame(connection, tests.Connection);
                Assert.AreEqual(ConnectionState.Open, tests.Connection.Connection.State);
            }
        }

        class ThreadTests
        {
            public DatabaseConnectionWrapper Connection;
            public Transaction Current;
            Database db;

            public ThreadTests()
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
                db = factory.CreateDefault();
            }

            public void GetConnection(object parameter)
            {
                Transaction transaction = (Transaction)parameter;
                Transaction.Current = transaction;
                Connection = TransactionScopeConnections.GetConnection(db);
            }

            public void GetTransactionScopeConnection()
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Current = Transaction.Current;
                    Connection = TransactionScopeConnections.GetConnection(db);
                }
            }
        }
    }
}
