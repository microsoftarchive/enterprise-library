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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_AddingIsolatedStorageCacheToConfigureCachingPassingNullName: Given_CachingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingIsolatedStorageCacheNamed_ThrowsArgumentException()
        {
            ConfigureCaching
                .SetupIsolatedStorageCacheNamed(null);
        }
    }

    [TestClass]
    public class When_AddingIsolatedStorageCacheToConfigureCaching : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureCaching
                .SetupIsolatedStorageCacheNamed("isolated storage cache");
        }

        [TestMethod]
        public void Then_IsolatedStorageCacheIsContainedInCachesCollection()
        {
            Assert.IsTrue(base.GetCachingSettings().Caches.OfType<IsolatedStorageCacheData>().Any());
        }

        [TestMethod]
        public void Then_IsolatedStorageCacheHasHasSpecifiedName()
        {
            var providerData = base.GetCachingSettings().Caches.OfType<IsolatedStorageCacheData>().First();
            Assert.AreEqual("isolated storage cache", providerData.Name);
        }
    }

    public abstract class Given_IsolatedStorageCacheInConfiguration : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected ISetupIsolatedStorageCacheNamed ConfigureIsolatedStorageCache;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureIsolatedStorageCache = ConfigureCaching
                .SetupIsolatedStorageCacheNamed("isolated storage cache");
        }

        protected IsolatedStorageCacheData GetIsolatedStorageCacheData()
        {
            return base.GetCachingSettings().Caches.OfType<IsolatedStorageCacheData>().First();
        }
    }

    [TestClass]
    public class When_LeavingDefaultOptionsInIsolatedStorage : Given_IsolatedStorageCacheInConfiguration
    {
        [TestMethod]
        public void Then_CachingSettingsContainCacheDataWithDefaults()
        {
            var cacheData = GetIsolatedStorageCacheData();

            Assert.AreEqual(TimeSpan.FromMinutes(2), cacheData.ExpirationPollingInterval);
            Assert.AreEqual(80, cacheData.PercentOfQuotaUsedBeforeScavenging);
            Assert.AreEqual(60, cacheData.PercentOfQuotaUsedAfterScavenging);
            Assert.AreEqual(1024, cacheData.MaxSizeInKilobytes);
            Assert.AreEqual(typeof(IsolatedStorageCacheEntrySerializer), cacheData.SerializerType);
        }
    }

    [TestClass]
    public class When_SettingIsolatedStorageCacheAsDefault : Given_IsolatedStorageCacheInConfiguration
    {
        protected override void Act()
        {
            ConfigureIsolatedStorageCache.SetAsDefault();
        }

        [TestMethod]
        public void Then_CachingSettingsContainDefaultCache()
        {
            var cachingSettings = GetCachingSettings();
            var cacheData = GetIsolatedStorageCacheData();

            Assert.AreEqual(cacheData.Name, cachingSettings.DefaultCache);
        }
    }

    [TestClass]
    public class When_SettingNullSerializerTypeOnIsolatedStorageCache : Given_IsolatedStorageCacheInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_UsingSerializerOfType_ThrowsArgumentNullException()
        {
            ConfigureIsolatedStorageCache.WithOptions.UsingSerializerOfType(null);
        }
    }

    [TestClass]
    public class When_SettingNonSerializerTypeOnIsolatedStorageCache : Given_IsolatedStorageCacheInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UsingSerializerOfType_ThrowsArgumentNullException()
        {
            ConfigureIsolatedStorageCache.WithOptions.UsingSerializerOfType(typeof(object));
        }
    }

    [TestClass]
    public class When_SettingAlgorithmOnIsolatedStorageCache : Given_IsolatedStorageCacheInConfiguration
    {
        protected override void Act()
        {
            ConfigureIsolatedStorageCache.WithOptions.UsingSerializerOfType(typeof(MockSerializer));
        }

        [TestMethod]
        public void Then_IsolatedStorageCacheHasSerializerTypeSet()
        {
            var cacheData = GetIsolatedStorageCacheData();

            Assert.AreEqual(typeof(MockSerializer), cacheData.SerializerType);
        }

        public class MockSerializer : IIsolatedStorageCacheEntrySerializer
        {
            public byte[] Serialize(IsolatedStorageCacheEntry entry)
            {
                throw new NotImplementedException();
            }

            public IsolatedStorageCacheEntry Deserialize(byte[] serializedEntry)
            {
                throw new NotImplementedException();
            }

            public EntryUpdate GetUpdateForLastUpdateTime(IsolatedStorageCacheEntry entry)
            {
                throw new NotImplementedException();
            }
        }
    }

    [TestClass]
    public class When_SettingOptionalPropertiesIsolatedStorageCache : Given_IsolatedStorageCacheInConfiguration
    {
        protected override void Act()
        {
            ConfigureIsolatedStorageCache.WithOptions
                .UsingExpirationPollingInterval(TimeSpan.FromSeconds(33))
                .WithScavengingThresholds(67, 66)
                .WithMaxSizeInKilobytes(900);
        }

        [TestMethod]
        public void Then_IsolatedStorageCacheHasOptionalPropertiesSet()
        {
            var cacheData = GetIsolatedStorageCacheData();

            Assert.AreEqual(TimeSpan.FromSeconds(33), cacheData.ExpirationPollingInterval);
            Assert.AreEqual(67, cacheData.PercentOfQuotaUsedBeforeScavenging);
            Assert.AreEqual(66, cacheData.PercentOfQuotaUsedAfterScavenging);
            Assert.AreEqual(900, cacheData.MaxSizeInKilobytes);
        }
    }
}
