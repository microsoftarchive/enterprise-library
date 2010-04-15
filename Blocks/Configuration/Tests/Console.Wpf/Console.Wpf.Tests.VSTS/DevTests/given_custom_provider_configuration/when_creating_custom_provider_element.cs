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
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.ComponentModel.Design;
using Console.Wpf.Tests.VSTS.DevTests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_custom_provider_configuration
{
    [TestClass]
    public class when_creating_custom_provider_element : ContainerContext
    {
        SectionViewModel viewModel;
        ConfigurationSectionWithCustomProvider section;

        Property attributesProperty;
        ElementViewModel customProviderElement;

        protected override void Arrange()
        {
            base.Arrange();
            section = new ConfigurationSectionWithCustomProvider();

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender());
        }

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(Container, "mockSection", section);
            customProviderElement = viewModel.DescendentElements(x => x.ConfigurationType == typeof(CustomProviderConfigurationElement)).First();
            attributesProperty = customProviderElement.Property("Attributes");
        }

        [TestMethod]
        public void then_has_attributes_property()
        {
            Assert.IsNotNull(attributesProperty);
        }

        [TestMethod]
        public void then_property_is_not_envrionment_overridable()
        {
            Assert.IsFalse(attributesProperty.Attributes.OfType<EnvironmentalOverridesAttribute>().First().CanOverride);
        }

        [TestMethod]
        public void then_attributes_can_be_set()
        {
            var values = new NameValueCollection();
            values.Add("a", "1");
            values.Add("b", "2");

            attributesProperty.Value = values;

            var attributesInConfiguration = ((ICustomProviderData)customProviderElement.ConfigurationElement).Attributes;
            Assert.AreEqual(2, attributesInConfiguration.Count);
            Assert.AreEqual("1", attributesInConfiguration["a"]);
            Assert.AreEqual("2", attributesInConfiguration["b"]);
        }
    }
}
