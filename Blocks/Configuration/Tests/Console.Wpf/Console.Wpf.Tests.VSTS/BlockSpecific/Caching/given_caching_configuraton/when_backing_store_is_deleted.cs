//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Caching
{ 
    [TestClass]
    public class when_backing_store_is_deleted : given_caching_configuraton.given_caching_configuration
    {
        ElementViewModel CacheManagerBackingStore;

        protected override void Arrange()
        {
            base.Arrange();

            var cacheManagersCollection = CacheSectionViewModel.GetDescendentsOfType<NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData>>().First();
            CacheManagerBackingStore = ((ElementCollectionViewModel)cacheManagersCollection).AddNewCollectionElement(typeof(IsolatedStorageCacheStorageData));

            CacheManager.Property("CacheStorage").Value = CacheManagerBackingStore.Name;
        }

        protected override void Act()
        {
            CacheManagerBackingStore.Delete();
        }

        [TestMethod]
        public void then_cache_manager_backing_store_is_reset_to_null_store()
        {
            string nullBackingStoreName = ((CacheManagerSectionViewModel)base.CacheSectionViewModel).NullBackingStoreName;
            Assert.AreEqual(nullBackingStoreName, ((CacheManagerData)base.CacheManager.ConfigurationElement).CacheStorage);
        }
    }
}
