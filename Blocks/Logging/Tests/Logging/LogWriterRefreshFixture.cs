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
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

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

            EnterpriseLibraryContainer.Current = null;
        }

        [TestMethod]
        public void AddingCategoryFilterToConfigIsReflectedInContainer()
        {
            using (var configurationSource = new SystemConfigurationSource(false))
            using (var serviceLocator = (UnityServiceLocator)EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource))
            {
                EnterpriseLibraryContainer.Current = serviceLocator;

                Logger.Reset();
                MockTraceListener.Reset();

                Logger.Write("test", "MockCategoryOne");
                Assert.AreEqual(1, MockTraceListener.Entries.Count);

                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
                ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
                rwConfiguration.Save();

                ((IConfigurationSourceTest)configurationSource).ConfigSourceChanged(string.Empty);

                var categoryFilter = EnterpriseLibraryContainer.Current.GetInstance<CategoryFilter>("Category");
                Assert.AreEqual(1,
                    categoryFilter.CategoryFilters.Count(c => c == "MockCategoryOne"));
            }
        }

        [TestMethod]
        public void ConfigurationChangeNotificationRefreshesLogger()
        {
            using (var configurationSource = new SystemConfigurationSource(false))
            using (var serviceLocator = (UnityServiceLocator)EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource))
            {
                EnterpriseLibraryContainer.Current = serviceLocator;

                Logger.Reset();
                MockTraceListener.Reset();

                Logger.Write("test", "MockCategoryOne");
                Assert.AreEqual(1, MockTraceListener.Entries.Count);

                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
                ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
                rwConfiguration.Save();

                ((IConfigurationSourceTest)configurationSource).ConfigSourceChanged(string.Empty);

                MockTraceListener.Reset();
                Logger.Write("test", "MockCategoryOne");
                Assert.AreEqual(0, MockTraceListener.Entries.Count, "should have been filtered out by the new category filter");
            }
        }

        [TestMethod]
        public void ConfigurationChangeNotificationRefreshesLoggerAutomatically()
        {
            using (var configurationSource = new SystemConfigurationSource(true, 100))
            using (var serviceLocator = (UnityServiceLocator)EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource))
            {
                EnterpriseLibraryContainer.Current = serviceLocator;

                Logger.Reset();
                MockTraceListener.Reset();

                Logger.Write("test", "MockCategoryOne");
                Assert.AreEqual(1, MockTraceListener.Entries.Count);

                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
                ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
                rwConfiguration.Save();

                Thread.Sleep(1000);

                MockTraceListener.Reset();
                Logger.Write("test", "MockCategoryOne");
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }
        }
    }
}
