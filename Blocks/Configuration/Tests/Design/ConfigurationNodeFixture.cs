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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationNodeFixture
    {
        ConfigurationApplicationNode node;
        TestConfigurationNode workingNode;
        IConfigurationUIHierarchy hierarchy;
        int addingChildEventCount;
        int addChildEventCount;
        int removingChildEventCount;
        int removeChildEventCount;
        int renamingEventCount;
        int renamedEventCount;
        int removingEventCount;
        int removeEventCount;

        [TestInitialize]
        public void TestInitialize()
        {
            workingNode = null;
            node = new ConfigurationApplicationNode();
            hierarchy = new ConfigurationUIHierarchy(node, ServiceBuilder.Build());
            addingChildEventCount = 0;
            addChildEventCount = 0;
            removingChildEventCount = 0;
            removeChildEventCount = 0;
            renamingEventCount = 0;
            renamedEventCount = 0;
            removingEventCount = 0;
            removeEventCount = 0;
        }

        [TestMethod]
        public void GetUniqueDisplayNameTest()
        {
            node.AddNode(new TestConfigurationNode("Node"));
            node.AddNode(new TestConfigurationNode("Node"));
            TestConfigurationNode newNode = node.Nodes[1] as TestConfigurationNode;
            Assert.AreEqual("Node1", newNode.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddingSameNodeToCollectionThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Foo");
            TestConfigurationNode node1 = new TestConfigurationNode("Node");
            testNode.AddNode(node1);
            testNode.AddNode(node1);
        }

        [TestMethod]
        public void RenameSameNameTest()
        {
            node.AddNode(new TestConfigurationNode("Test"));
            node.AddNode(new TestConfigurationNode("Test3"));
            workingNode = node.Nodes[1] as TestConfigurationNode;
            Assert.AreEqual("Test3", workingNode.Name);
        }

        [TestMethod]
        public void EnsureNodeIsRenamedAndRenameAndRenamingEventFires()
        {
            node.AddNode(new TestConfigurationNode("Test1"));
            node.AddNode(new TestConfigurationNode("Test2"));
            workingNode = node.Nodes[1] as TestConfigurationNode;
            workingNode.Renamed += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenamed);
            workingNode.Renaming += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenaming);
            workingNode.Name = "MyTest";

            Assert.AreEqual("MyTest", workingNode.Name);
            Assert.AreEqual(1, renamedEventCount);
            Assert.AreEqual(1, renamingEventCount);

            workingNode.Renamed -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenamed);
            workingNode.Renaming -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenaming);
        }

        [TestMethod]
        public void RenamingANodeToSameNodeDoesNotFireEvents()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test1");
            testNode.Renamed += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenamed);
            testNode.Renaming += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenaming);
            node.AddNode(testNode);
            testNode.Name = "Test1";

            Assert.AreEqual("Test1", testNode.Name);
            Assert.AreEqual(0, renamedEventCount);
            Assert.AreEqual(0, renamingEventCount);

            testNode.Renamed -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenamed);
            testNode.Renaming -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRenaming);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RenameAChildNodeWithTheSameNameNotInAContainer()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Root");
            testNode.AddNode(new TestConfigurationNode("Test"));
            TestConfigurationNode nameNode = new TestConfigurationNode("Test1");
            testNode.AddNode(nameNode);
            nameNode.Name = "Test";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RenameAChildNodeWithTheSameNameInAContainer()
        {
            node.AddNode(new TestConfigurationNode("Test"));
            TestConfigurationNode nameNode = new TestConfigurationNode("Test1");
            node.AddNode(nameNode);
            nameNode.Name = "Test";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SettingNameToNullThrows()
        {
            node.Name = null;
        }

        [TestMethod]
        public void EnsureAddingAChildNodeChildAddingAndChildAddedEvent()
        {
            node.ChildAdding += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildAdding);
            node.ChildAdded += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildAdded);
            workingNode = new TestConfigurationNode("Test");
            node.AddNode(workingNode);

            Assert.AreEqual(1, addChildEventCount);
            Assert.AreEqual(1, addingChildEventCount);

            node.ChildAdding -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildAdding);
            node.ChildAdded -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildAdded);
        }

        [TestMethod]
        public void EnusureCapacityGrowsForMoreThanDefault4Nodes()
        {
            node.AddNode(new TestConfigurationNode("1"));
            node.AddNode(new TestConfigurationNode("2"));
            node.AddNode(new TestConfigurationNode("3"));
            node.AddNode(new TestConfigurationNode("4"));
            node.AddNode(new TestConfigurationNode("5"));
            node.AddNode(new TestConfigurationNode("6"));

            Assert.AreEqual(6, node.Nodes.Count);
        }

        [TestMethod]
        public void EnsureParentNodeNotifiedOfChildRemovedAndRemovingEvent()
        {
            node.ChildRemoving += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoving);
            node.ChildRemoved += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoved);
            workingNode = new TestConfigurationNode("Test");
            node.AddNode(workingNode);
            workingNode.Remove();

            Assert.AreEqual(1, removeChildEventCount);
            Assert.AreEqual(1, removingChildEventCount);

            node.ChildRemoved -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoved);
            node.ChildRemoving -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoving);
        }

        [TestMethod]
        public void EnsureRemovingAParentNodeChildNodeDoesNotNotifyParent()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.ChildRemoving += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoving);
            testNode.ChildRemoved += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoved);
            testNode.AddNode(new TestConfigurationNode("A"));
            testNode.AddNode(new TestConfigurationNode("B"));
            node.AddNode(testNode);
            testNode.Remove();

            Assert.AreEqual(0, removeChildEventCount);
            Assert.AreEqual(0, removingChildEventCount);

            testNode.ChildRemoving += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoving);
            testNode.ChildRemoved += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeChildRemoved);
        }

        [TestMethod]
        public void RemoveNodeLeavesOnlyOneNodeInChildNodeArray()
        {
            TestConfigurationNode nodeA = new TestConfigurationNode("A");
            TestConfigurationNode nodeB = new TestConfigurationNode("B");
            node.AddNode(nodeA);
            node.AddNode(nodeB);
            nodeA.Remove();

            Assert.AreEqual(1, node.Nodes.Count);
        }

        [TestMethod]
        public void EnsureRemovedAndRemovingEventFiredForParentAndAllChildNodes()
        {
            workingNode = new TestConfigurationNode("B");
            node.AddNode(workingNode);
            workingNode.Removed += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoved);
            workingNode.Removing += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoving);
            TestConfigurationNode workingChildNode = new TestConfigurationNode("working child");
            workingNode.AddNode(workingChildNode);
            workingChildNode.Removed += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoved);
            workingChildNode.Removing += new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoving);
            workingNode.Remove();
            Assert.IsFalse(node.Nodes.Contains(workingNode));
            ConfigurationNode match = null;
            foreach (ConfigurationNode childNode in node.Nodes)
            {
                if (childNode == workingNode)
                {
                    match = childNode;
                }
            }

            Assert.IsNull(match);
            Assert.AreEqual(2, removeEventCount);

            workingNode.Removed -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoved);
            workingNode.Removing -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoving);
            workingChildNode.Removed -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoved);
            workingChildNode.Removing -= new EventHandler<ConfigurationNodeChangedEventArgs>(NodeRemoving);
        }

        [TestMethod]
        public void PathTest()
        {
            node.Name = "Test";
            string path = node.Name + Path.AltDirectorySeparatorChar + "A" + Path.AltDirectorySeparatorChar + "B" + Path.AltDirectorySeparatorChar + "C";
            int[] indexes = new int[3];
            indexes[0] = node.AddNode(new TestConfigurationNode("A"));
            indexes[1] = node.Nodes[indexes[0]].AddNode(new TestConfigurationNode("B"));
            indexes[2] = node.Nodes[indexes[0]].Nodes[indexes[1]].AddNode(new TestConfigurationNode("C"));

            Assert.AreEqual(path, node.Nodes[indexes[0]].Nodes[indexes[1]].Nodes[indexes[2]].Path);
        }

        [TestMethod]
        public void EnsureCanAddNodesWithNoHierarchySet()
        {
            TestConfigurationNode myNode = new TestConfigurationNode("Root");
            myNode.AddNode(new TestConfigurationNode("Child"));

            Assert.AreEqual(1, myNode.Nodes.Count);
            Assert.IsNull(myNode.Hierarchy);
        }

        [TestMethod]
        public void EnsureHierarchySetWhenAddingToNodeWithHierarchy()
        {
            TestConfigurationNode myNode = new TestConfigurationNode("Child1");
            TestConfigurationNode childNode = new TestConfigurationNode("Child2");
            myNode.AddNode(childNode);
            node.AddNode(myNode);

            Assert.IsNotNull(myNode.Hierarchy);
            Assert.AreSame(hierarchy, myNode.Hierarchy);
            Assert.IsNotNull(childNode.Hierarchy);
            Assert.AreSame(hierarchy, childNode.Hierarchy);
        }

        [TestMethod]
        public void EnsureWhenAddingNodesWithTheSameNameNotRootedInHierarchyItIsUpdatedInHierarchyAndNode()
        {
            TestConfigurationNode myNode = new TestConfigurationNode("Child");
            TestConfigurationNode myNode2 = new TestConfigurationNode("Child");
            node.AddNode(myNode2);
            node.AddNode(myNode);

            Assert.IsNotNull(myNode.Hierarchy);
            Assert.AreSame(hierarchy, myNode.Hierarchy);
            Assert.AreEqual("Child1", myNode.Name);
            Assert.IsNotNull(myNode2.Hierarchy);
            Assert.AreSame(hierarchy, myNode2.Hierarchy);
        }

        [TestMethod]
        public void CanGetNextSiblingNode()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode firstNode = (ConfigurationNode)testNode.Nodes[0];
            Assert.AreEqual("ATest", firstNode.NextSibling.Name);
        }

        [TestMethod]
        public void PreviousSiblingTest()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode lastNode = (ConfigurationNode)testNode.Nodes[2];
            Assert.AreEqual("ATest", lastNode.PreviousSibling.Name);
        }

        [TestMethod]
        public void CanMoveNodeAfterSiblingNode()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode nodeA = (ConfigurationNode)testNode.Nodes[1];
            testNode.MoveAfter(testNode.Nodes[0], nodeA);
            Assert.AreEqual("BTest", nodeA.NextSibling.Name);

            string[] expectedNames = new string[] { "ATest", "BTest", "CTest" };
            Assert.AreEqual(expectedNames.Length, testNode.Nodes.Count);
            for (int i = 0; i < expectedNames.Length; ++i)
            {
                Assert.AreEqual(expectedNames[i], testNode.Nodes[i].Name);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveNodeAfterSiblingWithNullSiblingNodeThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            testNode.MoveAfter(testNode.Nodes[0], null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNodeAfterSiblingWithNullParentThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.MoveAfter(testNode.Nodes[0], new TestConfigurationNode("Not A Child"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNodeAfterSiblingWithDifferentParrent()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            TestConfigurationNode testNode2 = new TestConfigurationNode("Test2");
            testNode2.AddNode(new TestConfigurationNode("ATest"));
            testNode.MoveAfter(testNode2.Nodes[0], testNode.Nodes[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveNodeAfterSiblingWithNullChildNodeThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.MoveAfter(null, null);
        }

        [TestMethod]
        public void CanMoveNodeBeforePreviousSiblingPosition()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode nodeA = (ConfigurationNode)testNode.Nodes[1];
            testNode.MoveBefore(testNode.Nodes[2], nodeA);
            Assert.AreEqual("CTest", nodeA.PreviousSibling.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveNodeBeforeSiblingPositionWithNullSiblingNodeThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            testNode.MoveBefore(testNode.Nodes[2], null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveNodeBeforeSiblingWithNullChildNodeThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.MoveBefore(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNodeBeforeSiblingWithNullParentThrows()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.MoveBefore(testNode.Nodes[0], new TestConfigurationNode("Not A Child"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNodeBeforeSiblingWithDifferentParrent()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            testNode.AddNode(new TestConfigurationNode("BTest"));
            TestConfigurationNode testNode2 = new TestConfigurationNode("Test2");
            testNode2.AddNode(new TestConfigurationNode("ATest"));
            testNode.MoveBefore(testNode2.Nodes[0], testNode.Nodes[0]);
        }

        [TestMethod]
        public void FirstNodeIsTheFirstChildAdded()
        {
            TestConfigurationNode node = new TestConfigurationNode("test");
            TestConfigurationNode firstChild = new TestConfigurationNode("firstchild");
            node.AddNode(firstChild);
            Assert.AreSame(firstChild, node.FirstNode);
        }

        [TestMethod]
        public void LastNodeIsTheLastChildAdded()
        {
            TestConfigurationNode node = new TestConfigurationNode("test");
            TestConfigurationNode firstChild = new TestConfigurationNode("firstchild");
            TestConfigurationNode lastChild = new TestConfigurationNode("lastchild");
            node.AddNode(firstChild);
            node.AddNode(lastChild);
            Assert.AreSame(lastChild, node.LastNode);
        }

        void NodeRemoving(object sender,
                          ConfigurationNodeChangedEventArgs e)
        {
            removingEventCount++;
        }

        void NodeRemoved(object sender,
                         ConfigurationNodeChangedEventArgs e)
        {
            removeEventCount++;
        }

        void NodeChildRemoving(object sender,
                               ConfigurationNodeChangedEventArgs e)
        {
            removingChildEventCount++;
        }

        void NodeChildRemoved(object sender,
                              ConfigurationNodeChangedEventArgs e)
        {
            removeChildEventCount++;
        }

        void NodeChildAdding(object sender,
                             ConfigurationNodeChangedEventArgs e)
        {
            addingChildEventCount++;
        }

        void NodeChildAdded(object sender,
                            ConfigurationNodeChangedEventArgs e)
        {
            addChildEventCount++;
        }

        void NodeRenaming(object sender,
                          ConfigurationNodeChangedEventArgs e)
        {
            renamingEventCount++;
        }

        void NodeRenamed(object sender,
                         ConfigurationNodeChangedEventArgs e)
        {
            renamedEventCount++;
        }

        class TestConfigurationNode : ConfigurationNode
        {
            public TestConfigurationNode(string name)
                : base(name) {}

            public override bool SortChildren
            {
                get { return false; }
            }
        }
    }
}