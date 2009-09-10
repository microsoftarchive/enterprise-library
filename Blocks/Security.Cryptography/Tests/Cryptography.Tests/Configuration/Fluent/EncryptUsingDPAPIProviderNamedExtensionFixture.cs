//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_AddingDpapiProviderToConfigureCryptographyPassingNull : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingDPAPIProviderNamedThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingDPAPIProviderNamed(null);
        }
    }

    [TestClass]
    public class When_AddingDpapiProviderToConfigureCryptography : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureCryptography
                .EncryptUsingDPAPIProviderNamed("dpapi provider");
        }

        [TestMethod]
        public void Then_DpapiProviderIsContainedInSymmetricCryptoProvider()
        {
            Assert.IsTrue(base.GetCryptographySettings().SymmetricCryptoProviders.OfType<DpapiSymmetricCryptoProviderData>().Any());
        }

        [TestMethod]
        public void Then_DpapiProviderHasTypeSet()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<DpapiSymmetricCryptoProviderData>().First();
            Assert.AreEqual(typeof(DpapiSymmetricCryptoProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_DpapiProviderHasHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<DpapiSymmetricCryptoProviderData>().First();
            Assert.AreEqual("dpapi provider", providerData.Name);
        }
    }

    public abstract class Given_DpapiProviderInConfiguration : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected IEncryptUsingDPAPIProviderNamed ConfigureDpapiProvider;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureDpapiProvider = ConfigureCryptography
                .EncryptUsingDPAPIProviderNamed("dpapi provider");
        }

        protected DpapiSymmetricCryptoProviderData GetDpapiProviderData()
        {
            return base.GetCryptographySettings().SymmetricCryptoProviders.OfType<DpapiSymmetricCryptoProviderData>().First();
        }
    }

    [TestClass]
    public class When_SettingDpapiProviderAsDefault : Given_DpapiProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureDpapiProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_CryptographySettingsContainDefaultSymmetricProvider()
        {
            var cryptoSettings = GetCryptographySettings();
            var dpapiProvider = GetDpapiProviderData();

            Assert.AreEqual(dpapiProvider.Name, cryptoSettings.DefaultSymmetricCryptoProviderName);
        }
    }

    [TestClass]
    public class When_SettingDpapiProtectionScope : Given_DpapiProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureDpapiProvider.SetProtectionScope(DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        public void Then_DpapiProviderDataHasProtectionScope()
        {
            var dpapiProvider = GetDpapiProviderData();

            Assert.AreEqual(DataProtectionScope.CurrentUser, dpapiProvider.Scope);
        }
    }
}
