//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationNodeCollectionFixture
    {
        ConfigurationApplicationNode rootNode;
        ConfigurationUIHierarchy hierachy;

        [TestInitialize]
        public void TestInitialize()
        {
            rootNode = new ConfigurationApplicationNode();
            hierachy = new ConfigurationUIHierarchy(rootNode, ServiceBuilder.Build());
        }

        [TestMethod]
        public void AddingNodesUpdatesCount()
        {
            MyNode node = new MyNode();
            rootNode.AddNode(node);

            Assert.AreEqual(1, rootNode.Nodes.Count);
        }

        [TestMethod]
        public void EnsureCanGetANodeByName()
        {
            MyNode nodeToGet = new MyNode();
            nodeToGet.Name = "NodeName";
            rootNode.AddNode(new MyNode());
            rootNode.AddNode(nodeToGet);
            rootNode.AddNode(new MyNode());

            MyNode foundNode = (MyNode)rootNode.Nodes["NodeName"];

            Assert.AreSame(nodeToGet, foundNode);
        }

        [TestMethod]
        public void EnsureCanGetNodeByIndex()
        {
            MyNode nodeToCompare = new MyNode();
            rootNode.AddNode(new MyNode());
            rootNode.AddNode(nodeToCompare);
            rootNode.AddNode(new MyNode());

            MyNode foundNode = (MyNode)rootNode.Nodes[1];

            Assert.AreSame(nodeToCompare, foundNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GettingANodeWithAnInvalidIndexThrows()
        {
            rootNode.AddNode(new MyNode());
            rootNode.AddNode(new MyNode());
            ConfigurationNode node = rootNode.Nodes[9];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GettingANodeWithANegativeIndexThrows()
        {
            rootNode.AddNode(new MyNode());
            rootNode.AddNode(new MyNode());
            ConfigurationNode node = rootNode.Nodes[-1];
        }

        [TestMethod]
        public void LookingUpANodeByNameThatDoesNotExistReturnsNull()
        {
            rootNode.AddNode(new MyNode());
            ConfigurationNode node = rootNode.Nodes["NotThere"];

            Assert.IsNull(node);
        }

        [TestMethod]
        public void CanEnumerateNodeCollection()
        {
            rootNode.AddNode(new MyNode());
            rootNode.AddNode(new MyNode());
            rootNode.AddNode(new MyNode());

            int count = 0;
            IEnumerable enumerable = (IEnumerable)rootNode.Nodes;
            foreach (MyNode node in enumerable)
            {
                count++;
            }

            Assert.AreEqual(3, count);
        }

        class MyNode : ConfigurationNode
        {
            public MyNode()
                : base() {}
        }
    }
}