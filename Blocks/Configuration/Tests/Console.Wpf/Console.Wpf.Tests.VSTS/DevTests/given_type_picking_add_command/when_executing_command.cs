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
using System.Configuration;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_type_picking_add_command
{
    public abstract class SectionAndTestableTypePickingAddCommandContext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var section = new ConfigurationSectionWithANamedElementCollection();
            var sectionViewModel = SectionViewModel.CreateSection(base.Container, "sectionName", section);
            CollectionViewModel = sectionViewModel.DescendentElements().OfType<ElementCollectionViewModel>().Single();
            AssemblyDiscoveryServiceMock = new Mock<IAssemblyDiscoveryService>();
        }

        public ANamedElementWithTypeProperty AddedElement
        {
            get
            {
                return
                    CollectionViewModel.DescendentElements()
                        .Select(x => x.ConfigurationElement).OfType<ANamedElementWithTypeProperty>().Last();
            }
        }

        protected ElementCollectionViewModel CollectionViewModel { get; set; }
        protected Mock<IAssemblyDiscoveryService> AssemblyDiscoveryServiceMock { get; set; }

        public class TestableTypePickingCollectionElementAddCommand : TypePickingCollectionElementAddCommand
        {
            public static Type ExceptionTypeUsed = typeof(InvalidOperationException);

            public TestableTypePickingCollectionElementAddCommand(Type configurationElementType, ElementCollectionViewModel elementCollectionModel, IUIServiceWpf uiService, IAssemblyDiscoveryService discoveryService)
                : base(uiService, discoveryService, new TypePickingCommandAttribute(), new ConfigurationElementType(configurationElementType), elementCollectionModel)
            {
            }

            public TestableTypePickingCollectionElementAddCommand(Type configurationElementType, ElementCollectionViewModel elementCollectionModel, string propertyToSet, IUIServiceWpf uiService, IAssemblyDiscoveryService discoveryService)
                : base(uiService, discoveryService, new TypePickingCommandAttribute(propertyToSet), new ConfigurationElementType(configurationElementType), elementCollectionModel)
            {
            }

            public Type BaseTypeRequested { get; set; }

            public Type SelectedTypeRequested { get; set; }

            public TypeSelectorIncludes SelectedTypeIncludes { get; set; }

            protected override Type GetSelectedType(Type selectedType, Type baseType, TypeSelectorIncludes selectorIncludes, Type configurationType)
            {
                BaseTypeRequested = baseType;
                SelectedTypeRequested = selectedType;
                SelectedTypeIncludes = selectorIncludes;
                return ExceptionTypeUsed;
            }
        }

        public class ConfigurationSectionWithANamedElementCollection : ConfigurationSection
        {
            const string elementPropertyCollection = "elementPropertyCollection";

            public ConfigurationSectionWithANamedElementCollection()
            {
                this[elementPropertyCollection] = new NamedElementCollection<ANamedElementWithTypeProperty>();
            }

            [ConfigurationProperty(elementPropertyCollection)]
            [ConfigurationCollection(typeof(ANamedElementWithTypeProperty))]
            public NamedElementCollection<ANamedElementWithTypeProperty> Children
            {
                get
                {
                    return (NamedElementCollection<ANamedElementWithTypeProperty>)this[elementPropertyCollection];
                }
            }

        }

        public class ANamedElementWithTypeProperty : NamedConfigurationElement
        {
            private const string typeNameProperty = "typeName";
            private const string someOtherTypeNameProperty = "someOtherTypeName";

            public ANamedElementWithTypeProperty()
            {
                this[typeNameProperty] = null;
                this[someOtherTypeNameProperty] = null;
            }

            [ConfigurationProperty(typeNameProperty)]
            [BaseType(typeof(ArgumentException), TypeSelectorIncludes.NonpublicTypes)]
            public string TypeName
            {
                get { return (string)this[typeNameProperty]; }
                set { this[typeNameProperty] = value; }
            }

            [ConfigurationProperty(someOtherTypeNameProperty)]
            public string SomeOtherTypeName
            {
                get { return (string)this[someOtherTypeNameProperty]; }
                set { this[someOtherTypeNameProperty] = value; }
            }
        }
    }

    [TestClass]
    public class when_executing_with_default_property_names : SectionAndTestableTypePickingAddCommandContext
    {
        private TestableTypePickingCollectionElementAddCommand addCommand;

        protected override void Arrange()
        {
            base.Arrange();

            addCommand =
                new TestableTypePickingCollectionElementAddCommand(typeof(ANamedElementWithTypeProperty),
                                                                   CollectionViewModel, 
                                                                   UIServiceMock.Object,
                                                                   AssemblyDiscoveryServiceMock.Object);
        }

        protected override void Act()
        {
            addCommand.Execute(null);
        }

        [TestMethod]
        public void then_name_is_set_to_type_retrieved()
        {
            Assert.AreEqual(TestableTypePickingCollectionElementAddCommand.ExceptionTypeUsed.Name, AddedElement.Name);
        }

        [TestMethod]
        public void then_the_properties_of_the_added_element_are_shown()
        {
            var element = CollectionViewModel.ChildElements.First();

            Assert.IsTrue(element.PropertiesShown);
        }

        [TestMethod]
        public void then_name_is_unique_when_adding_same_type_twice()
        {
            var firstElementName = AddedElement.Name;
            addCommand.Execute(null);
            var otherElementName = AddedElement.Name;

            Assert.AreNotEqual(firstElementName, otherElementName);
        }

        [TestMethod]
        public void then_type_property_is_set_to_selected_type()
        {
            Assert.AreEqual(TestableTypePickingCollectionElementAddCommand.ExceptionTypeUsed.AssemblyQualifiedName, AddedElement.TypeName);
        }

        [TestMethod]
        public void then_type_picker_invoked_with_base_type_from_attribute()
        {
            Assert.AreEqual(typeof(ArgumentException), addCommand.BaseTypeRequested);
        }

        [TestMethod]
        public void then_selected_type_starts_with_base_type()
        {
            Assert.AreEqual(typeof(ArgumentException), addCommand.SelectedTypeRequested);
        }

        [TestMethod]
        public void then_selector_includes_provided_from_attribute()
        {
            Assert.AreEqual(TypeSelectorIncludes.NonpublicTypes, addCommand.SelectedTypeIncludes);
        }
    }

    [TestClass]
    public class when_executing_against_specified_property : SectionAndTestableTypePickingAddCommandContext
    {
        private TestableTypePickingCollectionElementAddCommand addCommand;

        protected override void Arrange()
        {
            base.Arrange();
            addCommand = new TestableTypePickingCollectionElementAddCommand(typeof(ANamedElementWithTypeProperty),
                                                                            CollectionViewModel,
                                                                            "SomeOtherTypeName",
                                                                            UIServiceMock.Object,
                                                                            AssemblyDiscoveryServiceMock.Object);
        }

        protected override void Act()
        {
            addCommand.Execute(null);
        }

        [TestMethod]
        public void then_specified_property_is_set_with_exception_type()
        {
            Assert.AreEqual(TestableTypePickingCollectionElementAddCommand.ExceptionTypeUsed.AssemblyQualifiedName, AddedElement.SomeOtherTypeName);
        }

        [TestMethod]
        public void then_property_named_type_is_not_set()
        {
            Assert.IsNull(AddedElement.TypeName);
        }
    }
}
