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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
    [TestClass]
    public class LogFilterConfigurationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
        }

        private static ILogFilter GetFilter(string name, IConfigurationSource configurationSource)
        {
            var settings = LoggingSettings.GetLoggingSettings(configurationSource);
            return settings.LogFilters.Get(name).BuildFilter();
        }

        [TestMethod]
        public void CanReadWrittenFilterConfiguration()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwConfiguration.Sections.Add(LoggingSettings.SectionName, rwLoggingSettings);

            rwLoggingSettings.LogFilters.Add(new LogEnabledFilterData("enabled", true));

            NamedElementCollection<CategoryFilterEntry> categoryEntries = new NamedElementCollection<CategoryFilterEntry>();
            categoryEntries.Add(new CategoryFilterEntry("foo"));
            categoryEntries.Add(new CategoryFilterEntry("bar"));
            categoryEntries.Add(new CategoryFilterEntry("baz"));
            rwLoggingSettings.LogFilters.Add(new CategoryFilterData("category", categoryEntries, CategoryFilterMode.DenyAllExceptAllowed));

            rwLoggingSettings.LogFilters.Add(new PriorityFilterData("priority", 5));

            File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
            rwConfiguration.Save();

            System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LoggingSettings roLoggingSettings = (LoggingSettings)roConfiguration.Sections[LoggingSettings.SectionName];
            Assert.AreEqual(3, roLoggingSettings.LogFilters.Count);
            Assert.AreEqual(roLoggingSettings.LogFilters.Get("enabled").GetType(), typeof(LogEnabledFilterData));
            Assert.AreEqual(true, ((LogEnabledFilterData)roLoggingSettings.LogFilters.Get("enabled")).Enabled);
            Assert.AreEqual(roLoggingSettings.LogFilters.Get("category").GetType(), typeof(CategoryFilterData));
            Assert.AreEqual(3, ((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Count);
            Assert.IsNotNull(((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Get("foo"));
            Assert.IsNotNull(((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Get("bar"));
            Assert.IsNotNull(((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Get("baz"));
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, ((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilterMode);
            Assert.AreEqual(roLoggingSettings.LogFilters.Get("priority").GetType(), typeof(PriorityFilterData));
            Assert.AreEqual(5, ((PriorityFilterData)roLoggingSettings.LogFilters.Get("priority")).MinimumPriority);
        }

        [TestMethod]
        public void CanCreateCategoryFilterFromEmptyCategoryConfiguration()
        {
            var categoryEntries = new NamedElementCollection<CategoryFilterEntry>();
            var filterData = new CategoryFilterData("category", categoryEntries, CategoryFilterMode.DenyAllExceptAllowed);

            var helper = new MockLogObjectsHelper();
            helper.loggingSettings.LogFilters.Add(filterData);

            ILogFilter filter = GetFilter(filterData.Name, helper.configurationSource);

            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(CategoryFilter));
            Assert.AreEqual(0, ((CategoryFilter)filter).CategoryFilters.Count);
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, ((CategoryFilter)filter).CategoryFilterMode);
        }

        [TestMethod]
        public void CanCreateCategoryFilterFromNonEmptyCategoryConfiguration()
        {
            NamedElementCollection<CategoryFilterEntry> categoryEntries = new NamedElementCollection<CategoryFilterEntry>();
            categoryEntries.Add(new CategoryFilterEntry("category1"));
            categoryEntries.Add(new CategoryFilterEntry("category2"));
            categoryEntries.Add(new CategoryFilterEntry("category3"));
            CategoryFilterData filterData = new CategoryFilterData("category", categoryEntries, CategoryFilterMode.AllowAllExceptDenied);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.LogFilters.Add(filterData);

            ILogFilter filter = GetFilter(filterData.Name, helper.configurationSource);

            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(CategoryFilter));
            Assert.AreEqual(3, ((CategoryFilter)filter).CategoryFilters.Count);
            Assert.IsTrue(((CategoryFilter)filter).CategoryFilters.Contains("category1"));
            Assert.IsTrue(((CategoryFilter)filter).CategoryFilters.Contains("category2"));
            Assert.IsTrue(((CategoryFilter)filter).CategoryFilters.Contains("category3"));
            Assert.IsFalse(((CategoryFilter)filter).CategoryFilters.Contains("category4"));
            Assert.AreEqual(CategoryFilterMode.AllowAllExceptDenied, ((CategoryFilter)filter).CategoryFilterMode);
        }

        [TestMethod]
        public void CanCreatePriorityFilterFromConfiguration()
        {
            PriorityFilterData filterData = new PriorityFilterData(1000);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.LogFilters.Add(filterData);

            ILogFilter filter = GetFilter(filterData.Name, helper.configurationSource);

            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(PriorityFilter));
            Assert.AreEqual(1000, ((PriorityFilter)filter).MinimumPriority);
        }

        [TestMethod]
        public void PriorityFilterMaximumPriotDefaultsToMaxIntWhenNotSpecified()
        {
            PriorityFilterData filterData = new PriorityFilterData(1000);
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.LogFilters.Add(filterData);

            ILogFilter filter = GetFilter(filterData.Name, helper.configurationSource);

            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(PriorityFilter));
            Assert.AreEqual(int.MaxValue, ((PriorityFilter)filter).MaximumPriority);
        }

        [TestMethod]
        public void PriorityFilterShouldNotLogWhenPriotityIsAboveMaxPriority()
        {
            var filterData = new PriorityFilterData(0)
            {
                MaximumPriority = 100
            };

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.LogFilters.Add(filterData);

            ILogFilter filter = GetFilter(filterData.Name, helper.configurationSource);

            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(PriorityFilter));
            Assert.IsTrue(((PriorityFilter)filter).ShouldLog(100));
            Assert.IsFalse(((PriorityFilter)filter).ShouldLog(101));
        }

        [TestMethod]
        public void CanCreateLogEnabledFilterFromConfiguration()
        {
            LogEnabledFilterData filterData = new LogEnabledFilterData(true);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.LogFilters.Add(filterData);

            ILogFilter filter = GetFilter(filterData.Name, helper.configurationSource);

            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(LogEnabledFilter));
            Assert.AreEqual(true, ((LogEnabledFilter)filter).Enabled);
        }
    }
}
