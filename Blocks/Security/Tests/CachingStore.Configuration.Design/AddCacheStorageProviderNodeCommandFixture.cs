//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.Tests
{
    [TestClass]
    public class AddCacheStorageProviderNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AddCacheStorageProviderNodeCommandAddsCachingStoreNode()
        {
            AddCachingStoreProviderNodeCommand command = new AddCachingStoreProviderNodeCommand(ServiceProvider);
            command.Execute(ApplicationNode);

            Assert.IsNotNull(Hierarchy.FindNodeByType(ApplicationNode, typeof(CachingStoreProviderNode)));
        }

        [TestMethod]
        public void AddCacheStorageProviderNodeCommandAdsCacheManagerIfNotExists()
        {
            AddCachingStoreProviderNodeCommand command = new AddCachingStoreProviderNodeCommand(ServiceProvider);
            command.Execute(ApplicationNode);

            CacheManagerNode cacheManager = (CacheManagerNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(CacheManagerNode));
            CachingStoreProviderNode securityCachingStore = (CachingStoreProviderNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(CachingStoreProviderNode));
            Assert.IsNotNull(Hierarchy.FindNodeByType(ApplicationNode, typeof(CacheManagerSettingsNode)));
            Assert.AreEqual(cacheManager, securityCachingStore.CacheManager);
        }
    }
}