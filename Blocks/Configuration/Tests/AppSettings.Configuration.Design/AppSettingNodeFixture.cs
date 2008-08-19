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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Tests
{
    [TestClass]
    public class AppSettingNodeFixture
    {
        [TestMethod]
        public void DefaultAppSettingNodeHasProperName()
        {
            AppSettingNode node = new AppSettingNode();

            Assert.AreEqual("Setting", node.Name);
        }

        [TestMethod]
        public void ConstingWithAppSettingNode()
        {
            AppSettingNode node = new AppSettingNode("key", "value");

            Assert.AreEqual("key", node.Name);
            Assert.AreEqual("value", node.Value);
        }
    }
}