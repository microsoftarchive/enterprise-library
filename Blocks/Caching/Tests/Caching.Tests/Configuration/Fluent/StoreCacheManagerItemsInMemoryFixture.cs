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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_StoringCacheManagerItemsInMemory : Given_CachgeManagerInConfigurationSource
    {
        protected override void Act()
        {
            base.ConfigureCacheManager.StoreInMemory();
        }

        [TestMethod]
        public void Then_CacheManagerHasBackingStore()
        {
            Assert.IsFalse(string.IsNullOrEmpty(GetCacheManager().CacheStorage));
        }

        [TestMethod]
        public void Then_CacheManagerSettingsContainsNullBackingStore()
        {
            var cacheManagerSettings = GetCacheManagerSettings();
            Assert.IsTrue(cacheManagerSettings.BackingStores.Where(x => x.Type == typeof(NullBackingStore)).Any());
        }
    }

    [TestClass]
    public class When_StoringMultipleCacheManagersInMemory : Given_CachgeManagerInConfigurationSource
    {
        string otherCacheManager = "other cache manager";

        protected override void Act()
        {
            base.ConfigureCacheManager.StoreInMemory()
                .ForCacheManagerNamed(otherCacheManager).StoreInMemory();
        }

        private CacheManagerData GetOtherCacheManager()
        {
            return GetCacheManagerSettings().CacheManagers.OfType<CacheManagerData>().Where(x => x.Name == otherCacheManager).FirstOrDefault();
        }
        [TestMethod]
        public void Then_BothCacheManagersShareBackingStore()
        {
            var cacheManager = base.GetCacheManager();
            var otherCacheManager = GetOtherCacheManager();

            Assert.AreEqual(cacheManager.CacheStorage, otherCacheManager.CacheStorage);

        }
    }
}
