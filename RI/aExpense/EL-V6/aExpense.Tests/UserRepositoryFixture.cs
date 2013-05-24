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

using System.Linq;
using System.Security.Principal;
using System.Threading;
using AExpense.DataAccessLayer;
using AExpense.Model;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class UserRepositoryFixture
    {
        private IProfileStore ldapStore;
        private ExceptionManager exceptionMgr; 
        private IUnityContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(container);

            this.ldapStore = container.Resolve<IProfileStore>();
            this.exceptionMgr = container.Resolve<ExceptionManager>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            container.Dispose();
        }

        [TestMethod]
        public void GetUser()
        {
            string username = "ADATUM\\johndoe";
            var roles = new[] { "Employee" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);

            var repository = new UserRepository(this.ldapStore, this.exceptionMgr, this.container);
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
        public void UpdatePreferredReimbursementMethod()
        {
            string username = "ADATUM\\mary";
            var roles = new[] { "UpdatePreferredReimbursementMethod_Role" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);
            var repository = new UserRepository(this.ldapStore, this.exceptionMgr, this.container);

            var actual = repository.GetUser(username);
            actual.PreferredReimbursementMethod = ReimbursementMethod.Cash;

            repository.UpdateUserPreferredReimbursementMethod(actual);

            var updated = repository.GetUser(username);

            Assert.IsNotNull(updated);
            Assert.AreEqual(ReimbursementMethod.Cash, updated.PreferredReimbursementMethod);
        }
    }
}