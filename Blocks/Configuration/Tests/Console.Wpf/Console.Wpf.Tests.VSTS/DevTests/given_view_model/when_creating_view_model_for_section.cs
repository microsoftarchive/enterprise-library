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
using System.Configuration;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_creating_view_model_for_section : ExceptionHandlingSettingsContext
    {
        SectionViewModel viewModel;

        protected override void Arrange()
        {
            base.Arrange();
            this.Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
        }

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);
        }

        [TestMethod]
        public void then_policy_collection_can_be_found_in_section_children()
        {
            var policyCollections = viewModel.ChildElements.Where(x => x.ConfigurationType == typeof(NamedElementCollection<ExceptionPolicyData>));
            Assert.IsTrue(policyCollections.Any());
        }

        [TestMethod]
        public void then_policies_can_be_found_in_view_model()
        {
            var exceptionPolicies = viewModel.DescendentElements(x => x.ConfigurationType == typeof(ExceptionPolicyData));
            Assert.IsNotNull(exceptionPolicies);
            Assert.IsTrue(exceptionPolicies.Any());
        }

        [TestMethod]
        public void then_all_descendents_of_section_have_parent_element()
        {
            var elementsWithoutParents = viewModel.DescendentElements().Where(x => x.ParentElement == null);
            Assert.IsFalse(elementsWithoutParents.Any());
        }

        [TestMethod]
        public void then_exception_policy_collection_has_neat_name()
        {
            var policyCollection = viewModel.ChildElements.Where(x => x.ConfigurationType == typeof(NamedElementCollection<ExceptionPolicyData>)).First();
            Assert.AreEqual("Policies", policyCollection.Name);
        }

        [TestMethod]
        public void then_exception_policy_has_name_from_nameproperty()
        {
            var policies = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData));
            Assert.IsTrue(policies.Where(x => x.Name == "Global Policy").Any());
        }

        [TestMethod]
        public void then_exception_policy_has_name_property()
        {
            var anyPolicy = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData)).First();
            Assert.IsTrue(anyPolicy.Properties.Where(x => x.PropertyName == "Name").Any());
        }

        [TestMethod]
        public void then_exception_policy_has_no_exception_types_property()
        {
            var anyPolicy = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData)).First();
            Assert.IsFalse(anyPolicy.Properties.Where(x => x.PropertyName == "ExceptionTypes").Any());
        }

        [TestMethod]
        public void then_exception_policy_collection_has_element_type()
        {
            ElementCollectionViewModel policyCollection = (ElementCollectionViewModel)viewModel.ChildElements.Where(x => x.ConfigurationType == typeof(NamedElementCollection<ExceptionPolicyData>)).First();
            Assert.AreEqual(typeof(ExceptionPolicyData), policyCollection.CollectionElementType);
        }

        [TestMethod]
        public void then_exception_policy_collection_contains_adders()
        {
            ElementCollectionViewModel policyCollection = (ElementCollectionViewModel)viewModel.ChildElements.Where(x => x.ConfigurationType == typeof(NamedElementCollection<ExceptionPolicyData>)).First();
            Assert.IsTrue(policyCollection.Commands.OfType<DefaultCollectionElementAddCommand>().Any(a => a.Title == "Add Policy"));
        }

        [TestMethod]
        public void then_handlers_collection_contains_adders()
        {
            var handlerCollection =
                viewModel.DescendentElements().OfType<ElementCollectionViewModel>()
                    .Where(
                    x => typeof(NamedElementCollection<ExceptionHandlerData>).IsAssignableFrom(x.ConfigurationType)).
                    First();

            var adders =
                handlerCollection.Commands.OfType<DefaultElementCollectionAddCommand>()
                .SelectMany(x => x.ChildCommands);
            Assert.IsNotNull(adders.Single(a => a.Title == "Add Wrap Handler"));
            Assert.IsNotNull(adders.Single(a => a.Title == "Add Replace Handler"));
            Assert.IsNotNull(adders.Single(a => a.Title == "Add Custom Exception Handler"));
            Assert.IsFalse(adders.Any(a => a.Title == "Add ExceptionHandlerData"));
        }


        [TestMethod]
        public void when_getting_same_child_element_twice_returns_same_instance()
        {
            var firstChild = viewModel.ChildElements.First();
            var firstChild2 = viewModel.ChildElements.First();

            Assert.AreSame(firstChild, firstChild2);
        }


        [TestMethod]
        public void when_getting_same_property_twice_returns_same_instance()
        {
            var anyPolicy = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData)).First();
            var nameProperty = anyPolicy.Property("Name");
            var nameProperty2 = anyPolicy.Property("Name");

            Assert.AreSame(nameProperty, nameProperty2);
        }


        [TestMethod]
        public void when_changing_name_property_on_named_element_name_property_changes()
        {
            var anyPolicy = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData)).First();
            var nameProperty = anyPolicy.Property("Name");

            string name = anyPolicy.Name;

            using (PropertyChangedListener listener = new PropertyChangedListener(anyPolicy))
            {
                nameProperty.Value = "new value";
                Assert.IsTrue(listener.ChangedProperties.Contains("Name"));
            }
        }

        [TestMethod]
        public void when_getting_path_it_is_unique()
        {
            List<string> paths = new List<string>();
            foreach (var x in viewModel.DescendentElements())
            {
                if (paths.Contains(x.Path)) Assert.Fail();
                paths.Add(x.Path);
            }
        }

        [TestMethod]
        public void when_changing_parent_path_path_changes()
        {
            var anyPolicy = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData)).First();
            var handlerWithinPolicy = anyPolicy.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();

            string name = anyPolicy.Name;
            using (var propListener = new PropertyChangedListener(handlerWithinPolicy))
            {
                anyPolicy.Property("Name").Value = "new name";

                Assert.IsTrue(propListener.ChangedProperties.Contains("Path"));
            }
        }

        [TestMethod]
        public void when_changing_name_path_changes()
        {
            var anyPolicy = viewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(ExceptionPolicyData)).First();

            string name = anyPolicy.Name;
            using (var propListener = new PropertyChangedListener(anyPolicy))
            {
                anyPolicy.Property("Name").Value = "new name";

                Assert.IsTrue(propListener.ChangedProperties.Contains("Path"));
            }
        }
    }

    [TestClass]
    public class when_creating_connection_strings_collection : ContainerContext
    {
        ConnectionStringsSection section;
        SectionViewModel connectionStringsModel;

        protected override void Arrange()
        {
            base.Arrange();

            section = new ConnectionStringsSection();
            section.ConnectionStrings.Add(new ConnectionStringSettings("name", "conn1"));
            section.ConnectionStrings.Add(new ConnectionStringSettings("name2", "conn2"));
        }

        protected override void Act()
        {
            connectionStringsModel = SectionViewModel.CreateSection(Container, "connectionStrings", section);
            connectionStringsModel.Initialize(new InitializeContext(null));
        }

        [TestMethod]
        public void then_configuration_elements_collection_has_element_type()
        {
            ElementCollectionViewModel connectionStringsCollection = (ElementCollectionViewModel)
                connectionStringsModel.DescendentElements(x => x.ConfigurationType == typeof(ConnectionStringSettingsCollection)).First();

            Assert.IsNotNull(connectionStringsCollection);
            Assert.AreEqual(typeof(ConnectionStringSettings), connectionStringsCollection.CollectionElementType);
        }
    }

    [TestClass]
    public class when_executing_adder_on_leaf_nodes : ExceptionHandlingSettingsContext
    {
        private ElementCollectionViewModel handlerCollection;
        private int startingCount;


        protected override void Arrange()
        {
            base.Arrange();

            var viewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);

            handlerCollection = viewModel.DescendentElements(
                      e => e.ConfigurationType ==
                            typeof(NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData>)).Cast<ElementCollectionViewModel>().First();

            startingCount =
                handlerCollection.ChildElements.Count(
                    x => typeof(WrapHandlerData).IsAssignableFrom(x.ConfigurationType));

            this.Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
        }

        protected override void Act()
        {
            var command =
                handlerCollection.Commands.OfType<DefaultElementCollectionAddCommand>().SelectMany(a => a.ChildCommands)
                    .Where(c => c.Title == "Add Wrap Handler").First();

            command.Execute(null);
        }

        [TestMethod]
        public void then_new_child_is_added_to_collection()
        {
            Assert.AreEqual(startingCount + 1,
                            handlerCollection.ChildElements.Count(
                                x => typeof(WrapHandlerData).IsAssignableFrom(x.ConfigurationType)));
        }

        [TestMethod]
        public void then_new_child_has_expected_name()
        {
            Assert.IsTrue(handlerCollection.ChildElements.Any(x => x.Name == "Wrap Handler"));
        }
    }
}
