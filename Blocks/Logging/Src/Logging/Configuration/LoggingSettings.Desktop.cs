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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Container = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Container;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration settings for client-side logging applications.
    /// </summary>
    [ViewModel(LoggingDesignTime.ViewModelTypeNames.LogggingSectionViewModel)]
    [ResourceDescription(typeof(DesignResources), "LoggingSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsDisplayName")]
    public partial class LoggingSettings : SerializableConfigurationSection, ITypeRegistrationsProvider
    {
        private const string tracingEnabledProperty = "tracingEnabled";
        private const string nameProperty = "name";
        private const string traceListenerDataCollectionProperty = "listeners";
        private const string formatterDataCollectionProperty = "formatters";
        private const string logFiltersProperty = "logFilters";
        private const string traceSourcesProrperty = "categorySources";
        private const string defaultCategoryProperty = "defaultCategory";
        private const string logWarningsWhenNoCategoriesMatchProperty = "logWarningsWhenNoCategoriesMatch";
        private const string specialTraceSourcesProperty = "specialSources";
        private const string revertImpersonationProperty = "revertImpersonation";

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> with default values.
        /// </summary>
        public LoggingSettings()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Enable or disable trace logging.
        /// </summary>
        [ConfigurationProperty(tracingEnabledProperty, DefaultValue = true)]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsTracingEnabledDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsTracingEnabledDisplayName")]
        public bool TracingEnabled
        {
            get
            {
                return (bool)this[tracingEnabledProperty];
            }
            set
            {
                this[tracingEnabledProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the configuration node.
        /// </summary>
        [ConfigurationProperty(nameProperty)]
        [Browsable(false)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string Name
        {
            get
            {
                return (string)this[nameProperty];
            }
            set
            {
                this[nameProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the default logging category.
        /// </summary>
        [ConfigurationProperty(defaultCategoryProperty, IsRequired = true)]
        [Reference(typeof(NamedElementCollection<TraceSourceData>), typeof(TraceSourceData))]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsDefaultCategoryDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsDefaultCategoryDisplayName")]
        public string DefaultCategory
        {
            get
            {
                return (string)this[defaultCategoryProperty];
            }
            set
            {
                this[defaultCategoryProperty] = value;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="TraceListenerData"/> configuration elements that define 
        /// the available <see cref="System.Diagnostics.TraceListener"/>s.
        /// </summary>
        [ConfigurationProperty(traceListenerDataCollectionProperty)]
        [ViewModel(LoggingDesignTime.ViewModelTypeNames.TraceListenerElementCollectionViewModel)]
        [ConfigurationCollection(typeof(TraceListenerData))]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsTraceListenersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsTraceListenersDisplayName")]
        public TraceListenerDataCollection TraceListeners
        {
            get
            {
                return (TraceListenerDataCollection)base[traceListenerDataCollectionProperty];
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="FormatterData"/> configuration elements that define 
        /// the available <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.ILogFormatter"/>s.
        /// </summary>
        [ConfigurationProperty(formatterDataCollectionProperty)]
        [ConfigurationCollection(typeof(FormatterData))]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsFormattersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsFormattersDisplayName")]
        public NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData> Formatters
        {
            get
            {
                return (NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>)base[formatterDataCollectionProperty];
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="LogFilterData"/> configuration elements that define 
        /// the available <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Filters.ILogFilter"/>s.
        /// </summary>
        [ConfigurationProperty(logFiltersProperty)]
        [ConfigurationCollection(typeof(LogFilterData))]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsLogFiltersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsLogFiltersDisplayName")]
        public NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData> LogFilters
        {
            get
            {
                return (NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData>)base[logFiltersProperty];
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="TraceSourceData"/> configuration elements that define 
        /// the available <see cref="LogSource"/>s.
        /// </summary>
        [ConfigurationProperty(traceSourcesProrperty)]
        [ConfigurationCollection(typeof(TraceSourceData))]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsTraceSourcesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsTraceSourcesDisplayName")]
        public NamedElementCollection<TraceSourceData> TraceSources
        {
            get
            {
                return (NamedElementCollection<TraceSourceData>)base[traceSourcesProrperty];
            }
        }

        /// <summary>
        /// Gets or sets the configuration elements that define the distinguished <see cref="LogSource"/>s: 
        /// for all events. for missing categories, and for errors and warnings.
        /// </summary>
        [ConfigurationProperty(specialTraceSourcesProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsSpecialTraceSourcesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsSpecialTraceSourcesDisplayName")]
        public SpecialTraceSourcesData SpecialTraceSources
        {
            get
            {
                return (SpecialTraceSourcesData)base[specialTraceSourcesProperty];
            }
            set
            {
                base[specialTraceSourcesProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the indication that a warning should be logged when a category is not found while 
        /// dispatching a log entry.
        /// </summary>
        [ConfigurationProperty(logWarningsWhenNoCategoriesMatchProperty, DefaultValue = true)]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsLogWarningWhenNoCategoriesMatchDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsLogWarningWhenNoCategoriesMatchDisplayName")]
        public bool LogWarningWhenNoCategoriesMatch
        {
            get
            {
                return (bool)this[logWarningsWhenNoCategoriesMatchProperty];
            }
            set
            {
                this[logWarningsWhenNoCategoriesMatchProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the indication that impersonation should be reverted temporarily while logging, if enabled.
        /// </summary>
        [ConfigurationProperty(revertImpersonationProperty, DefaultValue = true, IsRequired = false)]
        [ResourceDescription(typeof(DesignResources), "LoggingSettingsRevertImpersonationDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsRevertImpersonationDisplayName")]
        public bool RevertImpersonation
        {
            get
            {
                return (bool)this[revertImpersonationProperty];
            }
            set
            {
                this[revertImpersonationProperty] = value;
            }
        }
        
        private TypeRegistration CreateLogWriterRegistration()
        {
            return
                new TypeRegistration<LogWriter>(() =>
                   new LogWriterImpl(
                       Container.Resolved<LogWriterStructureHolder>(),
                       Container.Resolved<ILoggingInstrumentationProvider>(),
                       Container.Resolved<ILoggingUpdateCoordinator>()))
                {
                    Lifetime = TypeRegistrationLifetime.Singleton,
                    IsDefault = true,
                    IsPublicName = true
                };
        }

        private TypeRegistration CreateLoggingUpdateCoordinatorRegistration()
        {
            return
                new TypeRegistration<ILoggingUpdateCoordinator>(() =>
                   new LoggingUpdateCoordinator(
                       Container.Resolved<ConfigurationChangeEventSource>(),
                       Container.Resolved<ILoggingInstrumentationProvider>()))
               {
                   Lifetime = TypeRegistrationLifetime.Singleton,
                   IsDefault = true
               };
        }

        private static TypeRegistration CreateLoggingInstrumentationProviderRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return
                new TypeRegistration<ILoggingInstrumentationProvider>(() =>
                    new LoggingInstrumentationProvider(
                        instrumentationSection.PerformanceCountersEnabled,
                        instrumentationSection.EventLoggingEnabled,
                        instrumentationSection.ApplicationInstanceName))
                {
                    Lifetime = TypeRegistrationLifetime.Singleton,
                    IsDefault = true
                };
        }

        private static TypeRegistration CreateTraceManagerRegistration()
        {
            return
                new TypeRegistration<TraceManager>(() =>
                    new TraceManager(
                        Container.Resolved<LogWriter>(),
                        Container.Resolved<ITracerInstrumentationProvider>()))
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true
                };
        }

        private static TypeRegistration CreateTracerInstrumentationProviderRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return
                new TypeRegistration<ITracerInstrumentationProvider>(() =>
                    new TracerInstrumentationProvider(
                        instrumentationSection.PerformanceCountersEnabled,
                        instrumentationSection.EventLoggingEnabled,
                        instrumentationSection.ApplicationInstanceName))
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true
                };
        }

        private static TypeRegistration CreateDefaultLoggingEventLoggerRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSettings = InstrumentationConfigurationSection.GetSection(configurationSource);
            return
                new TypeRegistration<DefaultLoggingEventLogger>(() =>
                    new DefaultLoggingEventLogger(instrumentationSettings.EventLoggingEnabled))
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true
                };
        }

        private IEnumerable<TypeRegistration> GetRegistrationsCore(IConfigurationSource configurationSource)
        {
            var registrations = new List<TypeRegistration>();

            registrations.Add(CreateLoggingInstrumentationProviderRegistration(configurationSource));
            registrations.Add(CreateLoggingUpdateCoordinatorRegistration());
            registrations.Add(CreateLogWriterRegistration());
            registrations.Add(CreateDefaultLoggingEventLoggerRegistration(configurationSource));
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
            registrations.Add(CreateTraceManagerRegistration());
            registrations.Add(CreateTracerInstrumentationProviderRegistration(configurationSource));

            return registrations.Where(r => r != null);
        }
    }
}
