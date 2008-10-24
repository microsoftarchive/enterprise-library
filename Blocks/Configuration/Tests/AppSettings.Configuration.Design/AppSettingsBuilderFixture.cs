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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Tests
{
    [TestClass]
    public class AppSettingsBuilderFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AppSettingIsAddForEverySettingNode()
        {
            AppSettingsNode settingsNode = new AppSettingsNode();
            base.ApplicationNode.AddNode(settingsNode);

            settingsNode.AddNode(new AppSettingNode("key", "value"));

            AppSettingsBuilder settingsBuilder = new AppSettingsBuilder(settingsNode);
            AppSettingsSection settingsSection = settingsBuilder.Build();

            Assert.IsNotNull(settingsSection);
            Assert.AreEqual(1, settingsSection.Settings.Count);
            Assert.AreEqual("key", settingsSection.Settings.AllKeys[0]);
            Assert.AreEqual("value", settingsSection.Settings["key"].Value);
        }
    }
}
