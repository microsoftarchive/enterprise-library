using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents the configuration options for the Logging Application Block.
    /// </summary>
    public sealed class LoggingConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfiguration"/> class.
        /// </summary>
        public LoggingConfiguration()
        {
            this.IsTracingEnabled = true;
            this.LogWarningsWhenNoCategoriesMatch = true;

            this.LogSources = new LogSourceDataCollection();
            this.SpecialSources = new SpecialSourcesConfiguration();
            this.Filters = new List<ILogFilter>();
        }

        internal LoggingConfiguration(IEnumerable<ILogFilter> filters, IEnumerable<LogSourceData> logSources, SpecialLogSourceData allEventsTraceSource, SpecialLogSourceData notProcessedTraceSource, SpecialLogSourceData errorsTraceSource, string defaultCategory, bool tracingEnabled, bool logWarningsWhenNoCategoriesMatch, bool revertImpersonation)
        {
            if (allEventsTraceSource == null)
            {
                throw new ArgumentNullException("allEventsTraceSource");
            }

            if (notProcessedTraceSource == null)
            {
                throw new ArgumentNullException("notProcessedTraceSource");
            }

            if (errorsTraceSource == null)
            {
                throw new ArgumentNullException("errorsTraceSource");
            }

            this.SpecialSources = new SpecialSourcesConfiguration();
            this.SpecialSources.AllEvents = allEventsTraceSource;
            this.SpecialSources.Unprocessed = notProcessedTraceSource;
            this.SpecialSources.LoggingErrorsAndWarnings = errorsTraceSource;

            this.Filters = new List<ILogFilter>(filters);

            this.LogSources = new LogSourceDataCollection(logSources);

            this.DefaultSource = defaultCategory;
            this.IsTracingEnabled = tracingEnabled;
            this.LogWarningsWhenNoCategoriesMatch = logWarningsWhenNoCategoriesMatch;
            this.UseImpersonation = !revertImpersonation;
        }

        /// <summary>
        /// Gets the collection of all available <see cref="LogSource"/> objects.
        /// </summary>
        public LogSourceDataCollection LogSources { get; private set; }

        /// <summary>
        /// Gets the list of all available <see cref="ILogFilter"/> objects.
        /// </summary>
        public IList<ILogFilter> Filters { get; private set; }

        /// <summary>
        /// Gets the configuration for special sources.
        /// </summary>
        public SpecialSourcesConfiguration SpecialSources { get; private set; }

        /// <summary>
        /// Gets or sets a value that indicates whether tracing is enabled.
        /// </summary>
        public bool IsTracingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether logging is enabled.
        /// </summary>
        public bool IsLoggingEnabled
        {
            get
            {
                var enabledFilter = this.Filters.OfType<LogEnabledFilter>().FirstOrDefault();

                return enabledFilter == null || enabledFilter.Enabled;
            }
            set
            {
                var enabledFilter = this.Filters.OfType<LogEnabledFilter>().FirstOrDefault();

                if (!value)
                {
                    if (enabledFilter == null)
                    {
                        this.Filters.Add(new LogEnabledFilter("enabled", false));
                    }
                    else
                    {
                        enabledFilter.Enabled = false;
                    }
                }
                else
                {
                    if (enabledFilter != null)
                    {
                        enabledFilter.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use impersonation.
        /// </summary>
        public bool UseImpersonation { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to log warnings when no matching categories are found.
        /// </summary>
        public bool LogWarningsWhenNoCategoriesMatch { get; set; }

        /// <summary>
        /// Gets or sets the default source name.
        /// </summary>
        public string DefaultSource { get; set; }

        /// <summary>
        /// Gets a collection of all the <see cref="TraceListener"/> objects used in this configuration.
        /// </summary>
        public IEnumerable<TraceListener> AllTraceListeners
        {
            get
            {
                return
                    this.LogSources.SelectMany(c => c.Listeners)
                        .Union(SpecialSources.AllEvents.Listeners)
                        .Union(SpecialSources.LoggingErrorsAndWarnings.Listeners)
                        .Union(SpecialSources.Unprocessed.Listeners)
                        .Distinct();
            }
        }

        /// <summary>
        /// Adds a new log source with the specified name.
        /// </summary>
        /// <param name="name">The name of the log source.</param>
        /// <returns>A new <see cref="LogSourceData"/> instance.</returns>
        public LogSourceData AddLogSource(string name)
        {
            return this.AddLogSource(name, SourceLevels.All, false, new TraceListener[] { });
        }

        /// <summary>
        /// Adds a new log source with the specified name and trace listeners.
        /// </summary>
        /// <param name="name">The name of the log source.</param>
        /// <param name="traceListeners">One or more <see cref="TraceListener"/> objects.</param>
        /// <returns>A new <see cref="LogSourceData"/> instance.</returns>
        public LogSourceData AddLogSource(string name, params TraceListener[] traceListeners)
        {
            return this.AddLogSource(name, SourceLevels.All, false, traceListeners);
        }

        /// <summary>
        /// Adds a new log source with the specified name and level.
        /// </summary>
        /// <param name="name">The name of the log source.</param>
        /// <param name="level">The filtering level of the log source.</param>
        /// <returns>A new <see cref="LogSourceData"/> instance.</returns>
        public LogSourceData AddLogSource(string name, SourceLevels level)
        {
            return this.AddLogSource(name, level, false, new TraceListener[] { });
        }

        /// <summary>
        /// Adds a new log source with the specified name and level, and optionally enables auto-flush.
        /// </summary>
        /// <param name="name">The name of the log source.</param>
        /// <param name="level">The filtering level of the log source.</param>
        /// <param name="autoFlush"><see langword="true"/> to enable auto-flush; otherwise, <see langword="false"/>.</param>
        /// <returns>A new <see cref="LogSourceData"/> instance.</returns>
        public LogSourceData AddLogSource(string name, SourceLevels level, bool autoFlush)
        {
            return this.AddLogSource(name, level, autoFlush, new TraceListener[] { });
        }

        /// <summary>
        /// Adds a new log source with the specified name, level, and trace listeners, and optionally enables auto-flush.
        /// </summary>
        /// <param name="name">The name of the log source.</param>
        /// <param name="level">The filtering level of the log source.</param>
        /// <param name="autoFlush"><see langword="true"/> to enable auto-flush; otherwise, <see langword="false"/>.</param>
        /// <param name="traceListeners">One or more <see cref="TraceListener"/> objects.</param>
        /// <returns>A new <see cref="LogSourceData"/> instance.</returns>
        public LogSourceData AddLogSource(string name, SourceLevels level, bool autoFlush, params TraceListener[] traceListeners)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (traceListeners == null)
            {
                throw new ArgumentNullException("traceListeners");
            }

            var sourceData = new LogSourceData()
            {
                AutoFlush = autoFlush,
                Name = name,
                Level = level
            };

            foreach (var traceListener in traceListeners)
            {
                if (traceListener == null)
                {
                    throw new ArgumentNullException("traceListeners");
                }

                sourceData.Listeners.Add(traceListener);
            }

            this.LogSources.Add(sourceData);

            // Make it default if necessary
            if (string.IsNullOrWhiteSpace(this.DefaultSource))
                this.DefaultSource = name;

            return sourceData;
        }
    }
}
