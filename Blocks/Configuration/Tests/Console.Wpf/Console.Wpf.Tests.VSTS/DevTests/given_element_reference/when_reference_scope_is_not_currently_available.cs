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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    [TestClass]
    public class when_reference_scope_becomes_available : ContainerContext
    {
        private TestReferencingSection testreferencingSection;
        private SectionViewModel section;
        private SectionViewModel newSection;
        private ConfigurationSourceModel configSource;

        protected override void Arrange()
        {
            base.Arrange();
            testreferencingSection = new TestReferencingSection();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new string[] {"testReferencingSection"});
            Container.RegisterInstance(locator.Object);

            DesignDictionaryConfigurationSource source = new DesignDictionaryConfigurationSource();
            source.Add("testReferencingSection", testreferencingSection);

            configSource = Container.Resolve<ConfigurationSourceModel>();
            configSource.Load(source);

            section = configSource.Sections.Where(x => x.SectionName == "testReferencingSection").Single();
            section.Property("ReferencingProperty").Value = "ReferringTo";
        }

        protected override void Act()
        {
            var referencedSection = new TestReferencedSection();

            configSource.AddSection("referencedSection", referencedSection);
            newSection = configSource.Sections.Where(x => x.SectionName == "referencedSection").Single();

            var referencedItems = newSection
               .GetDescendentsOfType<NamedElementCollection<ReferencedItemType>>()
               .OfType<ElementCollectionViewModel>().First();

            var newItem = referencedItems.AddNewCollectionElement(typeof(ReferencedItemType));
            newItem.Property("Name").Value = "ReferringTo";
        }

        [TestMethod]
        public void then_scope_can_resolve()
        {
            Assert.AreEqual("ReferringTo", section.Property("ReferencingProperty").Value);
        }
    }

    public class TestReferencingSection : ConfigurationSection
    {
        private const string referencingProperty = "referencingProperty";

        [ConfigurationProperty(referencingProperty)]
        [Reference(typeof(NamedElementCollection<ReferencedItemType>), typeof(ReferencedItemType))]
        public string ReferencingProperty
        {
            get { return (string)this[referencingProperty]; }
            set { this[referencingProperty] = value; }
        }
    }

    public class TestReferencedSection : ConfigurationSection
    {
        protected const string referencedItemsProperty = "referencedItemsProperyt";

        public TestReferencedSection()
        {
            this[referencedItemsProperty] = new NamedElementCollection<ReferencedItemType>();
        }

        [ConfigurationProperty(referencedItemsProperty)]
        [ConfigurationCollection(typeof(ReferencedItemType))]
        public NamedElementCollection<ReferencedItemType> ReferencedItems
        {
            get { return (NamedElementCollection<ReferencedItemType>)this[referencedItemsProperty]; }
            set { this[referencedItemsProperty] = value; }
        }


    }

    public class ReferencedItemType : NamedConfigurationElement
    {
    }
}
