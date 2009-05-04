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
    public class InstrumentationNodeFixture
    {
        [TestMethod]
        public void ConstingWithSectionExposesCorrectValues()
        {
            const string ApplicationInstanceName = "ApplicationInstanceName";
            InstrumentationConfigurationSection section = new InstrumentationConfigurationSection(true, true, true, ApplicationInstanceName);
            InstrumentationNode node = new InstrumentationNode(section);

            Assert.IsTrue(node.PerformanceCountersEnabled);
            Assert.IsTrue(node.EventLoggingEnabled);
            Assert.IsTrue(node.WmiEnabled);
            Assert.AreEqual(ApplicationInstanceName, node.ApplicationInstanceName);
        }
    }
}
