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
    public class when_backing_store_is_found : given_caching_configuraton.given_caching_configuration
    {
        ElementReferenceProperty backingStoreProperty;

        protected override void Arrange()
        {
            base.Arrange();

            backingStoreProperty = (ElementReferenceProperty)CacheManager.Property("CacheStorage");
            backingStoreProperty.Value = "backing store";
        }

        protected override void Act()
        {
            var cacheManagersCollection = CacheSectionViewModel.GetDescendentsOfType<NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData>>().First();
            var cacheManagerBackingStore = ((ElementCollectionViewModel)cacheManagersCollection).AddNewCollectionElement(typeof(IsolatedStorageCacheStorageData));
            cacheManagerBackingStore.NameProperty.Value = "backing store";
        }

        [TestMethod]
        public void then_cache_manager_backing_store_has_reference()
        {
            Assert.IsNotNull(backingStoreProperty.ReferencedElement);
        }
    }
}
