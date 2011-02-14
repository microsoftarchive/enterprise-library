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
using Console.Wpf.Tests.VSTS.BlockSpecific.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_adding_unity_alias_to_configuration_source_model : given_unity_section
    {
        ElementCollectionViewModel aliasesCollection;
        ElementViewModel addedAlias;

        protected override void Arrange()
        {
            base.Arrange();
            aliasesCollection = (ElementCollectionViewModel)base.UnitySectionViewModel.ChildElement("TypeAliases");
            Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
        }

        protected override void Act()
        {
            addedAlias = aliasesCollection.AddNewCollectionElement(typeof(AliasElement));
        }

        [TestMethod]
        public void then_add_command_displays_type_picker()
        {
            Assert.IsInstanceOfType(aliasesCollection.AddCommands.First(), typeof(TypePickingCollectionElementAddCommand));
        }

        [TestMethod]
        public void then_aliases_collection_contains_added_alias()
        {
            Assert.AreEqual(1, aliasesCollection.ChildElements.Count());
        }

        [TestMethod]
        public void then_can_access_command_collection()
        {
            Assert.AreNotEqual(0, addedAlias.Commands.Where(x => x.CanExecute(null)).Count());
        }

        [TestMethod]
        public void then_alias_property_has_default_value()
        {
            Assert.IsNotNull(addedAlias.Property("Alias").Value);
        }

        [TestMethod]
        public void then_type_name_property_has_type_picker_editor()
        {
            Assert.IsNotNull(addedAlias.Property("TypeName").BindableProperty as PopupEditorBindableProperty);
        }
    }
}
