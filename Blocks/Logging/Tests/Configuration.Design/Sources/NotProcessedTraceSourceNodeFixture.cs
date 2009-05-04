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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests.Sources
{
    [TestClass]
    public class NotProcessedTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInNotProcessedNodeThrows()
        {
            new NotProcessedTraceSourceNode(null);
        }

        [TestMethod]
        public void NotProcessedTraceSourcNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(NotProcessedTraceSourceNode), "Name"));
        }

        [TestMethod]
        public void NotProcessedTraceSourceDefaultDataTest()
        {
            NotProcessedTraceSourceNode notProcessedTraceSourcesNode = new NotProcessedTraceSourceNode(new TraceSourceData());
            ApplicationNode.AddNode(notProcessedTraceSourcesNode);

            Assert.AreEqual("Unprocessed Category", notProcessedTraceSourcesNode.Name);
            Assert.AreEqual(0, notProcessedTraceSourcesNode.Nodes.Count);
        }
    }
}
