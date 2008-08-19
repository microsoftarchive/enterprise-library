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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class InstrumentationConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        InstrumentationConfigurationDesignManager manager;

        protected override void InitializeCore()
        {
            HiearchyService.AddHierarchy(Hierarchy);
            manager = new InstrumentationConfigurationDesignManager();
        }

        protected override void CleanupCore()
        {
            HiearchyService.RemoveHierarchy(Hierarchy);
        }

        [TestMethod]
        public void CanReadAndWriteInstrumentationConfiguration()
        {
            manager.Register(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ApplicationNode.AddNode(new InstrumentationNode(new InstrumentationConfigurationSection(true, true, false)));
            manager.Save(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ApplicationNode.ClearChildNodes();
            manager.Open(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            InstrumentationNode node = (InstrumentationNode)ApplicationNode.Nodes[0];

            Assert.IsNotNull(node);
            Assert.IsTrue(node.EventLoggingEnabled);
            Assert.IsTrue(node.PerformanceCountersEnabled);
            Assert.IsFalse(node.WmiEnabled);

            ApplicationNode.ClearChildNodes();

            manager.Save(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
        }
    }
}