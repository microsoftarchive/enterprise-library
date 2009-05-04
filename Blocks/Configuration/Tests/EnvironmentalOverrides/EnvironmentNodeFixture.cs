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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    [TestClass]
    public class EnvironmentNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void EnvironmentNodeCanNotExtendApplicationNode()
        {
            EnvironmentNode environmentNode = new EnvironmentNode();
            Assert.IsFalse(environmentNode.CanExtend(ApplicationNode));
        }

        [TestMethod]
        public void EnvironmentNodeCanNotExtendEnvironmentNode()
        {
            EnvironmentNode environmentNode = new EnvironmentNode();
            Assert.IsFalse(environmentNode.CanExtend(environmentNode));
        }
    }
}
