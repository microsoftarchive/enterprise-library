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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteReaderFixture
    {
        TestConnectionString testConnection;
        const string insertString = "Insert into Region values (99, 'Midwest')";
        const string queryString = "Select * from Region";
        Database db;
        ExecuteReaderFixture baseFixture;
        private DbCommand queryCommand;

        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);

            DbCommand insertCommand = db.GetSqlStringCommand(insertString);
            queryCommand = db.GetSqlStringCommand(queryString);

            baseFixture = new ExecuteReaderFixture(db, insertString, insertCommand, queryString, queryCommand);
        }

        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
        }

        /// <summary>
        ///		SqlServerCE doesn't have support for stored procedures, so attempting to use a stored
        ///		procedure should throw an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteReaderWithStoredProcInTransactionNotSupported()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (db.ExecuteReader(transaction, "Ten Most Expensive Products")) { }
                    transaction.Commit();
                }
            }
        }

        [TestMethod]
        public void CanExecuteReaderWithCommandText()
        {
            baseFixture.CanExecuteReaderWithCommandText();
        }

        [TestMethod]
        public void CanExecuteReaderFromDbCommand()
        {
            // Repeating this from base instead of calling base class because base
            // asserts don't match SQL CE database close semantics - we do explicit
            // pooling in CE, so connection doesn't actually close on close.
            IDataReader reader = db.ExecuteReader(queryCommand);
            DbConnection connection = queryCommand.Connection;
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();

            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            Assert.AreEqual(ConnectionState.Open, connection.State);

            // Forcing pool closed should close the connection
            SqlCeConnectionPool.CloseSharedConnection(db);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlCeException))]
        public void ExecuteReaderWithBadCommandThrows()
        {
            DbCommand badCommand = db.GetSqlStringCommand("select * from invalid");
            db.ExecuteReader(badCommand);
        }

        [TestMethod]
        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            baseFixture.WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute();
        }

        [TestMethod]
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            baseFixture.CanExecuteQueryThroughDataReaderUsingTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanExecuteQueryThroughDataReaderUsingNullCommand()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullCommandThrows();
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullTransactionThrows();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteReaderWithNullCommand()
        {
            baseFixture.ExecuteReaderWithNullCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullQueryStringTest()
        {
            baseFixture.NullQueryStringTest();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyQueryStringTest()
        {
            baseFixture.EmptyQueryStringTest();
        }
    }
}
