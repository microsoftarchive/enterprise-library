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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    // TODO replace with corresponding tests
    public class GivenRegistrationsForEmptyLoggingSettings
    {
        //private IEnumerable<TypeRegistration> registrations;

        //    [TestInitialize]
        //    public void Setup()
        //    {
        //        LoggingSettings settings = new LoggingSettings();
        //        registrations = settings.GetRegistrations(null);
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForTraceListeners()
        //    {
        //        Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType == typeof(TraceListener)).Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForLogFilters()
        //    {
        //        Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType == typeof(ILogFilter)).Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForLogFormatters()
        //    {
        //        Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType == typeof(ILogFormatter)).Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationsForSpecialSources()
        //    {
        //        Assert.AreEqual(3, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForLogWriter()
        //    {
        //        Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(LogWriter)).Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForTracerInstrumentationProvider()
        //    {
        //        Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(ITracerInstrumentationProvider)).Count());
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForLogWriterIsDefaultAndSingletonAndPublicName()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

        //        registration
        //            .AssertForServiceType(typeof(LogWriter))
        //            .IsDefault()
        //            .IsSingleton()
        //            .IsPublicName()
        //            .ForImplementationType(typeof(LogWriterImpl));
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForLogWriterHasExpectedConstructorParameters()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriter));

        //        registration
        //            .AssertConstructor()
        //            .WithContainerResolvedParameter<LogWriterStructureHolder>(null)
        //            .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
        //            .WithContainerResolvedParameter<ILoggingUpdateCoordinator>(null)
        //            .VerifyConstructorParameters();
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForLoggingInstrumentationProvider()
        //    {
        //        Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(ILoggingInstrumentationProvider)).Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForTraceManager()
        //    {
        //        Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(TraceManager)).Count());
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForTraceManagerIsForNullName()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(TraceManager));

        //        registration
        //            .AssertForServiceType(typeof(TraceManager))
        //            .IsDefault()
        //            .IsNotPublicName()
        //            .ForImplementationType(typeof(TraceManager));
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForTraceManagerHasExpectedConstructorParameters()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(TraceManager));

        //        registration
        //            .AssertConstructor()
        //            .WithContainerResolvedParameter<LogWriter>(null)
        //            .WithContainerResolvedParameter<ITracerInstrumentationProvider>(null)
        //            .VerifyConstructorParameters();
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForLogWriterStructureHolder()
        //    {
        //        Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(LogWriterStructureHolder)).Count());
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForLogWriterStructureHolderIsForNullName()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriterStructureHolder));

        //        registration
        //            .AssertForServiceType(typeof(LogWriterStructureHolder))
        //            .IsDefault()
        //            .IsNotPublicName()
        //            .ForImplementationType(typeof(LogWriterStructureHolder));
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForLogWriterStructureHolderHasExpectedConstructorParameters()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriterStructureHolder));
        //        IEnumerable<string> traceSourceNames;

        //        registration
        //            .AssertConstructor()
        //            .WithContainerResolvedEnumerableConstructorParameter<ILogFilter>(new string[0])
        //            .WithValueConstructorParameter(out traceSourceNames)
        //            .WithContainerResolvedEnumerableConstructorParameter<LogSource>(new string[0])
        //            .WithContainerResolvedParameter<LogSource>("___ALL")
        //            .WithContainerResolvedParameter<LogSource>("___NO_MATCHES")
        //            .WithContainerResolvedParameter<LogSource>("___ERRORS")
        //            .WithValueConstructorParameter("")
        //            .WithValueConstructorParameter(true)
        //            .WithValueConstructorParameter(true)
        //            .WithValueConstructorParameter(true)
        //            .VerifyConstructorParameters();

        //        Assert.AreEqual(0, traceSourceNames.Count());
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForLoggingUpdateConfigurator()
        //    {
        //        Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(ILoggingUpdateCoordinator)).Count());
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForLoggingUpdateConfiguratorIsForNullName()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(ILoggingUpdateCoordinator));

        //        registration
        //            .AssertForServiceType(typeof(ILoggingUpdateCoordinator))
        //            .IsDefault()
        //            .IsNotPublicName()
        //            .ForImplementationType(typeof(LoggingUpdateCoordinator));
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForILoggingUpdateCoordinatorIsForDefaultSingleton()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(ILoggingUpdateCoordinator));

        //        Assert.AreEqual(TypeRegistrationLifetime.Singleton, registration.Lifetime);
        //        Assert.IsTrue(registration.IsDefault);
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForILoggingUpdateCoordinatorHasExpectedConstructorParameters()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(ILoggingUpdateCoordinator));

        //        registration
        //            .AssertConstructor()
        //            .WithContainerResolvedParameter<ConfigurationChangeEventSource>(null)
        //            .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
        //            .VerifyConstructorParameters();
        //    }
        //}

        //[TestClass]
        //public class GivenRegistrationsForLoggingSettingsWithTraceListeners
        //{
        //    private IEnumerable<TypeRegistration> registrations;

        //    [TestInitialize]
        //    public void Setup()
        //    {
        //        LoggingSettings settings = new LoggingSettings();
        //        settings.TraceListeners.Add(new FormattedEventLogTraceListenerData("event log", "source", null));
        //        settings.TraceListeners.Add(new WmiTraceListenerData("wmi"));

        //        registrations = settings.GetRegistrations(null);
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationsForTheTraceListenersInTheSettings()
        //    {
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(TraceListener)
        //                && tr.ImplementationType == typeof(ReconfigurableTraceListenerWrapper)
        //                && tr.Name == "event log"));
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(TraceListener)
        //                && tr.ImplementationType == typeof(FormattedEventLogTraceListener)
        //                && tr.Name == "event log\u200Cimplementation"));
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(TraceListener)
        //                && tr.ImplementationType == typeof(ReconfigurableTraceListenerWrapper)
        //                && tr.Name == "wmi"));
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(TraceListener)
        //                && tr.ImplementationType == typeof(WmiTraceListener)
        //                && tr.Name == "wmi\u200Cimplementation"));
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForOtherTraceListeners()
        //    {
        //        Assert.AreEqual(4, registrations.Where(tr => tr.ServiceType == typeof(TraceListener)).Count());
        //    }
        //}

        //[TestClass]
        //public class GivenRegistrationsForLoggingSettingsWithFilters
        //{
        //    private IEnumerable<TypeRegistration> registrations;

        //    [TestInitialize]
        //    public void Setup()
        //    {
        //        LoggingSettings settings = new LoggingSettings();
        //        settings.LogFilters.Add(new PriorityFilterData("priority", 10));
        //        settings.LogFilters.Add(new LogEnabledFilterData("enabled", false));

        //        registrations = settings.GetRegistrations(null);
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationsForTheFiltersInTheSettings()
        //    {
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(ILogFilter)
        //                && tr.ImplementationType == typeof(PriorityFilter)
        //                && tr.Name == "priority"));
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(ILogFilter)
        //                && tr.ImplementationType == typeof(LogEnabledFilter)
        //                && tr.Name == "enabled"));
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForOtherTraceListeners()
        //    {
        //        Assert.AreEqual(2, registrations.Where(tr => tr.ServiceType == typeof(ILogFilter)).Count());
        //    }

        //    [TestMethod]
        //    public void TheRegistrationForLogWriterStructureHolderInjectsTheFilters()
        //    {
        //        TypeRegistration registration = registrations.First(tr => tr.ServiceType == typeof(LogWriterStructureHolder));
        //        IEnumerable<string> sourceNames;

        //        registration
        //            .AssertConstructor()
        //            .WithContainerResolvedEnumerableConstructorParameter<ILogFilter>(new[] { "priority", "enabled" })
        //            .WithValueConstructorParameter(out sourceNames)
        //            .WithContainerResolvedEnumerableConstructorParameter<LogSource>(new string[0])
        //            .WithContainerResolvedParameter<LogSource>("___ALL")
        //            .WithContainerResolvedParameter<LogSource>("___NO_MATCHES")
        //            .WithContainerResolvedParameter<LogSource>("___ERRORS")
        //            .WithValueConstructorParameter("")
        //            .WithValueConstructorParameter(true)
        //            .WithValueConstructorParameter(true)
        //            .WithValueConstructorParameter(true)
        //            .VerifyConstructorParameters();
        //    }
        //}

        //[TestClass]
        //public class GivenRegistrationsForLoggingSettingsWithFormatters
        //{
        //    private IEnumerable<TypeRegistration> registrations;

        //    [TestInitialize]
        //    public void Setup()
        //    {
        //        LoggingSettings settings = new LoggingSettings();
        //        settings.Formatters.Add(new TextFormatterData("text", "template"));
        //        settings.Formatters.Add(new CustomFormatterData("custom", typeof(MockCustomLogFormatter)));

        //        registrations = settings.GetRegistrations(null);
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationsForTheFormattersInTheSettings()
        //    {
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(ILogFormatter)
        //                && tr.ImplementationType == typeof(TextFormatter)
        //                && tr.Name == "text"));
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(ILogFormatter)
        //                && tr.ImplementationType == typeof(MockCustomLogFormatter)
        //                && tr.Name == "custom"));
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForOtherTraceListeners()
        //    {
        //        Assert.AreEqual(2, registrations.Where(tr => tr.ServiceType == typeof(ILogFormatter)).Count());
        //    }
        //}

        //[TestClass]
        //public class GivenRegistrationsForLoggingSettingsWithUserDefinedTraceSources
        //{
        //    private IEnumerable<TypeRegistration> registrations;

        //    [TestInitialize]
        //    public void Setup()
        //    {
        //        LoggingSettings settings = new LoggingSettings();
        //        settings.TraceSources.Add(new TraceSourceData("source 1", SourceLevels.Critical));
        //        settings.TraceSources.Add(new TraceSourceData("source 2", SourceLevels.Critical));

        //        registrations = settings.GetRegistrations(null);
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationsForTheSourcesInTheSettings()
        //    {
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(LogSource)
        //                && tr.ImplementationType == typeof(LogSource)
        //                && tr.Name == "source 1"));
        //        Assert.IsTrue(
        //            registrations.Any(tr =>
        //                tr.ServiceType == typeof(LogSource)
        //                && tr.ImplementationType == typeof(LogSource)
        //                && tr.Name == "source 2"));
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForOtherTraceListeners()
        //    {
        //        Assert.AreEqual(5, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        //    }
        //}

        //[TestClass]
        //public class GivenRegistrationsForLoggingSettingsWithConfiguredSpecialSources
        //{
        //    private IEnumerable<TypeRegistration> registrations;

        //    [TestInitialize]
        //    public void Setup()
        //    {
        //        LoggingSettings settings = new LoggingSettings();
        //        settings.SpecialTraceSources.AllEventsTraceSource.DefaultLevel = SourceLevels.Critical;
        //        settings.SpecialTraceSources.AllEventsTraceSource.AutoFlush = false;
        //        settings.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.Error;
        //        settings.SpecialTraceSources.NotProcessedTraceSource.AutoFlush = true;
        //        settings.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.Verbose;
        //        settings.SpecialTraceSources.ErrorsTraceSource.AutoFlush = false;

        //        registrations = settings.GetRegistrations(null);
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForTheAllEventsSourceInTheSettings()
        //    {
        //        var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___ALL");
        //        registration
        //            .AssertConstructor()
        //            .WithValueConstructorParameter("Name")
        //            .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
        //            .WithValueConstructorParameter(SourceLevels.Critical)
        //            .WithValueConstructorParameter(false)
        //            .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
        //            .VerifyConstructorParameters();
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForTheNotProcessedSourceInTheSettings()
        //    {
        //        var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___NO_MATCHES");
        //        registration
        //            .AssertConstructor()
        //            .WithValueConstructorParameter("Name")
        //            .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
        //            .WithValueConstructorParameter(SourceLevels.Error)
        //            .WithValueConstructorParameter(true)
        //            .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
        //            .VerifyConstructorParameters();
        //    }

        //    [TestMethod]
        //    public void ThenHasRegistrationForTheErrorsSourceInTheSettings()
        //    {
        //        var registration = registrations.First(tr => tr.ServiceType == typeof(LogSource) && tr.Name == "___ERRORS");
        //        registration
        //            .AssertConstructor()
        //            .WithValueConstructorParameter("Name")
        //            .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
        //            .WithValueConstructorParameter(SourceLevels.Verbose)
        //            .WithValueConstructorParameter(false)
        //            .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
        //            .VerifyConstructorParameters();
        //    }

        //    [TestMethod]
        //    public void ThenHasNoRegistrationsForOtherTraceListeners()
        //    {
        //        Assert.AreEqual(3, registrations.Where(tr => tr.ServiceType == typeof(LogSource)).Count());
        //    }
    }
}
