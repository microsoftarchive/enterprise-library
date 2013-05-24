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
using System.Configuration;
using System.Diagnostics.Tracing;
using AExpense.DataAccessLayer;
using AExpense.FunctionalTests.Util;
using AExpense.Instrumentation;
using AExpense.Model;
using AExpense.Tests.Util;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class TracingAndInterceptionAppBlockFixture
    {
        private static readonly string TestDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ConnectionString;
        private static readonly string TracingDatabaseConnectionString = ConfigurationManager.ConnectionStrings["Tracing"].ConnectionString;
        private static readonly Guid testGuid = Guid.NewGuid();
        private UnityContainer container;
        private InMemoryEventListener listener;

        [TestInitialize]
        public void TestInit()
        {
            DatabaseHelper.CleanLoggingDB(TracingDatabaseConnectionString);
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(container);
            this.listener = new InMemoryEventListener();
            this.listener.EnableEvents(AExpenseEvents.Log, EventLevel.LogAlways, Keywords.All);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.container.Dispose();
            DatabaseHelper.DeleteExpenseInDB(TestDatabaseConnectionString, testGuid);
            this.listener.DisableEvents(AExpenseEvents.Log);
            this.listener.Dispose();
        }

        /// <summary>
        /// Tracing and interception happy path, for negative scenarios check exceptionHandling fixture.
        /// </summary>
        [TestMethod]
        public void CanInterceptAndTraceWithValidApprover()
        {
            var newExpense = new Model.Expense();
            newExpense.Id = testGuid;
            newExpense.Title = "testing title";
            newExpense.CostCenter = "testing cost center";
            newExpense.ReimbursementMethod = ReimbursementMethod.DirectDeposit;
            newExpense.User = new Model.User() { FullName = "test user", UserName = "tester" };
            newExpense.Date = System.DateTime.Today;
            newExpense.ApproverName = "ADATUM\\mary";
            newExpense.Details.Add(new Model.ExpenseItem() { Description = "test item", Amount = 10, Id = Guid.NewGuid() });

            var repository = new ExpenseRepository(TestDatabaseConnectionString);

            repository.SaveExpense(newExpense);

            var events = listener.EventsWritten;

            Assert.IsNotNull(events);
            Assert.AreEqual(6, events.Count);
        }

        [TestMethod]
        public void CanInterceptAndTraceWhenUserNameIsSet()
        {
            var user = container.Resolve<Model.User>();
            user.UserName = "foo";

            var events = listener.EventsWritten;

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Count);
            StringAssert.Contains(events[0].Payload[0].ToString(), "User");
            StringAssert.Contains(events[0].Payload[1].ToString(), "set_UserName");
        }
    }
}
