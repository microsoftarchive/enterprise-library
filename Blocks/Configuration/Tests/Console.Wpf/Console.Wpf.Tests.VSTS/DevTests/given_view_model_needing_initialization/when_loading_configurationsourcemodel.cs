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
using System.ComponentModel;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ConfigurationSection=System.Configuration.ConfigurationSection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source
{
    public abstract class SectionWithInitializableViewModels : ContainerContext
    {
        protected MockInitializeSection sectionWithInitializableModels;

        protected override void Arrange()
        {
            base.Arrange();

            var mockLocator = new Mock<ConfigurationSectionLocator>();
            mockLocator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "MockSection" });
            base.Container.RegisterInstance(mockLocator.Object);

            sectionWithInitializableModels = new MockInitializeSection();
            sectionWithInitializableModels.Children.Add(new MockInitialzeCollectionElement() { Name = "TestElement" });
            sectionWithInitializableModels.Children.Add(new MockInitialzeCollectionElement() { Name = "TestElement2" });
            sectionWithInitializableModels.LeafElement = new MockInitializeElement() { };

           
        }
    }

    [TestClass]
    public class when_loading_configurationsourcemodel : SectionWithInitializableViewModels
    {
        protected MockInitializeSectionViewModel section;

        protected override void Act()
        {
            var source = new DesignDictionaryConfigurationSource();
            source.Add("MockSection", sectionWithInitializableModels);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);
            section = sourceModel.Sections.OfType<MockInitializeSectionViewModel>().Single();
        }

        [TestMethod]
        public void then_section_was_initialized()
        {
            Assert.IsTrue(section.WasInitialized);
        }

        [TestMethod]
        public void then_all_collections_initialized()
        {
            var collections = section.DescendentElements().OfType<MockInitializeElementCollectionViewModel>();
            Assert.IsTrue(collections.Any());
            Assert.IsTrue(collections.All(x => x.WasInitialized));   
        }

        [TestMethod]
        public void then_all_collection_elements_initialized()
        {
            var elements = section.DescendentElements().OfType<MockInitializeCollectionElementViewModel>();
            Assert.IsTrue(elements.Any());
            Assert.IsTrue(elements.All(x => x.WasInitialized));
        }
    }

    [TestClass]
    public class when_adding_new_element_needing_initialization : SectionWithInitializableViewModels
    {
        protected MockInitializeSectionViewModel section;
        private MockInitializeCollectionElementViewModel addedElement;

        protected override void Arrange()
        {
            base.Arrange();
            var source = new DesignDictionaryConfigurationSource();
            source.Add("MockSection", sectionWithInitializableModels);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);
            section = sourceModel.Sections.OfType<MockInitializeSectionViewModel>().Single();
        }

        protected override void Act()
        {
            var collection = section.DescendentElements().OfType<MockInitializeElementCollectionViewModel>().First();
            addedElement = (MockInitializeCollectionElementViewModel)collection.AddNewCollectionElement(typeof (MockInitialzeCollectionElement));
        }

        [TestMethod]
        public void then_added_element_initialized()
        {
            Assert.IsTrue(addedElement.WasInitialized);
        }

    }

    [ViewModel(typeof(MockInitializeSectionViewModel))]
    public class MockInitializeSection : ConfigurationSection
    {
        private const string childrenProperty = "childrenProperty";
        private const string leafElementProperty = "leafElementProperty";

        public MockInitializeSection()
        {
            this[childrenProperty] = new NamedElementCollection<MockInitialzeCollectionElement>();
        }

        [ConfigurationProperty(childrenProperty)]
        [ConfigurationCollection(typeof(MockInitialzeCollectionElement))]
        [ViewModel(typeof(MockInitializeElementCollectionViewModel))]
        public NamedElementCollection<MockInitialzeCollectionElement> Children
        {
            get { return (NamedElementCollection<MockInitialzeCollectionElement>)this[childrenProperty]; }
        }
        [ConfigurationProperty(leafElementProperty)]
        public MockInitializeElement LeafElement
        {
            get { return (MockInitializeElement)this[leafElementProperty]; }
            set { this[leafElementProperty] = value;}
        }
    }

    [ViewModel(typeof(MockInitializeCollectionElementViewModel))]
    public class MockInitialzeCollectionElement : NamedConfigurationElement
    {
    }

    [ViewModel(typeof(MockInitializeElementViewModel))]
    public class MockInitializeElement : ConfigurationElement
    {
        
    }

    public class MockInitializeElementViewModel : ElementViewModel
    {
        public MockInitializeElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty) 
            : base(parentElementModel, declaringProperty)
        {
        }

        public override void Initialize(InitializeContext context)
        {
            WasInitialized = true;
        }

        public bool WasInitialized { get; set; }
    }

    public class MockInitializeSectionViewModel : SectionViewModel
    {
        public MockInitializeSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }

        public override void Initialize(InitializeContext context)
        {
            WasInitialized = true;
        }

        public bool WasInitialized { get; set; }
    }

    public class MockInitializeCollectionElementViewModel : CollectionElementViewModel
    {
        public MockInitializeCollectionElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        public override void Initialize(InitializeContext context)
        {
            WasInitialized = true;
        }

        public bool WasInitialized { get; set; }
    }

    public class MockInitializeElementCollectionViewModel : ElementCollectionViewModel
    {
        public MockInitializeElementCollectionViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(parentElementModel, declaringProperty)
        {
        }

        public override void Initialize(InitializeContext context)
        {
            WasInitialized = true;
        }

        public bool WasInitialized { get; set; }
    }
}
