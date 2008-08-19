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
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanCreateSymmetricStorageEncryptionProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            SymmetricStorageEncryptionProviderNode node = nodeCreationService.CreateNodeByDataType(typeof(SymmetricStorageEncryptionProviderData)) as SymmetricStorageEncryptionProviderNode;

            Assert.IsNotNull(node);
        }

        [TestMethod]
        public void SymmetricStorageEncryptionProviderNodeName()
        {
            SymmetricStorageEncryptionProviderNode node = new SymmetricStorageEncryptionProviderNode();

            Assert.AreEqual("Symmetric Storage Encryption", node.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInSymmetricStorageEncryptionProviderNodeThrows()
        {
            new SymmetricStorageEncryptionProviderNode(null);
        }
    }
}