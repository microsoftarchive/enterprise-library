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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Messaging;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    public abstract class Given_LoggingSettingsInConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected ILoggingConfigurationStart ConfigureLogging;

        protected override void Arrange()
        {
            base.Arrange();
            ConfigureLogging = base.ConfigurationSourceBuilder.ConfigureLogging();
        }

        protected LoggingSettings GetLoggingConfiguration()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            return loggingSettings;
        }
    }

    [TestClass]
    public class When_AddingSettingsThroughLoggingConfigurationBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        public void Then_LoggingSettingsContainSpecialSources()
        {
            var loggingConfig = GetLoggingConfiguration();
            Assert.IsNotNull(loggingConfig.SpecialTraceSources);
        }
    }

    [TestClass]
    public class When_AccessingErrorsAndWarningsCategoryOnLoggingConfigurationBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        ILoggingConfigurationCategoryStart categoryStart;
        
        protected override void Act()
        {
            categoryStart = ConfigureLogging.SpecialSources.LoggingErrorsAndWarningsCategory;
        }

        [TestMethod]
        public void Then_LoggingSettingsContainsErrorsAndWarningCategory()
        {
            var loggingConfig = GetLoggingConfiguration();
            Assert.IsNotNull(loggingConfig.SpecialTraceSources.ErrorsTraceSource);
            Assert.AreEqual("Logging Errors & Warnings", loggingConfig.SpecialTraceSources.ErrorsTraceSource.Name);
        }

        [TestMethod]
        public void Then_CurrentCategoryIsErrorsAndWarnings()
        {
            var loggingConfig = GetLoggingConfiguration();
            ILoggingConfigurationSendToExtension internalC = (ILoggingConfigurationSendToExtension)categoryStart;

            Assert.AreEqual(loggingConfig.SpecialTraceSources.ErrorsTraceSource, internalC.CurrentTraceSource);
        }
    }

    [TestClass]
    public class When_AccessingUnprocessedCategoryOnLoggingConfigurationBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        ILoggingConfigurationCategoryStart categoryStart;

        protected override void Act()
        {
            categoryStart = ConfigureLogging.SpecialSources.UnprocessedCategory;
        }

        [TestMethod]
        public void Then_LoggingSettingsContainsErrorsAndWarningCategory()
        {
            var loggingConfig = GetLoggingConfiguration();
            Assert.IsNotNull(loggingConfig.SpecialTraceSources.NotProcessedTraceSource);
            Assert.AreEqual("Unprocessed Category", loggingConfig.SpecialTraceSources.NotProcessedTraceSource.Name);
        }

        [TestMethod]
        public void Then_CurrentCategoryIsErrorsAndWarnings()
        {
            var loggingConfig = GetLoggingConfiguration();
            ILoggingConfigurationSendToExtension internalC = (ILoggingConfigurationSendToExtension)categoryStart;

            Assert.AreEqual(loggingConfig.SpecialTraceSources.NotProcessedTraceSource, internalC.CurrentTraceSource);
        }
    }

    [TestClass]
    public class When_AccessingAllEventsCategoryOnLoggingConfigurationBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        ILoggingConfigurationCategoryStart categoryStart;

        protected override void Act()
        {
            categoryStart = ConfigureLogging.SpecialSources.AllEventsCategory;
        }

        [TestMethod]
        public void Then_LoggingSettingsContainsErrorsAndWarningCategory()
        {
            var loggingConfig = GetLoggingConfiguration();
            Assert.IsNotNull(loggingConfig.SpecialTraceSources.AllEventsTraceSource);
            Assert.AreEqual("All Events", loggingConfig.SpecialTraceSources.AllEventsTraceSource.Name);
        }

        [TestMethod]
        public void Then_CurrentCategoryIsErrorsAndWarnings()
        {
            var loggingConfig = GetLoggingConfiguration();
            ILoggingConfigurationSendToExtension internalC = (ILoggingConfigurationSendToExtension)categoryStart;

            Assert.AreEqual(loggingConfig.SpecialTraceSources.AllEventsTraceSource, internalC.CurrentTraceSource);
        }
    }

    [TestClass]
    public class When_AddingLogFiltersToLoggingConfigurationBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureLogging.WithOptions
                                .FilterEnableOrDisable("enabled")
                                .FilterOnCategory("cat filter").AllowAllCategoriesExcept("cat1", "cat2")
                                .FilterCustom("custom", typeof(object))
                                .FilterOnPriority("prio filter").StartingWithPriority(12).UpToPriority(21);
        }

        public void Then_LoggingConfigurationContainsFilters()
        {
            var loggingConfig = GetLoggingConfiguration();

            Assert.AreEqual(4, loggingConfig.LogFilters.Count);
            Assert.IsTrue(loggingConfig.LogFilters.OfType<PriorityFilterData>().Any());
            Assert.IsTrue(loggingConfig.LogFilters.OfType<CustomLogFilterData>().Any());
            Assert.IsTrue(loggingConfig.LogFilters.OfType<LogEnabledFilterData>().Any());
            Assert.IsTrue(loggingConfig.LogFilters.OfType<CategoryFilterData>().Any());
        }
    }

    [TestClass]
    public class When_DisablingTracingOnLoggingSettings : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
            ConfigureLogging.WithOptions.DisableTracing();
        }

        [TestMethod]
        public void Then_TracingIsDisabledInConfiguration()
        {
            Assert.IsFalse(GetLoggingConfiguration().TracingEnabled);
        }
    }

    [TestClass]
    public class When_CallingDoNotRevertImpersonationOnLoggginSettings : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureLogging.WithOptions.DoNotRevertImpersonation();
        }

        [TestMethod]
        public void Then_RevertImpersonationIsFalse()
        {
            Assert.IsFalse(GetLoggingConfiguration().RevertImpersonation);
        }
    }

    [TestClass]
    public class When_CallingDoNotLogWarningsWhenNoCategoryExists : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureLogging.WithOptions.DoNotLogWarningsWhenNoCategoryExists();
        }

        [TestMethod]
        public void Then_LogWarningIfNoCategoryExistsIsFalse()
        {
            Assert.IsFalse(GetLoggingConfiguration().LogWarningWhenNoCategoriesMatch);
        }
    }

    [TestClass]
    public class When_LogToCategoryNamedOnLoggingSettings : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        private string categoryName = "category";
        protected override void Arrange()
        {
            base.Arrange();

            base.ConfigureLogging.LogToCategoryNamed(categoryName);
        }

        [TestMethod]
        public void Then_CategorySourceIsContainedInConfiguration()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).Any());
        }

        [TestMethod]
        public void Then_CategorySourceHasDefaultLevelSetToAll()
        {
            TraceSourceData data = GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
            Assert.AreEqual(SourceLevels.All, data.DefaultLevel);
        }

        [TestMethod]
        public void Then_CategorySourceNoListeners()
        {
            TraceSourceData data = GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
            Assert.AreEqual(0, data.TraceListeners.Count);
        }

        [TestMethod]
        public void Then_AutoFlushIsSetToTrue()
        {
            TraceSourceData data = GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
            Assert.AreEqual(true, data.AutoFlush);
        }

    }


}
