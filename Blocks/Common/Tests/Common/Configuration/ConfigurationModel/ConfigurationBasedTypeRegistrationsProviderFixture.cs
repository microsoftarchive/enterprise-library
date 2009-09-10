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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel
{
    [TestClass]
    public class GivenConfigurationBasedTypeRegistrationsProvider
    {
        DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

        [TestMethod]
        public void WhenConfigurationDoesntContainSection_ThenDefaultRegistrationsAreReturned()
        {

            var locators = ConfigurationBasedTypeRegistrationsProviderFactory.CreateTypeRegistrationsProviderLocators(configurationSource, new NullContainerReconfiguringEventSource());
            Assert.AreEqual(9, locators.Count());
        }

        [TestMethod]
        public void WhenConfigurationContainsSectionName_ThenConfigSectionLocatorIsUsedToRetrieveRegistrations()
        {
            TypeRegistrationProvidersConfigurationSection section = new TypeRegistrationProvidersConfigurationSection();
            section.TypeRegistrationProviders.Clear();
            section.TypeRegistrationProviders.Add(new TypeRegistrationProviderElement { Name = "test", SectionName = "Section Name" });
            configurationSource.Add(TypeRegistrationProvidersConfigurationSection.SectionName, section);

            var locators = ConfigurationBasedTypeRegistrationsProviderFactory.CreateTypeRegistrationsProviderLocators(configurationSource, new NullContainerReconfiguringEventSource());
            Assert.AreEqual(1, locators.Count());
            Assert.IsInstanceOfType(locators.First(), typeof(ConfigSectionLocator));
        }

        [TestMethod]
        public void WhenConfigurationContainsProviderType_ThenTypeLoadingLocatorIsUsedToRetrieveRegistrations()
        {
            TypeRegistrationProvidersConfigurationSection section = new TypeRegistrationProvidersConfigurationSection();
            section.TypeRegistrationProviders.Clear();
            section.TypeRegistrationProviders.Add(new TypeRegistrationProviderElement { Name = "test", ProviderTypeName = "ProviderType" });
            configurationSource.Add(TypeRegistrationProvidersConfigurationSection.SectionName, section);


            var locators = ConfigurationBasedTypeRegistrationsProviderFactory.CreateTypeRegistrationsProviderLocators(configurationSource, new NullContainerReconfiguringEventSource());
            Assert.AreEqual(1, locators.Count());
            Assert.IsInstanceOfType(locators.First(), typeof(TypeLoadingLocator));
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WhenBothSectionAndProviderTypeAreSpecified_ThenThrowsConfigurationErrorsException()
        {
            TypeRegistrationProvidersConfigurationSection section = new TypeRegistrationProvidersConfigurationSection();
            section.TypeRegistrationProviders.Clear();
            section.TypeRegistrationProviders.Add(new TypeRegistrationProviderElement { Name = "test", SectionName = "Section Name", ProviderTypeName = "ProviderType" });
            configurationSource.Add(TypeRegistrationProvidersConfigurationSection.SectionName, section);


            ConfigurationBasedTypeRegistrationsProviderFactory.CreateProvider(configurationSource, new NullContainerReconfiguringEventSource()).GetRegistrations(configurationSource);
        }
    }
}
