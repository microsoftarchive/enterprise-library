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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class IsolatedStorageCacheStorageNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInIsolatedStorageThrows()
        {
            new IsolatedStorageCacheStorageNode(null);
        }

        [TestMethod]
        public void EnsureIsolatedStorageCacheStorageNodePropertyCatoriesAndDescriptions()
        {
            Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(IsolatedStorageCacheStorageNode), "PartitionName", Resources.IsolatedStorageAreaNameDescription));
            Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(IsolatedStorageCacheStorageNode), "PartitionName"));
        }

        [TestMethod]
        public void CanCreateIsolatedStorageCacheStorageNodeByDataType()
        {
            ConfigurationNode node = ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(IsolatedStorageCacheStorageData), new object[] { new IsolatedStorageCacheStorageData() });
            Assert.IsNotNull(node);
            Assert.AreEqual(typeof(IsolatedStorageCacheStorageNode), node.GetType());
        }

        [TestMethod]
        public void IsolatedStorageCacheStorageNodeTest()
        {
            string isolatedStorageName = "IsolatedStorage";
            string isolatedStoragePartitionName = "testStoragePartitionName";

            IsolatedStorageCacheStorageNode node = new IsolatedStorageCacheStorageNode();
            node.Name = isolatedStorageName;
            node.PartitionName = isolatedStoragePartitionName;

            Assert.AreEqual(isolatedStoragePartitionName, node.PartitionName);
            Assert.AreEqual(isolatedStorageName, node.Name);

            IsolatedStorageCacheStorageData data = (IsolatedStorageCacheStorageData)node.CacheStorageData;
            Assert.AreEqual(isolatedStoragePartitionName, data.PartitionName);
            Assert.AreEqual(isolatedStorageName, data.Name);
        }

        [TestMethod]
        public void IsolatedStorageCacheStorageDataTest()
        {
            string isolatedStorageName = "IsolatedStorage";
            string isolatedStoragePartitionName = "testStoragePartitionName";

            IsolatedStorageCacheStorageData data = new IsolatedStorageCacheStorageData();
            data.PartitionName = isolatedStoragePartitionName;
            data.Name = isolatedStorageName;
            Assert.AreEqual(isolatedStorageName, data.Name);

            IsolatedStorageCacheStorageNode node = new IsolatedStorageCacheStorageNode(data);
            Assert.AreEqual(isolatedStoragePartitionName, node.PartitionName);
            Assert.AreEqual(isolatedStorageName, node.Name);
        }
    }
}
