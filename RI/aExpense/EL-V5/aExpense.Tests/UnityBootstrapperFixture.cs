// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using AExpense.DataAccessLayer;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class UnityBootstrapperFixture
    {
        private UnityContainer container;

        [TestInitialize]
        public void Arrange()
        {
            container = new UnityContainer();
            ContainerBootstrapper.Configure(container);
        }

        [TestCleanup]
        public void Cleanup()
        {
            container.Dispose();
        }

        [TestMethod]
        public void BootstrapperRegistersDatabase()
        {
            var database = container.Registrations.FirstOrDefault(e => e.Name == "aExpense");

            Assert.IsNotNull(database);
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
    }
}
