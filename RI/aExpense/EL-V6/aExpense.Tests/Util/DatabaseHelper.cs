#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using AExpense.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.Security;
using AExpense.Model;
using AExpense.FunctionalTests.DataAccess;

namespace AExpense.Tests.Util
{
    public class DatabaseHelper
    {
        public static void CreateDatabase(string connectionString)
        {
            using (var db = new ExpensesDataContext(connectionString))
            {
                if (db.DatabaseExists())
                {
                    db.DeleteDatabase();
                }

                db.CreateDatabase();
            }
        }

        public static void DeleteDatabase(string connectionString)
        {
            using (var db = new ExpensesDataContext(connectionString))
            {
                    if (db.DatabaseExists())
                    {
                        db.DeleteDatabase();
                    }
            }
        }

        public static AExpense.DataAccessLayer.Expense GetExpenseById(string databaseConnectionString, Guid expenseId)
        {
            using (var db = new ExpensesDataContext(databaseConnectionString))
            {
                return db.Expenses.SingleOrDefault(e => e.Id == expenseId);
            }
        }

        public static void CreateExpense(string databaseConnectionString, Model.Expense expense)
        {
            using (var db = new ExpensesDataContext(databaseConnectionString))
            {
                db.Expenses.InsertOnSubmit(expense.ToEntity());
                db.SubmitChanges();
            }
        }

        public static void BypassRepositoryAndDeleteExpense(string databaseConnectionString, Guid guid)
        {
            using (var db = new ExpensesDataContext(databaseConnectionString))
            {
                DataAccessLayer.Expense expense = db.Expenses.Where(e => e.Id == guid).ToList().FirstOrDefault();
                db.Expenses.DeleteOnSubmit(expense);
                db.SubmitChanges();
            }
        }

        internal static void CleanLoggingDB(string databaseConnectionString)
        {
            using (var db = new TracingDataContext(databaseConnectionString))
            {
                db.Traces.DeleteAllOnSubmit(db.Traces);
                db.SubmitChanges();
            }
        }

        internal static string GetFirstLogFormattedMessage(string databaseConnectionString)
        {
            using (var db = new TracingDataContext(databaseConnectionString))
            {
                var log = db.Traces.Where(e => e.FormattedMessage.Contains("Extended Properties: value - testing title")).SingleOrDefault();
                if (log == null)
                    return null;

                return log.FormattedMessage;
            }
        }

        internal static Trace GetFirstLog(string databaseConnectionString)
        {
            using (var db = new TracingDataContext(databaseConnectionString))
            {
                return db.Traces.FirstOrDefault();
            }
        }

        internal static List<Trace> GetExceptionsFromDB(string databaseConnectionString)
        {
            using (var db = new TracingDataContext(databaseConnectionString))
            {
                return db.Traces.Where(e => e.Level == 2).ToList();
            }
        }

        internal static List<Trace> GetAllLogEntries(string databaseConnectionString)
        {
            using (var db = new TracingDataContext(databaseConnectionString))
            {
                return db.Traces.ToList();
            }
        }

        internal static void DeleteExpenseInDB(string TestDatabaseConnectionString, Guid testGuid)
        {
            using (var db = new ExpensesDataContext(TestDatabaseConnectionString))
            {
                var expDetails = db.ExpenseDetails.Where(e => e.ExpenseId == testGuid);
                db.ExpenseDetails.DeleteAllOnSubmit(expDetails);

                var exp = db.Expenses.Where(e => e.Id == testGuid);
                db.Expenses.DeleteAllOnSubmit(exp);
                db.SubmitChanges();
            }
        }
    }
}
