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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public class ExecuteNonQueryFixture
    {
        DbCommand countCommand;
        string countQuery;
        Database db;
        DbCommand insertionCommand;
        string insertString;

        public ExecuteNonQueryFixture(Database db,
                                      string insertString,
                                      string countQuery,
                                      DbCommand insertionCommand,
                                      DbCommand countCommand)
        {
            this.db = db;
            this.insertString = insertString;
            this.countQuery = countQuery;
            this.insertionCommand = insertionCommand;
            this.countCommand = countCommand;
        }

        public void CanExecuteNonQueryThroughTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    int rowsAffected = db.ExecuteNonQuery(insertionCommand, transaction.Transaction);

                    int count = Convert.ToInt32(db.ExecuteScalar(countCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                    Assert.AreEqual(1, rowsAffected);
                }
            }
        }

        public void CanExecuteNonQueryWithCommandTextAndTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(transaction, insertionCommand.CommandText);
                    transaction.Commit();
                }
            }

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void CanExecuteNonQueryWithCommandTextAsStoredProcAndTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    connection.Open();
                    db.ExecuteNonQuery(transaction, insertionCommand.CommandText);
                    transaction.Commit();
                }
            }

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(transaction, CommandType.Text, insertionCommand.CommandText);
                    transaction.Commit();
                }
            }

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void CanExecuteNonQueryWithDbCommand()
        {
            db.ExecuteNonQuery(insertionCommand);

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            db.ExecuteNonQuery(null, (string)null);
        }

        public void ExecuteNonQueryWithNullDbTransaction()
        {
            db.ExecuteNonQuery(insertionCommand, null);
        }

        public void TransactionActuallyRollsBack()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    db.ExecuteNonQuery(insertionCommand, transaction.Transaction);
                }
            }

            DbCommand wrapper = db.GetSqlStringCommand(countQuery);
            int count = Convert.ToInt32(db.ExecuteScalar(wrapper));
            Assert.AreEqual(4, count);
        }
    }
}
