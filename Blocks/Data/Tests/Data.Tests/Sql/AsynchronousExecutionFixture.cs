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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class WhenUsingASqlDatabase
    {
        [TestMethod]
        public void ThenSupportsAsyncIsTrue()
        {
            var db = new SqlDatabase(@"server=(localdb)\v11.0;database=Northwind;Integrated Security=true");
            Assert.IsTrue(db.SupportsAsync);
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenConnectionStringDoesntContainAsyncTrue : ArrangeActAssert
    {
        protected SqlDatabase database;

        protected override void Arrange()
        {
            database = new SqlDatabase(@"server=(localdb)\v11.0;database=Northwind;Integrated Security=true");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThenAsynchronousExecuteReaderThrows()
        {
            database.BeginExecuteReader(CommandType.Text, "SELECT 'hello world'", null, null);
        }
    }

    [TestClass]
    public class WhenUsingAnySqlDatabase : ArrangeActAssert
    {
        protected string connectionstring = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true;Async=True";
        protected SqlDatabase database;

        protected override void Arrange()
        {
            database = new SqlDatabase(connectionstring);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCallingBeginExecuteNonQueryWithEmptySprocameThrowsArgException()
        {
            database.BeginExecuteNonQuery(String.Empty, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCallingBeginExecuteReaderWithEmptySprocameThrowsArgException()
        {
            database.BeginExecuteReader(String.Empty, null, null);
        }

    }

    public abstract class AsynchronousConnectionContext : ArrangeActAssert
    {
        protected int numberOfConnectionsCreated;
        protected ConnectionState lastConnectionStateChange;
        protected string connectionstring = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true;Async=True";
        protected SqlDatabase database;

        protected override void Arrange()
        {
            numberOfConnectionsCreated = 0;
            database = new TestableSqlConnection(connectionstring, this);
        }

        protected class CommandFailedArgs
        {
            public Exception Exception { get; set; }
            public string CommandText { get; set; }
            public string ConnectionString { get; set; }
        }

        private class TestableSqlConnection : SqlDatabase
        {
            AsynchronousConnectionContext context;
            public TestableSqlConnection(string connectionstring, AsynchronousConnectionContext context)
                : base(connectionstring)
            {
                this.context = context;
            }

            public override DbConnection CreateConnection()
            {
                context.numberOfConnectionsCreated++;

                DbConnection createdConnection = base.CreateConnection();
                createdConnection.StateChange += (sender, args) =>
                    {
                        context.lastConnectionStateChange = args.CurrentState;
                    };

                return createdConnection;
            }
        }
    }

    public abstract class TransactionalAsynchronousConnectionWithRollback : AsynchronousConnectionContext
    {
        protected SqlTransaction transaction;
        protected DbConnection connection;

        protected override void Arrange()
        {
            base.Arrange();

            connection = database.CreateConnection();
            connection.Open();

            //reset number of connections
            numberOfConnectionsCreated = 0;

            transaction = connection.BeginTransaction() as SqlTransaction;
        }

        protected override void Teardown()
        {
            transaction.Rollback();
            connection.Close();
        }

        protected void EnsureParametersCached(string procName)
        {
            using (var command = database.GetStoredProcCommand(procName, new object[] { }))
            {
            }
        }
    }

    [TestClass]
    public class WhenAsynchronousCommandWithDelayExecutes : AsynchronousConnectionContext
    {
        private IAsyncResult asyncResult;

        protected override void Act()
        {
            asyncResult = database.BeginExecuteReader(CommandType.Text, "waitfor delay '00:00:02'; SELECT 'hello async world'", null, null);
        }

        [TestMethod]
        public void ThenThreadContinuesToExecute()
        {
            Assert.AreEqual(false, asyncResult.IsCompleted);
        }

        [TestMethod]
        public void ThenCorresponsingEndExecuteBlocksThreadAndReturnsValue()
        {
            using (IDataReader reader = database.EndExecuteReader(asyncResult))
            {
                Assert.IsFalse(asyncResult.CompletedSynchronously);

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());
                Assert.AreEqual("hello async world", reader.GetString(0));
            }
        }
    }

    [TestClass]
    public class WhenAsyncronousCommandExecutes : AsynchronousConnectionContext
    {
        private readonly Barrier asyncComplete = new Barrier(2);
        private bool callbackHasRun;
        private bool canReadOneRecord;
        private bool canReadSecondRecord;
        private string result;

        protected override void Act()
        {
            database.BeginExecuteReader(CommandType.Text,
                "SELECT 'hello async world'",
                ar =>
                {
                    callbackHasRun = true;
                    try
                    {
                        using (IDataReader reader = database.EndExecuteReader(ar))
                        {
                            canReadOneRecord = reader.Read();
                            result = reader.GetString(0);
                            canReadSecondRecord = reader.Read();
                        }
                    }
                    finally
                    {
                        asyncComplete.Await();
                    }
                }, null);
        }

        [TestMethod]
        public void ThenOperationCompletesOnBackgroundThread()
        {
            // Wait for async operation to complete
            asyncComplete.Await(4000);
            Assert.IsTrue(callbackHasRun);
            Assert.IsTrue(canReadOneRecord);
            Assert.AreEqual("hello async world", result);
            Assert.IsFalse(canReadSecondRecord);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryWithTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        int numberOforderDetailsInDatabase;
        private readonly Barrier barrier = new Barrier(2);
        private int numberOfAffectedRecords = 0;
        protected override void Arrange()
        {
            base.Arrange();

            numberOforderDetailsInDatabase = (int)database.ExecuteScalar(CommandType.Text, "SELECT count(*) FROM [Order Details]");

            //reset number of  connections created
            numberOfConnectionsCreated = 0;

            database.BeginExecuteNonQuery(transaction, CommandType.Text, "DELETE FROM [Order Details]",
                ar =>
                {
                    numberOfAffectedRecords = database.EndExecuteNonQuery(ar);
                    barrier.Await();
                }, null);
        }

        [TestMethod]
        public void ThenEndExecuteNonQueryReturnsNumberOfAffectedRecords()
        {
            barrier.Await(4000);
            Assert.AreEqual(numberOforderDetailsInDatabase, numberOfAffectedRecords);
        }

        [TestMethod]
        public void ThenConnectionIsNotClosedAfterEndExecuteNonQuery()
        {
            barrier.Await(4000);
            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            barrier.Await(4000);
            Assert.AreEqual(0, numberOfConnectionsCreated);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryWithBadSql : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteNonQuery(CommandType.Text, "BAD sql", null, null);

        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void ThenEndExecuteNonQueryThrows()
        {
            database.EndExecuteNonQuery(asyncResult);
        }

        [TestMethod]
        public void ThenConnectionIsClosedAfterEndExecute()
        {
            try
            {
                database.EndExecuteNonQuery(asyncResult);
            }
            catch (SqlException) { }

            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryOnSproc : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteNonQuery("Ten Most Expensive Products", null, null);
        }

        [TestMethod]
        public void CanCallEndExecute()
        {
            database.EndExecuteNonQuery(asyncResult);
        }

        [TestMethod]
        public void ThenConnectionIsClosedAfterEndExecute()
        {
            database.EndExecuteNonQuery(asyncResult);

            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryOnSprocCommand : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult =
                database.BeginExecuteNonQuery(
                    new SqlCommand
                    {
                        CommandText = "Ten Most Expensive Products",
                        CommandType = CommandType.StoredProcedure
                    },
                    null,
                    null);
        }

        [TestMethod]
        public void CanCallEndExecute()
        {
            database.EndExecuteNonQuery(asyncResult);
        }

        [TestMethod]
        public void ThenConnectionIsClosedAfterEndExecute()
        {
            database.EndExecuteNonQuery(asyncResult);

            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryOnSprocInsideTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();
            string procName = "Ten Most Expensive Products";
            EnsureParametersCached(procName);

            numberOfConnectionsCreated = 0;
            asyncResult = database.BeginExecuteNonQuery(transaction, procName, null, null);
        }

        [TestMethod]
        public void CanCallEndExecute()
        {
            database.EndExecuteNonQuery(asyncResult);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            database.EndExecuteNonQuery(asyncResult);
            Assert.AreEqual(0, numberOfConnectionsCreated);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReader : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteReader(CommandType.Text, "SELECT 'hello there'", null, null);
        }

        [TestMethod]
        public void ThenConnectionIsClosedOnceReaderIsClosed()
        {
            var reader = database.EndExecuteReader(asyncResult);
            Assert.AreEqual(ConnectionState.Open, lastConnectionStateChange);

            reader.Close();
            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReaderWithCommand : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult =
                database.BeginExecuteReader(
                    new SqlCommand { CommandType = CommandType.Text, CommandText = "SELECT 'hello there'" },
                    null,
                    null);
        }

        [TestMethod]
        public void ThenConnectionIsClosedOnceReaderIsClosed()
        {
            var reader = database.EndExecuteReader(asyncResult);
            Assert.AreEqual(ConnectionState.Open, lastConnectionStateChange);

            reader.Close();
            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteXmlReader : AsynchronousConnectionContext
    {
        private readonly Barrier barrier = new Barrier(2);
        private XmlReader reader;

        protected override void Arrange()
        {
            base.Arrange();

            string queryString = "Select * from Region for xml auto, xmldata";
            SqlCommand sqlCommand = database.GetSqlStringCommand(queryString) as SqlCommand;
            database.BeginExecuteXmlReader(sqlCommand,
                ar =>
                {
                    barrier.Await();
                    reader = database.EndExecuteXmlReader(ar);
                    barrier.Await();
                }, null);
        }

        [TestMethod]
        public void ThenEndExecuteXmlReaderReturnsXml()
        {
            barrier.Await(4000);
            barrier.Await(4000);
            using (reader)
            {
                Assert.IsNotNull(reader);
                Assert.IsNotNull(reader.MoveToContent());
            }
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteXmlReaderAndThenInvokingEndExecute : AsynchronousConnectionContext
    {
        private IAsyncResult result;

        protected override void Arrange()
        {
            base.Arrange();

            string queryString = "Select * from [Order Details] for xml auto, xmldata";
            SqlCommand command = database.GetSqlStringCommand(queryString) as SqlCommand;
            result = database.BeginExecuteXmlReader(command, null, null);
        }

        [TestMethod]
        public void ThenEndExecuteXmlReaderReturnsXml()
        {
            using (var reader = database.EndExecuteXmlReader(result))
            {
                Assert.IsNotNull(reader);
                Assert.IsNotNull(reader.MoveToContent());
            }
        }

        [TestMethod]
        public void ThenCanReadToTheEnd()
        {
            using (var reader = database.EndExecuteXmlReader(result))
            {
                while (reader.Read()) ;
            }
        }

        [TestMethod]
        public void ThenConnectionIsClosedWhenReaderIsClosed()
        {
            Assert.AreNotEqual(ConnectionState.Closed, this.lastConnectionStateChange);
            using (var reader = database.EndExecuteXmlReader(result))
            {
                Assert.AreNotEqual(ConnectionState.Closed, this.lastConnectionStateChange);
                while (reader.Read())
                {
                }
            }
            Assert.AreEqual(ConnectionState.Closed, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteXmlReaderWithTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            string queryString = "Select * from Region for xml auto, xmldata";
            SqlCommand sqlCommand = database.GetSqlStringCommand(queryString) as SqlCommand;
            asyncResult = database.BeginExecuteXmlReader(sqlCommand, transaction, null, null);
        }


        [TestMethod]
        public void ThenEndExecuteXmlReaderReturnsXml()
        {
            using (XmlReader reader = database.EndExecuteXmlReader(asyncResult))
            {
                Assert.IsNotNull(reader);
                Assert.IsNotNull(reader.MoveToContent());
            }
        }

        [TestMethod]
        public void ThenNoNewConnectionWasCreated()
        {
            database.EndExecuteXmlReader(asyncResult).Close();
            Assert.AreEqual(0, numberOfConnectionsCreated);
        }

        [TestMethod]
        public void ThenConnectionIsNotClosedWhenReaderIsClosed()
        {
            Assert.AreNotEqual(ConnectionState.Closed, this.lastConnectionStateChange);
            using (var reader = database.EndExecuteXmlReader(asyncResult))
            {
                Assert.AreNotEqual(ConnectionState.Closed, this.lastConnectionStateChange);
                while (reader.Read()) ;
            }
            Assert.AreNotEqual(ConnectionState.Closed, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteXmlReaderWithBadSQL : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            string queryString = "Select * from Region for LMX auto, xmldata";
            SqlCommand sqlCommand = database.GetSqlStringCommand(queryString) as SqlCommand;
            asyncResult = database.BeginExecuteXmlReader(sqlCommand, null, null);
        }


        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void ThenEndExecuteXmlReaderThrows()
        {
            database.EndExecuteXmlReader(asyncResult);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReaderWithTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteReader(transaction, CommandType.Text, "SELECT 'hello there'", null, null);
        }

        private void DisposeReader()
        {
            database.EndExecuteReader(asyncResult).Dispose();
        }

        [TestMethod]
        public void ThenConnectionIsNotClosedAfterReaderIsClosed()
        {
            DisposeReader();

            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            Assert.AreEqual(0, numberOfConnectionsCreated);
            DisposeReader();
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReaderWithBadSQL : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteReader(CommandType.Text, "bad sql", null, null);
        }


        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void ThenEndExecuteReaderThrows()
        {
            database.EndExecuteReader(asyncResult);
        }

        [TestMethod]
        public void ThenConnectionIsClosed()
        {
            try
            {
                database.EndExecuteReader(asyncResult);
            }
            catch (SqlException) { }

            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReaderWithBadSQLWithinTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteReader(transaction, CommandType.Text, "Bad SQL", null, null);
        }

        [TestMethod]
        public void ThenConnectionStaysOpen()
        {
            try
            {
                database.EndExecuteReader(asyncResult);
            }
            catch (SqlException) { }

            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            try
            {
                database.EndExecuteReader(asyncResult);
            }
            catch (SqlException) { }
            Assert.AreEqual(0, numberOfConnectionsCreated);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReaderOnSproc : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteReader("Ten Most Expensive Products", null, null);
        }

        [TestMethod]
        public void ThenEndExecuteReaderReturnsDataReader()
        {
            IDataReader reader = database.EndExecuteReader(asyncResult);
            Assert.IsNotNull(reader);
            Assert.IsTrue(reader.Read());

            reader.Dispose();
        }


        [TestMethod]
        public void ThenConnectionIsClosedOnceReaderIsClosed()
        {
            IDataReader reader = database.EndExecuteReader(asyncResult);
            Assert.AreEqual(ConnectionState.Open, lastConnectionStateChange);

            reader.Close();
            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteReaderOnSprocWithinTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteReader(transaction, "Ten Most Expensive Products", null, null);
        }

        private void DisposeReader()
        {
            database.EndExecuteReader(asyncResult).Dispose();
        }

        [TestMethod]
        public void ThenEndExecuteReaderReturnsDataReader()
        {
            IDataReader reader = database.EndExecuteReader(asyncResult);
            Assert.IsNotNull(reader);
            Assert.IsTrue(reader.Read());

            reader.Dispose();
        }

        [TestMethod]
        public void ThenConnectionStaysOpen()
        {
            DisposeReader();

            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            Assert.AreEqual(0, numberOfConnectionsCreated);
            DisposeReader();
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingBeginExecuteReaderOnSprocInsideAmbientTransaction : AsynchronousConnectionContext
    {
        TransactionScope transactionScope;

        protected override void Arrange()
        {
            base.Arrange();

            this.transactionScope = new TransactionScope();
        }

        protected override void Teardown()
        {
            this.transactionScope.Dispose();
        }

        protected override void Act()
        {
            var asyncResult = database.BeginExecuteReader("Ten Most Expensive Products", null, null);
            var reader = database.EndExecuteReader(asyncResult);
            reader.Dispose();
        }

        [TestMethod]
        public void SharedConnectionIsStillOpen()
        {
            database.ExecuteNonQuery("Ten Most Expensive Products");
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalar : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar(CommandType.Text, "SELECT 'hello there'", null, null);
        }

        [TestMethod]
        public void ThenConnectionIsClosedOnceScalarExecuted()
        {
            var scalar = database.EndExecuteScalar(asyncResult);
            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);

            Assert.AreEqual("hello there", scalar);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarWithoutAnyResults : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar(CommandType.Text, "waitfor delay '00:00:02';", null, null);
        }

        [TestMethod]
        public void ThenEndResultReturnsNull()
        {
            var result = database.EndExecuteScalar(asyncResult);
            Assert.IsNull(result);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarWithoutAnyRecords : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar(CommandType.Text, "SELECT 'Hello World' WHERE 1=0", null, null);
        }

        [TestMethod]
        public void ThenEndResultReturnsNull()
        {
            var result = database.EndExecuteScalar(asyncResult);
            Assert.IsNull(result);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarWithTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar(transaction, CommandType.Text, "SELECT 'hello there'", null, null);
        }

        [TestMethod]
        public void ThenConnectionIsNotClosedAfterScalarIsClosed()
        {
            var Scalar = database.EndExecuteScalar(asyncResult);
            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            Assert.AreEqual(0, numberOfConnectionsCreated);
            database.EndExecuteScalar(asyncResult);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarWithBadSQL : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar(CommandType.Text, "bad sql", null, null);
        }


        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void ThenEndExecuteScalarThrows()
        {
            database.EndExecuteScalar(asyncResult);
        }

        [TestMethod]
        public void ThenConnectionIsClosed()
        {
            try
            {
                database.EndExecuteScalar(asyncResult);
            }
            catch (SqlException) { }

            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarWithBadSQLWithinTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar(transaction, CommandType.Text, "Bad SQL", null, null);
        }

        [TestMethod]
        public void ThenConnectionStaysOpen()
        {
            try
            {
                database.EndExecuteScalar(asyncResult);
            }
            catch (SqlException) { }

            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            try
            {
                database.EndExecuteScalar(asyncResult);
            }
            catch (SqlException) { }
            Assert.AreEqual(0, numberOfConnectionsCreated);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarOnSproc : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult = database.BeginExecuteScalar("Ten Most Expensive Products", null, null);
        }

        [TestMethod]
        public void ThenEndExecuteScalarReturnsScalarValue()
        {
            var result = database.EndExecuteScalar(asyncResult);
            Assert.IsNotNull(result);
            Assert.AreEqual("Côte de Blaye", result);
        }

        [TestMethod]
        public void ThenConnectionIsClosedOnceScalarIsClosed()
        {
            var result = database.EndExecuteScalar(asyncResult);
            Assert.AreEqual(ConnectionState.Closed, lastConnectionStateChange);
        }
    }

    [TestClass]
    public class WhenAsynchronouslyInvokingExecuteScalarOnSprocWithinTransaction : TransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            string procName = "Ten Most Expensive Products";
            EnsureParametersCached(procName);

            numberOfConnectionsCreated = 0;

            asyncResult = database.BeginExecuteScalar(transaction, procName, null, null);
        }

        [TestMethod]
        public void ThenEndExecuteScalarReturnsScalarResult()
        {
            object result = database.EndExecuteScalar(asyncResult);
            Assert.IsNotNull(result);
            Assert.AreEqual("Côte de Blaye", result);
        }

        [TestMethod]
        public void ThenConnectionStaysOpen()
        {
            object result = database.EndExecuteScalar(asyncResult);

            Assert.AreEqual(ConnectionState.Open, transaction.Connection.State);
        }

        [TestMethod]
        public void ThenNoNewConnectionIsCreated()
        {
            Assert.AreEqual(0, numberOfConnectionsCreated);
            database.EndExecuteScalar(asyncResult);
        }
    }

    [TestClass]
    public class WhenCreatingMultipleAsyncReaders : AsynchronousConnectionContext
    {
        IAsyncResult asyncResult1;
        IAsyncResult asyncResult2;
        IAsyncResult asyncResult3;

        protected override void Arrange()
        {
            base.Arrange();

            asyncResult1 = database.BeginExecuteReader("Ten Most Expensive Products", null, null);
            asyncResult2 = database.BeginExecuteReader("Ten Most Expensive Products", null, null);
            asyncResult3 = database.BeginExecuteReader("Ten Most Expensive Products", null, null);
        }

        [TestMethod]
        public void ThenMultipleConnectionsAreCreated()
        {
            Assert.AreEqual(3, numberOfConnectionsCreated);

            database.EndExecuteReader(asyncResult1).Dispose();
            database.EndExecuteReader(asyncResult2).Dispose();
            database.EndExecuteReader(asyncResult3).Dispose();

        }
    }


    public abstract class SystemTransactionalAsynchronousConnectionWithRollback : AsynchronousConnectionContext
    {
        protected TransactionScope transactionScope;

        protected override void Arrange()
        {
            transactionScope = new TransactionScope();
            base.Arrange();
        }

        protected override void Teardown()
        {
            transactionScope.Dispose();
        }
    }


    [TestClass]
    public class WhenExecutingNonQueryWithinSystemTransaction : SystemTransactionalAsynchronousConnectionWithRollback
    {
        IAsyncResult asyncResult;

        protected override void Act()
        {
            base.Act();

            asyncResult = database.BeginExecuteNonQuery(CommandType.Text,
                @"CREATE TABLE [test] (c2 int);
                 INSERT INTO test (c2) VALUES (12)",
                null, null);
        }

        [TestMethod]
        public void ThenExecuteNonQueryParticipatesInTransaction()
        {
            database.EndExecuteNonQuery(asyncResult);

            var doesntThrow = database.ExecuteReader(CommandType.Text, "Select * from test");
            doesntThrow.Dispose();

            //roll back tx
            transactionScope.Dispose();
            try
            {
                database.ExecuteReader(CommandType.Text, "Select * from test");
                Assert.Fail();
            }
            catch (SqlException se)
            {
                Assert.AreEqual("Invalid object name 'test'.", se.Message);
            }
        }

        [TestMethod]
        public void ThenAsyncExecuteReaderParticipatesInSameTransaction()
        {
            database.EndExecuteNonQuery(asyncResult);

            IAsyncResult execReader = database.BeginExecuteReader(CommandType.Text, "Select * from test", null, null);
            using (IDataReader reader = database.EndExecuteReader(execReader))
            {
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(12, reader.GetInt32(0));
            }
        }
    }

    public abstract class AsynchronousConnectionContextWithNonAsyncConnectionString
        : AsynchronousConnectionContext
    {
        protected override void Arrange()
        {
            this.connectionstring = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true";
            base.Arrange();
        }
    }

    public abstract class TransactionalAsynchronousConnectionWithRollbackNonAsyncConnectionString
        : TransactionalAsynchronousConnectionWithRollback
    {
        protected override void Arrange()
        {
            this.connectionstring = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true";
            base.Arrange();
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryWithNonAsyncConnectionString
        : AsynchronousConnectionContextWithNonAsyncConnectionString
    {
        protected override void Act()
        {
            try
            {
                database.BeginExecuteNonQuery(CommandType.Text, "SELECT 'hello there'", null, null);
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void ThenConnectionIsClosed()
        {
            Assert.AreEqual(ConnectionState.Closed, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenAsynchronouslyInvokingBeginExecuteReaderWithNonAsyncConnectionString
        : AsynchronousConnectionContextWithNonAsyncConnectionString
    {
        protected override void Act()
        {
            try
            {
                database.BeginExecuteReader(CommandType.Text, "SELECT 'hello there'", null, null);
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void ThenConnectionIsClosed()
        {
            Assert.AreEqual(ConnectionState.Closed, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenAsynchronouslyInvokingBeginExecuteXmlReaderWithNonAsyncConnectionString
        : AsynchronousConnectionContextWithNonAsyncConnectionString
    {
        protected override void Act()
        {
            try
            {
                database.BeginExecuteXmlReader(
                    new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = "Select * from Region for xml auto, xmldata"
                    },
                    null,
                    null);
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void ThenConnectionIsClosed()
        {
            Assert.AreEqual(ConnectionState.Closed, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenAsynchronouslyInvokingBeginExecuteNonQueryInTransactionWithNonAsyncConnectionString
        : TransactionalAsynchronousConnectionWithRollbackNonAsyncConnectionString
    {
        protected override void Act()
        {
            try
            {
                database.BeginExecuteNonQuery(this.transaction, CommandType.Text, "SELECT 'hello there'", null, null);
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void ThenConnectionIsNotClosed()
        {
            Assert.AreEqual(ConnectionState.Open, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenAsynchronouslyInvokingBeginExecuteReaderInTransactionWithNonAsyncConnectionString
        : TransactionalAsynchronousConnectionWithRollbackNonAsyncConnectionString
    {
        protected override void Act()
        {
            try
            {
                database.BeginExecuteReader(this.transaction, CommandType.Text, "SELECT 'hello there'", null, null);
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void ThenConnectionIsNotClosed()
        {
            Assert.AreEqual(ConnectionState.Open, this.lastConnectionStateChange);
        }
    }

    [TestClass]
    [Ignore]    // No longer an error in .NET 4.5
    public class WhenAsynchronouslyInvokingBeginExecuteXmlReaderInTransactionWithNonAsyncConnectionString
        : TransactionalAsynchronousConnectionWithRollbackNonAsyncConnectionString
    {
        protected override void Act()
        {
            try
            {
                database.BeginExecuteXmlReader(
                    new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = "Select * from Region for xml auto, xmldata"
                    },
                    this.transaction,
                    null,
                    null);
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void ThenConnectionIsNotClosed()
        {
            Assert.AreEqual(ConnectionState.Open, this.lastConnectionStateChange);
        }
    }
}
