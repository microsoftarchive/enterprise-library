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
using AExpense.DataAccessLayer;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class ContainerBootstrapperFixture
    {
        private UnityContainer container;

        [TestInitialize]
        public void Arrange()
        {
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(container);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.container.Dispose();
        }

        [TestMethod]
        public void BootstrapperRegistersDatabase()
        {
            Assert.IsTrue(container.Registrations.Any(e => e.RegisteredType.Name == "Database"));
        }

        [TestMethod]
        public void BootstrapperRegistersExpenseRepository()
        {
            var repository = container.Resolve<IExpenseRepository>();

            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void BootstrapperRegistersProfileStore()
        {
            var profileStore = container.Resolve<IProfileStore>();

            Assert.IsNotNull(profileStore);
        }

        [TestMethod]
        public void BootstrapperRegistersUserRepository()
        {

            var repository = container.Resolve<IUserRepository>();

            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void BootstrapperRegistersExpense()
        {
            var expense = container.Resolve<Model.Expense>();

            Assert.IsNotNull(expense);
        }
    }
}
