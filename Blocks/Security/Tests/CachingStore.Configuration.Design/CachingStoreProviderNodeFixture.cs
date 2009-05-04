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

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.Tests
{
    [TestClass]
    public class CachingStoreProviderNodeFixture : ConfigurationDesignHost
    {
        protected override void InitializeCore()
        {
            new SecurityCacheCachingStoreConfigurationDesignManager().Register(ServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCachingStoreProviderNodeThrows()
        {
            new CachingStoreProviderNode(null);
        }

        [TestMethod]
        public void CachingStoreProviderNodeDefaults()
        {
            CachingStoreProviderNode cacheStoreNode = new CachingStoreProviderNode();
            Assert.AreEqual("Caching Store Provider", cacheStoreNode.Name);
            Assert.AreEqual(60, cacheStoreNode.AbsoluteExpiration);
            Assert.AreEqual(10, cacheStoreNode.SlidingExpiration);
            Assert.AreEqual(null, cacheStoreNode.CacheManager);
        }

        [TestMethod]
        public void CanCreateCachingStoreProviderNodeFromData()
        {
            ConfigurationNode createdNode = ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(CachingStoreProviderData), new object[] { new CachingStoreProviderData() });
            Assert.IsNotNull(createdNode);
            Assert.AreEqual(typeof(CachingStoreProviderNode), createdNode.GetType());
        }

        [TestMethod]
        public void CachingStoreProviderNodeTest()
        {
            string name = "some name";
            int absoluteExpiration = 123;
            int slidingExpiration = 345;

            CachingStoreProviderData data = new CachingStoreProviderData();
            data.Name = name;
            data.AbsoluteExpiration = absoluteExpiration;
            data.SlidingExpiration = slidingExpiration;

            CachingStoreProviderNode node = new CachingStoreProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(absoluteExpiration, node.AbsoluteExpiration);
            Assert.AreEqual(slidingExpiration, node.SlidingExpiration);
        }

        [TestMethod]
        public void CachingStoreProviderNodeDataTest()
        {
            string name = "some name";
            int absoluteExpiration = 123;
            int slidingExpiration = 345;

            CachingStoreProviderData data = new CachingStoreProviderData();
            data.Name = name;
            data.AbsoluteExpiration = absoluteExpiration;
            data.SlidingExpiration = slidingExpiration;

            CachingStoreProviderNode node = new CachingStoreProviderNode(data);
            ApplicationNode.AddNode(node);

            CachingStoreProviderData nodeData = (CachingStoreProviderData)node.SecurityCacheProviderData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(absoluteExpiration, nodeData.AbsoluteExpiration);
            Assert.AreEqual(slidingExpiration, nodeData.SlidingExpiration);
        }
    }
}
