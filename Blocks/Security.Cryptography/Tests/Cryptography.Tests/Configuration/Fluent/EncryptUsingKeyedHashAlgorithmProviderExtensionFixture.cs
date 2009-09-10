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
    public class When_AddingKeyedHashAlgorithmProviderToConfigureCryptographyPassingNullForName : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingKeyedHashAlgorithmProviderNamed_ThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingKeyedHashAlgorithmProviderNamed(null);
        }
    }

    [TestClass]
    public class When_AddingKeyedHashAlgorithmProviderToConfigureCryptography : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureCryptography
                .EncryptUsingKeyedHashAlgorithmProviderNamed("keyed hash provider");
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderIsContainedInKeyedHashCryptoProvider()
        {
            Assert.IsTrue(base.GetCryptographySettings().HashProviders.OfType<KeyedHashAlgorithmProviderData>().Any());
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderHasTypeSet()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<KeyedHashAlgorithmProviderData>().First();
            Assert.AreEqual(typeof(KeyedHashAlgorithmProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderHasHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<KeyedHashAlgorithmProviderData>().First();
            Assert.AreEqual("keyed hash provider", providerData.Name);
        }
    }

    public abstract class Given_KeyedHashAlgorithmProviderInConfiguration : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected IEncryptUsingKeyedHashAlgorithmProviderNamed ConfigureKeyedHashAlgorithmProvider;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureKeyedHashAlgorithmProvider = ConfigureCryptography
                .EncryptUsingKeyedHashAlgorithmProviderNamed("keyed hash provider");
        }

        protected KeyedHashAlgorithmProviderData GetKeyedHashAlgorithmProviderData()
        {
            return base.GetCryptographySettings().HashProviders.OfType<KeyedHashAlgorithmProviderData>().First();
        }
    }

    [TestClass]
    public class When_SettingKeyedHashAlgorithmProviderAsDefault : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureKeyedHashAlgorithmProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_CryptographySettingsContainDefaultKeyedHashProvider()
        {
            var cryptoSettings = GetCryptographySettings();
            var hashAlgoProvider = GetKeyedHashAlgorithmProviderData();

            Assert.AreEqual(hashAlgoProvider.Name, cryptoSettings.DefaultHashProviderName);
        }
    }


    [TestClass]
    public class When_SettingAlgorithmOnKeyedHashAlgorithmProvider : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureKeyedHashAlgorithmProvider.WithOptions.UsingKeyedHashAlgorithm(typeof(HMAC));
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderHasAlgorithmSet()
        {
            var hashAlgoProvider = GetKeyedHashAlgorithmProviderData();

            Assert.AreEqual(typeof(HMAC), hashAlgoProvider.AlgorithmType);
        }
    }

    [TestClass]
    public class When_SettingNullAlgorithmOnKeyedHashAlgorithmProvider : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_UsingKeyedHashAlgorithm_ThrowsArgumentNullException()
        {
            ConfigureKeyedHashAlgorithmProvider.WithOptions.UsingKeyedHashAlgorithm(null);
        }
    }

    [TestClass]
    public class When_SettingNonHashAlgorithmTypeOnKeyedHashAlgorithmProvider : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UsingKeyedHashAlgorithm_ThrowsArgumentException()
        {
            ConfigureKeyedHashAlgorithmProvider.WithOptions.UsingKeyedHashAlgorithm(typeof(object));
        }
    }

    [TestClass]
    public class When_SettingAlgorithmOnKeyedHashAlgorithmProviderGeneric : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureKeyedHashAlgorithmProvider.WithOptions.UsingKeyedHashAlgorithm<HMAC>();
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderHasAlgorithmSet()
        {
            var hashAlgoProvider = GetKeyedHashAlgorithmProviderData();

            Assert.AreEqual(typeof(HMAC), hashAlgoProvider.AlgorithmType);
        }
    }


    [TestClass]
    public class When_DisablingSaltOnKeyedHashAlgorithmProvider : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureKeyedHashAlgorithmProvider.WithOptions.DisableSalt();
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderHasAlgorithmSet()
        {
            var hashAlgoProvider = GetKeyedHashAlgorithmProviderData();

            Assert.IsFalse(hashAlgoProvider.SaltEnabled);
        }
    }

    [TestClass]
    public class When_SettingKeyOnKeyedHashAlgorithmProvider : Given_KeyedHashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureKeyedHashAlgorithmProvider.WithOptions.UseKeyFile("c:\\key.file", DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        public void Then_KeyedHashAlgorithmProviderHasKeyInfoSet()
        {
            var hashAlgoProvider = GetKeyedHashAlgorithmProviderData();

            Assert.AreEqual("c:\\key.file", hashAlgoProvider.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, hashAlgoProvider.ProtectedKeyProtectionScope);
        }
    }

}
