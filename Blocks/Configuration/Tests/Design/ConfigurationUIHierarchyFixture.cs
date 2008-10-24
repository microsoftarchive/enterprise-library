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
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationUIHierarchyFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanDetermineIfHierarchyContainsANodeType()
        {
            TempNode node = new TempNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TestNode("Child2"));
            node.Nodes[0].AddNode(new TempNode("Child"));

            Assert.IsTrue(Hierarchy.ContainsNodeType(typeof(TempNode)));
        }

        [TestMethod]
        public void CreatingHierarchyWithPopulatedRootNodeSetsSites()
        {
            ConfigurationApplicationNode node1 = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            TempNode node2 = new TempNode("2");
            node1.AddNode(node2);
            ConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(node1, ServiceProvider);

            Assert.IsNotNull(node2.Site);
        }

        [TestMethod]
        public void CanDetermineIfAParentInTheHierarchyContainsANodeType()
        {
            TempNode node = new TempNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TestNode("Child2"));
            node.Nodes[0].AddNode(new TempNode("Child"));

            Assert.IsTrue(Hierarchy.ContainsNodeType(node, typeof(TempNode)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructinWithANullServiceProviderThrows()
        {
            ConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(new ConfigurationApplicationNode(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructinWithANullRootNodeThrows()
        {
            ConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(null, ServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdddingANullNodeToTheHierarchyThrows()
        {
            Hierarchy.AddNode(null);
        }

        [TestMethod]
        public void AssertRootNodeIsSet()
        {
            Assert.AreSame(ApplicationNode, Hierarchy.RootNode);
        }

        [TestMethod]
        public void AssertSelectedNodeIsSet()
        {
            Assert.AreSame(ApplicationNode, Hierarchy.SelectedNode);
        }

        [TestMethod]
        public void CanFindNodeByPath()
        {
            TestNode node = new TestNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TestNode("Child2"));
            ConfigurationNode foundNode = Hierarchy.FindNodeByPath(node.Path);

            Assert.AreSame(node, foundNode);
        }

        [TestMethod]
        public void CanFindNodeByPath2()
        {
            TestNode node = new TestNode("Child1");
            ApplicationNode.AddNode(node);
            node.AddNode(new TestNode("Child11"));

            TestNode node2 = new TestNode("Child2");
            ApplicationNode.AddNode(node2);
            TestNode node21 = new TestNode("Child21");
            node2.AddNode(node21);

            ConfigurationNode foundNode = Hierarchy.FindNodeByPath(node21.Path);

            Assert.AreSame(node21, foundNode);
        }

        [TestMethod]
        public void CanFindNodeById()
        {
            TestNode node = new TestNode("Child1");
            ApplicationNode.AddNode(node);
            node.AddNode(new TestNode("Child11"));

            ConfigurationNode foundNode = Hierarchy.FindNodeById(node.Id);

            Assert.AreSame(node, foundNode);
        }

        [TestMethod]
        public void FindNodeByTypeForParentReturnsFirstNode()
        {
            TestNode node = new TestNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TempNode("Child2"));
            node.AddNode(new TestNode("Child3"));
            ConfigurationNode foundNode = Hierarchy.FindNodeByType(ApplicationNode, typeof(TestNode));

            Assert.AreEqual("Child", foundNode.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindNodesByTypeWithNullTypeThrows()
        {
            IList<ConfigurationNode> foundNode = Hierarchy.FindNodesByType(null, null);
        }

        [TestMethod]
        public void CanInitializeTheHierarchyWhenRootIsAttachedWithChildren()
        {
            TestNode node = new TestNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TempNode("Child2"));
            node.AddNode(new TestNode("Child3"));

            Assert.AreEqual(4, ((IContainer)Hierarchy).Components.Count);
        }

        [TestMethod]
        public void FindNodeByTypeForParentReturnsNullIfTypeNotFound()
        {
            TestNode node = new TestNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TempNode("Child2"));
            node.AddNode(new TestNode("Child3"));
            ConfigurationNode foundNode = Hierarchy.FindNodeByType(ApplicationNode, typeof(Object));

            Assert.IsNull(foundNode);
        }

        [TestMethod]
        public void FindNodeByNameForAParent()
        {
            TestNode node = new TestNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TempNode("Child2"));
            node.AddNode(new TestNode("Child3"));
            ConfigurationNode foundNode = Hierarchy.FindNodeByName(node, "Child3");

            Assert.AreEqual("Child3", foundNode.Name);
        }

        [TestMethod]
        public void FindNodeByNameForAParentWithNoNameReturnsNull()
        {
            TestNode node = new TestNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TempNode("Child2"));
            node.AddNode(new TestNode("Child3"));
            ConfigurationNode foundNode = Hierarchy.FindNodeByName(node, "Child4");

            Assert.IsNull(foundNode);
        }

        [TestMethod]
        public void FindNodeByNameWhenParentNotInHierarchyReturnsNull()
        {
            TestNode node = new TestNode("Child");
            ConfigurationNode foundNode = Hierarchy.FindNodeByName(node, "Child3");

            Assert.IsNull(foundNode);
        }

        [TestMethod]
        public void CanRemoveANodeAsComponetToContainer()
        {
            TestNode appNode = new TestNode("Root");
            ((IContainer)Hierarchy).Add(appNode);
            ((IContainer)Hierarchy).Remove(appNode);

            Assert.AreEqual(1, ((IContainer)Hierarchy).Components.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindNodeByNameWithNullParentThrows()
        {
            ConfigurationNode foundNode = Hierarchy.FindNodeByName(null, "Child3");
        }

        [TestMethod]
        public void EnsureNodesAreNamesAreUniqueWhenAddedToTheContainer()
        {
            TestNode child1 = new TestNode("Child");
            TestNode child2 = new TestNode("Child");
            ApplicationNode.AddNode(child1);
            ApplicationNode.AddNode(child2);

            Assert.AreEqual("Child1", child2.Name);
        }

        [TestMethod]
        public void CanFindNodeByTypeWithADepthOfOne()
        {
            IList<ConfigurationNode> foundNodes = Hierarchy.FindNodesByType(typeof(ConfigurationApplicationNode));

            Assert.AreEqual(1, foundNodes.Count);
            Assert.AreSame(ApplicationNode, foundNodes[0]);
        }

        [TestMethod]
        public void CanFindChildOfParentByType()
        {
            ApplicationNode.AddNode(new TempNode("Child"));
            ApplicationNode.AddNode(new TempNode("Child"));
            IList<ConfigurationNode> foundNodes = Hierarchy.FindNodesByType(typeof(TempNode));

            Assert.AreEqual(2, foundNodes.Count);
        }

        [TestMethod]
        public void FindGrandChildNodeByType()
        {
            TempNode node = new TempNode("Child");
            ApplicationNode.AddNode(node);
            node.AddNode(new TempNode("Child"));
            IList<ConfigurationNode> foundNodes = Hierarchy.FindNodesByType(typeof(TempNode));

            Assert.AreEqual(2, foundNodes.Count);
        }

        [TestMethod]
        public void FindByTypeGrandGrandChildTest()
        {
            TempNode node = new TempNode("Child");
            ApplicationNode.AddNode(node);
            TempNode deepNode = new TempNode("Child");
            node.AddNode(deepNode);
            deepNode.AddNode(new AnotherNode("Another"));
            IList<ConfigurationNode> foundNodes = Hierarchy.FindNodesByType(typeof(AnotherNode));

            Assert.AreEqual(1, foundNodes.Count);
        }

        [TestMethod]
        public void FindByTypeTwoTopSearchFromOneLowerChildTest()
        {
            TempNode node = new TempNode("Child");
            ApplicationNode.AddNode(node);
            TempNode node2 = new TempNode("Child");
            TempNode node3 = new TempNode("Child");
            ApplicationNode.AddNode(node2);
            node2.AddNode(node3);
            node3.AddNode(new AnotherNode("Child"));
            node3.AddNode(new AnotherNode("Another"));
            IList<ConfigurationNode> foundNodes = Hierarchy.FindNodesByType(typeof(AnotherNode));

            Assert.AreEqual(2, foundNodes.Count);
        }

        class AnotherNode : ConfigurationNode
        {
            public AnotherNode(string name)
                : base(name) {}
        }

        class TestNode : ConfigurationNode
        {
            public TestNode(string name)
                : base(name) {}
        }

        class TempNode : TestNode
        {
            public TempNode(string name)
                : base(name) {}
        }
    }
}
