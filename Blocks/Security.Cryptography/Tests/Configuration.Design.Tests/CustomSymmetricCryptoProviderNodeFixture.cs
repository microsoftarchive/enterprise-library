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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class CustomSymmetricCryptoProviderNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CustomSymmetricCryptoProviderNodeName()
        {
            CustomSymmetricCryptoProviderNode cryptoProviderNode = new CustomSymmetricCryptoProviderNode();
            Assert.AreEqual("Custom Symmetric Cryptography Provider", cryptoProviderNode.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInCustomSymmetricCryptoProviderNodeThrows()
        {
            CustomSymmetricCryptoProviderNode customCryptoProviderNode = new CustomSymmetricCryptoProviderNode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInSymmetricCryptoProviderNodeThrows()
        {
            CustomSymmetricCryptoProviderNode node = new CustomSymmetricCryptoProviderNode(null);
        }

        [TestMethod]
        public void CustomSymmetricCryptoProviderNodeTest()
        {
            Type type = typeof(CustomSymmetricCryptoProviderNodeFixture);
            string name = "some name";
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Test", "Value");

            CustomSymmetricCryptoProviderNode node = new CustomSymmetricCryptoProviderNode();
            ApplicationNode.AddNode(node);
            Assert.AreEqual("Custom Symmetric Cryptography Provider", node.Name);

            node.Name = name;
            node.Type = type.AssemblyQualifiedName;
            node.Attributes.Add(new EditableKeyValue(attributes.GetKey(0), attributes[attributes.GetKey(0)]));

            Assert.AreEqual(attributes[0], node.Attributes[0].Value);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);

            CustomSymmetricCryptoProviderData data = (CustomSymmetricCryptoProviderData)node.SymmetricCryptoProviderData;
            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(type, data.Type);
            Assert.AreEqual(attributes.AllKeys[0], data.Attributes.AllKeys[0]);
            Assert.AreEqual(attributes.Get(0), data.Attributes.Get(0));
        }

        [TestMethod]
        public void CustomSymmetricCryptoProviderDataTest()
        {
            Type type = typeof(CustomSymmetricCryptoProviderNodeFixture);
            string name = "some name";
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("test", "value");

            CustomSymmetricCryptoProviderData data = new CustomSymmetricCryptoProviderData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributes.GetKey(0), attributes[attributes.GetKey(0)]);

            CustomSymmetricCryptoProviderNode node = new CustomSymmetricCryptoProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(attributes.AllKeys[0], node.Attributes[0].Key);
            Assert.AreEqual(attributes.Get(0), node.Attributes[0].Value);
        }
    }
}
