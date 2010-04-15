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

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.given_config_that_redirects_logging
{
    [TestClass]
    public class when_changing_logging_configuration_in_redirected_to_source : Context
    {
        private readonly ManualResetEvent sourceChangedEvent = new ManualResetEvent(false);
        private bool sectionChangeEventTimedOut = true;

        private FileConfigurationSource secondSource;

        protected override void Arrange()
        {
            base.Arrange();

            RootConfiguration.SourceChanged += OnRootConfigurationSourceChanged;
        }

        protected override void Act()
        {
            LogWriter.Write("Message 1");

            GetSecondConfigurationSource();
            UpdateLoggingTemplate();

            WaitForSectionChangeEvent(60000);

            LogWriter.Write("Message 2");

            CloseLogFile();
        }

        protected override void Teardown()
        {
            Thread.Sleep(60000);
            secondSource.Dispose();
            sourceChangedEvent.Close();
            base.Teardown();
        }

        [TestMethod]
        public void then_change_event_happens()
        {
            Assert.IsFalse(sectionChangeEventTimedOut);
        }

        [TestMethod]
        public void then_log_file_contains_two_lines()
        {
            string[] logLines = File.ReadAllLines(LogFileName);

            Assert.AreEqual(2, logLines.Length);
        }

        [TestMethod]
        public void then_modified_template_was_applied_to_second_message()
        {
            string[] logLines = File.ReadAllLines(LogFileName);

            Assert.IsFalse(logLines[0].StartsWith("*** "));
            Assert.IsTrue(logLines[1].StartsWith("*** "));
        }

        private void OnRootConfigurationSourceChanged(object sender, ConfigurationSourceChangedEventArgs e)
        {
            sourceChangedEvent.Set();
        }

        private void WaitForSectionChangeEvent(int timeoutMS)
        {
            sectionChangeEventTimedOut = !sourceChangedEvent.WaitOne(timeoutMS);
        }

        private void GetSecondConfigurationSource()
        {
            var configSourceSection =
                (ConfigurationSourceSection) RootConfiguration.GetSection(ConfigurationSourceSection.SectionName);
            secondSource = (FileConfigurationSource) configSourceSection.Sources.Get(SecondSourceName).CreateSource();
        }

        private void UpdateLoggingTemplate()
        {
            var originalSettings = LoggingSettings.GetLoggingSettings(secondSource);

            var newSettings = new LoggingSettings(originalSettings.Name, originalSettings.TracingEnabled,
                originalSettings.DefaultCategory)
            {
                SpecialTraceSources = originalSettings.SpecialTraceSources,
                LogWarningWhenNoCategoriesMatch = originalSettings.LogWarningWhenNoCategoriesMatch,
                RevertImpersonation = originalSettings.RevertImpersonation
            };

            CopyData(originalSettings.TraceListeners, tl => newSettings.TraceListeners.Add(tl));
            CopyData(originalSettings.Formatters, f => newSettings.Formatters.Add(f));
            CopyData(originalSettings.LogFilters, lf => newSettings.LogFilters.Add(lf));
            CopyData(originalSettings.TraceSources, ts => newSettings.TraceSources.Add(ts));

            var formatter = (TextFormatterData)newSettings.Formatters.Get("Text Formatter");
            formatter.Template = "*** " + formatter.Template;

            secondSource.Save(LoggingSettings.SectionName, newSettings);
        }
    }
}
