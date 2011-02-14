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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
namespace Console.Wpf.Tests.VSTS.BlockSpecific.Unity
{
    [TestClass]
    public class when_adding_unity_container_to_configuration_source_model : given_unity_application_model_with_unity_section
    {
        ElementCollectionViewModel containersCollection;
        ElementViewModel addedContainer;

        protected override void Arrange()
        {
            base.Arrange();
            containersCollection = (ElementCollectionViewModel)base.UnitySectionViewModel.ChildElement("Containers");
            
        }

        protected override void Act()
        {
            containersCollection.AddCommands.First().Execute(null);
            addedContainer = containersCollection.ChildElements.First();
        }

        [TestMethod]
        public void then_containers_collection_contains_added_container()
        {
            Assert.AreEqual(1, containersCollection.ChildElements.Count);
        }

        [TestMethod]
        public void then_container_element_is_displayed_as_hierarchical()
        {
            var addedContainerViewModel = containersCollection.ChildElements.First();

            Assert.IsInstanceOfType(addedContainerViewModel.Bindable, typeof(HierarchicalViewModel));
        }

        [TestMethod]
        public void then_container_element_has_name()
        {
            Assert.IsFalse(string.IsNullOrEmpty((string)addedContainer.Property("Name").Value));
        }

        [TestMethod]
        public void then_container_element_contains_add_command_for_registrations()
        {
            Assert.IsTrue(addedContainer.AddCommands.Any());
        }

        [TestMethod]
        public void then_container_element_has_instances_property()
        {
            Assert.IsNotNull(addedContainer.Property("Instances"));
        }

        [TestMethod]
        [Ignore]
        public void then_container_element_add_command_for_registrations_has_type_picker()
        {
            var addCommandForRegistrations = addedContainer.AddCommands.First();
            Assert.IsInstanceOfType(addCommandForRegistrations, typeof(TypePickingCollectionElementAddCommand));
        }

    }
}
