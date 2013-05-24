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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aExpense.AutomationTests.DataAccess;

namespace aExpense.AutomationTests.Util
{
    public class DatabaseHelper
    {
        public static void DeleteAllNonDefaultExpenses(string databaseConnectionString)
        {
            Guid guid1 = new Guid("abafc874-d0cc-4245-9319-1e5a75108a41");
            Guid guid2 = new Guid("abafc874-d0cc-4245-9319-1e5c77158b42");
            Guid guid3 = new Guid("abafc874-a0cc-4145-9319-1e5c78508a41");

            using (var db = new aExpenseDataContext(databaseConnectionString))
            {
                var expenseDetails = db.ExpenseDetails.Where(e => e.ExpenseId != guid1 &&
                                                                    e.ExpenseId != guid2 &&
                                                                        e.ExpenseId != guid3 );
                var expenses = db.Expenses.Where(e => e.Id != guid1 &&
                                                        e.Id != guid2 &&
                                                            e.Id != guid3);

                db.ExpenseDetails.DeleteAllOnSubmit(expenseDetails);
                db.Expenses.DeleteAllOnSubmit(expenses);
                db.SubmitChanges();
            }
        }


        internal static void CleanLoggingDB(string databaseConnectionString)
        {
            using (var db = new LoggingDataContext(databaseConnectionString))
            {
                db.CategoryLogs.DeleteAllOnSubmit(db.CategoryLogs);
                db.Logs.DeleteAllOnSubmit(db.Logs);
                db.SubmitChanges();
            }
        }

        internal static string GetFirstLogFormattedMessage(string databaseConnectionString)
        {
            using (var db = new LoggingDataContext(databaseConnectionString))
            {
                var log = db.Logs.Where(e => e.FormattedMessage.Contains("Extended Properties: value - testing title")).SingleOrDefault();
                if (log == null)
                    return null;

                return log.FormattedMessage;
            }
        }

        internal static string GetFirstErrorFormattedMessage(string databaseConnectionString)
        {
            using (var db = new LoggingDataContext(databaseConnectionString))
            {
                var log = db.Logs.Where(e => e.Severity == "error").SingleOrDefault();
                if (log == null)
                    return null;

                return log.FormattedMessage;
            }
        }

        internal static List<Expense> GetExpenses(string databaseConnectionString)
        {
            using (var db = new aExpenseDataContext(databaseConnectionString))
            {
                return db.Expenses.ToList();
            }
        }

        internal static List<ExpenseDetail> GetExpenseDetailsForExpense(string databaseConnectionString, Guid expenseId)
        {
            using (var db = new aExpenseDataContext(databaseConnectionString))
            {
                return db.ExpenseDetails.Where(e => e.ExpenseId == expenseId).ToList();
            }
        }
    }
}
