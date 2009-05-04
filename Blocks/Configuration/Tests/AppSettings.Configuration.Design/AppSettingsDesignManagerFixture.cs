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

using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Tests
{
    [TestClass]
    public class AppSettingsDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod, DeploymentItem("AdditionalAppsettings.config")]
        public void SavingUntouchedSettingsDoesntChangeFile()
        {
            using (new ConfigFileSnapshot())
            {
                string originalFileContents = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ApplicationNode.Hierarchy.Load();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                ApplicationNode.Hierarchy.Open();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                AppSettingsNode appSettingsNode = (AppSettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(AppSettingsNode));
                Assert.IsNotNull(appSettingsNode);

                ApplicationNode.Hierarchy.Save();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                string fileContentsAfterSave = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                Assert.AreEqual(originalFileContents, fileContentsAfterSave);
            }
        }

        [TestMethod, DeploymentItem("AdditionalAppsettings.config")]
        public void AddedAppSettingsAreSaved()
        {
            using (new ConfigFileSnapshot())
            {
                ApplicationNode.Hierarchy.Load();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                ApplicationNode.Hierarchy.Open();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                AppSettingsNode appSettingsNode = (AppSettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(AppSettingsNode));
                Assert.IsNotNull(appSettingsNode);

                appSettingsNode.AddNode(new AppSettingNode("addedKey", "value"));

                ApplicationNode.Hierarchy.Save();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                string fileContentsAfterSave = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                Assert.IsTrue(fileContentsAfterSave.Contains("addedKey"));
            }
        }

        [TestMethod, DeploymentItem("AdditionalAppsettings.config")]
        public void RemovedAppSettingsAreRemoved()
        {
            using (new ConfigFileSnapshot())
            {
                ApplicationNode.Hierarchy.Load();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                ApplicationNode.Hierarchy.Open();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                AppSettingsNode appSettingsNode = (AppSettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(AppSettingsNode));
                Assert.IsNotNull(appSettingsNode);

                AppSettingNode removeThisNode = appSettingsNode.Nodes["removethissetting"] as AppSettingNode;
                Assert.IsNotNull(removeThisNode);
                removeThisNode.Remove();

                ApplicationNode.Hierarchy.Save();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                string fileContentsAfterSave = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                Assert.IsFalse(fileContentsAfterSave.Contains("removethissetting"));
            }
        }
    }

    class ConfigFileSnapshot : IDisposable
    {
        string contents;

        public ConfigFileSnapshot()
        {
            using (StreamReader reader = new StreamReader(GetConfigFilePath()))
            {
                contents = reader.ReadToEnd();
            }
        }

        public void Dispose()
        {
            if (contents != null)
            {
                using (StreamWriter writer = new StreamWriter(GetConfigFilePath(), false))
                {
                    writer.Write(contents);
                }
                contents = null;
            }
        }

        string GetConfigFilePath()
        {
            return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        }
    }
}
