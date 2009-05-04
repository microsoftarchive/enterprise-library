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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class HashAlgorithmProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanCreateHashAlgorithmProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            HashAlgorithmProviderNode hashAlgorithmProviderNode = nodeCreationService.CreateNodeByDataType(typeof(HashAlgorithmProviderData)) as HashAlgorithmProviderNode;

            Assert.IsNotNull(hashAlgorithmProviderNode);
        }

        [TestMethod]
        public void HashAlgorithmProviderNodeName()
        {
            HashAlgorithmProviderNode hashAlgorithmProviderNode = new HashAlgorithmProviderNode();

            Assert.AreEqual("HashAlgorithm Provider", hashAlgorithmProviderNode.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInHashAlgorithmProviderNodeThrows()
        {
            HashAlgorithmProviderNode node = new HashAlgorithmProviderNode(null);
        }

        [TestMethod]
        public void HashAlgorithmProviderNodeTest()
        {
            bool saltEnabled = false;
            Type algorithmType = typeof(MD5CryptoServiceProvider);
            string name = "some name";

            HashAlgorithmProviderNode node = new HashAlgorithmProviderNode();
            node.Name = name;
            node.SaltEnabled = saltEnabled;
            node.AlgorithmType = algorithmType;

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(algorithmType, node.AlgorithmType);
            Assert.AreEqual(saltEnabled, node.SaltEnabled);

            HashAlgorithmProviderData data = (HashAlgorithmProviderData)node.HashProviderData;
            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(algorithmType, data.AlgorithmType);
            Assert.AreEqual(saltEnabled, data.SaltEnabled);
        }

        [TestMethod]
        public void HashAlgorithmProviderDataTest()
        {
            bool saltEnabled = false;
            Type algorithmType = typeof(MD5CryptoServiceProvider);
            string name = "some name";

            HashAlgorithmProviderData data = new HashAlgorithmProviderData();
            data.Name = name;
            data.AlgorithmType = algorithmType;
            data.SaltEnabled = saltEnabled;

            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(algorithmType, data.AlgorithmType);
            Assert.AreEqual(saltEnabled, data.SaltEnabled);

            HashAlgorithmProviderNode node = new HashAlgorithmProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(algorithmType, node.AlgorithmType);
            Assert.AreEqual(saltEnabled, node.SaltEnabled);
        }
    }
}
