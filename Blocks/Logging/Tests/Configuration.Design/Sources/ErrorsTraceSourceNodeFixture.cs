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
    public class ErrorsTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInErrorsTraceSourceNodeThrows()
        {
            new ErrorsTraceSourceNode(null);
        }

        [TestMethod]
        public void ErrorsTraceSourcNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(ErrorsTraceSourceNode), "Name"));
        }

        [TestMethod]
        public void ErrorsTraceSourceDefaultDataTest()
        {
            ErrorsTraceSourceNode errorsTraceSourcesNode = new ErrorsTraceSourceNode(new TraceSourceData());
            ApplicationNode.AddNode(errorsTraceSourcesNode);

            Assert.AreEqual("Logging Errors & Warnings", errorsTraceSourcesNode.Name);
            Assert.AreEqual(0, errorsTraceSourcesNode.Nodes.Count);
        }
    }
}
