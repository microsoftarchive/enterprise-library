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
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Tests
{
    [TestClass]
    public class AppSettingsNodeBuilderFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void NodeBuilderAddsChildNodesForEverySettingInAppSettings()
        {
            AppSettingsSection appSettingsSection = new AppSettingsSection();
            appSettingsSection.Settings.Add("key1", "value1");
            appSettingsSection.Settings.Add("key2", "value2");
            appSettingsSection.Settings.Add("key3", "value3");

            AppSettingsNodeBuilder builder = new AppSettingsNodeBuilder(ServiceProvider, appSettingsSection);
            AppSettingsNode appSettingsNode = builder.Build();

            Assert.AreEqual(3, appSettingsNode.Nodes.Count);
        }

        [TestMethod]
        public void NodeBuilderAddsNodesWithoutConfigurationSourceAsReadWrite()
        {
            AppSettingsSection appSettingsSection = new AppSettingsSection();
            appSettingsSection.Settings.Add("key", "value");

            AppSettingsNodeBuilder builder = new AppSettingsNodeBuilder(ServiceProvider, appSettingsSection);
            AppSettingsNode appSettingsNode = builder.Build();

            Assert.AreEqual(1, appSettingsNode.Nodes.Count);
            Assert.AreEqual(typeof(AppSettingNode), appSettingsNode.Nodes[0].GetType());
        }

        //[TestMethod, DeploymentItem("AdditionalAppsettings.config")]
        //public void NodeBuilderAddsNodesFromExternalFileAsReadOnly()
        //{
        //    ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
        //    filemap.ExeConfigFilename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        //    System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
        //    AppSettingsNodeBuilder builder = new AppSettingsNodeBuilder(ServiceProvider, configuration.GetSection("appSettings") as AppSettingsSection);

        //    AppSettingsNode appSettingsNode = builder.Build();

        //    Assert.IsNotNull(appSettingsNode.Nodes["externalSetting1"]);
        //    Assert.AreEqual(typeof(ReadOnlyAppSettingNode), appSettingsNode.Nodes["externalSetting1"].GetType());
        //}

        [TestMethod, DeploymentItem("AdditionalAppsettings.config"), DeploymentItem("AppSettingsWithConfigSource.config")]
        public void SettingsFromConfigSectionAreAddedAsReadWriteNodes()
        {
            ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
            filemap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettingsWithConfigSource.config");

            System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
            AppSettingsNodeBuilder builder = new AppSettingsNodeBuilder(ServiceProvider, configuration.GetSection("appSettings") as AppSettingsSection);

            AppSettingsNode appSettingsNode = builder.Build();

            Assert.IsNotNull(appSettingsNode);
            Assert.AreEqual(1, appSettingsNode.Nodes.Count);
            Assert.AreEqual(typeof(AppSettingNode), appSettingsNode.Nodes[0].GetType());
        }
    }
}
