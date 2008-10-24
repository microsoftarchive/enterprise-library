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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class SpecialTraceSourcesNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void SpecialTraceSourcesNodeDefaults()
        {
            SpecialTraceSourcesNode specialTraceSources = new SpecialTraceSourcesNode();

            Assert.AreEqual("Special Sources", specialTraceSources.Name);
        }

        [TestMethod]
        public void SpecialTraceSourcesNamePropertyIsReadOnly()
        {
            Assert.IsTrue(CommonUtil.IsPropertyReadOnly(typeof(SpecialTraceSourcesNode), "Name"));
        }
    }
}
