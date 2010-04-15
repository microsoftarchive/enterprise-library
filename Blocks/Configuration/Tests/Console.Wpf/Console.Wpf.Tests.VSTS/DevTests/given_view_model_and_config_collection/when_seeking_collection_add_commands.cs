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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    [TestClass]
    public class when_creating_polymorphic_add_command : ContainerContext
    {
        private ElementCollectionViewModel polymorphicCollection;
        private CommandModel commandContainer;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new Mocks.MockSectionWithMultipleChildCollections();
            var sectionViewModel = SectionViewModel.CreateSection(Container, "mockSection", section);
            polymorphicCollection = sectionViewModel.ChildElements
                .OfType<ElementCollectionViewModel>()
                .Where(
                c =>
                typeof(NameTypeConfigurationElementCollection<TestHandlerData, CustomTestHandlerData>).IsAssignableFrom
                    (c.ConfigurationType)).First();

        }
        protected override void Act()
        {
            var mockUIService = new Mock<IUIServiceWpf>();
            commandContainer = new DefaultElementCollectionAddCommand(polymorphicCollection, mockUIService.Object);
        }

        [TestMethod]
        public void then_has_children()
        {
            Assert.IsTrue(commandContainer.ChildCommands.Any());
        }

        [TestMethod]
        public void then_derived_types_add_children_command_are_provided()
        {
            var collectionAddCommandTitles =
                commandContainer.ChildCommands.OfType<DefaultCollectionElementAddCommand>().Select(c => c.Title);

            var expectedCommandTitles =
                new[] { typeof(TestHandlerAnotherDerivedData).Name, typeof(TestHandlerSonOfDerivedData).Name }.Select(
                    s => "Add " + s);

            CollectionAssert.IsSubsetOf(expectedCommandTitles.ToArray(),
                                        collectionAddCommandTitles.ToArray());
        }

        [TestMethod]
        public void then_child_commands_includes_custom_data_add_command()
        {
            var collectionAddCommandTitles =
                commandContainer.ChildCommands.OfType<DefaultCollectionElementAddCommand>().Select(c => c.Title);

            CollectionAssert.Contains(collectionAddCommandTitles.ToArray(), "Add " + typeof(CustomTestHandlerData).Name);
        }

        [TestMethod]
        public void then_child_commands_do_not_include_non_referenced_derived_types()
        {
            var collectionAddCommandTitles =
                commandContainer.ChildCommands.OfType<DefaultCollectionElementAddCommand>().Select(c => c.Title);

            CollectionAssert.DoesNotContain(collectionAddCommandTitles.ToArray(), "Add " + typeof(TestHandlerData).Name);
            CollectionAssert.DoesNotContain(collectionAddCommandTitles.ToArray(), "Add " + typeof(TestHandlerDerivedData).Name);
        }

        [TestMethod]
        public void then_derived_data_with_custom_element_add_command_provides_custom_commands()
        {
            Assert.IsTrue(commandContainer.ChildCommands.OfType<CustomElementCollectionAddCommand>().Any());
        }

        [TestMethod]
        public void then_dervied_data_with_multiple_add_command_attributes_produce_multiple_commands()
        {
            Assert.AreEqual(1, commandContainer.ChildCommands.OfType<CustomElementCollectionAddCommand>().Count());
            Assert.AreEqual(1, commandContainer.ChildCommands.OfType<AnotherCustomElementCollectionAddCommand>().Count());
        }

        [TestMethod]
        public void then_add_child_commands_are_ordered_alphabetically()
        {
            var childCommandTitles = commandContainer.ChildCommands.Select(x => x.Title).ToArray();
            var sortedChildCommandTitles = childCommandTitles.OrderBy(x => x).ToArray();

            CollectionAssert.AreEqual(sortedChildCommandTitles, childCommandTitles);
        }
    }


    [TestClass]
    public class when_creating_single_type_add_commands : ContainerContext
    {
        private ElementCollectionViewModel singleTypeCollection;
        private DefaultElementCollectionAddCommand addCommand;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new Mocks.MockSectionWithMultipleChildCollections();
            var sectionViewModel = SectionViewModel.CreateSection(Container, "mock section", section);
            singleTypeCollection = sectionViewModel.ChildElements
                .OfType<ElementCollectionViewModel>()
                .Where(
                c =>
                typeof(NamedElementCollection<TestHandlerData>).IsAssignableFrom
                    (c.ConfigurationType)).First();

        }

        protected override void Act()
        {
            var uiService = new Mock<IUIServiceWpf>();

            addCommand = new DefaultElementCollectionAddCommand(singleTypeCollection, uiService.Object);
        }

        [TestMethod]
        public void then_add_command_contains_single_add()
        {
            Assert.AreEqual(1, addCommand.ChildCommands.Count());
        }

        [TestMethod]
        public void then_command_title_matches_element_displayname()
        {
            Assert.AreEqual("Add " + typeof(TestHandlerData).Name, addCommand.ChildCommands.ElementAt(0).Title);
        }
    }

    [TestClass]
    public class when_creating_single_type_add_commands_with_attributed_elements : ContainerContext
    {
        private ElementCollectionViewModel singleTypeCollection;
        private DefaultElementCollectionAddCommand addCommand;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new Mocks.MockSectionWithMultipleChildCollections();
            var sectionViewModel = SectionViewModel.CreateSection(Container, "mock section", section);
            singleTypeCollection = sectionViewModel.ChildElements
                .OfType<ElementCollectionViewModel>()
                .Where(
                c =>
                typeof(NamedElementCollection<TestHandlerDataWithChildren>).IsAssignableFrom
                    (c.ConfigurationType)).First();

        }

        protected override void Act()
        {
            var mockUIService = new Mock<IUIServiceWpf>();
            addCommand = new DefaultElementCollectionAddCommand(singleTypeCollection, mockUIService.Object);
        }

        [TestMethod]
        public void then_add_command_contains_single_add()
        {
            Assert.AreEqual(1, addCommand.ChildCommands.Count());
        }

        [TestMethod]
        public void then_command_is_custom_command_type()
        {
            Assert.IsTrue(addCommand.ChildCommands.OfType<CustomElementCollectionAddCommand>().Any());
        }
    }
}
