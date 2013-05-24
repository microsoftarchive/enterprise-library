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
using System.Data.Common;
using System.Data.SqlServerCe;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteScalarFixture
    {
        TestConnectionString testConnection;
        SqlCeDatabase db;
        ExecuteScalarFixture baseFixture;

        [TestInitialize]
        public void SetUp()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
            DbCommand command = db.GetSqlStringCommand("Select count(*) from region");

            baseFixture = new ExecuteScalarFixture(db, command);
        }

        [TestCleanup]
        public void DeleteDb()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }

        [TestMethod]
        public void CanOpenDatabase()
        {
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void ExecuteScalarWithIDbCommand()
        {
            baseFixture.ExecuteScalarWithIDbCommand();
        }

        [TestMethod]
        public void ExecuteScalarWithCommandTextAndTypeInTransaction()
        {
            baseFixture.ExecuteScalarWithCommandTextAndTypeInTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotExecuteReaderWithStoredProcInTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.ExecuteScalar(transaction, "Ten Most Expensive Products");
                    transaction.Rollback();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SqlCeException))]
        public void ExecuteSqlWithBadCommandThrows()
        {
            DbCommand badCommand = db.GetSqlStringCommand("select * from invalid");
            db.ExecuteScalar(badCommand);
        }

        [TestMethod]
        public void ExecuteScalarWithIDbTransaction()
        {
            baseFixture.ExecuteScalarWithIDbTransaction();
        }

        [TestMethod]
        public void CanExecuteScalarDoAnInsertion()
        {
            baseFixture.CanExecuteScalarDoAnInsertion();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbCommand()
        {
            baseFixture.ExecuteScalarWithNullIDbCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbTransaction();
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbCommandAndNullTransaction();
        }
    }
}
