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

using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.given_config_that_redirects_logging
{
    /// <summary>
    /// Summary description for when_changing_sources_section
    /// </summary>
    [TestClass]
    public class when_changing_sources_section : Context
    {
        private readonly ManualResetEvent sourceChanged = new ManualResetEvent(false);
        private bool sourceChangedEventOccurred;

        protected override void Arrange()
        {
            base.Arrange();

            RootConfiguration.SourceChanged += OnRootConfigurationSourceChanged;
        }

        protected override void Act()
        {
            LogWriter.Write("Message A");

            ChangeSourcesSection();
            WaitForChangeEvent(60000);

            LogWriter.Write("Message B");
            //CloseLogFile();
        }

        protected override void Teardown()
        {
            sourceChanged.Close();
            RootConfiguration.SourceChanged -= OnRootConfigurationSourceChanged;
            base.Teardown();
        } 
        
        [TestMethod]
        public void then_change_event_happens()
        {
            Assert.IsTrue(sourceChangedEventOccurred);
        }

        [TestMethod]
        public void then_log_file_contains_both_messages()
        {
            string[] logLines = File.ReadAllLines(LogFileName);

            Assert.AreEqual(2, logLines.Length);
        }



        private void OnRootConfigurationSourceChanged(object sender, ConfigurationSourceChangedEventArgs e)
        {
            sourceChanged.Set();
        }

        private void WaitForChangeEvent(int timeoutMS)
        {
            sourceChangedEventOccurred = sourceChanged.WaitOne(timeoutMS);
        }

        private void ChangeSourcesSection()
        {
            var sourcesSection = (ConfigurationSourceSection)RootConfiguration.GetSection(ConfigurationSourceSection.SectionName);

            var newSources = new ConfigurationSourceSection
            {
                ParentSource = sourcesSection.ParentSource,
                SelectedSource = sourcesSection.SelectedSource
            };

            CopyData(sourcesSection.Sources, s => newSources.Sources.Add(s));
            CopyData(sourcesSection.RedirectedSections, r => newSources.RedirectedSections.Add(r));

            newSources.Sources.Get("Second Source").Name = "New Source Name";
            newSources.RedirectedSections.Get(LoggingSettings.SectionName).SourceName = "New Source Name";

            ((FileConfigurationSource) RootConfiguration).Save(ConfigurationSourceSection.SectionName, newSources);
        }
    }
}
