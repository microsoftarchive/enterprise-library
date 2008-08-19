//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class DpapiSymmetricCryptoProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanCreateDpapiSymmetricCryptoProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            DpapiSymmetricCryptoProviderNode dpapiProviderNode = nodeCreationService.CreateNodeByDataType(typeof(DpapiSymmetricCryptoProviderData)) as DpapiSymmetricCryptoProviderNode;

            Assert.IsNotNull(dpapiProviderNode);
        }

        [TestMethod]
        public void DpapiSymmetricCryptoProviderNodeName()
        {
            DpapiSymmetricCryptoProviderNode dpapiProviderNode = new DpapiSymmetricCryptoProviderNode();

            Assert.AreEqual("DPAPI Symmetric Cryptography Provider", dpapiProviderNode.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInDpapiSymmetricCryptoProviderNodeThrows()
        {
            DpapiSymmetricCryptoProviderNode node = new DpapiSymmetricCryptoProviderNode(null);
        }

        [TestMethod]
        public void DpapiSymmetricCryptoProviderNodeTest()
        {
            DataProtectionScope scope = DataProtectionScope.LocalMachine;
            string name = "some name";

            DpapiSymmetricCryptoProviderNode node = new DpapiSymmetricCryptoProviderNode();
            node.ProtectionScope = scope;
            node.Name = name;

            Assert.AreEqual(scope, node.ProtectionScope);
            Assert.AreEqual(name, node.Name);

            DpapiSymmetricCryptoProviderData data = (DpapiSymmetricCryptoProviderData)node.SymmetricCryptoProviderData;
            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(scope, data.Scope);
        }

        [TestMethod]
        public void DpapiSymmetricCryptoProviderDataTest()
        {
            DataProtectionScope scope = DataProtectionScope.LocalMachine;
            string name = "some name";

            DpapiSymmetricCryptoProviderData data = new DpapiSymmetricCryptoProviderData();
            data.Name = name;
            data.Scope = scope;

            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(scope, data.Scope);

            DpapiSymmetricCryptoProviderNode node = new DpapiSymmetricCryptoProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(scope, node.ProtectionScope);
        }
    }
}