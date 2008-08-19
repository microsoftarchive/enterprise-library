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
    public class CustomAuthorizationProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomAuthorizationProviderNodeThrows()
        {
            new CustomAuthorizationProviderNode(null);
        }

        [TestMethod]
        public void CustomAuthorizationProviderNodeDefaults()
        {
            CustomAuthorizationProviderNode customAuthorizationNode = new CustomAuthorizationProviderNode();
            Assert.AreEqual(0, customAuthorizationNode.Attributes.Count);
            Assert.AreEqual("Custom Authorization Provider", customAuthorizationNode.Name);
        }

        [TestMethod]
        public void CanCreateCustomAuthorizationNodeWithData()
        {
            Assert.IsNotNull(ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(CustomAuthorizationProviderData), new object[] { new CustomAuthorizationProviderData() }));
        }

        [TestMethod]
        public void CustomHandlerDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(Array);

            CustomAuthorizationProviderData data = new CustomAuthorizationProviderData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);

            CustomAuthorizationProviderNode node = new CustomAuthorizationProviderNode(data);

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

            CustomAuthorizationProviderData data = new CustomAuthorizationProviderData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);

            CustomAuthorizationProviderNode customCacheProviderNode = new CustomAuthorizationProviderNode(data);

            CustomAuthorizationProviderData nodeData = (CustomAuthorizationProviderData)customCacheProviderNode.AuthorizationProviderData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}