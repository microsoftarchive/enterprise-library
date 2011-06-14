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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_AddingInMemoryCacheToConfigureCachingPassingNullName: Given_CachingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingInMemoryCacheNamed_ThrowsArgumentException()
        {
            ConfigureCaching
                .SetupInMemoryCacheNamed(null);
        }
    }

    [TestClass]
    public class When_AddingInMemoryCacheToConfigureCaching : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureCaching
                .SetupInMemoryCacheNamed("in memory cache");
        }

        [TestMethod]
        public void Then_InMemoryCacheIsContainedInCachesCollection()
        {
            Assert.IsTrue(base.GetCachingSettings().Caches.OfType<InMemoryCacheData>().Any());
        }

        [TestMethod]
        public void Then_InMemoryCacheHasHasSpecifiedName()
        {
            var providerData = base.GetCachingSettings().Caches.OfType<InMemoryCacheData>().First();
            Assert.AreEqual("in memory cache", providerData.Name);
        }
    }

    public abstract class Given_InMemoryCacheInConfiguration : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected ISetupInMemoryCacheNamed ConfigureInMemoryCache;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureInMemoryCache = ConfigureCaching
                .SetupInMemoryCacheNamed("in memory cache");
        }

        protected InMemoryCacheData GetInMemoryCacheData()
        {
            return base.GetCachingSettings().Caches.OfType<InMemoryCacheData>().First();
        }
    }

    [TestClass]
    public class When_LeavingDefaultOptionsInInMemory : Given_InMemoryCacheInConfiguration
    {
        [TestMethod]
        public void Then_CachingSettingsContainCacheDataWithDefaults()
        {
            var cacheData = GetInMemoryCacheData();

            Assert.AreEqual(TimeSpan.FromMinutes(2), cacheData.ExpirationPollingInterval);
            Assert.AreEqual(200, cacheData.MaxItemsBeforeScavenging);
            Assert.AreEqual(80, cacheData.ItemsLeftAfterScavenging);
        }
    }

    [TestClass]
    public class When_SettingInMemoryCacheAsDefault : Given_InMemoryCacheInConfiguration
    {
        protected override void Act()
        {
            ConfigureInMemoryCache.SetAsDefault();
        }

        [TestMethod]
        public void Then_CachingSettingsContainDefaultCache()
        {
            var cachingSettings = GetCachingSettings();
            var cacheData = GetInMemoryCacheData();

            Assert.AreEqual(cacheData.Name, cachingSettings.DefaultCache);
        }
    }

    [TestClass]
    public class When_SettingOptionalPropertiesInMemoryCache : Given_InMemoryCacheInConfiguration
    {
        protected override void Act()
        {
            ConfigureInMemoryCache.WithOptions
                .UsingExpirationPollingInterval(TimeSpan.FromSeconds(33))
                .WithScavengingThresholds(45, 40);
        }

        [TestMethod]
        public void Then_InMemoryCacheHasOptionalPropertiesSet()
        {
            var cacheData = GetInMemoryCacheData();

            Assert.AreEqual(TimeSpan.FromSeconds(33), cacheData.ExpirationPollingInterval);
            Assert.AreEqual(45, cacheData.MaxItemsBeforeScavenging);
            Assert.AreEqual(40, cacheData.ItemsLeftAfterScavenging);
        }
    }
}
