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
using AExpense.Tests.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class UnityBootstrapperScenarios
    {
        private UnityContainer container;

        private static readonly string TestDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ConnectionString;
        private static readonly Guid testGuid = Guid.NewGuid();


        [TestInitialize]
        public void Arrange()
        {
            container = new UnityContainer();
            ContainerBootstrapper.Configure(container);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseHelper.DeleteExpenseInDB(TestDatabaseConnectionString, testGuid);
            container.Dispose();
        }


        [TestMethod]
        public void CanGetAttributesForUserUsingBootstrapper()
        {

            string username = "ADATUM\\johndoe";

            var lDAPStore = container.Resolve<IProfileStore>();

            var attributes = lDAPStore.GetAttributesFor(username, new[] { "costCenter", "manager", "displayName" });

            Assert.IsNotNull(attributes);
            Assert.AreEqual(3, attributes.Keys.Count);
            Assert.AreEqual("31023", attributes["costCenter"]);
            Assert.AreEqual("ADATUM\\mary", attributes["manager"]);
            Assert.AreEqual("John Doe", attributes["displayName"]);


        }

        [TestMethod]
        public void GetUserUsingBootstrapper()
        {
            string username = "ADATUM\\johndoe";
            var roles = new[] { "Employee" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);

            var repository = container.Resolve<IUserRepository>();


            var user = repository.GetUser(username);

            Assert.IsNotNull(user);
            Assert.AreEqual(username, user.UserName);
            Assert.AreEqual(1, user.Roles.Count);
            Assert.AreEqual("Employee", user.Roles.First());
            Assert.AreEqual("John Doe", user.FullName);
            Assert.AreEqual("ADATUM\\mary", user.Manager);
            Assert.AreEqual("31023", user.CostCenter);
        }

        [TestMethod]
        public void UpdatePreferredReimbursementMethodUsingBootstrapper()
        {
            string username = "ADATUM\\mary";
            var roles = new[] { "Employee" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);
            var repository = container.Resolve<IUserRepository>();

            var actual = repository.GetUser(username);
            actual.PreferredReimbursementMethod = ReimbursementMethod.Cash;

            repository.UpdateUserPreferredReimbursementMethod(actual);

            var updated = repository.GetUser(username);

            Assert.IsNotNull(updated);
            Assert.AreEqual(ReimbursementMethod.Cash, updated.PreferredReimbursementMethod);
        }

        [TestMethod]
        public void GetAllExpensesUsingBootstrapper()
        {
            var expected = new Model.Expense
            {
                Id = testGuid,
                Date = new DateTime(1900, 01, 01),
                Title = "Title",
                Description = "Description",
                Total = 1.0,
                ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                CostCenter = "CostCenter",
                Approved = true,
                User = new User { UserName = "user name" },
                ApproverName = "approver name"
            };
            DatabaseHelper.CreateExpense(TestDatabaseConnectionString, expected);

            var repository = container.Resolve<IExpenseRepository>();
            var expenses = repository.GetAllExpenses();

            Assert.IsNotNull(expenses);
            var actual = expenses.ToList().Where(e => e.Id == expected.Id).FirstOrDefault();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Total, actual.Total);
            Assert.AreEqual(expected.Approved, actual.Approved);
            Assert.AreEqual(expected.CostCenter, actual.CostCenter);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ReimbursementMethod, actual.ReimbursementMethod);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.User.UserName, actual.User.UserName);
        }


    }
}
