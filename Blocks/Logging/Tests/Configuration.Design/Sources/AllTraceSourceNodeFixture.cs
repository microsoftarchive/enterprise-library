//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests.Sources
{
    [TestClass]
    public class AllTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInAllSourcesNodeThrows()
        {
            new AllTraceSourceNode(null);
        }

        [TestMethod]
        public void AllTraceSourceNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(AllTraceSourceNode), "Name"));
        }

        [TestMethod]
        public void AllTraceSourcesDefaultDataTest()
        {
            AllTraceSourceNode allTraceSourcesNode = new AllTraceSourceNode(new TraceSourceData());
            ApplicationNode.AddNode(allTraceSourcesNode);

            Assert.AreEqual("All Events", allTraceSourcesNode.Name);
            Assert.AreEqual(0, allTraceSourcesNode.Nodes.Count);
        }
    }
}