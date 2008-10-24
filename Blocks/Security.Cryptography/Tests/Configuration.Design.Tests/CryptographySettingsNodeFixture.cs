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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class CryptographySettingsNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CryptographySettingsNodeIsNotSorted()
        {
            CryptographySettingsNode cryptoSettingsNode = new CryptographySettingsNode();
            Assert.IsFalse(cryptoSettingsNode.SortChildren);
        }

        [TestMethod]
        public void CryptographySettingsNodeName()
        {
            CryptographySettingsNode cryptoSettingsNode = new CryptographySettingsNode();

            Assert.AreEqual("Cryptography Application Block", cryptoSettingsNode.Name);
            Assert.IsTrue(CommonUtil.IsPropertyReadOnly(typeof(CryptographySettingsNode), "Name"));
        }

        [TestMethod]
        public void CryptographySettingsNodeTest()
        {
            CustomHashProviderNode defaultHashProviderNode = new CustomHashProviderNode();
            ApplicationNode.AddNode(defaultHashProviderNode);

            CustomSymmetricCryptoProviderNode defaultSymmetricCryptoProviderNode = new CustomSymmetricCryptoProviderNode();
            ApplicationNode.AddNode(defaultSymmetricCryptoProviderNode);

            CryptographySettingsNode node = new CryptographySettingsNode();
            ApplicationNode.AddNode(node);

            node.DefaultHashProvider = defaultHashProviderNode;
            node.DefaultSymmetricCryptoProvider = defaultSymmetricCryptoProviderNode;

            Assert.AreEqual(defaultHashProviderNode, node.DefaultHashProvider);
            Assert.AreEqual(defaultSymmetricCryptoProviderNode, node.DefaultSymmetricCryptoProvider);
        }
    }
}
