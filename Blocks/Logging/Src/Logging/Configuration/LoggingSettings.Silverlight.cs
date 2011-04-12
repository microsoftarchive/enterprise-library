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
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration settings for client-side logging applications.
    /// </summary>
    public partial class LoggingSettings : ConfigurationSection, ITypeRegistrationsProvider
    {
        /// <summary>
        /// Enable or disable trace logging.
        /// </summary>
        public bool TracingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the configuration node.
        /// </summary>
        [Browsable(false)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default logging category.
        /// </summary>
        public string DefaultCategory
        {
            get;
            set;
        }


        private readonly NamedElementCollection<TraceListenerData> traceListeners = new NamedElementCollection<TraceListenerData>();

        /// <summary>
        /// Gets the collection of <see cref="TraceListenerData"/> configuration elements that define 
        /// the available <see cref="TraceListener"/>s.
        /// </summary>
        public NamedElementCollection<TraceListenerData> TraceListeners
        {
            get { return this.traceListeners; }
        }

        private readonly NamedElementCollection<FormatterData> formatters = new NamedElementCollection<FormatterData>();

        /// <summary>
        /// Gets the collection of <see cref="FormatterData"/> configuration elements that define 
        /// the available <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.ILogFormatter"/>s.
        /// </summary>
        public NamedElementCollection<FormatterData> Formatters
        {
            get { return this.formatters; }
        }

        private readonly NamedElementCollection<LogFilterData> logFilters = new NamedElementCollection<LogFilterData>();

        /// <summary>
        /// Gets the collection of <see cref="LogFilterData"/> configuration elements that define 
        /// the available <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Filters.ILogFilter"/>s.
        /// </summary>
        public NamedElementCollection<LogFilterData> LogFilters
        {
            get { return this.logFilters; }
        }

        private readonly NamedElementCollection<TraceSourceData> traceSources = new NamedElementCollection<TraceSourceData>();

        /// <summary>
        /// Gets the collection of <see cref="TraceSourceData"/> configuration elements that define 
        /// the available <see cref="LogSource"/>s.
        /// </summary>
        public NamedElementCollection<TraceSourceData> TraceSources
        {
            get { return this.traceSources; }
        }

        private SpecialTraceSourcesData specialTraceSources = new SpecialTraceSourcesData();

        /// <summary>
        /// Gets or sets the configuration elements that define the distinguished <see cref="LogSource"/>s: 
        /// for all events. for missing categories, and for errors and warnings.
        /// </summary>
        public SpecialTraceSourcesData SpecialTraceSources
        {
            get { return this.specialTraceSources; }
            set { this.specialTraceSources = value; }
        }

        /// <summary>
        /// Gets or sets the indication that a warning should be logged when a category is not found while 
        /// dispatching a log entry.
        /// </summary>
        public bool LogWarningWhenNoCategoriesMatch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the indication that impersonation should be reverted temporarily while logging, if enabled.
        /// </summary>
        public bool RevertImpersonation
        {
            get;
            set;
        }

        private TypeRegistration CreateLogWriterRegistration()
        {
            return
                new TypeRegistration<LogWriter>(() =>
                   new LogWriterImpl(
                       Container.Resolved<LogWriterStructureHolder>(),
                       Container.Resolved<ILoggingInstrumentationProvider>(),
                       Container.Resolved<IAsyncTracingErrorReporter>()))
                {
                    Lifetime = TypeRegistrationLifetime.Singleton,
                    IsDefault = true,
                    IsPublicName = true
                };
        }

        private TypeRegistration CreateLoggingUpdateCoordinatorRegistration()
        {
            return null;
        }

        private static TypeRegistration CreateLoggingInstrumentationProviderRegistration(IConfigurationSource configurationSource)
        {
            return
                new TypeRegistration<ILoggingInstrumentationProvider>(() => new NullLoggingInstrumentationProvider())
                {
                    Lifetime = TypeRegistrationLifetime.Singleton,
                    IsDefault = true
                };
        }

        private static TypeRegistration CreateDefaultNotificationTraceRegistration()
        {
            return new TypeRegistration<ITraceDispatcher>(() =>
                    new DefaultTraceDispatcher())
                {
                    Lifetime = TypeRegistrationLifetime.Singleton,
                    IsDefault = true,
                    IsPublicName = true
                };
        }

        private static TypeRegistration CreateAsyncTracingErrorReporterRegistration()
        {
            return new TypeRegistration<IAsyncTracingErrorReporter>(() =>
                    new AsyncTracingErrorReporter())
            {
                Lifetime = TypeRegistrationLifetime.Singleton,
                IsDefault = true,
                IsPublicName = true
            };
        }

        private IEnumerable<TypeRegistration> GetRegistrationsCore(IConfigurationSource configurationSource)
        {
            var registrations = new List<TypeRegistration>();

            registrations.Add(CreateLoggingInstrumentationProviderRegistration(configurationSource));
            registrations.Add(CreateLogWriterRegistration());
            registrations.AddRange(TraceListeners.SelectMany(tld => tld.GetRegistrations()));
            registrations.AddRange(LogFilters.SelectMany(lfd => lfd.GetRegistrations()));
            registrations.AddRange(Formatters.SelectMany(fd => fd.GetRegistrations()));
            registrations.AddRange(TraceSources.Select(tsd => tsd.GetRegistrations()));
            registrations.Add(
                CreateLogSourceRegistration(SpecialTraceSources.AllEventsTraceSource, AllTraceSourceKey));
            registrations.Add(
                CreateLogSourceRegistration(SpecialTraceSources.NotProcessedTraceSource, NoMatchesTraceSourceKey));
            registrations.Add(
                CreateLogSourceRegistration(SpecialTraceSources.ErrorsTraceSource, ErrorsTraceSourceKey));
            registrations.Add(CreateLogWriterStructureHolderRegistration());

            registrations.Add(CreateDefaultNotificationTraceRegistration());
            registrations.Add(CreateAsyncTracingErrorReporterRegistration());

            return registrations.Where(r => r != null);
        }
    }
}
