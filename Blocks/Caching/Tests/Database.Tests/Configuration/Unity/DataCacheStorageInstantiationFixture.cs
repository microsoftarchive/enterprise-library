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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using System.Linq.Expressions;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Unity
{
    [TestClass]
    public class DataCacheStorageInstantiationFixture
    {
        private const string providerName = "foo";
        private IServiceLocator container;
        private CacheManagerSettings settings;
        private DatabaseSettings dbSettings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            settings = new CacheManagerSettings();
            dbSettings = new DatabaseSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, settings);
            configurationSource.Add(DatabaseSettings.SectionName, dbSettings);
        }

        [TestCleanup]
        public void TearDown()
        {
            
        }

        [TestMethod]
        public void CanResolveDataBackingStoreWithOutEncryptionProvider()
        {
            const string key = "fooKey";
            DataCacheStorageData data = new DataCacheStorageData("Data Cache Storage", "CachingDatabase", "fooPartition");
            settings.BackingStores.Add(data);

            CacheManagerData managerData = new CacheManagerData("defaultCacheManager", 300, 200, 100, "Data Cache Storage");
            settings.CacheManagers.Add(managerData);
            settings.DefaultCacheManager = "defaultCacheManager";

            dbSettings.DefaultDatabase = "CachingDatabase";

            container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);
            DataBackingStore createdStore = container.GetInstance<DataBackingStore>("Data Cache Storage");

            Assert.IsNotNull(createdStore);
            createdStore.Add(new CacheItem(key, 1, CacheItemPriority.Low, null, null));
            Assert.AreEqual(1, createdStore.Count);
            createdStore.Remove(key);
            Assert.AreEqual(0, createdStore.Count);
        }

        [TestMethod]
        public void CanResolveDataBackingStoreWithEncryptionProvider()
        {
            const string key = "fooKey";
            DataCacheStorageData data = new DataCacheStorageData("Data Cache Storage", "CachingDatabase", "fooPartition");
            settings.BackingStores.Add(data);

            CacheManagerData managerData = new CacheManagerData("defaultCacheManager", 300, 200, 100, "Data Cache Storage");
            settings.CacheManagers.Add(managerData);
            settings.DefaultCacheManager = "defaultCacheManager";
            settings.EncryptionProviders.Add(new MockStorageEncryptionProviderData("encryptionProvider", typeof(MockStorageEncryptionProviderData)));

            dbSettings.DefaultDatabase = "CachingDatabase";

            container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);
            DataBackingStore createdStore = container.GetInstance<DataBackingStore>("Data Cache Storage");

            Assert.IsNotNull(createdStore);
            createdStore.Add(new CacheItem(key, 1, CacheItemPriority.Low, null, null));
            Assert.AreEqual(1, createdStore.Count);
            createdStore.Remove(key);
            Assert.AreEqual(0, createdStore.Count);
        }
    }

    internal class MockStorageEncryptionProviderData : StorageEncryptionProviderData
    {
        public MockStorageEncryptionProviderData(string name, Type type)
            :base(name, type)
        {
        }
        protected override Expression<System.Func<IStorageEncryptionProvider>> GetCreationExpression()
        {
            return () => new MockStorageEncryptionProvider();
        }
    }
}
