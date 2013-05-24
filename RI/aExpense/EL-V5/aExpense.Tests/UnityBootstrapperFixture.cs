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
