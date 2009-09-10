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
    public class When_AddingHashAlgorithmProviderToConfigureCryptographyPassingNullForName : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingHashAlgorithmProviderNamedThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingHashAlgorithmProviderNamed(null);
        }
    }

    [TestClass]
    public class When_AddingHashAlgorithmProviderToConfigureCryptography : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureCryptography
                .EncryptUsingHashAlgorithmProviderNamed("hash provider");
        }

        [TestMethod]
        public void Then_HashAlgorithmProviderIsContainedInKeyedHashCryptoProvider()
        {
            Assert.IsTrue(base.GetCryptographySettings().HashProviders.OfType<HashAlgorithmProviderData>().Any());
        }

        [TestMethod]
        public void Then_HashAlgorithmProviderHasTypeSet()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<HashAlgorithmProviderData>().First();
            Assert.AreEqual(typeof(HashAlgorithmProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_HashAlgorithmProviderHasHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<HashAlgorithmProviderData>().First();
            Assert.AreEqual("hash provider", providerData.Name);
        }
    }

    public abstract class Given_HashAlgorithmProviderInConfiguration : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected IEncryptUsingHashAlgorithmProviderNamed ConfigureHashAlgorithmProvider;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureHashAlgorithmProvider = ConfigureCryptography
                .EncryptUsingHashAlgorithmProviderNamed("hash provider");
        }

        protected HashAlgorithmProviderData GetHashAlgorithmProviderData()
        {
            return base.GetCryptographySettings().HashProviders.OfType<HashAlgorithmProviderData>().First();
        }
    }

    [TestClass]
    public class When_SettingHashAlgorithmProviderAsDefault : Given_HashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureHashAlgorithmProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_CryptographySettingsContainDefaultKeyedHashProvider()
        {
            var cryptoSettings = GetCryptographySettings();
            var hashAlgoProvider = GetHashAlgorithmProviderData();

            Assert.AreEqual(hashAlgoProvider.Name, cryptoSettings.DefaultHashProviderName);
        }
    }


    [TestClass]
    public class When_SettingAlgorithmOnHashAlgorithmProvider : Given_HashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureHashAlgorithmProvider.WithOptions.UsingHashAlgorithm(typeof(HMAC));
        }

        [TestMethod]
        public void Then_HashAlgorithmProviderHasAlgorithmSet()
        {
            var hashAlgoProvider = GetHashAlgorithmProviderData();

            Assert.AreEqual(typeof(HMAC), hashAlgoProvider.AlgorithmType);
        }
    }

    [TestClass]
    public class When_SettingNullAlgorithmOnHashAlgorithmProvider : Given_HashAlgorithmProviderInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_UsingHashAlgorithm_ThrowsArgumentNullException()
        {
            ConfigureHashAlgorithmProvider.WithOptions.UsingHashAlgorithm(null);
        }
    }

    [TestClass]
    public class When_SettingNonHashAlgorithmTypeOnHashAlgorithmProvider : Given_HashAlgorithmProviderInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UsingHashAlgorithm_ThrowsArgumentException()
        {
            ConfigureHashAlgorithmProvider.WithOptions.UsingHashAlgorithm(typeof(object));
        }
    }

    
    [TestClass]
    public class When_SettingAlgorithmOnHashAlgorithmProviderGeneric : Given_HashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureHashAlgorithmProvider.WithOptions.UsingHashAlgorithm<HMAC>();
        }

        [TestMethod]
        public void Then_HashAlgorithmProviderHasAlgorithmSet()
        {
            var hashAlgoProvider = GetHashAlgorithmProviderData();

            Assert.AreEqual(typeof(HMAC), hashAlgoProvider.AlgorithmType);
        }
    }


    [TestClass]
    public class When_DisablingSaltOnHashAlgorithmProvider : Given_HashAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureHashAlgorithmProvider.WithOptions.DisableSalt();
        }

        [TestMethod]
        public void Then_HashAlgorithmProviderHasAlgorithmSet()
        {
            var hashAlgoProvider = GetHashAlgorithmProviderData();

            Assert.IsFalse(hashAlgoProvider.SaltEnabled);
        }
    }
}
