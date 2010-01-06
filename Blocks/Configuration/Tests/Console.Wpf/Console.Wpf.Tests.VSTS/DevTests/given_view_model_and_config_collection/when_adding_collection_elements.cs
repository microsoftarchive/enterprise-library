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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    [TestClass]
    public class when_adding_collection_elements : SectionWithMultipleChildrenContext
    {
        private ElementViewModel newCollectionItem;

        protected override void Act()
        {
            var collectionViewModel =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(NamedElementCollection<TestHandlerData>))
                    .OfType<ElementCollectionViewModel>()
                    .First();

            newCollectionItem = collectionViewModel.AddNewCollectionElement(typeof (TestHandlerData));
        }

        [TestMethod]
        public void then_new_items_bindable_property_initialized()
        {
            Assert.IsFalse(string.IsNullOrEmpty(newCollectionItem.Property("Name").BindableProperty.BindableValue));
        }
     
    }

    [TestClass]
    public class when_adding_collection_elements_via_add_commands : SectionWithMultipleChildrenContext
    {
        private bool descendentElementsChangedFired;
        private ElementViewModel addedElement;

        protected override void Arrange()
        {
            base.Arrange();

            ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            var collectionViewModel =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(NamedElementCollection<TestHandlerData>))
                    .OfType<ElementCollectionViewModel>()
                    .First();

            var command = collectionViewModel.Commands.OfType<DefaultCollectionElementAddCommand>().Where(c => c.ConfigurationElementType == typeof(TestHandlerData)).First();
            command.Execute(null);
            addedElement = collectionViewModel.ChildElements.Last();
        }

        [TestMethod]
        public void then_section_raises_change_notification()
        {
            Assert.IsTrue(descendentElementsChangedFired);
        }

        [TestMethod]
        public void then_new_element_is_selected()
        {
            Assert.IsTrue(addedElement.IsSelected);
        }

        [TestMethod]
        public void then_new_element_shows_properties()
        {
            Assert.IsTrue(addedElement.PropertiesShown);
        }
    }

    [TestClass]
    public class when_executing_adder_on_middle_nodes : ContainerContext
    {
        private ElementCollectionViewModel childrenCollection;
        private int startingCount;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithSingleChild();

            section.Children.Add(CreateNewElement("One"));
            section.Children.Add(CreateNewElement("Two"));

            ViewModel = SectionViewModel.CreateSection(Container, "mock section", section);

            childrenCollection = ViewModel.DescendentElements(
                      e => e.ConfigurationType ==
                            typeof(NamedElementCollection<TestHandlerDataWithChildren>)).Cast<ElementCollectionViewModel>().First();

            startingCount =
                childrenCollection.ChildElements.Count(
                    x => typeof(TestHandlerDataWithChildren).IsAssignableFrom(x.ConfigurationType));
        }

        private static TestHandlerDataWithChildren CreateNewElement(string name)
        {
            var element = new TestHandlerDataWithChildren() { Name = name };
            element.Children.Add(new TestHandlerData() { Name = name + ".One" });
            element.Children.Add(new TestHandlerData() { Name = name + ".Two" });
            return element;
        }

        protected override void Act()
        {
            var command = childrenCollection.Commands
                            .OfType<DefaultCollectionElementAddCommand>()
                            .Where(x => x.ConfigurationElementType == typeof(TestHandlerDataWithChildren))
                            .First();
            command.Execute(null);
        }

        public SectionViewModel ViewModel { get; set; }
        
        [TestMethod]
        public void then_new_child_is_added_to_collection()
        {
            Assert.AreEqual(startingCount + 1,
                            childrenCollection.ChildElements.Count(
                                x => typeof(TestHandlerDataWithChildren).IsAssignableFrom(x.ConfigurationType)));
        }
    }
}
