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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LoggingSettingsNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void LoggingSettingsNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(LoggingSettingsNode), "Name"));
        }

        [TestMethod]
        public void LoggingSettingsNodeDefaultDataTest()
        {
            LoggingSettingsNode loggingSettings = new LoggingSettingsNode();
            ApplicationNode.AddNode(loggingSettings);

            Assert.AreEqual(false, loggingSettings.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(false, loggingSettings.TracingEnabled);
            Assert.AreEqual("Logging Application Block", loggingSettings.Name);
            Assert.IsNull(loggingSettings.DefaultCategory);
        }

        [TestMethod]
        public void LoggingSettingsNodeTest()
        {
            LoggingSettingsNode node = new LoggingSettingsNode();
            ApplicationNode.AddNode(node);

            bool logWarningWhenNoCategoriesMatch = true;
            bool tracingEnabled = true;

            node.LogWarningWhenNoCategoriesMatch = logWarningWhenNoCategoriesMatch;
            Assert.AreEqual(logWarningWhenNoCategoriesMatch, node.LogWarningWhenNoCategoriesMatch);

            node.TracingEnabled = tracingEnabled;
            Assert.AreEqual(tracingEnabled, node.TracingEnabled);

            LoggingSettingsBuilder builder = new LoggingSettingsBuilder(ServiceProvider, node);
            LoggingSettings data = builder.Build();
            Assert.AreEqual("Logging Application Block", data.Name);
            Assert.AreEqual(logWarningWhenNoCategoriesMatch, data.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(tracingEnabled, data.TracingEnabled);
        }

        [TestMethod]
        public void LogCategoryOnlyLinksWithCateogrySources()
        {
            Type defaultCategoryType = typeof(LoggingSettingsNode).GetProperty("DefaultCategory").PropertyType;
            Assert.AreEqual(typeof(CategoryTraceSourceNode), defaultCategoryType);
        }
    }
}
