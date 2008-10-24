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
    public class KeyedHashAlgortihmProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanCreateKeyedHashAlgorithmProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            KeyedHashAlgorithmProviderNode keyedHashAlgorithmProviderNode = nodeCreationService.CreateNodeByDataType(typeof(KeyedHashAlgorithmProviderData)) as KeyedHashAlgorithmProviderNode;

            Assert.IsNotNull(keyedHashAlgorithmProviderNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInKeyedHashAlgorithmProviderNodeThrows()
        {
            new KeyedHashAlgorithmProviderNode(null);
        }

        [TestMethod]
        public void KeyedHashAlgorithmProviderNodeTest()
        {
            bool saltEnabled = false;
            Type algorithmType = typeof(HMACSHA1);
            ProtectedKeySettings keySettings = new ProtectedKeySettings("some filename", DataProtectionScope.CurrentUser);
            string name = "some name";

            KeyedHashAlgorithmProviderNode node = new KeyedHashAlgorithmProviderNode();
            node.Name = name;
            node.SaltEnabled = saltEnabled;
            node.AlgorithmType = algorithmType;
            node.Key = keySettings;

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(algorithmType, node.AlgorithmType);
            Assert.AreEqual(saltEnabled, node.SaltEnabled);
            Assert.AreEqual(keySettings.Filename, node.Key.Filename);
            Assert.AreEqual(keySettings.Scope, node.Key.Scope);

            KeyedHashAlgorithmProviderData data = (KeyedHashAlgorithmProviderData)node.HashProviderData;
            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(algorithmType, data.AlgorithmType);
            Assert.AreEqual(saltEnabled, data.SaltEnabled);
            Assert.AreEqual(keySettings.Filename, data.ProtectedKeyFilename);
            Assert.AreEqual(keySettings.Scope, data.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void KeyedHashAlgorithmProviderDataTest()
        {
            ProtectedKeySettings keySettings = new ProtectedKeySettings("some filename", DataProtectionScope.CurrentUser);
            bool saltEnabled = false;
            Type algorithmType = typeof(HMACSHA1);
            string name = "some name";

            KeyedHashAlgorithmProviderData data = new KeyedHashAlgorithmProviderData();
            data.Name = name;
            data.AlgorithmType = algorithmType;
            data.SaltEnabled = saltEnabled;
            data.ProtectedKeyFilename = keySettings.Filename;
            data.ProtectedKeyProtectionScope = keySettings.Scope;

            KeyedHashAlgorithmProviderNode node = new KeyedHashAlgorithmProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(algorithmType, node.AlgorithmType);
            Assert.AreEqual(saltEnabled, node.SaltEnabled);
            Assert.AreEqual(keySettings.Filename, node.Key.Filename);
            Assert.AreEqual(keySettings.Scope, node.Key.Scope);
        }
    }
}
