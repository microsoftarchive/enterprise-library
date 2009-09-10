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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_CallingForCachgeManagerNamedOnCachingConfigurationWithNullName : Given_CachingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            base.CachingConfiguration.ForCacheManagerNamed(null);
        }
    }

    [TestClass]
    public class When_CallingForCachgeManagerNamedOnCachingConfiguration : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CachingConfiguration.ForCacheManagerNamed("cache manager name");
        }

        [TestMethod]
        public void Then_CacheManagerConfigurationIsContainedInCachingSettings()
        {
            var cachingSettings = GetCacheManagerSettings();
            Assert.IsTrue(cachingSettings.CacheManagers.OfType<CacheManagerData>().Any());
        }


        [TestMethod]
        public void Then_CacheManagerConfigurationHasAppropriateName()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CacheManagerData>().First();

            Assert.AreEqual("cache manager name", cacheManager.Name);
        }


        [TestMethod]
        public void Then_CacheManagerConfigurationHasTheRightType()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CacheManagerData>().First();

            Assert.AreEqual(typeof(CacheManager), cacheManager.Type);
        }


        [TestMethod]
        public void Then_CacheManagerHasExpirationPollFreqOf60s()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CacheManagerData>().First();

            Assert.AreEqual(60, cacheManager.ExpirationPollFrequencyInSeconds);
        }


        [TestMethod]
        public void Then_CacheManagerHas1000ElementsInCacheBeforeScavanging()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CacheManagerData>().First();

            Assert.AreEqual(1000, cacheManager.MaximumElementsInCacheBeforeScavenging);
        }


        [TestMethod]
        public void Then_CacheManagerRemoves10ElementsBeforeScavenging()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CacheManagerData>().First();

            Assert.AreEqual(10, cacheManager.NumberToRemoveWhenScavenging);
        }

        [TestMethod]
        public void Then_CacheManagerHasNoCacheStore()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CacheManagerData>().First();

            Assert.IsTrue(String.IsNullOrEmpty(cacheManager.CacheStorage));
        }
    }

    public abstract class Given_CachgeManagerInConfigurationSource : Given_CachingSettingsInConfigurationSourceBuilder
    {
        private string cacheManagerName = "test cache manager";
        protected ICachingConfigurationCacheManager ConfigureCacheManager;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureCacheManager = CachingConfiguration.ForCacheManagerNamed(cacheManagerName);
        }

        protected CacheManagerData GetCacheManager()
        {
            var cachingConfiguration = GetCacheManagerSettings();
            return cachingConfiguration.CacheManagers.OfType<CacheManagerData>().Where(x => x.Name == cacheManagerName).First();
        }
    }

    [TestClass]
    public class When_SettingCachgeManagerAsDefault : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager = ConfigureCacheManager.UseAsDefaultCache();
        }

        [TestMethod]
        public void Then_ConfigurationHasAppropriateExpirationPollInterval()
        {
            string cachgeManagerName = GetCacheManager().Name;

            Assert.AreEqual(cachgeManagerName, GetCacheManagerSettings().DefaultCacheManager);
        }
    }

    [TestClass]
    public class When_SpecifyingExpirationPollInterval : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager = ConfigureCacheManager.WithOptions.PollWhetherItemsAreExpiredIntervalSeconds(20);
        }

        [TestMethod]
        public void Then_ConfigurationHasAppropriateExpirationPollInterval()
        {
            Assert.AreEqual(20, GetCacheManager().ExpirationPollFrequencyInSeconds);
        }
    }

    [TestClass]
    public class When_SpecifyingMaxNumberOfElementsBeforScavenging : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager = ConfigureCacheManager.WithOptions.StartScavengingAfterItemCount(50);
        }

        [TestMethod]
        public void Then_ConfigurationHasAppropriateMaximumElementsInCacheBeforeScavenging()
        {
            Assert.AreEqual(50, GetCacheManager().MaximumElementsInCacheBeforeScavenging);
        }
    }

    [TestClass]
    public class When_SpecifyingWhenScavengingRemoveItemCount : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager = ConfigureCacheManager.WithOptions.WhenScavengingRemoveItemCount(2);
        }

        [TestMethod]
        public void Then_ConfigurationHasAppropriateNumberToRemoveWhenScavenging()
        {
            Assert.AreEqual(2, GetCacheManager().NumberToRemoveWhenScavenging);
        }
    }

    [TestClass]
    public class When_SpecifyingSharedBackingStore : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            ConfigureCacheManager.StoreInSharedBackingStore("shared Store");
        }

        [TestMethod]
        public void Then_ConfigurationHasCacheStorageSetToSharedStore()
        {
            Assert.AreEqual("shared Store", GetCacheManager().CacheStorage);
        }
    }
}
