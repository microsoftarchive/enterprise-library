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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationUIHierachyDesignManagerFixture : ConfigurationDesignHost
    {
        int hierarchySavedEventFiredCount;

        protected override void InitializeCore()
        {
            base.InitializeCore();
            HiearchyService.AddHierarchy(Hierarchy);
        }

        protected override void CleanupCore()
        {
            HiearchyService.RemoveHierarchy(Hierarchy);
            base.CleanupCore();
        }

        [TestMethod]
        public void CanSaveInstrumentationCofigurationInHierarch()
        {
            SaveInstrumentationSection();

            InstrumentationNode instrumentationNode = (InstrumentationNode)Hierarchy.FindNodeByType(typeof(InstrumentationNode));

            Assert.IsFalse(instrumentationNode.PerformanceCountersEnabled);
            Assert.IsFalse(instrumentationNode.WmiEnabled);
            Assert.IsTrue(instrumentationNode.EventLoggingEnabled);
        }

        [TestMethod]
        public void CallingSaveFiresSaveHierarchyEvent()
        {
            Hierarchy.Saved += new EventHandler<HierarchySavedEventArgs>(OnHierarchySaved);
            SaveInstrumentationSection();
            Hierarchy.Saved -= new EventHandler<HierarchySavedEventArgs>(OnHierarchySaved);
            Assert.AreEqual(1, hierarchySavedEventFiredCount);
            hierarchySavedEventFiredCount = 0;
        }

        [TestMethod]
        public void BuildConfigurationSourceFromHiearchy()
        {
            AddConfigurationSourcesToHierarchy();
            AddInstrumentationToHierarchy();
            IConfigurationSource source = Hierarchy.BuildConfigurationSource();

            Assert.IsNotNull(source.GetSection(InstrumentationConfigurationSection.SectionName));
            Assert.IsNotNull(source.GetSection(ConfigurationSourceSection.SectionName));
        }

        void AddConfigurationSourcesToHierarchy()
        {
            ConfigurationSourceSection section = new ConfigurationSourceSection();
            section.Sources.Add(new SystemConfigurationSourceElement("System"));
            section.Sources.Add(new FileConfigurationSourceElement("File", "app.config"));
            section.SelectedSource = "File";
            ConfigurationSourceSectionNodeBuilder builder = new ConfigurationSourceSectionNodeBuilder(ServiceProvider, section);
            ApplicationNode.AddNode(builder.Build());
        }

        void OnHierarchySaved(object sender,
                              HierarchySavedEventArgs e)
        {
            hierarchySavedEventFiredCount = 1;
        }

        void SaveInstrumentationSection()
        {
            AddInstrumentationToHierarchy();
            Hierarchy.Save();
        }

        void AddInstrumentationToHierarchy()
        {
            InstrumentationNode instrumentationNode = new InstrumentationNode(new InstrumentationConfigurationSection(false, true, false));
            ApplicationNode.AddNode(instrumentationNode);
        }
    }
}
