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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    public abstract class Given_CacheManagerInConfigurationSource : ArrangeActAssert
    {
        private string cacheManagerName = "test cache manager";
        protected ConfigurationSourceBuilder ConfigurationSourceBuilder;
        protected ICachingConfiguration CachingConfiguration;
        protected ICachingConfigurationCacheManager ConfigureCacheManager;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();

            CachingConfiguration = ConfigurationSourceBuilder.ConfigureCaching();

            ConfigureCacheManager = CachingConfiguration.ForCacheManagerNamed(cacheManagerName);
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
    }


    [TestClass]
    public class When_CallingStoreInDatabasePassingNullName : Given_CacheManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase(null);
        }
    }

    [TestClass]
    public class When_CallingStoreInDatabasePassingAndSettingDatabaseToNull : Given_CacheManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase("instance name").UseSharedDatabaseNamed(null);
        }
    }

    [TestClass]
    public class When_CallingStoreInDatabasePassingAndSettingPartitiionToNull : Given_CacheManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase("instance name").UsePartition(null);
        }
    }

    [TestClass]
    public class When_CallingStoreInDatabase : Given_CacheManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase("database storage");
        }

        [TestMethod]
        public void Then_CachingSettingsContainsBackingStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            Assert.IsTrue(cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().Any());
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().First();

            Assert.AreEqual("database storage", cacheStorage.Name);
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateType()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().First();

            Assert.AreEqual(typeof(DataBackingStore), cacheStorage.Type);
        }

        [TestMethod]
        public void Then_CacheStoreIsSetOnCacheManager()
        {
            var cacheManagerConfig = GetCacheManager();
            Assert.AreEqual("database storage", cacheManagerConfig.CacheStorage);
        }

        [TestMethod]
        public void Then_CachingStoreHasNoEncyption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheStorage.StorageEncryption));
        }
    }

    [TestClass]
    public class When_SettingPartitionNameOnDatabaseProvider : Given_CacheManagerInConfigurationSource
    {

        protected override void Act()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase("database storage")
                .UsePartition("partition");
        }

        [TestMethod]
        public void Then_ConfigurationHasPartitionName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().First();

            Assert.AreEqual("partition", cacheStorage.PartitionName);

        }
    }

    [TestClass]
    public class When_SettingDatabaseInstanceOnDatabaseProvider : Given_CacheManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase("database storage")
                .UseSharedDatabaseNamed("test connection");
        }

        [TestMethod]
        public void Then_ConfigurationHasPartitionName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().First();

            Assert.AreEqual("test connection", cacheStorage.DatabaseInstanceName);

        }
    }


    [TestClass]
    public class When_SettingSharedEncryptionProviderOnDatabaseProvider : Given_CacheManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreCacheMangerItemsInDatabase("database storage")
                .EncryptUsing.SharedEncryptionProviderNamed("encryption provider");
        }

        [TestMethod]
        public void Then_ConfigurationHasStorageEncryption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<DataCacheStorageData>().First();

            Assert.AreEqual("encryption provider", cacheStorage.StorageEncryption);

        }
    }
}
