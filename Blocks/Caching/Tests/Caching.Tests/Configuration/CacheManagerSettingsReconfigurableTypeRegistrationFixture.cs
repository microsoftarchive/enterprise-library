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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration
{
    [TestClass]
    public class GivenUpdatableConfigurationSource
    {
        ConfigurationSourceUpdatable configurationSource;
        CacheManagerSettings cacheSettings;
        UnityContainerConfigurator configurator;
        IServiceLocator serviceLcoator;
        CacheManagerData cacheManagerData;
        CacheStorageData cacheStorageData;
        StorageEncryptionProviderData storageEncryptionProviderData;

        [TestInitialize]
        public void Setup()
        {
            configurationSource = new ConfigurationSourceUpdatable();
            cacheSettings = new CacheManagerSettings();

            configurationSource.Add(null, CacheManagerSettings.SectionName, cacheSettings);

            cacheStorageData = new CacheStorageData("Null Storage", typeof(NullBackingStore));
            cacheManagerData = new CacheManagerData("Default Cache Manager", 10, 10, 10, cacheStorageData.Name);
            
            CacheManagerSettings settings = new CacheManagerSettings();
            cacheSettings.CacheManagers.Add(cacheManagerData);
            cacheSettings.BackingStores.Add(cacheStorageData);
            cacheSettings.DefaultCacheManager = cacheManagerData.Name;

            UnityContainer container = new UnityContainer();
            configurator = new UnityContainerConfigurator(container);
            serviceLcoator = new UnityServiceLocator(container);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
        }

        [TestMethod]
        public void ThenReturnsNewlyConfiguredInstanceAfterReconfigureOfCacheManager()
        {
            cacheManagerData.Name = "New Name";
            configurationSource.DoSourceChanged(new string[]{CacheManagerSettings.SectionName});

            CacheManagerFactory factory = new CacheManagerFactory(serviceLcoator);
            ICacheManager cacheManager = factory.Create("New Name");
            Assert.IsNotNull(cacheManager);
        }
       
        [TestMethod]
        public void ThenReturnsNewlyConfiguredInstanceAfterReconfigureOfBackingStore()
        {
            MockCustomStorageBackingStore.Instantiated = false;

            cacheSettings.BackingStores.Remove("Null Storage");
            cacheSettings.BackingStores.Add(new CustomCacheStorageData("Custom Store", typeof(MockCustomStorageBackingStore)));
            cacheManagerData.CacheStorage = "Custom Store";

            configurationSource.DoSourceChanged(new string[] { CacheManagerSettings.SectionName });

            CacheManagerFactory factory = new CacheManagerFactory(serviceLcoator);
            ICacheManager cacheManager = factory.Create(cacheSettings.DefaultCacheManager);
            Assert.IsTrue(MockCustomStorageBackingStore.Instantiated);
        }

        [TestMethod]
        public void ThenReturnsNewlyConfiguredInstanceAfterReconfigureOfStorageEncryptionProvider()
        {
            MockStorageEncryptionProvider.Instantiated = false;

            storageEncryptionProviderData = new MockStorageEncryptionProviderData("Storage Encryption");
            cacheSettings.EncryptionProviders.Add(storageEncryptionProviderData);
            
            IsolatedStorageCacheStorageData isolatedStoreData = new IsolatedStorageCacheStorageData();
            isolatedStoreData.Name = "Isolated Storage";
            isolatedStoreData.PartitionName = "entlib";
            isolatedStoreData.StorageEncryption = storageEncryptionProviderData.Name;

            cacheSettings.BackingStores.Add(isolatedStoreData);
            
            cacheManagerData.CacheStorage = isolatedStoreData.Name;
            configurationSource.DoSourceChanged(new string[] { CacheManagerSettings.SectionName });

            CacheManagerFactory factory = new CacheManagerFactory(serviceLcoator);
            ICacheManager cacheManager = factory.Create(cacheSettings.DefaultCacheManager);
            Assert.IsTrue(MockStorageEncryptionProvider.Instantiated);
        }
    }


}
