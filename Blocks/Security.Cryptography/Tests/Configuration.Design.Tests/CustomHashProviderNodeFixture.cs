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
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class CustomHashProviderNodeFixture : ConfigurationDesignHost
    {
        public override void BeforeSetup()
        {
            CreateKeyFile("KeyedHashKey.file");
        }

        public override void BeforeCleanup()
        {
            File.Delete("KeyedHashKey.file");
        }

        [TestMethod]
        public void CanCreateCustomHashProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            CustomHashProviderNode customHashProviderNode = nodeCreationService.CreateNodeByDataType(typeof(CustomHashProviderData)) as CustomHashProviderNode;

            Assert.IsNotNull(customHashProviderNode);
        }

        [TestMethod]
        public void CustomHashProviderNodeName()
        {
            CustomHashProviderNode customHashProviderNode = new CustomHashProviderNode();

            Assert.AreEqual("Custom Hash Provider", customHashProviderNode.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInCustomHashProviderNodeThrows()
        {
            CustomHashProviderNode node = new CustomHashProviderNode(null);
        }

        [TestMethod]
        public void CustomHashProviderNodeTest()
        {
            string name = "testName1";
            Type type = typeof(CustomHashProviderData);
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("test", "value");

            CustomHashProviderNode node = new CustomHashProviderNode();
            ApplicationNode.AddNode(node);
            Assert.AreEqual("Custom Hash Provider", node.Name);

            node.Type = type.AssemblyQualifiedName;
            node.Name = name;
            node.Attributes.Add(new EditableKeyValue(attributes.GetKey(0), attributes[attributes.GetKey(0)]));

            Assert.AreEqual(attributes[0], node.Attributes[0].Value);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(name, node.Name);

            CustomHashProviderData nodeData = (CustomHashProviderData)node.HashProviderData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributes.AllKeys[0], nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributes.Get(0), nodeData.Attributes.Get(0));
        }

        [TestMethod]
        public void CustomHashProviderDataTest()
        {
            try
            {
                string name = "testName2";
                Type type = typeof(CustomHashProviderNodeFixture);
                NameValueCollection attributes = new NameValueCollection();
                attributes.Add("test", "value");

                CustomHashProviderData data = new CustomHashProviderData();
                data.Name = name;
                data.Type = type;
                data.Attributes.Add(attributes.GetKey(0), attributes[attributes.GetKey(0)]);

                CustomHashProviderNode node = new CustomHashProviderNode(data);
                ApplicationNode.AddNode(node);
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(type.AssemblyQualifiedName, node.Type);

                Assert.AreEqual(attributes.AllKeys[0], node.Attributes[0].Key);
                Assert.AreEqual(attributes.Get(0), node.Attributes[0].Value);
            }
            finally
            {
                File.Delete("KeyedHashKey.file");
            }
        }

        void CreateKeyFile(string fileName)
        {
            ProtectedKey key = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                KeyManager.Write(stream, key);
            }
        }
    }
}
