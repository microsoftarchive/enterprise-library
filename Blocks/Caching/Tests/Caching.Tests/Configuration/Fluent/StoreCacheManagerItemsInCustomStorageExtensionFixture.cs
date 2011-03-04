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
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_CallingStoreInCustomStoreWithNullName : Given_CachgeManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_StoreInCustomStore_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreInCustomStore(null, typeof(TestCustomCacheStore));
        }
    }

    [TestClass]
    public class When_CallingStoreInCustomStoreWithNullType : Given_CachgeManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_StoreInCustomStore_ThrowsArgumentNullException()
        {
            ConfigureCacheManager.StoreInCustomStore("name", null);
        }
    }

    [TestClass]
    public class When_CallingStoreInCustomStoreWithTypeThatDoesntDeriveFromIBackingStore : Given_CachgeManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_StoreInCustomStore_ThrowsArgumentException()
        {
            ConfigureCacheManager.StoreInCustomStore("name", typeof(object));
        }
    }
    
    [TestClass]
    public class When_CallingStoreInCustomStoreWithNullAttributes : Given_CachgeManagerInConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ThrowsArgumentNullException()
        {
            ConfigureCacheManager.StoreInCustomStore("name", typeof(TestCustomCacheManager), null);
        }
    }

    [TestClass]
    public class When_CallingStoreInCustomStore : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreInCustomStore("custom storage", typeof(TestCustomCacheStore));
        }

        [TestMethod]
        public void Then_CachingSettingsContainsBackingStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            Assert.IsTrue(cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().Any());
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual("custom storage", cacheStorage.Name);
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateType()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(typeof(TestCustomCacheStore), cacheStorage.Type);
        }

        [TestMethod]
        public void Then_CacheStoreIsSetOnCacheManager()
        {
            var cacheManagerConfig = GetCacheManager();
            Assert.AreEqual("custom storage", cacheManagerConfig.CacheStorage);
        }

        [TestMethod]
        public void Then_CachingStoreHasNoEncyption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheStorage.StorageEncryption));
        }

        [TestMethod]
        public void Then_CachingStoreHasNoAttributes()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(0, cacheStorage.Attributes.Count);
        }
    }

    [TestClass]
    public class When_CallingStoreInCustomStoreGeneric : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreInCustomStore <TestCustomCacheStore>("custom storage");
        }

        [TestMethod]
        public void Then_CachingSettingsContainsBackingStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            Assert.IsTrue(cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().Any());
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual("custom storage", cacheStorage.Name);
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateType()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(typeof(TestCustomCacheStore), cacheStorage.Type);
        }

        [TestMethod]
        public void Then_CacheStoreIsSetOnCacheManager()
        {
            var cacheManagerConfig = GetCacheManager();
            Assert.AreEqual("custom storage", cacheManagerConfig.CacheStorage);
        }

        [TestMethod]
        public void Then_CachingStoreHasNoEncyption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheStorage.StorageEncryption));
        }

        [TestMethod]
        public void Then_CachingStoreHasNoAttributes()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(0, cacheStorage.Attributes.Count);
        }
    }

    [TestClass]
    public class When_CallingStoreInCustomStorePassingAttribute : Given_CachgeManagerInConfigurationSource
    {
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            ConfigureCacheManager.StoreInCustomStore("custom storage", typeof(TestCustomCacheStore), attributes);
        }

        [TestMethod]
        public void Then_CachingSettingsContainsBackingStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            Assert.IsTrue(cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().Any());
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual("custom storage", cacheStorage.Name);
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateType()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(typeof(TestCustomCacheStore), cacheStorage.Type);
        }

        [TestMethod]
        public void Then_CacheStoreIsSetOnCacheManager()
        {
            var cacheManagerConfig = GetCacheManager();
            Assert.AreEqual("custom storage", cacheManagerConfig.CacheStorage);
        }

        [TestMethod]
        public void Then_CachingStoreHasNoEncyption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheStorage.StorageEncryption));
        }

        [TestMethod]
        public void Then_CachingStoreHasAppropriateAttributes()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(attributes.Count, cacheStorage.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], cacheStorage.Attributes[attKey]);
            }
        }
    }

    [TestClass]
    public class When_CallingStoreInCustomStorePassingAttributeGeneric : Given_CachgeManagerInConfigurationSource
    {
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            ConfigureCacheManager.StoreInCustomStore <TestCustomCacheStore>("custom storage", attributes);
        }

        [TestMethod]
        public void Then_CachingSettingsContainsBackingStore()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            Assert.IsTrue(cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().Any());
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateName()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual("custom storage", cacheStorage.Name);
        }

        [TestMethod]
        public void Then_CachingStoreHasApproriateType()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(typeof(TestCustomCacheStore), cacheStorage.Type);
        }

        [TestMethod]
        public void Then_CacheStoreIsSetOnCacheManager()
        {
            var cacheManagerConfig = GetCacheManager();
            Assert.AreEqual("custom storage", cacheManagerConfig.CacheStorage);
        }

        [TestMethod]
        public void Then_CachingStoreHasNoEncyption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheStorage.StorageEncryption));
        }

        [TestMethod]
        public void Then_CachingStoreHasAppropriateAttributes()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual(attributes.Count, cacheStorage.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], cacheStorage.Attributes[attKey]);
            }
        }
    }


    [TestClass]
    public class When_SettingSharedEncryptionProviderOnCustomStorageProvider : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreInCustomStore("custom storage", typeof(TestCustomCacheStore))
                .EncryptUsing.SharedEncryptionProviderNamed("encryption provider");
        }

        [TestMethod]
        public void Then_ConfigurationHasStorageEncryption()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            var cacheStorage = cachingConfiguration.BackingStores.OfType<CustomCacheStorageData>().First();

            Assert.AreEqual("encryption provider", cacheStorage.StorageEncryption);

        }
    }

    public class TestCustomCacheStore : IBackingStore
    {
        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(CacheItem newCacheItem)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void UpdateLastAccessedTime(string key, DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public System.Collections.IDictionary Load()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
