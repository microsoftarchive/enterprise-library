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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Unity
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderPolicyCreatorFixture
    {
        private const string providerName = "foo";
        private IUnityContainer container;
        private CacheManagerSettings settings;
        private DatabaseSettings dbSettings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            container = new UnityContainer();

            settings = new CacheManagerSettings();
            dbSettings = new DatabaseSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, settings);
            configurationSource.Add(DatabaseSettings.SectionName, dbSettings);

            container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
        }

        [TestCleanup]
        public void TearDown()
        {
            container.Dispose();
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

            container.AddExtension(new DataAccessBlockExtension());
            container.AddExtension(new CachingBlockExtension());

            DataBackingStore createdStore = container.Resolve<DataBackingStore>("Data Cache Storage");

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
            settings.EncryptionProviders.Add(new StorageEncryptionProviderData("encryptionProvider", typeof(MockStorageEncryptionProviderData)));

            dbSettings.DefaultDatabase = "CachingDatabase";

            container.AddExtension(new DataAccessBlockExtension());
            container.AddExtension(new CachingBlockExtension());

            DataBackingStore createdStore = container.Resolve<DataBackingStore>("Data Cache Storage");

            Assert.IsNotNull(createdStore);
            createdStore.Add(new CacheItem(key, 1, CacheItemPriority.Low, null, null));
            Assert.AreEqual(1, createdStore.Count);
            createdStore.Remove(key);
            Assert.AreEqual(0, createdStore.Count);
        }
    }
}
