//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Linq;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Cryptography.given_cryptography_settings
{
    [TestClass]
    public class when_creating_add_command_for_keyed_hash_provider_with_child_commands : given_crypto_configuration
    {
        private IEnumerable<ElementViewModel> hashProviderCollection;
        private CommandModel command;
        private Mock<IAssemblyDiscoveryService> discoveryService;

        protected override void Arrange()
        {
            base.Arrange();

            hashProviderCollection = base.CryptographyModel.GetDescendentsOfType<NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData>>();
            this.discoveryService = new Mock<IAssemblyDiscoveryService>();
            this.Container.RegisterInstance(this.discoveryService.Object);
        }

        protected override void Act()
        {
            command = hashProviderCollection.First().Commands.First();
        }

        [TestMethod]
        public void then_child_commmand_is_not_returned()
        {
            int commandCount = command.ChildCommands.Count();
            Assert.AreEqual(2, commandCount);
        }
    }
}
