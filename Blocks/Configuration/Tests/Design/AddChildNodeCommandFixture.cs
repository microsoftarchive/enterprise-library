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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class AddChildNodeCommandFixture : ConfigurationDesignHost
    {
        int executed;

        protected override void InitializeCore()
        {
            executed = 0;
        }

        [TestMethod]
        public void ExecuteCommandAddsAChildNode()
        {
            AddChildNodeCommand addChildNodeCommand = new AddChildNodeCommand(ServiceProvider, typeof(TestConfigurationNode));
            addChildNodeCommand.Execute(ApplicationNode);

            Assert.AreEqual(typeof(TestConfigurationNode), addChildNodeCommand.ChildNode.GetType());
            Assert.AreEqual("My Test", addChildNodeCommand.ChildNode.Name);
            Assert.AreEqual(1, ApplicationNode.Nodes.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructAddChildNodeCommandWithNullChildNodeThrows()
        {
            AddChildNodeCommand cmd = new AddChildNodeCommand(ServiceProvider, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructAddChildNodeCommandWithNullServiceProviderThrows()
        {
            AddChildNodeCommand cmd = new AddChildNodeCommand(null, null);
        }

        [TestMethod]
        public void OnExecutedFires()
        {
            AddChildNodeCommand addChildNodeCommand = new AddChildNodeCommand(ServiceProvider, typeof(TestConfigurationNode));
            addChildNodeCommand.Executed += new EventHandler(addChildNodeCommand_Executed);
            addChildNodeCommand.Execute(ApplicationNode);
            Assert.AreEqual(1, executed);
        }

        void addChildNodeCommand_Executed(object sender,
                                          EventArgs e)
        {
            executed++;
        }

        class TestConfigurationNode : ConfigurationNode
        {
            public TestConfigurationNode()
                : base("My Test") { }
        }
    }
}
