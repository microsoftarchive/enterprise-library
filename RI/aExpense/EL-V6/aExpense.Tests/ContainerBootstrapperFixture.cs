// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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
