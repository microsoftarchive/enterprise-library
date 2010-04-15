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
using System.Reflection;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.TypeBrowser
{
    [TestClass]
    public class NodesFixture : INodeCreator
    {
        [TestInitialize]
        public void SetUp()
        {
        }

        [TestMethod]
        public void TypeNodeIsInitialized()
        {
            var node = new TypeNode(typeof(object));

            Assert.AreSame(typeof(object), node.Data);
            Assert.AreEqual("Object", node.DisplayName);
            Assert.AreEqual("System.Object", node.FullName);
            Assert.AreEqual(Visibility.Visible, node.Visibility);
            Assert.IsFalse(node.IsExpanded);
            Assert.IsFalse(node.IsSelected);
        }

        [TestMethod]
        public void NamespaceNodeIsInitialized()
        {
            var node = new NamespaceNode("test");

            Assert.AreEqual("test", node.DisplayName);
            CollectionAssert.AreEqual(new TypeNode[0], node.Types);
            Assert.AreEqual(Visibility.Visible, node.Visibility);
            Assert.IsFalse(node.IsExpanded);
            Assert.IsFalse(node.IsSelected);
        }

        [TestMethod]
        public void AssemblyNodeIsInitialized()
        {
            var node = new AssemblyNode(typeof(TestAssembly1.Namespace1.Class1).Assembly, this, null);

            Assert.AreEqual("TestAssembly1", node.DisplayName);
            Assert.AreEqual(2, node.Namespaces.Count);
            Assert.AreEqual(Visibility.Visible, node.Visibility);
            Assert.IsFalse(node.IsExpanded);
            Assert.IsFalse(node.IsSelected);
        }

        [TestMethod]
        public void AssemblyNodeNamespacesHaveTypes()
        {
            var node = new AssemblyNode(typeof(TestAssembly1.Namespace1.Class1).Assembly, this, null);

            Assert.AreEqual(3, node.Namespaces[0].Types.Count);
            Assert.AreSame(typeof(TestAssembly1.Namespace1.Class1), node.Namespaces[0].Types[0].Data);
            Assert.AreSame(typeof(TestAssembly1.Namespace1.Class2), node.Namespaces[0].Types[1].Data);
            Assert.AreEqual("TestAssembly1.Namespace1.InternalClass1", node.Namespaces[0].Types[2].Data.FullName);
            Assert.AreEqual(1, node.Namespaces[1].Types.Count);
            Assert.AreEqual("TestAssembly1.Namespace2.AnotherInternalClass", node.Namespaces[1].Types[0].Data.FullName);
        }

        [TestMethod]
        public void AssemblyNodeNamespacesHaveFilteredTypes()
        {
            var node = new AssemblyNode(typeof(TestAssembly1.Namespace1.Class1).Assembly, this, t => !t.Name.Contains("Internal"));

            Assert.AreEqual(1, node.Namespaces.Count);
            Assert.AreEqual(2, node.Namespaces[0].Types.Count);
            Assert.AreSame(typeof(TestAssembly1.Namespace1.Class1), node.Namespaces[0].Types[0].Data);
            Assert.AreSame(typeof(TestAssembly1.Namespace1.Class2), node.Namespaces[0].Types[1].Data);
        }

        [TestMethod]
        public void AssemblyGroupNodeIsInitialized()
        {
            var node =
                new AssemblyGroupNode(
                    new AssemblyGroup(
                        "test",
                        new[] {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly,
                            typeof(TestAssembly2.Namespace1.Class1).Assembly}),
                    this);

            Assert.AreEqual("test", node.DisplayName);
            Assert.AreEqual(2, node.Assemblies.Count);
            Assert.AreEqual(Visibility.Visible, node.Visibility);
            Assert.IsTrue(node.IsExpanded);
            Assert.IsFalse(node.IsSelected);
        }

        public TypeNode CreateTypeNode(Type type)
        {
            return new TypeNode(type);
        }

        public NamespaceNode CreateNamespaceNode(string name)
        {
            return new NamespaceNode(name);
        }

        public AssemblyNode CreateAssemblyNode(Assembly assembly)
        {
            return null;
        }
    }
}
