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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteDatasetFixture
    {
        TestConnectionString testConnection;
        const string queryString = "Select * from Region";
        const string storedProc = "Ten Most Expensive Products";
        Database db;

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
            testConnection = new TestConnectionString();
            testConnection.DeleteFile();
        }

        [TestMethod]
        public void CanRetriveDataSetFromSqlString()
        {
            DataSet dataSet = db.ExecuteDataSet(CommandType.Text, queryString);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotRetiveDataSetFromStoredProcedure()
        {
            DataSet dataSet = db.ExecuteDataSet(storedProc);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }

        [TestMethod]
        public void CanRetriveDataSetFromSqlStringWithTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    DataSet dataSet = db.ExecuteDataSet(transaction, CommandType.Text, queryString);
                    Assert.AreEqual(1, dataSet.Tables.Count);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotRetiveDataSetFromStoredProcedureWithTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    DataSet dataSet = db.ExecuteDataSet(transaction, storedProc);
                    Assert.AreEqual(1, dataSet.Tables.Count);
                }
            }
        }
    }
}
