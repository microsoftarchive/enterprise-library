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
using System.Collections;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class CacheStorageNodeFixture : ConfigurationDesignHost
    {
        protected override void InitializeCore()
        {
            ServiceHelper.GetUIHierarchyService(ServiceProvider).SelectedHierarchy = Hierarchy;

            CachingConfigurationDesignManager manager = new CachingConfigurationDesignManager();
            manager.Register(ServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCacheStorageNodeThrows()
        {
            new CustomCacheStorageNode(null);
        }

        [TestMethod]
        public void CanCreateCacheStorageNodeByDataType()
        {
            ConfigurationNode node = ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(CustomCacheStorageData), new object[] { new CustomCacheStorageData() });
            Assert.IsNotNull(node);
            Assert.AreEqual(typeof(CustomCacheStorageNode), node.GetType());
        }

        [TestMethod]
        public void CustomCacheStorageNodeTest()
        {
            string name = "testName1";
            Type type = typeof(CustomCacheStorageData);
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("test", "value");

            CustomCacheStorageNode node = new CustomCacheStorageNode();
            ApplicationNode.AddNode(node);
            Assert.AreEqual("Cache Storage", node.Name);

            node.Type = type.AssemblyQualifiedName;
            node.Name = name;
            node.Attributes.Add(new EditableKeyValue(attributes.GetKey(0), attributes[attributes.GetKey(0)]));

            Assert.AreEqual(attributes[0], node.Attributes[0].Value);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(name, node.Name);

            CustomCacheStorageData nodeData = (CustomCacheStorageData)node.CacheStorageData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributes.AllKeys[0], nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributes.Get(0), nodeData.Attributes.Get(0));
        }

        [TestMethod]
        public void CustomCacheStorageDataTest()
        {
            string name = "testName2";
            Type type = typeof(CacheStorageNodeFixture);
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("test", "value");

            CustomCacheStorageData data = new CustomCacheStorageData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributes.GetKey(0), attributes[attributes.GetKey(0)]);

            CustomCacheStorageNode node = new CustomCacheStorageNode(data);
            ApplicationNode.AddNode(node);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);

            Assert.AreEqual(attributes.AllKeys[0], node.Attributes[0].Key);
            Assert.AreEqual(attributes.Get(0), node.Attributes[0].Value);
        }

        class MyBackingStore : BaseBackingStore
        {
            public override int Count
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            protected override void AddNewItem(int storageKey,
                                               CacheItem newItem)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override void Flush()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            protected override Hashtable LoadDataFromStore()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            protected override void Remove(int storageKey)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            protected override void RemoveOldItem(int storageKey)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            protected override void UpdateLastAccessedTime(int storageKey,
                                                           DateTime timestamp)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}