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
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using ConfigurationSection = System.Configuration.ConfigurationSection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;


namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    public abstract class OverridableSectionContext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var overridableSection = new OverridableSection();

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", overridableSection);

            var applicationModel = Container.Resolve<ApplicationViewModel>();
            var sourceModel = applicationModel.CurrentConfigurationSource;
            sourceModel.Load(source);
            
            BaseSection = sourceModel.Sections.Where(s => s.SectionName == "testSection").Single();
            applicationModel.NewEnvironment();

            OverridesProperty = BaseSection.Properties.Where(x => x.PropertyName.StartsWith("Overrides")).First();
            OverridesProperty.Value = true;
        }

        protected Property OverridesProperty { get; private set; }
        protected SectionViewModel BaseSection { get; private set; }
    }

    [TestClass]
    public class when_validating_override_property : OverridableSectionContext
    {
        private Property overriddenSomeValueProperty;
        private Property overriddenReferenceProperty;
        private bool overridesPropertyNotifiedChange;

        protected override void Arrange()
        {
            base.Arrange();

            ((INotifyCollectionChanged)OverridesProperty.ValidationResults).CollectionChanged +=
                (s, e) =>
                {
                    overridesPropertyNotifiedChange = true;
                };

            overriddenSomeValueProperty =
                OverridesProperty.ChildProperties.Where(p => p.PropertyName == "SomeProperty").Single();

            overriddenReferenceProperty =
                OverridesProperty.ChildProperties.Where(p => p.PropertyName == "ReferencingProperty").Single();
        }

        protected override void Act()
        {
            BaseSection.Property("SomeProperty").Value = "ValidValue";
            overriddenSomeValueProperty.Value = "";

            overriddenReferenceProperty.Value = "NotAValue";
        }

        [TestMethod]
        public void then_override_property_has_validation_errors()
        {
            Assert.IsTrue(overriddenSomeValueProperty.ValidationResults.Any());
        }

        [TestMethod]
        public void then_base_property_has_no_validation_errors()
        {
            Assert.IsFalse(BaseSection.Property("SomeProperty").ValidationResults.Any());
        }

        [TestMethod]
        public void then_overridden_property_reflects_base_element_name()
        {
            var basePropertyElementName = ((ElementProperty)BaseSection.Property("SomeProperty")).DeclaringElement.Name;
            Assert.IsTrue(overriddenSomeValueProperty.ValidationResults.All(e => e.ElementName == string.Format("{0}.{1}", basePropertyElementName, OverridesProperty.DisplayName)));
        }

        [TestMethod]
        public void then_reference_validation_fails()
        {
            Assert.IsTrue(overriddenReferenceProperty.ValidationResults.Any());
        }

        [TestMethod]
        public void then_overrides_property_reflects_childrens_errors()
        {
            Assert.IsTrue(OverridesProperty.ValidationResults.Any());
        }

        [TestMethod]
        public void then_overrides_property_notifies_change()
        {
            Assert.IsTrue(overridesPropertyNotifiedChange);
        }
    }

    [TestClass]
    public class when_overrides_property_disabled : OverridableSectionContext
    {
        private Property overriddenSomeValueProperty;

        protected override void Arrange()
        {
            base.Arrange();

            overriddenSomeValueProperty =
                OverridesProperty.ChildProperties.Where(p => p.PropertyName == "SomeProperty").Single();
            
            overriddenSomeValueProperty.Value = "";
            Assert.IsTrue(OverridesProperty.ChildProperties.SelectMany(p => p.ValidationResults).Any());
        }

        protected override void Act()
        {
            OverridesProperty.Value = false;
        }

        [TestMethod]
        public void then_validation_errors_removed()
        {
            Assert.IsFalse(OverridesProperty.ChildProperties.SelectMany(p => p.ValidationResults).Any());
        }
    }

    public class OverridableSection : ConfigurationSection
    {
        private const string referencingProperty = "referencingProperty";
        private const string referencedElementsProperty = "referencedElementsProperty";

        private const string someProperty = "someProperty";

        [ConfigurationProperty(someProperty, IsRequired = true, DefaultValue = "Test")]
        [System.Configuration.StringValidator(MinLength = 1)]
        [EnvironmentalOverrides(true)]
        public string SomeProperty
        {
            get { return (string)this[someProperty]; }
            set { this[someProperty] = value; }
        }

        [ConfigurationProperty(referencingProperty, IsRequired = true)]
        [Reference(typeof(OverridableSection), typeof(CollectedItem))]
        [EnvironmentalOverrides(true)]
        public string ReferencingProperty
        {
            get { return (string)this[referencingProperty]; }
            set { this[referencingProperty] = value; }
        }

        [ConfigurationCollection(typeof(CollectedItem))]
        public NamedElementCollection<CollectedItem> ReferencedElements
        {
            get { return (NamedElementCollection<CollectedItem>)this[referencedElementsProperty]; }
        }
    }

    public class CollectedItem : NamedConfigurationElement
    {

    }
}
