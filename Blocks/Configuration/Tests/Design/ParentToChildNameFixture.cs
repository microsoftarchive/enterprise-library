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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ParentToChildNameFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void StrangeBehaviour()
        {
            ConfigurationNode parentNode = new NodeImpl("parent");
            ConfigurationNode childNode = new NodeImpl("child");
            parentNode.AddNode(childNode);

            ApplicationNode.AddNode(parentNode);

            Assert.AreEqual("child", childNode.Name);
            Assert.AreEqual("parent", parentNode.Name);
        }

        [TestMethod]
        public void RenameNodeChangesIndex()
        {
            ConfigurationNode childNode = new NodeImpl("Test");
            ApplicationNode.AddNode(childNode);
            childNode.Name = "test3";

            ConfigurationNode childNode2 = new NodeImpl("Test2");
            ApplicationNode.AddNode(childNode2);

            childNode2.Name = ServiceHelper.GetNameCreationService(ServiceProvider).GetUniqueName("Test", childNode, childNode.Parent);
        }

        class NodeImpl : ConfigurationNode
        {
            public NodeImpl(string name)
                : base(name) { }
        }
    }
}
