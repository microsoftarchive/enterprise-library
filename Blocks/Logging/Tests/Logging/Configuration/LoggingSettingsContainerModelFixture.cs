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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationsForEmptyLoggingSettings
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            LoggingSettings settings = new LoggingSettings();
            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForTraceListeners()
        {
            Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType == typeof(TraceListener)).Count());
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForLogFilters()
        {
            Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType == typeof(ILogFilter)).Count());
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForLogFormatters()
        {
            Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType == typeof(ILogFormatter)).Count());
        }

        [TestMethod]
        public void ThenHasRegistrationsForSpecialSources()
        {
            Assert.AreEqual(3, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        }

        [TestMethod]
        public void ThenHasRegistrationForLogWriter()
        {
            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(LogWriter)).Count());
        }

        [TestMethod]
        public void TheRegistrationForLogWriterIsForNullName()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

            registration
                .AssertForServiceType(typeof(LogWriter))
                .ForName(null)
                .ForImplementationType(typeof(LogWriter));
        }

        [TestMethod]
        public void TheRegistrationForLogWriterHasExpectedConstructorParameters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

            registration
                .AssertConstructor()
                .WithContainerResolvedEnumerableConstructorParameter<ILogFilter>(new string[0])
                .WithContainerResolvedEnumerableConstructorParameter<LogSource>(new string[0])
                .WithContainerResolvedParameter<LogSource>("___ALL")
                .WithContainerResolvedParameter<LogSource>("___NO_MATCHES")
                .WithContainerResolvedParameter<LogSource>("___ERRORS")
                .WithValueConstructorParameter("")
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(false)
                .VerifyConstructorParameters();
        }


        //todo: Remove when instrumentation resolved.

        [TestMethod]
        [Ignore]
        public void ThenHasRegistrationForTraceManager()
        {
            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(TraceManager)).Count());
        }

        [TestMethod]
        [Ignore]
        public void TheRegistrationForTraceManagerIsForNullName()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(TraceManager));

            registration
                .AssertForServiceType(typeof(TraceManager))
                .ForName(null)
                .ForImplementationType(typeof(TraceManager));
        }

        [TestMethod]
        [Ignore]
        public void TheRegistrationForTraceManagerHasExpectedConstructorParameters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(TraceManager));

            registration
                .AssertConstructor()
                .WithContainerResolvedParameter<LogWriter>(null)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenRegistrationsForLoggingSettingsWithTraceListeners
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            LoggingSettings settings = new LoggingSettings();
            settings.TraceListeners.Add(new FormattedEventLogTraceListenerData("event log", "source", null));
            settings.TraceListeners.Add(new WmiTraceListenerData("wmi"));

            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenHasRegistrationsForTheTraceListenersInTheSettings()
        {
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(TraceListener)
                    && tr.ImplementationType == typeof(FormattedEventLogTraceListener)
                    && tr.Name == "event log"));
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(TraceListener)
                    && tr.ImplementationType == typeof(WmiTraceListener)
                    && tr.Name == "wmi"));
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(2, registrations.Where(tr => tr.ServiceType == typeof(TraceListener)).Count());
        }
    }

    [TestClass]
    public class GivenRegistrationsForLoggingSettingsWithFilters
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            LoggingSettings settings = new LoggingSettings();
            settings.LogFilters.Add(new PriorityFilterData("priority", 10));
            settings.LogFilters.Add(new LogEnabledFilterData("enabled", false));

            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenHasRegistrationsForTheFiltersInTheSettings()
        {
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(ILogFilter)
                    && tr.ImplementationType == typeof(PriorityFilter)
                    && tr.Name == "priority"));
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(ILogFilter)
                    && tr.ImplementationType == typeof(LogEnabledFilter)
                    && tr.Name == "enabled"));
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(2, registrations.Where(tr => tr.ServiceType == typeof(ILogFilter)).Count());
        }

        [TestMethod]
        public void TheRegistrationForLogWriterInjectsTheFilters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

            registration
                .AssertConstructor()
                .WithContainerResolvedEnumerableConstructorParameter<ILogFilter>(new[] { "priority", "enabled" })
                .WithContainerResolvedEnumerableConstructorParameter<LogSource>(new string[0])
                .WithContainerResolvedParameter<LogSource>("___ALL")
                .WithContainerResolvedParameter<LogSource>("___NO_MATCHES")
                .WithContainerResolvedParameter<LogSource>("___ERRORS")
                .WithValueConstructorParameter("")
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(false)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenRegistrationsForLoggingSettingsWithFormatters
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            LoggingSettings settings = new LoggingSettings();
            settings.Formatters.Add(new TextFormatterData("text", "template"));
            settings.Formatters.Add(new CustomFormatterData("custom", typeof(MockCustomLogFormatter)));

            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenHasRegistrationsForTheFormattersInTheSettings()
        {
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(ILogFormatter)
                    && tr.ImplementationType == typeof(TextFormatter)
                    && tr.Name == "text"));
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(ILogFormatter)
                    && tr.ImplementationType == typeof(MockCustomLogFormatter)
                    && tr.Name == "custom"));
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(2, registrations.Where(tr => tr.ServiceType == typeof(ILogFormatter)).Count());
        }
    }

    [TestClass]
    public class GivenRegistrationsForLoggingSettingsWithUserDefinedTraceSources
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            LoggingSettings settings = new LoggingSettings();
            settings.TraceSources.Add(new TraceSourceData("source 1", SourceLevels.Critical));
            settings.TraceSources.Add(new TraceSourceData("source 2", SourceLevels.Critical));

            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenHasRegistrationsForTheSourcesInTheSettings()
        {
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(LogSource)
                    && tr.ImplementationType == typeof(LogSource)
                    && tr.Name == "source 1"));
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(LogSource)
                    && tr.ImplementationType == typeof(LogSource)
                    && tr.Name == "source 2"));
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(5, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        }
    }

    [TestClass]
    public class GivenRegistrationsForLoggingSettingsWithConfiguredSpecialSources
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            LoggingSettings settings = new LoggingSettings();
            settings.SpecialTraceSources.AllEventsTraceSource.DefaultLevel = SourceLevels.Critical;
            settings.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.Error;
            settings.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.Verbose;

            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenHasRegistrationForTheAllEventsSourceInTheSettings()
        {
            var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___ALL");
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("Name")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Critical);
        }

        [TestMethod]
        public void ThenHasRegistrationForTheNotProcessedSourceInTheSettings()
        {
            var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___NO_MATCHES");
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("Name")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenHasRegistrationForTheErrorsSourceInTheSettings()
        {
            var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___ERRORS");
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("Name")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Verbose);
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(3, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        }
    }
}
