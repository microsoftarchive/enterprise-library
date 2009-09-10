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

    [TestClass]
    public class When_CallingSendToEventLogListenerOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        string listenerName = "event listener";
        protected override void Arrange()
        {
            base.Arrange();
            CategorySourceBuilder.SendTo.EventLog(listenerName);
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
    public class When_CallingSendToEventLogListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToEventLog_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.EventLog(null);
        }
    }

    public abstract class Given_EventLogListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToEventLogTraceListener EventLogTraceListenerBuilder;
        private string eventLogListenerName = "eventLogListener";

        protected override void Arrange()
        {
            base.Arrange();

            EventLogTraceListenerBuilder = base.CategorySourceBuilder.SendTo.EventLog(eventLogListenerName);
        }

        protected FormattedEventLogTraceListenerData GetEventLogTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<FormattedEventLogTraceListenerData>().Where(x => x.Name == eventLogListenerName).First();
        }
    }

    [TestClass]
    public class When_SettingLogToOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            base.EventLogTraceListenerBuilder.ToLog("log name");
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
                                                    .WithTraceOptions(TraceOptions.Callstack);
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
                                                    .WithTraceOptions(TraceOptions.Callstack);
        }

        protected override void Act()
        {
            base.EventLogTraceListenerBuilder.WithTraceOptions(TraceOptions.DateTime);
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
            EventLogTraceListenerBuilder.UsingEventLogSource(sourceName);
        }

        [TestMethod]
        public void ThenConfiguredEventLogTracelistnerCallstackAndDateTimeOptions()
        {
            Assert.AreEqual(sourceName, GetEventLogTraceListenerData().Source);
        }
    }

    [TestClass]
    public class When_SettingNullEventSourceOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithEventSource_ThrowsArgumentException()
        {
            EventLogTraceListenerBuilder.UsingEventLogSource(null);
        }
    }

    [TestClass]
    public class When_SettingFilterOnEventLogListenerBuilder : Given_EventLogListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            EventLogTraceListenerBuilder
                .Filter(SourceLevels.Warning);
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
            base.EventLogTraceListenerBuilder =
               base.EventLogTraceListenerBuilder
                       .FormatWith(new FormatterBuilder()
                                        .TextFormatterNamed("myNewFormatter")
                                        .UsingTemplate("some template"))
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
    public class When_SettingTextFormatterOnEventLogListenerBuilderWithSpecificTempalte : Given_EventLogListenerInConfigurationSourceBuilder
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


    [TestClass]
    public class When_SettingNullFormatterBuilderOnEventLogListener : Given_EventLogListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FormatWith_ThrowsArgumentNullException()
        {
            base.EventLogTraceListenerBuilder.FormatWith(null);
        }
    }
}
