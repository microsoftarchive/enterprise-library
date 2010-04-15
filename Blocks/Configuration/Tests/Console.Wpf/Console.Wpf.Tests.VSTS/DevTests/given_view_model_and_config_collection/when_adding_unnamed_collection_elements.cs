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
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    [TestClass]
    public class when_adding_configurationelements_to_configurationelementcollections : ContainerContext
    {
        private SectionViewModel ViewModel;
        private ElementCollectionViewModel collection;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithUnnamedCollection();
            this.ViewModel = SectionViewModel.CreateSection(Container, "mockSection", section);
        }

        protected override void Act()
        {
            collection =
                ViewModel.GetDescendentsOfType<UnnamedChildCollection>().OfType<ElementCollectionViewModel>().First();
            collection.AddNewCollectionElement(typeof (UnnamedChild));
        }

        [TestMethod]
        public void then_new_child_is_added_to_underlying_collection()
        {
            var configurationCollection = (ConfigurationElementCollection) collection.ConfigurationElement;
            Assert.AreEqual(1, configurationCollection.Count);            
        }

        [TestMethod]
        public void then_new_child_is_in_viewmodel_collection()
        {
            var childElements = collection.ChildElements.Where(x => typeof (UnnamedChild).IsAssignableFrom(x.ConfigurationType));
            Assert.AreEqual(1, childElements.Count());
        }
    }

    [TestClass]
    public class when_adding_named_configelements_to_configurationelementcollections : ContainerContext
    {
        private SectionViewModel ViewModel;
        private ElementCollectionViewModel collection;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithSingleChild();
            this.ViewModel = SectionViewModel.CreateSection(Container, "mock section", section);
        }

        protected override void Act()
        {
            collection = ViewModel.ChildElements.Where(x => x.ConfigurationType == typeof(NamedElementCollection<TestHandlerDataWithChildren>)).OfType<ElementCollectionViewModel>().First();
            collection.AddNewCollectionElement(typeof(TestHandlerDataWithChildren));
        }

        [TestMethod]
        public void then_name_is_assigned_initial_value()
        {
            Assert.AreEqual(TestHandlerDataWithChildren.DisplayName,collection.ChildElements.ElementAt(0).Name);
        }
    }
}
