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

using System.Linq;
using Common.ContainerInfrastructure.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    [TestClass]
    public class GivenAConfigSourceWithCryptoBlockInformation
    {
        private IConfigurationSource configSource;
        private ConfigSectionLocator configSectionLocator;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddCryptoSettings().ConfigSource;
            configSectionLocator = new ConfigSectionLocator(CryptographySettings.SectionName);
        }

        [TestMethod]
        public void WhenUsingConfigurationSectionLocatorProvider_ThenAProviderIsReturned()
        {
            var registrations = configSectionLocator.GetRegistrations(configSource).ToList();

            Assert.IsTrue(registrations.Count > 0);
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenCryptoBlockProviderIsReturned()
        {
            var locator = TypeRegistrationsProvider.CreateDefaultProvider();
            var registrations = locator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof(IHashProvider));


            Assert.AreEqual(2, registrations.Count()); // 2 are in the config source
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithoutCryptoInformation
    {
        private IConfigurationSource configSource;
        private TypeRegistrationsProvider configSectionLocator;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddExceptionHandlingSettings().ConfigSource;
            configSectionLocator = new ConfigSectionLocator(CryptographySettings.SectionName);
        }

        [TestMethod]
        public void WhenFindingTypeRegistrationProviders_ThenNoProvidersAreReturned()
        {
            var registrations = configSectionLocator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof(IHashProvider));

            Assert.AreEqual(0, registrations.Count());
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenCryptoBlockProviderIsNotReturned()
        {
            var locator = TypeRegistrationsProvider.CreateDefaultProvider();
            var registrations = locator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof(IHashProvider));

            Assert.AreEqual(0, registrations.Count());
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithEHABAndCryptoInformation
    {
        private IConfigurationSource configSource;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddExceptionHandlingSettings().AddCryptoSettings().ConfigSource;
        }

        [TestMethod]
        public void WhenFindingTypeRegistrationProviders_ThenCryptoAndEHABRegistrationsAreReturned()
        {
            var locator = new CompositeTypeRegistrationsProviderLocator(
                new ConfigSectionLocator(CryptographySettings.SectionName),
                new ConfigSectionLocator(ExceptionHandlingSettings.SectionName));

            var registrations = locator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof(IHashProvider) || r.ServiceType == typeof(ExceptionPolicyImpl));

            Assert.AreEqual(3, registrations.Count());
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenEHABAndCryptoProvidersAreReturned()
        {
            var locator = TypeRegistrationsProvider.CreateDefaultProvider();

            var registrations = locator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof(IHashProvider) || r.ServiceType == typeof(ExceptionPolicyImpl));

            Assert.AreEqual(3, registrations.Count());
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithDataInformation
    {
        private IConfigurationSource configSource;
        private ITypeRegistrationsProvider locator;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddConnectionStringSettings().ConfigSource;
            locator = new TypeLoadingLocator(BlockSectionNames.DataRegistrationProviderLocatorType);
        }

        [TestMethod]
        public void WhenGettingRegistrations_ThenDatabaseRegistrationsAreReturned()
        {
            var registrations = locator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof(Database));

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenUsingDefaultLocators_ThenDataProviderIsReturned()
        {
            locator = TypeRegistrationsProvider.CreateDefaultProvider();

            var registrations = locator.GetRegistrations(configSource)
                .Where(r => r.ServiceType == typeof (Database));

            Assert.AreEqual(1, registrations.Count());
        }
    }
}
