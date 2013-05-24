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
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Sql
{
    [TestClass]
    public class SqlTransactionScopeFixture
    {
        TransactionScopeFixture baseFixture;
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();

            try
            {
                DeleteStoredProcedures();
            }
            catch { }
            CreateStoredProcedures();

            baseFixture = new TransactionScopeFixture(db);
            baseFixture.Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            baseFixture.Cleanup();
            DeleteStoredProcedures();
        }

        [TestMethod]
        public void TransactionScope_ShouldDiscardChangesOnDispose()
        {
            baseFixture.TransactionScope_ShouldDiscardChangesOnDispose();
        }

        [TestMethod]
        public void TransactionScope_ShouldNotPromoteToDTC()
        {
            baseFixture.TransactionScope_ShouldNotPromoteToDTC();
        }

        [TestMethod]
        public void Commit_ShouldKeepChanges()
        {
            baseFixture.Commit_ShouldKeepChanges();
        }

        [TestMethod]
        public void Comit_ShouldKeepInnerChangesForNestedTransaction()
        {
            baseFixture.Comit_ShouldKeepInnerChangesForNestedTransaction();
        }

        [TestMethod]
        public void Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted()
        {
            baseFixture.Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted();
        }

        [TestMethod]
        public void Insert_ShouldAddRowsWhenNoTransactionActive()
        {
            baseFixture.Insert_ShouldAddRowsWhenNoTransactionActive();
        }

        [TestMethod]
        public void ShouldAllowCommandsAfterInnerScopeDisposed()
        {
            baseFixture.ShouldAllowCommandsAfterInnerScopeDisposed();
        }

        [TestMethod]
        public void Commit_ShouldDisposeTransactionConnection()
        {
            baseFixture.Commit_ShouldDisposeTransactionConnection();
        }

        [TestMethod]
        public void Rollback_ShouldDisposeTransactionConnection()
        {
            baseFixture.Rollback_ShouldDisposeTransactionConnection();
        }

        [TestMethod]
        public void ExecuteNonQueryWithTextCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithTextCommand_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteNonQueryWithCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithCommand_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithCommand_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithCommandText_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithCommandText_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithStoredProcedure_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithStoredProcedure_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteDataSetWithCommandText_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithCommandText_ShouldRetriveDataSet();
        }

        [TestMethod]
        public void ExecuteDataSetWithCommand_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithCommand_ShouldRetriveDataSet();
        }

        [TestMethod]
        public void ExecuteDataSetWithStoredProcedure_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithStoredProcedure_ShouldRetriveDataSet();
        }

        [TestMethod]
        public void ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction();
        }

        [TestMethod]
        public void ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction();
        }

        [TestMethod]
        public void ExecuteReaderWithStoredProcedure_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithStoredProcedure_ShouldRetrieveDataInTransaction();
        }

        [TestMethod]
        public void LoadDataSetWithCommandText_LoadsDataInTransaction()
        {
            baseFixture.LoadDataSetWithCommandText_LoadsDataInTransaction();
        }

        [TestMethod]
        public void LoadDataSetWithCommand_LoadsDataInTransaction()
        {
            baseFixture.LoadDataSetWithCommand_LoadsDataInTransaction();
        }

        [TestMethod]
        public void UpdateDataSet_ShouldAddToTransaction()
        {
            baseFixture.UpdateDataSet_ShouldAddToTransaction();
        }

        [TestMethod]
        public void UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction()
        {
            baseFixture.UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction();
        }

        [TestMethod]
        public void CanExecuteXmlQueryRetrivingDataOnly()
        {
            int dtcCount = 0;

            TransactionManager.DistributedTransactionStarted += delegate(object sender,
                                                                         TransactionEventArgs e)
                                                                {
                                                                    dtcCount++;
                                                                };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteScalar(CommandType.Text, "select count(*) from region");

                ExecuteXmlReaderFixture fixture = new ExecuteXmlReaderFixture();
                fixture.SetUp();
                fixture.CanExecuteXmlQueryRetrivingDataOnly();
            }

            Assert.AreEqual(0, dtcCount);
        }

        void CreateStoredProcedures()
        {
            SqlDataSetHelper.CreateStoredProcedures(db);
        }

        void DeleteStoredProcedures()
        {
            SqlDataSetHelper.DeleteStoredProcedures(db);
        }
    }

    [TestClass]
    public class TransactionScopeBugsFixture
    {
        [TestMethod]
        public void CanExecuteOperationAfterExecuteReaderThrowsExceptionWithAmbientTransaction_Bug2769()
        {
            var database = new SqlDatabase(@"server=(localdb)\v11.0;database=Northwind;Integrated Security=true");

            using (var scope = new TransactionScope())
            {
                try
                {
                    database.ExecuteReader(
                        new SqlCommand { CommandText = "invalid invalid invalid", CommandType = CommandType.Text });
                    Assert.Fail("should have thrown");
                }
                catch (DbException)
                {
                    database.ExecuteNonQuery("Ten Most Expensive Products");
                }
            }
        }
    }
}
