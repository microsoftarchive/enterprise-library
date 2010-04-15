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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using System.Configuration;
using Moq;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_with_containedelementproperty
{
    [TestClass]
    public class when_querying_associated_element_data : ContainerContext
    {
        private SectionViewModel sectionViewModel;
        private Property property;

        protected override void Arrange()
        {
            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new SectionWithCollectionProperties()
                              {
                                  Children = {new ChildConfigurationElement() {Name = "AChild"}}
                              };


            sectionViewModel = SectionViewModel.CreateSection(Container, "testSection", section);
            
        }

        protected override void Act()
        {
            
            property = sectionViewModel.DescendentConfigurationsOfType<ChildConfigurationElement>().First().Property(
                "SomeStringProperty");

        }

        [TestMethod]
        public void then_associated_element_returns_collections_parent()
        {
            Assert.AreSame(sectionViewModel, ((ILogicalPropertyContainerElement) property).ContainingElement);
        }

        [TestMethod]
        public void then_associated_element_display_name_includes_grandparent_and_parent()
        {
            var displayName = ((ILogicalPropertyContainerElement) property).ContainingElementDisplayName;
            var collectionModel = sectionViewModel.DescendentConfigurationsOfType<NamedElementCollection<ChildConfigurationElement>>().First();
            Assert.AreEqual(string.Format("{0}.{1}", sectionViewModel.Name, collectionModel.Name), displayName);
        }
    }


    public class SectionWithCollectionProperties : ConfigurationSection
    {
        private const string childrenProperty = "childrenProperty";

        public SectionWithCollectionProperties()
        {
            Children = new NamedElementCollection<ChildConfigurationElement>();
        }

        [ConfigurationProperty(childrenProperty)]
        [ConfigurationCollection(typeof(ChildConfigurationElement))]
        public NamedElementCollection<ChildConfigurationElement> Children
        {
            get { return (NamedElementCollection<ChildConfigurationElement>)this[childrenProperty]; }
            private set { this[childrenProperty] = value;}
        }
    }

    public class ChildConfigurationElement : NamedConfigurationElement
    {
        private const string someStringProperty = "someStringProperty";

        [ConfigurationProperty(someStringProperty)]
        [ViewModel(typeof(CollectionEditorContainedElementProperty))]
        public string SomeStringProperty 
        { 
            get { return (string) this[someStringProperty];}
            set { this[someStringProperty] = value;}
        }

    }
}
