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
    public class SymmetricAlgorithmProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanCreateSymmetricAlgorithmProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            SymmetricAlgorithmProviderNode symmetricAlgorithmNode = nodeCreationService.CreateNodeByDataType(typeof(SymmetricAlgorithmProviderData)) as SymmetricAlgorithmProviderNode;

            Assert.IsNotNull(symmetricAlgorithmNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInSymmetricAlgorithmProviderNodeThrows()
        {
            new SymmetricAlgorithmProviderNode(null);
        }

        [TestMethod]
        public void SymmetricAlgorithmProviderNodeTest()
        {
            Type algorithmType = typeof(RijndaelManaged);
            ProtectedKeySettings keySettings = new ProtectedKeySettings("some filename", DataProtectionScope.CurrentUser);
            string name = "some name";

            SymmetricAlgorithmProviderNode node = new SymmetricAlgorithmProviderNode();
            node.Name = name;
            node.AlgorithmType = algorithmType;
            node.Key = keySettings;

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(algorithmType, node.AlgorithmType);
            Assert.AreEqual(keySettings.Filename, node.Key.Filename);
            Assert.AreEqual(keySettings.Scope, node.Key.Scope);

            SymmetricAlgorithmProviderData data = (SymmetricAlgorithmProviderData)node.SymmetricCryptoProviderData;
            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(algorithmType, data.AlgorithmType);
            Assert.AreEqual(keySettings.Filename, data.ProtectedKeyFilename);
            Assert.AreEqual(keySettings.Scope, data.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void SymmetricAlgorithmProviderDataTest()
        {
            Type algorithmType = typeof(RijndaelManaged);
            string protectedKeyFilename = "some filename";
            DataProtectionScope protectedKeyProtectionScope = DataProtectionScope.LocalMachine;
            string name = "some name";

            SymmetricAlgorithmProviderData data = new SymmetricAlgorithmProviderData();
            data.Name = name;
            data.AlgorithmType = algorithmType;
            data.ProtectedKeyFilename = protectedKeyFilename;
            data.ProtectedKeyProtectionScope = protectedKeyProtectionScope;

            SymmetricAlgorithmProviderNode node = new SymmetricAlgorithmProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(algorithmType, node.AlgorithmType);
            Assert.AreEqual(protectedKeyProtectionScope, node.Key.Scope);
            Assert.AreEqual(protectedKeyFilename, node.Key.Filename);
        }
    }
}
