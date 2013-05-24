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

using System.Configuration;
using AExpense.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System;
using AExpense.FunctionalTests.Util;
using System.Xml.Linq;
using System.Linq;
using AExpense.Instrumentation;
using System.Diagnostics.Tracing;
using AExpense.FunctionalTests;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using AExpense.DataAccessLayer;
using Microsoft.Practices.Unity;
using AExpense.Model;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class SemanticLoggingActivityFixture
    {
        private IUnityContainer container;

        private static readonly string TracingDatabaseConnectionString = ConfigurationManager.ConnectionStrings["Tracing"].ConnectionString;
        private static readonly string TestDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpenseTesting"].ConnectionString;

        string tracingFilename = "aExpense.DataAccess.log";
        string UIlogFilename = "aExpense.UserInterface.log";
        private ObservableEventListener obsListener;

        [TestInitialize]
        public void TestInitialize()
        {
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(container);

            DatabaseHelper.CleanLoggingDB(TracingDatabaseConnectionString);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "").Replace(".Tests", "");

            tracingFilename = Path.Combine(baseDirectory, tracingFilename);
            UIlogFilename = Path.Combine(baseDirectory, UIlogFilename);

            File.Delete(tracingFilename);
            File.Delete(UIlogFilename);

            obsListener = new ObservableEventListener();
            obsListener.EnableEvents(AExpenseEvents.Log, EventLevel.LogAlways, Keywords.All);

            DatabaseHelper.CreateDatabase(TestDatabaseConnectionString);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.obsListener.DisableEvents(AExpenseEvents.Log);
            this.obsListener.Dispose();
            this.container.Dispose();
            DatabaseHelper.CleanLoggingDB(TracingDatabaseConnectionString);

            DatabaseHelper.DeleteDatabase(TestDatabaseConnectionString);

        }

        [TestMethod]
        public void GetAllExpensesLogsActivity()
        {
            using (var subscription = obsListener.LogToSqlDatabase("Activity", TracingDatabaseConnectionString))
            {
                var repository = new ExpenseRepository(TestDatabaseConnectionString, TimeSpan.MinValue);
                repository.GetAllExpenses();

                subscription.Sink.FlushAsync().Wait();

                var logEntries = DatabaseHelper.GetAllLogEntries(TracingDatabaseConnectionString);

                Assert.AreEqual(3, logEntries.Count());

                var GetAllExpensesStartedEntry = logEntries.SingleOrDefault(e => e.EventId == 300);
                Assert.IsNotNull(GetAllExpensesStartedEntry);

                var GetAllExpensesFinishedEntry = logEntries.SingleOrDefault(e => e.EventId == 301);
                Assert.IsNotNull(GetAllExpensesFinishedEntry);
                
            }
        }

        [TestMethod]
        public void GetAllExpensesForApprovalLogsActivity()
        {
            using (var subscription = obsListener.LogToSqlDatabase("Activity", TracingDatabaseConnectionString))
            {
                var repository = new ExpenseRepository(TestDatabaseConnectionString, TimeSpan.MinValue);
                repository.GetExpensesForApproval("ApproverUser");

                subscription.Sink.FlushAsync().Wait();

                var logEntries = DatabaseHelper.GetAllLogEntries(TracingDatabaseConnectionString);

                Assert.AreEqual(6, logEntries.Count());

                var GetExpensesForApprovalStartedEntry = logEntries.SingleOrDefault(e => e.EventId == 320);
                Assert.IsNotNull(GetExpensesForApprovalStartedEntry);
                StringAssert.Contains(GetExpensesForApprovalStartedEntry.Payload, "ApproverUser");

                var GetExpensesForApprovalQueryStartedEntry = logEntries.SingleOrDefault(e => e.EventId == 327);
                Assert.IsNotNull(GetExpensesForApprovalQueryStartedEntry);
                StringAssert.Contains(GetExpensesForApprovalQueryStartedEntry.Payload, "ApproverUser");

                var GetExpensesForApprovalQueryFinishedEntry = logEntries.SingleOrDefault(e => e.EventId == 324);
                Assert.IsNotNull(GetExpensesForApprovalQueryFinishedEntry);
                StringAssert.Contains(GetExpensesForApprovalQueryFinishedEntry.Payload, "ApproverUser");

                var GetExpensesForApprovalCacheUpdateEntry = logEntries.SingleOrDefault(e => e.EventId == 326);
                Assert.IsNotNull(GetExpensesForApprovalCacheUpdateEntry);
                StringAssert.Contains(GetExpensesForApprovalCacheUpdateEntry.Payload, "ApproverUser");

                var GetExpensesForApprovalFinishedEntry = logEntries.SingleOrDefault(e => e.EventId == 321);
                Assert.IsNotNull(GetExpensesForApprovalFinishedEntry);
                StringAssert.Contains(GetExpensesForApprovalFinishedEntry.Payload, "ApproverUser");
            }
        }

        [TestMethod]
        public void AddExpenseLogsActivity()
        {
            using(var subscription = obsListener.LogToSqlDatabase("Activity", TracingDatabaseConnectionString))
            {
                var repository = new ExpenseRepository(TestDatabaseConnectionString, TimeSpan.MinValue);

                Guid expenseIdToSave = Guid.NewGuid();
                Assert.IsNull(DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave));
                var stubUser = new User { UserName = "user name" };
                var stubManager = "the manager";
                var expenseToSave = new Model.Expense
                                        {
                                            Id = expenseIdToSave,
                                            Date = new DateTime(1900, 01, 01),
                                            Title = "Title",
                                            Description = "Description",
                                            Total = 1.0,
                                            ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                                            CostCenter = "CostCenter",
                                            Approved = false,
                                            User = stubUser,
                                      
                                            ApproverName = stubManager
                                        };

                repository.SaveExpense(expenseToSave);
                subscription.Sink.FlushAsync().Wait();

                var logEntries = DatabaseHelper.GetAllLogEntries(TracingDatabaseConnectionString);

                Assert.AreEqual(6, logEntries.Count());
                
                var SaveExpenseStartedEntry = logEntries.SingleOrDefault(e=> e.EventId == 290);
                Assert.IsNotNull(SaveExpenseStartedEntry);
                StringAssert.Contains(SaveExpenseStartedEntry.Payload, expenseToSave.Id.ToString());

                var SaveExpenseInsertStartedEntry = logEntries.SingleOrDefault(e=> e.EventId == 295);
                Assert.IsNotNull(SaveExpenseInsertStartedEntry);
                StringAssert.Contains(SaveExpenseInsertStartedEntry.Payload, expenseToSave.Id.ToString());

                var SaveExpenseInsertFinishedEntry = logEntries.SingleOrDefault(e=> e.EventId == 294);
                Assert.IsNotNull(SaveExpenseInsertFinishedEntry);
                StringAssert.Contains(SaveExpenseInsertFinishedEntry.Payload, expenseToSave.Id.ToString());

                var SaveExpenseCacheUpdatedEntry = logEntries.SingleOrDefault(e=> e.EventId == 292);
                Assert.IsNotNull(SaveExpenseCacheUpdatedEntry);

                var SaveExpenseFinishedEntry = logEntries.SingleOrDefault(e => e.EventId == 291);
                Assert.IsNotNull(SaveExpenseFinishedEntry);
                StringAssert.Contains(SaveExpenseFinishedEntry.Payload, expenseToSave.Id.ToString());
            }
        }
    }
}
