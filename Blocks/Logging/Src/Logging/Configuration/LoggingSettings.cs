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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration settings for client-side logging applications.
    /// </summary>
    [ViewModel(LoggingDesignTime.ViewModelTypeNames.LogggingSectionViewModel)]
    [ResourceDescription(typeof(DesignResources), "LoggingSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "LoggingSettingsDisplayName")]
    public class LoggingSettings : SerializableConfigurationSection
    {
        private const string ErrorsTraceSourceKey = "___ERRORS";
        private const string AllTraceSourceKey = "___ALL";
        private const string NoMatchesTraceSourceKey = "___NO_MATCHES";

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
        /// Configuration section name for logging client settings.
        /// </summary>
        public const string SectionName = BlockSectionNames.Logging;

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> with default values.
        /// </summary>
        public LoggingSettings()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> using the given name.
        /// </summary>
        /// <param name="name">The name to use for this instance</param>
        public LoggingSettings(string name)
            : this(name, true, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> using the given values.
        /// </summary>
        /// <param name="name">The name to use for this instance</param>
        /// <param name="tracingEnabled">Should tracing be enabled?</param>
        /// <param name="defaultCategory">The default category to use.</param>
        public LoggingSettings(string name, bool tracingEnabled, string defaultCategory)
        {
            Name = name;
            TracingEnabled = tracingEnabled;
            DefaultCategory = defaultCategory;
        }

        /// <summary>
        /// Retrieves the <see cref="LoggingSettings"/> section from the configuration source.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
        /// <returns>The logging section.</returns>
        public static LoggingSettings GetLoggingSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            return (LoggingSettings)configurationSource.GetSection(SectionName);
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

        /// <summary>
        /// Builds the log writer represented by the section.
        /// </summary>
        /// <returns>A <see cref="LogWriter"/>.</returns>
        public LogWriter BuildLogWriter()
        {
            var listeners = new Dictionary<string, TraceListener>();

            var sources = this.TraceSources.ToDictionary(tsd => tsd.Name, tsd => this.BuildTraceSource(tsd, listeners));
            var all = this.BuildTraceSource(this.SpecialTraceSources.AllEventsTraceSource, listeners);
            var notProcessed = this.BuildTraceSource(this.SpecialTraceSources.NotProcessedTraceSource, listeners);
            var errors = this.BuildTraceSource(this.SpecialTraceSources.ErrorsTraceSource, listeners);

            var filters = this.LogFilters.Select(tfd => tfd.BuildFilter());

            var logWriter =
                new LogWriter(
                    filters,
                    sources,
                    all,
                    notProcessed,
                    errors,
                    this.DefaultCategory,
                    this.TracingEnabled,
                    this.LogWarningWhenNoCategoriesMatch,
                    this.RevertImpersonation);

            return logWriter;
        }

        private LogSource BuildTraceSource(TraceSourceData tsd, Dictionary<string, TraceListener> listeners)
        {
            var sourceListeners =
                tsd.TraceListeners.Select(
                    tln =>
                    {
                        TraceListener listener;
                        if (!listeners.TryGetValue(tln.Name, out listener))
                        {
                            listener = this.BuildTraceListener(tln.Name, tln);
                            listeners[tln.Name] = listener;
                        }

                        return listener;
                    });

            return new LogSource(tsd.Name, sourceListeners, tsd.DefaultLevel, tsd.AutoFlush);
        }

        private TraceListener BuildTraceListener(string name, ConfigurationElement requestor)
        {
            var listenerData = this.TraceListeners.Get(name);
            if (listenerData == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionTraceListenerNotDefined,
                        requestor.ElementInformation.Source,
                        requestor.ElementInformation.LineNumber,
                        name));
            }

            return listenerData.BuildTraceListener(this);
        }
    }
}
