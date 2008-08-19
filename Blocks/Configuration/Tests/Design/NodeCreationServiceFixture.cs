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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class NodeCreationServiceFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CreateNodeTest()
        {
            Type t = typeof(InstrumentationNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(
                new AddChildNodeCommand(ServiceProvider, t),
                t, typeof(InstrumentationConfigurationSection), "Instrumentation");
            NodeCreationService.AddNodeCreationEntry(entry);

            InstrumentationNode node = NodeCreationService.CreateNodeByDataType(
                                           typeof(InstrumentationConfigurationSection)) as InstrumentationNode;
            Assert.IsNotNull(node);
        }
    }
}