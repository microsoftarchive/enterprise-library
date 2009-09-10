//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Tests.Configuration.Fluent
{
    public abstract class Given_CachgeManagerStoreInConfigurationSource : ArrangeActAssert
    {
        private string cacheManagerName = "test cache manager";
        private string cacheManagerStoreName = "isolated storage";
        protected ConfigurationSourceBuilder ConfigurationSourceBuilder;
        protected ICachingConfiguration CachingConfiguration;
        protected ICachingConfigurationCacheManager ConfigureCacheManager;
        protected IBackingStoreEncryptItemsUsing EncryptUsing;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();

            CachingConfiguration = ConfigurationSourceBuilder.ConfigureCaching();

            ConfigureCacheManager = CachingConfiguration.ForCacheManagerNamed(cacheManagerName);

            EncryptUsing = ConfigureCacheManager.StoreInIsolatedStorage(cacheManagerStoreName).EncryptUsing;
        }

        public IConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }

        protected CacheManagerSettings GetCacheManagerSettings()
        {
            var configurationSource = GetConfigurationSource();
            return (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);
        }

        protected CacheManagerData GetCacheManager()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            return cachingConfiguration.CacheManagers.OfType<CacheManagerData>().Where(x => x.Name == cacheManagerName).First();
        }

        protected CacheStorageData GetCacheManagerStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            return cachingConfiguration.BackingStores.OfType<CacheStorageData>().Where(x => x.Name == cacheManagerStoreName).First();
        }
    }

    [TestClass]
    public class When_EncryptingUsingSymmetricProviderInstancePassingNullName : Given_CachgeManagerStoreInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            EncryptUsing.SymmetricEncryptionProviderNamed(null);
        }
    }

    [TestClass]
    public class When_SettingNullSymmetricProviderInstance : Given_CachgeManagerStoreInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            EncryptUsing.SymmetricEncryptionProviderNamed("provider")
                .UsingSharedSymmetricEncryptionInstanceNamed(null);
        }
    }


    [TestClass]
    public class When_EncryptingUsingSymmetricProviderInstance : Given_CachgeManagerStoreInConfigurationSource
    {
        protected override void Act()
        {
            EncryptUsing.SymmetricEncryptionProviderNamed("symm instance provider name");
        }

        [TestMethod]
        public void Then_CachingConfigurationContainsSymmetricInstanceEncryptionProvider()
        {
            Assert.IsTrue(GetCacheManagerSettings().EncryptionProviders.OfType<SymmetricStorageEncryptionProviderData>().Any());
        }

        [TestMethod]
        public void Then_StorageEncryptionProviderHasAppropriateName()
        {
            var storageProvider = GetCacheManagerSettings().EncryptionProviders.OfType<SymmetricStorageEncryptionProviderData>().First();

            Assert.AreEqual("symm instance provider name", storageProvider.Name);
        }

        [TestMethod]
        public void Then_StorageEncryptionProviderHasAppropriateType()
        {
            var storageProvider = GetCacheManagerSettings().EncryptionProviders.OfType<SymmetricStorageEncryptionProviderData>().First();

            Assert.AreEqual(typeof(SymmetricStorageEncryptionProvider), storageProvider.Type);
        }

        [TestMethod]
        public void Then_StorageEncryptionProviderIsSetOnBackingStore()
        {
            var backingStore = GetCacheManagerStore();

            Assert.AreEqual("symm instance provider name", backingStore.StorageEncryption);
        }

        
    }
}
