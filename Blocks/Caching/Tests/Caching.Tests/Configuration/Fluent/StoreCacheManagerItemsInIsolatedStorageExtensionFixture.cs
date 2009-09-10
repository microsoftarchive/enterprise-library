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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_CallingStoreInIsolatedStorageWithNullName : Given_CachgeManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreInIsolatedStorage(null);
        }
    }

    [TestClass]
    public class When_CallingStoreInIsolatedStorage : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreInIsolatedStorage("isolated storage");
        }

        [TestMethod]
        public void Then_CachingSettingsContainsBackingStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            Assert.IsTrue(cachingConfiguration.BackingStores.OfType<IsolatedStorageCacheStorageData>().Any());
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<IsolatedStorageCacheStorageData>().First();

            Assert.AreEqual("isolated storage", cacheStorage.Name);
        }


        [TestMethod]
        public void Then_CachingStoreHasApproriateType()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<IsolatedStorageCacheStorageData>().First();

            Assert.AreEqual(typeof(IsolatedStorageBackingStore), cacheStorage.Type);
        }

        [TestMethod]
        public void Then_CacheStoreIsSetOnCacheManager()
        {
            var cacheManagerConfig = GetCacheManager();
            Assert.AreEqual("isolated storage", cacheManagerConfig.CacheStorage);
        }

        [TestMethod]
        public void Then_CachingStoreHasNoEncyption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<IsolatedStorageCacheStorageData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheStorage.StorageEncryption));
        }
    }

    [TestClass]
    public class When_SettingPartitionNameOnIsolatedStorageProvider : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreInIsolatedStorage("isolated storage")
                .UsePartition("partition");
        }

        [TestMethod]
        public void Then_ConfigurationHasPartitionName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<IsolatedStorageCacheStorageData>().First();

            Assert.AreEqual("partition", cacheStorage.PartitionName);
        }
    }

    [TestClass]
    public class When_SettingPartitionNameOnIsolatedStorageProviderToNull : Given_CachgeManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreInIsolatedStorage("isolated storage")
                .UsePartition(null);

        }
    }
    [TestClass]
    public class When_SettingSharedEncryptionOnIsolatedStorageProvider : Given_CachgeManagerInConfigurationSource
    {

        protected override void Act()
        {
            ConfigureCacheManager.StoreInIsolatedStorage("isolated storage")
                .EncryptUsing.SharedEncryptionProviderNamed("shared encryption provider");
        }

        [TestMethod]
        public void Then_ConfigurationHasSharedEncryptionProvider()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<IsolatedStorageCacheStorageData>().First();

            Assert.AreEqual("shared encryption provider", cacheStorage.StorageEncryption);

        }
    }
}
