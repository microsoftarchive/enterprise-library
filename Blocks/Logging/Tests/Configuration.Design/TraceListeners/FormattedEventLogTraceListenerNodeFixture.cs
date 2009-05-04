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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class FormattedEventLogTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInFormattedEventLogTraceListenerNodeThrows()
        {
            new FormattedEventLogTraceListenerNode(null);
        }

        [TestMethod]
        public void FormattedEventLogSourcePropertyIsRequired()
        {
            Assert.IsTrue(CommonUtil.IsPropertyRequired(typeof(FormattedEventLogTraceListenerNode), "Source"));
        }

        [TestMethod]
        public void FormattedEventLogTraceListenerNodeTest()
        {
            string name = "some name";
            string source = "some source";
            string log = "some log";
            string machineName = "some machine";

            FormattedEventLogTraceListenerNode formattedEventLogTraceListenerNode = new FormattedEventLogTraceListenerNode();
            formattedEventLogTraceListenerNode.Name = name;
            formattedEventLogTraceListenerNode.Source = source;
            formattedEventLogTraceListenerNode.Log = log;
            formattedEventLogTraceListenerNode.MachineName = machineName;

            ApplicationNode.AddNode(formattedEventLogTraceListenerNode);

            FormattedEventLogTraceListenerData nodeData = (FormattedEventLogTraceListenerData)formattedEventLogTraceListenerNode.TraceListenerData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(source, nodeData.Source);
            Assert.AreEqual(log, nodeData.Log);
            Assert.AreEqual(machineName, nodeData.MachineName);
        }

        [TestMethod]
        public void FormattedEventLogTraceListenerNodeDataTest()
        {
            string name = "some name";
            string source = "some source";
            string log = "some log";
            string machineName = "some machine";

            FormattedEventLogTraceListenerData formattedEventLogTraceListenerData = new FormattedEventLogTraceListenerData();
            formattedEventLogTraceListenerData.Name = name;
            formattedEventLogTraceListenerData.Log = log;
            formattedEventLogTraceListenerData.Source = source;
            formattedEventLogTraceListenerData.MachineName = machineName;

            FormattedEventLogTraceListenerNode formattedEventLogTraceListenerNode = new FormattedEventLogTraceListenerNode(formattedEventLogTraceListenerData);
            ApplicationNode.AddNode(formattedEventLogTraceListenerNode);

            Assert.AreEqual(name, formattedEventLogTraceListenerNode.Name);
            Assert.AreEqual(log, formattedEventLogTraceListenerNode.Log);
            Assert.AreEqual(source, formattedEventLogTraceListenerNode.Source);
            Assert.AreEqual(machineName, formattedEventLogTraceListenerNode.MachineName);
        }
    }
}
