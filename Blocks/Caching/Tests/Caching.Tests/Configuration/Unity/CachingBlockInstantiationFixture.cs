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

using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Unity
{
    [TestClass]
    public class CachingBlockInstantiationFixture
    {
        private CacheManagerSettings settings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            settings = new CacheManagerSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, settings);
        }

        [TestCleanup]
        public void TearDown()
        {
            
        }

        [TestMethod]
        public void CanCreateCustomCacheStorage()
        {
            CustomCacheStorageData data = new CustomCacheStorageData("name", typeof(MockCustomStorageBackingStore));
            data.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
            settings.BackingStores.Add(data);

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);

            MockCustomStorageBackingStore createdObject = (MockCustomStorageBackingStore)container.GetInstance<IBackingStore>("name");

            Assert.IsNotNull(createdObject);
            Assert.AreEqual("value1", createdObject.customValue);
        }

        [TestMethod]
        public void CanCreateIsolatedCacheStorageWithoutEncryption()
        {
            IsolatedStorageCacheStorageData data = new IsolatedStorageCacheStorageData("name", "", "partition");
            settings.BackingStores.Add(data);

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);

            IsolatedStorageBackingStore createdObject = (IsolatedStorageBackingStore)container.GetInstance<IBackingStore>("name");

            Assert.IsNotNull(createdObject);
        }

        //[TestMethod]
        //public void CanCreateIsolatedCacheStorageWithEncryption()
        //{
        //     encryptionData = new CustomCacheStorageData("encryption", typeof(MockCustomStorageBackingStore));
        //    encryptionData.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
        //    settings.BackingStores.Add(encryptionData);

        //    IsolatedStorageCacheStorageData data = new IsolatedStorageCacheStorageData("name", "encryption", "partition");
        //    settings.BackingStores.Add(data);

        //    container.AddExtension(new CachingBlockExtension());

        //    IsolatedStorageBackingStore createdObject = (IsolatedStorageBackingStore)container.Resolve<IBackingStore>("name");

        //    Assert.IsNotNull(createdObject);
        //}

        [TestMethod]
        public void CanCreateCustomCacheManager()
        {
            CustomCacheManagerData data = new CustomCacheManagerData("name", typeof(CustomCacheManager));
            data.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
            settings.CacheManagers.Add(data);

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);

            CustomCacheManager createdObject = (CustomCacheManager)container.GetInstance<ICacheManager>("name");

            Assert.IsNotNull(createdObject);
            Assert.AreEqual("value1", createdObject.customValue);
        }

        [TestMethod]
        public void CanCreateDefaultCacheManager()
        {
            CustomCacheManagerData data = new CustomCacheManagerData("name", typeof(CustomCacheManager));
            data.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
            settings.CacheManagers.Add(data);

            settings.DefaultCacheManager = "name";

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);

            CustomCacheManager createdObject = (CustomCacheManager)container.GetInstance<ICacheManager>();

            Assert.IsNotNull(createdObject);
            Assert.AreEqual("value1", createdObject.customValue);
        }

        [TestMethod]
        public void CanCreateCacheManager()
        {
            CacheStorageData data = new CacheStorageData("storage", typeof(NullBackingStore));
            settings.BackingStores.Add(data);

            CacheManagerData managerData = new CacheManagerData("name", 300, 200, 100, "storage");
            settings.CacheManagers.Add(managerData);

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);

            CacheManager createdObject = (CacheManager)container.GetInstance<ICacheManager>("name");

            Assert.IsNotNull(createdObject);

            // does it work?
            object value = new object();
            createdObject.Add("key", value);
            Assert.AreSame(value, createdObject.GetData("key"));
        }

        [TestMethod]
        public void CacheManagerIsSingleton()
        {
            CacheStorageData data = new CacheStorageData("storage", typeof(NullBackingStore));
            settings.BackingStores.Add(data);

            CacheManagerData managerData = new CacheManagerData("name", 300, 200, 100, "storage");
            settings.CacheManagers.Add(managerData);

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);

            CacheManager createdObject1 = (CacheManager)container.GetInstance<ICacheManager>("name");
            CacheManager createdObject2 = (CacheManager)container.GetInstance<ICacheManager>("name");

            Assert.IsNotNull(createdObject1);
            Assert.AreSame(createdObject1, createdObject2);
        }

        [TestMethod]
        public void CheckLifetime()
        {
            CustomCacheManagerData data = new CustomCacheManagerData("name", typeof(CustomCacheManager));
            settings.CacheManagers.Add(data);

            CustomCacheManager createdObject = null;

            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);
            createdObject = (CustomCacheManager) container.GetInstance<ICacheManager>("name");
            Assert.IsFalse(createdObject.wasDisposed);

            container.Dispose();
            Assert.IsTrue(createdObject.wasDisposed);
        }
    }
}
