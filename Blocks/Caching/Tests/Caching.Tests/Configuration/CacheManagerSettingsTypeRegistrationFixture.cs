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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationForEmptyCacheManagerSettings
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CacheManagerSettings settings = new CacheManagerSettings();
            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForCacheManagers()
        {
            Assert.AreEqual(0, registrations.Where(r => r.ServiceType == typeof(CacheManager)).Count());
        }
    }

    [TestClass]
    public class GivenRegistrationForMinimalCacheManagerSettings
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CacheStorageData cacheStorageData = new CacheStorageData("Null Storage", typeof(NullBackingStore));
            CacheManagerData cacheManagerData = new CacheManagerData("Default Cache Manager", 10, 10, 10, cacheStorageData.Name);
            
            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(cacheManagerData);
            settings.BackingStores.Add(cacheStorageData);

            settings.DefaultCacheManager = cacheManagerData.Name;

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenReturnsRegistrationForCacheManager()
        {
            TypeRegistration cacheManagerRegistration = registrations.Where(r => r.ServiceType == typeof(ICacheManager)).FirstOrDefault();

            Assert.IsNotNull(cacheManagerRegistration);
            Assert.AreEqual(typeof(CacheManager), cacheManagerRegistration.ImplementationType);
        }

        [TestMethod]
        public void ThenRegistrationForCacheManagerIsDefault()
        {
            TypeRegistration cacheManagerRegistration = registrations.Where(r => r.ServiceType == typeof(ICacheManager)).FirstOrDefault();

            Assert.IsNotNull(cacheManagerRegistration);
            Assert.IsTrue(cacheManagerRegistration.IsDefault);
        }

        [TestMethod]
        public void ThenCacheManagerRegistrationIsDefault()
        {
            TypeRegistration cacheManagerRegistration = registrations.Where(r => r.ServiceType == typeof(ICacheManager)).FirstOrDefault();

            Assert.IsNotNull(cacheManagerRegistration);
            Assert.IsTrue(cacheManagerRegistration.IsDefault);
        }
        
        [TestMethod]
        public void ThenCacheManagerRegistrationIsPublicName()
        {
            TypeRegistration cacheManagerRegistration =
                registrations.Where(r => r.ServiceType == typeof (ICacheManager)).First();
            Assert.IsTrue(cacheManagerRegistration.IsPublicName);
        }

        [TestMethod]
        public void ThenReturnsRegistrationForNullBackingStore()
        {
            TypeRegistration backingStoreRegistration = registrations.Where(r => r.ServiceType == typeof(IBackingStore)).FirstOrDefault();
            
            Assert.IsNotNull(backingStoreRegistration);
            Assert.AreEqual(typeof(NullBackingStore), backingStoreRegistration.ImplementationType);
        }

        [TestMethod]
        public void ThenReturnsNoRegistrationsForIStorageEncryptionProvider()
        {
            Assert.AreEqual(0, registrations.Where(r => r.ServiceType == typeof(IStorageEncryptionProvider)).Count());
        }


        [TestMethod]
        public void ThenReturnsRegistrationForInternalCache()
        {
            TypeRegistration internalCacheRegistration = registrations.Where(r => r.ServiceType == typeof(Cache)).FirstOrDefault();

            Assert.IsNotNull(internalCacheRegistration);
        }

        [TestMethod]
        public void ThenRegistrationForInternalCacheIsRegistredWithCacheManagerName()
        {
            TypeRegistration internalCacheRegistration = registrations.Where(r => r.ServiceType == typeof(Cache)).FirstOrDefault();

            Assert.IsNotNull(internalCacheRegistration);
            Assert.AreEqual("Default Cache Manager", internalCacheRegistration.Name);
        }


        [TestMethod]
        public void ThenReturnsRegistrationForInstrumentationProvider()
        {
            TypeRegistration instrumentationProviderRegistration = registrations.Where(r => r.ServiceType == typeof(ICachingInstrumentationProvider)).FirstOrDefault();

            Assert.IsNotNull(instrumentationProviderRegistration);
        }

        [TestMethod]
        public void ThenRegistrationForInstrumentationProviderIsRegistredWithCacheManagerName()
        {
            TypeRegistration instrumentationProviderRegistration = registrations.Where(r => r.ServiceType == typeof(ICachingInstrumentationProvider)).FirstOrDefault();

            Assert.IsNotNull(instrumentationProviderRegistration);
            Assert.AreEqual("Default Cache Manager", instrumentationProviderRegistration.Name);
        }

        [TestMethod]
        public void ThenRegistrationForDefaultEventLoggerIsDefault()
        {
            var registration = registrations.Where(r => r.ServiceType == typeof(DefaultCachingEventLogger)).First();

            registration.AssertForServiceType(typeof(DefaultCachingEventLogger))
                .IsDefault();
        }
    }

    [TestClass]
    public class GivenRegistrationForCacheManagerWithIsolatedStorage
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            IsolatedStorageCacheStorageData isolatedStorageData = new IsolatedStorageCacheStorageData("Isolated Storage", string.Empty, "part");
            CacheManagerData cacheManagerData = new CacheManagerData("Default Cache Manager", 10, 10, 10, isolatedStorageData.Name);

            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(cacheManagerData);
            settings.BackingStores.Add(isolatedStorageData);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenReturnsRegistrationForIsolatedStorageStore()
        {
            TypeRegistration backingStoreRegistration = registrations.Where(r => r.ServiceType == typeof(IBackingStore)).FirstOrDefault();

            Assert.IsNotNull(backingStoreRegistration);
            Assert.AreEqual(typeof(IsolatedStorageBackingStore), backingStoreRegistration.ImplementationType);
        }

        [TestMethod]
        public void ThenBackingStoreRegistrationIsNotPublicName()
        {
            TypeRegistration backingStoreRegistration = registrations.Where(r => r.ServiceType == typeof(IBackingStore)).First();

            Assert.IsFalse(backingStoreRegistration.IsPublicName);
        }

    }

    [TestClass]
    public class GivenRegistrationForCacheManagerWithCustomBackingStore
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CustomCacheStorageData customStorageData = new CustomCacheStorageData("Custom Storage", typeof(MockCustomStorageBackingStore));
            CacheManagerData cacheManagerData = new CacheManagerData("Default Cache Manager", 10, 10, 10, customStorageData.Name);

            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(cacheManagerData);
            settings.BackingStores.Add(customStorageData);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationForCustomStorage()
        {
            TypeRegistration customStoreRegistration = registrations.Where(r=>r.ServiceType == typeof(IBackingStore)).FirstOrDefault();

            Assert.IsNotNull(customStoreRegistration);
            Assert.AreEqual(typeof(MockCustomStorageBackingStore), customStoreRegistration.ImplementationType);

        }
    }

    [TestClass]
    public class GivenRegistrationForCacheManagerAndCustomCacheManager
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CustomCacheManagerData customCacheManager = new CustomCacheManagerData("Custom Cache Manager", typeof(CustomCacheManager));

            CacheStorageData cacheStorageData = new CacheStorageData("Null Storage", typeof(NullBackingStore));
            CacheManagerData cacheManagerData = new CacheManagerData("Default Cache Manager", 10, 10, 10, cacheStorageData.Name);

            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(cacheManagerData);
            settings.BackingStores.Add(cacheStorageData);

            settings.CacheManagers.Add(customCacheManager);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenReturnsMultipleInstancesOfCacheManager()
        {
            Assert.AreEqual(2, registrations.Where(r => r.ServiceType == typeof(ICacheManager)).Count());
        }

        [TestMethod]
        public void ThenInstrumentationProviderIsOnlyRegisteredForCacheManager()
        {
            Assert.AreEqual(1, registrations.Where(r => r.ServiceType == typeof(ICachingInstrumentationProvider)).Count());
            
            TypeRegistration registrationForInstrumentationProvider = registrations.Where(r => r.ServiceType == typeof(ICachingInstrumentationProvider)).First();
            Assert.AreEqual("Default Cache Manager", registrationForInstrumentationProvider.Name);
        }

        [TestMethod]
        public void ThenInternalCacheIsOnlyRegisteredForCacheManager()
        {
            Assert.AreEqual(1, registrations.Where(r => r.ServiceType == typeof(Cache)).Count());

            TypeRegistration registrationForInternalCache = registrations.Where(r => r.ServiceType == typeof(Cache)).First();
            Assert.AreEqual("Default Cache Manager", registrationForInternalCache.Name);
        }
    }


    [TestClass]
    public class GivenRegistrationWithIncompatibleTypeInCustomCacheManager
    {
        private CacheManagerSettings settings;

        [TestInitialize]
        public void Setup()
        {
            CustomCacheManagerData customCacheManager = new CustomCacheManagerData();
            customCacheManager.Name = "Custom Cache Manager";
            customCacheManager.Type = typeof(FaultyType);

            settings = new CacheManagerSettings();
            settings.CacheManagers.Add(customCacheManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenGettingRegistrationsThrows()
        {
            settings.GetRegistrations(null).ToList();
        }


        private class FaultyType
        {
            public FaultyType(NameValueCollection nvc)
            {
            }
        }
    }

    [TestClass]
    public class GivenRegistrationWithIncompatibleTypeInCustomBackingStore
    {
        private CacheManagerSettings settings;

        [TestInitialize]
        public void Setup()
        {
            CustomCacheStorageData customStoreData = new CustomCacheStorageData();
            customStoreData.Name = "Custom Store";
            customStoreData.Type = typeof(FaultyType);

            settings = new CacheManagerSettings();
            settings.BackingStores.Add(customStoreData);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenGettingRegistrationsThrows()
        {
            settings.GetRegistrations(null).ToList();
        }


        private class FaultyType
        {
            public FaultyType(NameValueCollection nvc)
            {
            }
        }
    }


    [TestClass]
    public class GivenRegistrationWithBackingStoreAsCacheStorageData
    {
        private CacheManagerSettings settings;

        [TestInitialize]
        public void Setup()
        {
            CacheStorageData storeData = new CacheStorageData("cache store",typeof(BackingStoreWithoutDefaultCtor));

            settings = new CacheManagerSettings();
            settings.BackingStores.Add(storeData);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ThenGetRegistrationsWillThrowIfTypeDoesntHaveDefaultCtor()
        {
            settings.GetRegistrations(null).ToList();
        }


        private class BackingStoreWithoutDefaultCtor : IBackingStore
        {
            public BackingStoreWithoutDefaultCtor(string arg)
            {
            }

            #region IBackingStore Members

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

            public System.Collections.Hashtable Load()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
