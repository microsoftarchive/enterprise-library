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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    [TestClass]
    public class SaveMergedEnvironmentCommandFixture : ConfigurationDesignHost
    {
        [TestInitialize]
        public void SetUp()
        {
            string mergedFileName
                = Path.Combine(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile),
                    "test.exe.config");
            if (File.Exists(mergedFileName))
            {
                File.SetAttributes(mergedFileName, FileAttributes.Normal);
                File.Delete(mergedFileName);
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            File.SetAttributes(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, FileAttributes.Normal);
        }

        [TestMethod]
        [DeploymentItem("Environment.dconfig")]
        public void SavesMergedConfiguration()
        {
            File.SetAttributes(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, FileAttributes.Normal);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            InstrumentationNode instrumentationNode
                = (InstrumentationNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(InstrumentationNode));

            // test the settings from the original configuration file
            Assert.IsNotNull(instrumentationNode);
            Assert.IsTrue(instrumentationNode.EventLoggingEnabled);
            Assert.IsFalse(instrumentationNode.PerformanceCountersEnabled);
            Assert.IsTrue(instrumentationNode.WmiEnabled);

            // load the environment override
            ConfigurationNode overridesNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentalOverridesNode));
            Assert.IsNotNull(overridesNode);

            EnvironmentNode overrideNode
                = new EnvironmentNodeBuilder(ServiceProvider).Build("Environment.dconfig", overridesNode.Hierarchy);
            overridesNode.AddNode(overrideNode);

            // run the save merged environment command
            SaveMergedEnvironmentCommand command = new SaveMergedEnvironmentCommand(ServiceProvider);
            command.Execute(overrideNode);

            Assert.IsTrue(command.MergeSucceeded);

            // load the resulting configuration
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename
                = Path.Combine(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile),
                    overrideNode.EnvironmentConfigurationFile);
            System.Configuration.Configuration configuration
                = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            // get the instrumentation node
            InstrumentationConfigurationSection instrumentationSection
                = (InstrumentationConfigurationSection)configuration.GetSection(InstrumentationConfigurationSection.SectionName);

            // test the settings from the merged configuration file
            Assert.IsNotNull(instrumentationSection);
            Assert.IsFalse(instrumentationSection.EventLoggingEnabled);
            Assert.IsTrue(instrumentationSection.PerformanceCountersEnabled);
            Assert.IsTrue(instrumentationSection.WmiEnabled);
        }

        [TestMethod]
        [DeploymentItem("Environment.dconfig")]
        public void SavesMergedConfigurationIfSourceConfigurationFileIsReadOnly()
        {
            File.SetAttributes(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, FileAttributes.ReadOnly);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            // load the environment override
            ConfigurationNode overridesNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentalOverridesNode));
            Assert.IsNotNull(overridesNode);

            EnvironmentNode overrideNode
                = new EnvironmentNodeBuilder(ServiceProvider).Build("Environment.dconfig", overridesNode.Hierarchy);
            overridesNode.AddNode(overrideNode);

            // run the save merged environment command
            SaveMergedEnvironmentCommand command = new SaveMergedEnvironmentCommand(ServiceProvider);
            command.Execute(overrideNode);

            Assert.IsTrue(command.MergeSucceeded);

            // load the resulting configuration
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename
                = Path.Combine(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile),
                    overrideNode.EnvironmentConfigurationFile);
            System.Configuration.Configuration configuration
                = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            // get the instrumentation node
            InstrumentationConfigurationSection instrumentationSection
                = (InstrumentationConfigurationSection)configuration.GetSection(InstrumentationConfigurationSection.SectionName);

            // test the settings from the merged configuration file
            Assert.IsNotNull(instrumentationSection);
            Assert.IsFalse(instrumentationSection.EventLoggingEnabled);
            Assert.IsTrue(instrumentationSection.PerformanceCountersEnabled);
            Assert.IsTrue(instrumentationSection.WmiEnabled);
        }

        [TestMethod]
        [DeploymentItem("Environment.dconfig")]
        public void MergingOverridesRemovesTheConfigurationSourcesSection()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            ConfigurationNode configurationSourceSectionNode
                = ApplicationNode.Hierarchy.FindNodeByType(typeof(ConfigurationSourceSectionNode));
            Assert.IsNotNull(configurationSourceSectionNode);

            // load the environment override
            ConfigurationNode overridesNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentalOverridesNode));
            Assert.IsNotNull(overridesNode);

            EnvironmentNode overrideNode
                = new EnvironmentNodeBuilder(ServiceProvider).Build("Environment.dconfig", overridesNode.Hierarchy);
            overridesNode.AddNode(overrideNode);

            // run the save merged environment command
            SaveMergedEnvironmentCommand command = new SaveMergedEnvironmentCommand(ServiceProvider);
            command.Execute(overrideNode);

            Assert.IsTrue(command.MergeSucceeded);

            // load the resulting configuration
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename
                = Path.Combine(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile),
                    overrideNode.EnvironmentConfigurationFile);
            System.Configuration.Configuration configuration
                = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            // get the instrumentation node
            ConfigurationSection configurationSourceSection = configuration.GetSection(ConfigurationSourceSection.SectionName);
            Assert.IsNull(configurationSourceSection);
        }
    }
}
