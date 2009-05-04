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
    public class CommandExecutionControlFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CanCancelExecutionOfACommand()
        {
            AddChildNodeCommand cmd = null;
            try
            {
                cmd = new AddChildNodeCommand(ServiceProvider, typeof(TestNode));
                cmd.Executing += new EventHandler<CommandExecutingEventArgs>(OnCommandExecuting);
                cmd.Execute(ApplicationNode);

                Assert.IsNull(Hierarchy.FindNodeByType(ApplicationNode, typeof(TestNode)));
            }
            finally
            {
                cmd.Dispose();
            }
        }

        void OnCommandExecuting(object sender,
                                CommandExecutingEventArgs e)
        {
            e.Cancel = true;
        }

        class TestNode : ConfigurationNode { }
    }
}
