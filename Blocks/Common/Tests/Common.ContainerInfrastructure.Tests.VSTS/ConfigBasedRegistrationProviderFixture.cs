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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    [TestClass]
    public class GivenAConfigSourceWithCryptoBlockInformation
    {
        private IConfigurationSource configSource;
        private ConfigSectionLocationStrategy strategy;
        private TypeRegistrationsProviderLocator locator;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddCryptoSettings().ConfigSource;
            strategy = new ConfigSectionLocationStrategy(CryptographyConfigurationView.SectionName);
            locator = new TypeRegistrationsProviderLocator(strategy);
        }

        [TestMethod]
        public void WhenUsingConfigurationSectionLocatorProvider_ThenAProviderIsReturned()
        {
            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(1, providers.Count);
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenCryptoBlockProviderIsReturned()
        {
            locator = new TypeRegistrationsProviderLocator();
            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(2, providers.Count); // 2 because Data will always be returned
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithoutCryptoInformation
    {
        private IConfigurationSource configSource;
        private TypeRegistrationsProviderLocator locator;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddExceptionHandlingSettings().ConfigSource;
            var strategy = new ConfigSectionLocationStrategy(CryptographyConfigurationView.SectionName);
            locator = new TypeRegistrationsProviderLocator(strategy);
        }

        [TestMethod]
        public void WhenFindingTypeRegistrationProviders_ThenNoProvidersAreReturned()
        {
            var providers = locator.GetProviders(configSource).ToList();
            Assert.AreEqual(0, providers.Count);
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenCryptoBlockProviderIsNotReturned()
        {
            locator = new TypeRegistrationsProviderLocator();
            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(2, providers.Count); // EHAB and Data
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
        public void WhenFindingTypeRegistrationProviders_ThenTwoProvidersAreReturned()
        {
            var locator = new TypeRegistrationsProviderLocator(
                new ConfigSectionLocationStrategy(CryptographyConfigurationView.SectionName),
                new ConfigSectionLocationStrategy(ExceptionHandlingSettings.SectionName));

            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(2, providers.Count);
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenEHABAndCryptoProvidersAreReturned()
        {
            var locator = new TypeRegistrationsProviderLocator();

            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(3, providers.Count);
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithDataInformation
    {
        private IConfigurationSource configSource;
        private TypeRegistrationsProviderLocator locator;

        [TestInitialize]
        public void Given()
        {
            configSource = new ConfigSourceBuilder().AddConnectionStringSettings().ConfigSource;
            locator = new TypeRegistrationsProviderLocator(new TypeLoadingLocationStrategy(
                "Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSyntheticConfigSettings, Microsoft.Practices.EnterpriseLibrary.Data"));
        }

        [TestMethod]
        public void WhenGettingProviders_ThenDataProviderIsReturned()
        {
            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(1, providers.Count);
            Assert.IsTrue(providers[0].GetType() == typeof(DatabaseSyntheticConfigSettings));
        }

        [TestMethod]
        public void WhenGettingModelFromProvider_ThenTheModelIsNotEmpty()
        {
            var providers = locator.GetProviders(configSource).ToList();
            var model = providers[0].CreateRegistrations().ToList();

            Assert.AreEqual(1, model.Count);
        }

        [TestMethod]
        public void WhenUsingDefaultStrategyList_ThenDataProviderIsReturned()
        {
            locator = new TypeRegistrationsProviderLocator();
            var providers = locator.GetProviders(configSource).ToList();
            var model = providers[0].CreateRegistrations().ToList();
            Assert.AreEqual(1, providers.Count);
            Assert.AreEqual(1, model.Count);
        }

    }
}
