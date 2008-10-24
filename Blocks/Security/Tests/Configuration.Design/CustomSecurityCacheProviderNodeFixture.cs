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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestClass]
    public class CustomSecurityCacheProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomSecurityCacheProviderNodeThrows()
        {
            new CustomSecurityCacheProviderNode(null);
        }

        [TestMethod]
        public void CustomSecurityCacheProviderNodeDefaults()
        {
            CustomSecurityCacheProviderNode customSecurityCacheNode = new CustomSecurityCacheProviderNode();
            Assert.AreEqual(0, customSecurityCacheNode.Attributes.Count);
            Assert.AreEqual("Custom Cache Provider", customSecurityCacheNode.Name);
        }

        [TestMethod]
        public void CanCreateCustomSecurityCacheProviderNodeWithData()
        {
            Assert.IsNotNull(ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(CustomSecurityCacheProviderData), new object[] { new CustomSecurityCacheProviderData() }));
        }

        [TestMethod]
        public void CustomHandlerDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(Array);

            CustomSecurityCacheProviderData data = new CustomSecurityCacheProviderData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);

            CustomSecurityCacheProviderNode node = new CustomSecurityCacheProviderNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }

        [TestMethod]
        public void CustomHandlerNodeDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(Array);

            CustomSecurityCacheProviderData data = new CustomSecurityCacheProviderData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);

            CustomSecurityCacheProviderNode customCacheProviderNode = new CustomSecurityCacheProviderNode(data);

            CustomSecurityCacheProviderData nodeData = (CustomSecurityCacheProviderData)customCacheProviderNode.SecurityCacheProviderData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
