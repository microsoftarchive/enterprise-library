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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LogEnabledFilterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInLogEnabledFilterNodeThrows()
        {
            new LogEnabledFilterNode(null);
        }

        [TestMethod]
        public void LogEnabledFilterNodeDefaults()
        {
            LogEnabledFilterNode logEnabledFilter = new LogEnabledFilterNode();

            Assert.AreEqual("LogEnabled Filter", logEnabledFilter.Name);
            Assert.IsFalse(logEnabledFilter.Enabled);
        }

        [TestMethod]
        public void LogEnabledFilterDataTest()
        {
            string name = "some name";
            bool enabled = true;

            LogEnabledFilterData data = new LogEnabledFilterData();
            data.Name = name;
            data.Enabled = enabled;

            LogEnabledFilterNode node = new LogEnabledFilterNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.IsTrue(node.Enabled);
        }

        [TestMethod]
        public void LogEnabledFilterNodeTest()
        {
            string name = "some name";
            bool enabled = true;

            LogEnabledFilterNode node = new LogEnabledFilterNode();
            node.Name = name;
            node.Enabled = enabled;

            LogEnabledFilterData nodeData = (LogEnabledFilterData)node.LogFilterData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.IsTrue(nodeData.Enabled);
        }
    }
}