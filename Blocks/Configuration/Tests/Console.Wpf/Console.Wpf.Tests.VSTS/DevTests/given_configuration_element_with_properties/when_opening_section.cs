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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.DevTests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    [TestClass]
    public class when_opening_section : ContainerContext
    {
        SectionViewModel sectionViewModel;
        private DesignDictionaryConfigurationSource source;

        protected override void Arrange()
        {
            base.Arrange();

            var sectionLocator = new Mock<ConfigurationSectionLocator>();
            sectionLocator.Setup(x => x.ConfigurationSectionNames).Returns(new[] {"section"});
            Container.RegisterInstance<ConfigurationSectionLocator>(sectionLocator.Object);

            SectionWithExtendedViewModel section = new SectionWithExtendedViewModel();
            
            source = new DesignDictionaryConfigurationSource();
            source.Add("section", section);
        }

        protected override void Act()
        {
            var configSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configSourceModel.Load(source);

            sectionViewModel = configSourceModel.Sections.OfType<SectionViewModelEx>().First();
        }

        [TestMethod]
        public void then_properties_are_initialized()
        {
            var property =
                sectionViewModel.DescendentElements().SelectMany(x => x.Properties).OfType<CustomProperty>().First();

            Assert.IsTrue(property.WasInitialized);
        }

        [TestMethod]
        public void then_custom_attribute_property_applied()
        {
            Assert.IsTrue(
                sectionViewModel.GetDescendentsOfType<ElementWithExtendedViewModel>().First()
                    .Properties.Where(p => p.PropertyName == "Attributes").Any());

        }
        
    }
}
