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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{

    public abstract class Given_ConfigurationSourceBuilder : ArrangeActAssert
    {
        protected ConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        public IConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }
    }

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

    public abstract class Given_LoggingCategorySourceInConfigurationSourceBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        private string categoryName = "categegory";
        protected ILoggingConfigurationCategoryStart CategorySourceBuilder;

        protected override void Arrange()
        {
            base.Arrange();

            CategorySourceBuilder = ConfigureLogging.LogToCategoryNamed(categoryName);
        }

        protected TraceSourceData GetTraceSourceData()
        {
            return base.GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
        }
    }

    public abstract class Given_EventLogListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToEventLogTraceListener EventLogTraceListenerBuilder;
        private string eventLogListenerName = "eventLogListener";

        protected override void Arrange()
        {
            base.Arrange();

            EventLogTraceListenerBuilder = base.CategorySourceBuilder.SendTo().EventLog(eventLogListenerName);
        }

        protected FormattedEventLogTraceListenerData GetEventLogTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<FormattedEventLogTraceListenerData>().Where(x => x.Name == eventLogListenerName).First();
        }
    }

    [TestClass]
    public class When_ConfiguringLoggongOnConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder.ConfigureLogging();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsLoggingSettings()
        {
            var configurationSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configurationSource);
            
            Assert.IsNotNull(configurationSource.GetSection(LoggingSettings.SectionName));
        }

        [TestMethod]
        public void Then_RevertImpersonationIsTrue()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.IsTrue(loggingSettings.RevertImpersonation);
        }

        [TestMethod]
        public void Then_EnableTracingIsFalse()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.IsFalse(loggingSettings.TracingEnabled);
        }
    }

    [TestClass]
    public class When_EnablingTracingOnLoggingSettings : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
            ConfigureLogging.EnableTracing();
        }

        [TestMethod]
        public void Then_TracingIsEnabledInConfiguration()
        {
            Assert.IsTrue(GetLoggingConfiguration().TracingEnabled);
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
        [Ignore]
        public void Then_AutoFlushIsSetToFalse()
        {
            TraceSourceData data = GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
            Assert.AreEqual(false, data.AutoFlush);
        }

    }

    [TestClass]
    public class When_CallingToSourceLevelOnLogToCategoryOnLoggginSettings : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            CategorySourceBuilder.ToSourceLevel(SourceLevels.Information);
        }

        [TestMethod]
        public void Then_SourceLevelIsSetOnTraceSource()
        {
            Assert.AreEqual(SourceLevels.Information, GetTraceSourceData().DefaultLevel);
        }
    }

    [TestClass]
    public class When_CallingSendToSharedListenerOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
            CategorySourceBuilder.SendTo().SharedListenerNamed("shared listener");
        }

        [TestMethod]
        public void Then_CategorySourceContainsTraceListenerReference()
        {
            Assert.AreEqual(1, GetTraceSourceData().TraceListeners.Count);
        }

        [TestMethod]
        public void Then_TraceListenerReferenceHasAppropriateName()
        {
            Assert.AreEqual("shared listener", GetTraceSourceData().TraceListeners.First().Name);
        }
    }

    [TestClass]
    public class When_CallingSendToEventLogListenerOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        string listenerName = "event listener";
        protected override void Arrange()
        {
            base.Arrange();
            CategorySourceBuilder.SendTo()
                .EventLog(listenerName);
        }

        [TestMethod]
        public void Then_CategorySourceContainsTraceListenerReference()
        {
            Assert.AreEqual(1, GetTraceSourceData().TraceListeners.Count);
        }

        [TestMethod]
        public void Then_TraceListenerReferenceHasAppropriateName()
        {
            Assert.AreEqual(listenerName, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void Then_LoggingConfigurationContainsTracelistenerDefinition()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.Where(x => x.Name == listenerName).Any());
        }
    }

    [TestClass]
    public class When_SettingLogToOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            base.EventLogTraceListenerBuilder
                    .ToLog("log name");
        }

        [TestMethod]
        public void ThenConfiguredEventLogTracelistnerHasLogName()
        {
            Assert.AreEqual("log name", GetEventLogTraceListenerData().Log);
        }
    }

    [TestClass]
    public class When_SettingLogToMachineOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            base.EventLogTraceListenerBuilder.ToMachine("MACHINENAME");
        }

        [TestMethod]
        public void ThenConfiguredEventLogTracelistnerHasLogName()
        {
            Assert.AreEqual("MACHINENAME", GetEventLogTraceListenerData().MachineName);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionsOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EventLogTraceListenerBuilder = base.EventLogTraceListenerBuilder
                                                    .WithTraceOption(TraceOptions.Callstack);
        }

        [TestMethod]
        public void ThenConfiguredEventLogTracelistnerHasTraceOptionCallstack()
        {
            Assert.AreEqual(TraceOptions.Callstack, GetEventLogTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenCanContinueSettingMachineName()
        {
            //fails compilation if method is not available
            base.EventLogTraceListenerBuilder
                .ToMachine("string");
        }
    }

    [TestClass]
    public class When_SettingMulitpleTraceOptionsOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            base.EventLogTraceListenerBuilder = base.EventLogTraceListenerBuilder
                                                    .WithTraceOption(TraceOptions.Callstack);
        }

        protected override void Act()
        {
            base.EventLogTraceListenerBuilder.WithTraceOption(TraceOptions.DateTime);
        }

        [TestMethod]
        public void Then_LastInWins()
        {
            Assert.AreEqual(TraceOptions.DateTime, GetEventLogTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingEventSourceOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        private string sourceName = "eventSource";

        protected override void Act()
        {
            EventLogTraceListenerBuilder.WithEventSource(sourceName);
        }

        [TestMethod]
        public void ThenConfiguredEventLogTracelistnerCallstackAndDateTimeOptions()
        {
            Assert.AreEqual(sourceName, GetEventLogTraceListenerData().Source);
        }
    }

    [TestClass]
    public class When_SettingFilterOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            EventLogTraceListenerBuilder
                .LogEventSourceLevel(SourceLevels.Warning);
        }

        [TestMethod]
        public void ThenConfiguredEventLogTracelistnerCallstackAndDateTimeOptions()
        {
            Assert.AreEqual(SourceLevels.Warning, GetEventLogTraceListenerData().Filter);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EventLogTraceListenerBuilder =
                base.EventLogTraceListenerBuilder
                        .FormatWithSharedFormatter("formatter name");
        }

        [TestMethod]
        public void ThenTraceListenerHasFormatter()
        {
            var eventLogTraceListener = base.GetEventLogTraceListenerData();
            Assert.AreEqual("formatter name", eventLogTraceListener.Formatter);
        }
    }

    [TestClass]
    public class When_SettingTextFormatterOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {

            //var configurationSourceBuilder = new ConfigurationSourceBuilder();

            //configurationSourceBuilder.ConfigureLogging()
            //        .EnableTracing()
            //        .DoNotRevertImpersonation()
            //        .LogToCategoryNamed("General")
            //            .SendTo().EventLog("EventLogTraceListner")
            //         .LogToCategoryNamed("Other Category")  // InterfaceStart
            //            .ToSourceLevel(SourceLevels.Error) //<-- log category
            //            .DoNotAutoFlushEntries() // <-- log category

            //            .SendTo().SharedListenerNamed("EventLogTraceListener")
            //            .SendTo().EventLog("Other EventLog Category") // <-- return Contd;
            //                .FormatWith(new FormatterBuilder().TextFormatterNamed("Text formatter1"))
            //                .ToMachine("BOBBRUM")
            //                .WithTraceOption(TraceOptions.Callstack | TraceOptions.ThreadId)
            //                .WithEventSource("event Source")
            //                .LogEventSourceLevel(SourceLevels.Warning)
            //            .SendTo().SharedListenerNamed("foobar1")
            //         .LogToCategoryNamed("third Category")
            //            .DoNotAutoFlushEntries()
            //            .ToSourceLevel(SourceLevels.Error)
            //            .SendTo().SharedListenerNamed("One of 'm above me");
            
            //var source = configurationSourceBuilder.ConfigurationSource;


            base.EventLogTraceListenerBuilder =
               base.EventLogTraceListenerBuilder
                       .FormatWith( new FormatterBuilder()
                                        .TextFormatterNamed("myNewFormatter")
                                        .UsingTemplate("some template") )
                        .ToMachine("Machine Name");
        }

        [TestMethod]
        public void ThenTraceListenerHasFormatter()
        {
            var eventLogTraceListener = base.GetEventLogTraceListenerData();
            Assert.AreEqual("myNewFormatter", eventLogTraceListener.Formatter);
        }

        [TestMethod]
        public void ThenTextFormatterOfNameIsAddedToSettings()
        {
            Assert.IsTrue(base.GetLoggingConfiguration().Formatters.OfType<TextFormatterData>().Where(f => f.Name == "myNewFormatter").Any());
        }

        [TestMethod]
        public void ThenTextFormatterTemplateIsConfigured()
        {
            var formatterData = base.GetLoggingConfiguration().Formatters.OfType<TextFormatterData>().Single(f => f.Name == "myNewFormatter");
            Assert.AreEqual("some template", formatterData.Template);
        }
    }

    [TestClass]
    public class When_SettingTextFormatterOnEventLogListenerBuilderWithSpecifyingTempalte : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EventLogTraceListenerBuilder =
               base.EventLogTraceListenerBuilder
                       .FormatWith(new FormatterBuilder()
                                        .TextFormatterNamed("myNewFormatter"))
                        .ToMachine("Machine Name");
        }

        [TestMethod]
        public void Then_TemplateShouldNotBeEmpty()
        {
            var formatterData = base.GetLoggingConfiguration().Formatters.OfType<TextFormatterData>().Single(f => f.Name == "myNewFormatter");
            Assert.IsFalse(string.IsNullOrEmpty(formatterData.Template));
        }

    }
}
