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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
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
            registrations = settings.GetRegistrations(null);
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
        public void TheRegistrationForLogWriterIsDefaultAndSingletonAndPublicName()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

            registration
                .AssertForServiceType(typeof(LogWriter))
                .IsDefault()
                .IsSingleton()
                .IsPublicName()
                .ForImplementationType(typeof(LogWriterImpl));
        }

        [TestMethod]
        public void TheRegistrationForLogWriterHasExpectedConstructorParameters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

            registration
                .AssertConstructor()
                .WithContainerResolvedParameter<LogWriterStructureHolder>(null)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenHasRegistrationForLoggingInstrumentationProvider()
        {
            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(ILoggingInstrumentationProvider)).Count());
        }

        //[TestMethod]
        //public void ThenHasRegistrationForTraceManager()
        //{
        //    Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(TraceManager)).Count());
        //}

        //[TestMethod]
        //public void TheRegistrationForTraceManagerIsForNullName()
        //{
        //    TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(TraceManager));

        //    registration
        //        .AssertForServiceType(typeof(TraceManager))
        //        .IsDefault()
        //        .IsNotPublicName()
        //        .ForImplementationType(typeof(TraceManager));
        //}

        //[TestMethod]
        //public void TheRegistrationForTraceManagerHasExpectedConstructorParameters()
        //{
        //    TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(TraceManager));

        //    registration
        //        .AssertConstructor()
        //        .WithContainerResolvedParameter<LogWriter>(null)
        //        .WithContainerResolvedParameter<ITracerInstrumentationProvider>(null)
        //        .VerifyConstructorParameters();
        //}

        [TestMethod]
        public void ThenHasRegistrationForLogWriterStructureHolder()
        {
            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(LogWriterStructureHolder)).Count());
        }

        [TestMethod]
        public void TheRegistrationForLogWriterStructureHolderIsForNullName()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriterStructureHolder));

            registration
                .AssertForServiceType(typeof(LogWriterStructureHolder))
                .IsDefault()
                .IsNotPublicName()
                .ForImplementationType(typeof(LogWriterStructureHolder));
        }

        [TestMethod]
        public void TheRegistrationForLogWriterStructureHolderHasExpectedConstructorParameters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriterStructureHolder));
            IEnumerable<string> traceSourceNames;

            registration
                .AssertConstructor()
                .WithContainerResolvedEnumerableConstructorParameter<ILogFilter>(new string[0])
                .WithValueConstructorParameter(out traceSourceNames)
                .WithContainerResolvedEnumerableConstructorParameter<LogSource>(new string[0])
                .WithContainerResolvedParameter<LogSource>("___ALL")
                .WithContainerResolvedParameter<LogSource>("___NO_MATCHES")
                .WithContainerResolvedParameter<LogSource>("___ERRORS")
                .WithValueConstructorParameter("")
                .WithValueConstructorParameter(true)
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(false)
                .VerifyConstructorParameters();

            Assert.AreEqual(0, traceSourceNames.Count());
        }

        [TestMethod]
        public void TheRegistrationForDefaultNotificationTraceIsDefaultAndSingletonAndPublicName()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(ITraceDispatcher));

            registration
                .AssertForServiceType(typeof(ITraceDispatcher))
                .IsDefault()
                .IsSingleton()
                .IsPublicName()
                .ForImplementationType(typeof(DefaultTraceDispatcher));
        }

        [TestMethod]
        public void TheRegistrationForDefaultNotificationTraceHasExpectedConstructorParameters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(ITraceDispatcher));

            registration
                .AssertConstructor()
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
            settings.TraceListeners.Add(new MockTraceListenerData<MockTraceListener1> { Name = "listener1" });
            settings.TraceListeners.Add(new MockTraceListenerData<MockTraceListener2> { Name = "listener2" });

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationsForTheTraceListenersInTheSettings()
        {
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(TraceListener)
                    && tr.ImplementationType == typeof(MockTraceListener1)
                    && tr.Name == "listener1"));
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(TraceListener)
                    && tr.ImplementationType == typeof(MockTraceListener2)
                    && tr.Name == "listener2"));
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(2, registrations.Where(tr => tr.ServiceType == typeof(TraceListener)).Count());
        }

        [TestMethod]
        public void ThenTraceListenerRegistrationIsSingleton()
        {
            var registration = registrations.FirstOrDefault(tr => tr.ServiceType == typeof(TraceListener));

            Assert.AreEqual(TypeRegistrationLifetime.Singleton, registration.Lifetime);
        }

        public class MockTraceListener1 : TraceListener
        {
            public override void Write(string message) { }
            public override void WriteLine(string message) { }
        }

        public class MockTraceListener2 : TraceListener
        {
            public override void Write(string message) { }
            public override void WriteLine(string message) { }
        }

        public class MockTraceListenerData<T> : TraceListenerData
            where T : TraceListener, new()
        {
            protected override System.Linq.Expressions.Expression<System.Func<TraceListener>> GetCreationExpression()
            {
                return () => new T();
            }
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
            settings.LogFilters.Add(new PriorityFilterData { Name = "priority", MinimumPriority = 10 });
            settings.LogFilters.Add(new LogEnabledFilterData { Name = "enabled", Enabled = false });

            registrations = settings.GetRegistrations(null);
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
        public void TheRegistrationForLogWriterStructureHolderInjectsTheFilters()
        {
            TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriterStructureHolder));
            IEnumerable<string> sourceNames;

            registration
                .AssertConstructor()
                .WithContainerResolvedEnumerableConstructorParameter<ILogFilter>(new[] { "priority", "enabled" })
                .WithValueConstructorParameter(out sourceNames)
                .WithContainerResolvedEnumerableConstructorParameter<LogSource>(new string[0])
                .WithContainerResolvedParameter<LogSource>("___ALL")
                .WithContainerResolvedParameter<LogSource>("___NO_MATCHES")
                .WithContainerResolvedParameter<LogSource>("___ERRORS")
                .WithValueConstructorParameter("")
                .WithValueConstructorParameter(true)
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
            settings.Formatters.Add(new TextFormatterData { Name = "text", Template = "template" });

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationsForTheFormattersInTheSettings()
        {
            Assert.IsTrue(
                registrations.Any(tr =>
                    tr.ServiceType == typeof(ILogFormatter)
                    && tr.ImplementationType == typeof(TextFormatter)
                    && tr.Name == "text"));
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherFormatters()
        {
            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(ILogFormatter)).Count());
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
            settings.TraceSources.Add(new TraceSourceData { Name = "source 1", DefaultLevel = SourceLevels.Critical });
            settings.TraceSources.Add(new TraceSourceData { Name = "source 2", DefaultLevel = SourceLevels.Critical });

            registrations = settings.GetRegistrations(null);
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
            settings.SpecialTraceSources.AllEventsTraceSource.AutoFlush = false;
            settings.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.Error;
            settings.SpecialTraceSources.NotProcessedTraceSource.AutoFlush = true;
            settings.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.Verbose;
            settings.SpecialTraceSources.ErrorsTraceSource.AutoFlush = false;

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationForTheAllEventsSourceInTheSettings()
        {
            var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___ALL");
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("Name")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Critical)
                .WithValueConstructorParameter(false)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenHasRegistrationForTheNotProcessedSourceInTheSettings()
        {
            var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___NO_MATCHES");
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("Name")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Error)
                .WithValueConstructorParameter(true)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenHasRegistrationForTheErrorsSourceInTheSettings()
        {
            var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___ERRORS");
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("Name")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Verbose)
                .WithValueConstructorParameter(false)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenHasNoRegistrationsForOtherTraceListeners()
        {
            Assert.AreEqual(3, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        }
    }
}
