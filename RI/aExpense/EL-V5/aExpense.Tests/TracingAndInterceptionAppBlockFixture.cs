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
using AExpense.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using AExpense.Tests.Util;
using AExpense.FunctionalTests.Properties;
using AExpense.FunctionalTests.DataAccess;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class TracingAndInterceptionAppBlockFixture
    {
        private static readonly string TestDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ConnectionString;
        private static readonly string LoggingDatabaseConnectionString = ConfigurationManager.ConnectionStrings["Logging"].ConnectionString;
        private static readonly Guid testGuid = Guid.NewGuid();
        private UnityContainer container;

        [TestInitialize]
        public void TestInit()
        {
            DatabaseHelper.CleanLoggingDB(LoggingDatabaseConnectionString);
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(container);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.container.Dispose();
            DatabaseHelper.DeleteExpenseInDB(TestDatabaseConnectionString, testGuid);
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

            var repository = container.Resolve<IExpenseRepository>();

            repository.SaveExpense(newExpense);

            Log log = DatabaseHelper.GetFirstLog(LoggingDatabaseConnectionString);

            Assert.IsNotNull(log);
            Assert.AreEqual("Call Logging", log.Title);
        }

        [TestMethod]
        public void CanInterceptAndTraceWhenUserNameIsSet()
        {
            var user = container.Resolve<Model.User>();
            user.UserName = "foo";

            Log log = DatabaseHelper.GetFirstLog(LoggingDatabaseConnectionString);

            Assert.IsNotNull(log, "No data logged");
            StringAssert.Contains(log.FormattedMessage, Resources.SetUserNameWasExecutedMessage);
        }
    }
}
