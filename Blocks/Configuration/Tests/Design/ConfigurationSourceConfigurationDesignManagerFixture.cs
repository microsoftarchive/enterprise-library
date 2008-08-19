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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationSourceConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        ConfigurationSourceConfigurationDesignManager manager;

        protected override void InitializeCore()
        {
            HiearchyService.AddHierarchy(Hierarchy);
            manager = new ConfigurationSourceConfigurationDesignManager();
        }

        protected override void CleanupCore()
        {
            HiearchyService.RemoveHierarchy(Hierarchy);
        }

        [TestMethod]
        public void CanReadAndWriteConfigurationSourceConfiguration()
        {
            manager.Register(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ApplicationNode.AddNode(GetSectionNode());
            manager.Save(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ApplicationNode.ClearChildNodes();
            manager.Open(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ConfigurationSourceSectionNode node = (ConfigurationSourceSectionNode)ApplicationNode.Nodes[0];

            Assert.IsNotNull(node);
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual(typeof(FileConfigurationSourceElementNode), node.SelectedSource.GetType());

            ApplicationNode.ClearChildNodes();

            manager.Save(ServiceProvider);
            Assert.AreEqual(0, ErrorLogService.ValidationErrorCount);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
        }

        ConfigurationSourceSectionNode GetSectionNode()
        {
            ConfigurationSourceSection section = new ConfigurationSourceSection();
            section.Sources.Add(new SystemConfigurationSourceElement("System"));
            section.Sources.Add(new FileConfigurationSourceElement("File", "app.config"));
            section.SelectedSource = "File";
            ConfigurationSourceSectionNodeBuilder builder = new ConfigurationSourceSectionNodeBuilder(ServiceProvider, section);
            return builder.Build();
        }
    }
}