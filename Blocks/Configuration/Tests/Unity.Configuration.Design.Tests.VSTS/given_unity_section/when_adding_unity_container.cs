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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Unity
{
    [TestClass]
    public class when_adding_unity_container_to_configuration_source_model : given_unity_section
    {
        ElementCollectionViewModel containersCollection;
        ElementViewModel addedContainer;

        protected override void Arrange()
        {
            base.Arrange();
            containersCollection = (ElementCollectionViewModel)base.UnitySectionViewModel.ChildElement("Containers");
            Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
        }

        protected override void Act()
        {
            containersCollection.AddCommands.First().Execute(null);
            addedContainer = containersCollection.ChildElements.First();
        }

        [TestMethod]
        public void then_containers_collection_contains_added_container()
        {
            Assert.AreEqual(1, containersCollection.ChildElements.Count());
        }

        [TestMethod]
        public void then_container_element_contains_add_command_for_registrations()
        {
            Assert.IsTrue(addedContainer.AddCommands.Any());
        }

        [TestMethod]
        public void then_container_element_add_command_for_registrations_has_type_picker()
        {
            Assert.AreEqual(1, addedContainer.AddCommands.SelectMany(x => x.ChildCommands == null ? new CommandModel[0] : x.ChildCommands).OfType<TypePickingCollectionElementAddCommand>().Count());
        }
    }
}
