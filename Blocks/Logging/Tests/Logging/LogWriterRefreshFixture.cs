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

using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    /// <summary>
    /// Summary description for LoggerRefreshFixture
    /// </summary>
    [TestClass]
    public class LogWriterRefreshFixture
    {
        [TestCleanup]
        public void TearDown()
        {
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Remove("MockCategoryOne");
            rwConfiguration.Save();
            ConfigurationManager.RefreshSection(LoggingSettings.SectionName);

            ConfigurationChangeWatcher.ResetDefaultPollDelay();
        }

        [TestMethod]
        public void ConfigurationChangeNotificationRefreshesLogger()
        {
            SystemConfigurationSource.ResetImplementation(false);
            Logger.Reset();

            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(1, MockTraceListener.Entries.Count);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
            rwConfiguration.Save();
            SystemConfigurationSource.Implementation.ConfigSourceChanged(string.Empty);

            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(0, MockTraceListener.Entries.Count, "should have been filtered out by the new category filter");
        }

        [TestMethod]
        [Ignore] // This test fails intermittently due to timing problems
        public void ConfigurationChangeNotificationRefreshesLoggerAutomatically()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);
            SystemConfigurationSource.ResetImplementation(true);
            Logger.Reset();

            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(1, MockTraceListener.Entries.Count);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
            rwConfiguration.Save();

            Thread.Sleep(400);

            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }
    }
}
