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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Instance based class to write log messages based on a given configuration.
    /// Messages are routed based on category.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To write log messages to the default configuration, use the <see cref="Logger"/> facade.  
    /// </para>
    /// <para>
    /// The <see cref="LogWriter"/> works as an entry point to the <see cref="System.Diagnostics"/> trace listeners. 
    /// It will trace the <see cref="LogEntry"/> through the <see cref="TraceListeners"/>s associated with the <see cref="LogSource"/>s 
    /// for all the matching categories in the elements of the <see cref="LogEntry.Categories"/> property of the log entry. 
    /// If the "all events" special log source is configured, the log entry will be traced through the log source regardles of other categories 
    /// that might have matched.
    /// If the "all events" special log source is not configured and the "unprocessed categories" special log source is configured,
    /// and the category specified in the logEntry being logged is not defined, then the logEntry will be logged to the "unprocessed categories"
    /// special log source.
    /// If both the "all events" and "unprocessed categories" special log sources are not configured and the property LogWarningsWhenNoCategoriesMatch
    /// is set to true, then the logEntry is logged to the "logging errors and warnings" special log source.
    /// </para>
    /// </remarks>
    public class LogWriter : ILogFilterErrorHandler, IDisposable
    {
        private LogWriterStructureHolder structureHolder;
        private LogFilterHelper filter;

        private ReaderWriterLockSlim rwSyncLock = new ReaderWriterLockSlim();

        /// <summary>
        /// EventID used on LogEntries that occur when internal LogWriter mechanisms fail.
        /// </summary>
        public const int LogWriterFailureEventID = 6352;

        private const int DefaultPriority = -1;
        private const TraceEventType DefaultSeverity = TraceEventType.Information;
        private const int DefaultEventId = 1;
        private const string DefaultTitle = "";
        private static readonly ICollection<string> emptyCategoriesList = new string[0];

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified configuration.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        public LogWriter(LoggingConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            var structureHolder = CreateStructureHolder(config.Filters,
                                                        config.LogSources.ToDictionary(c => c.Name, c => c.ToLogSource()),
                                                        config.SpecialSources.AllEvents.ToLogSource(),
                                                        config.SpecialSources.Unprocessed.ToLogSource(),
                                                        config.SpecialSources.LoggingErrorsAndWarnings.ToLogSource(),
                                                        config.DefaultSource,
                                                        config.IsTracingEnabled,
                                                        config.LogWarningsWhenNoCategoriesMatch,
                                                        !config.UseImpersonation);

            this.ReplaceStructureHolder(structureHolder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified filters, trace sources, and default category.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when the entry categories list is empty.</param>
        public LogWriter(IEnumerable<ILogFilter> filters,
                         IDictionary<string, LogSource> traceSources,
                         LogSource errorsTraceSource,
                         string defaultCategory)
            : this(filters, traceSources, null, null, errorsTraceSource, defaultCategory, false, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified filters, trace sources, default category, and tracing options.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when the entry categories list of a log entry is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch"><see langword="true"/> to log warnings when a non-matching category is found; otherwise, <see langword="false"/>.</param>
        public LogWriter(
            IEnumerable<ILogFilter> filters,
            IDictionary<string, LogSource> traceSources,
            LogSource allEventsTraceSource,
            LogSource notProcessedTraceSource,
            LogSource errorsTraceSource,
            string defaultCategory,
            bool tracingEnabled,
            bool logWarningsWhenNoCategoriesMatch)
            : this(
                filters,
                traceSources,
                allEventsTraceSource,
                notProcessedTraceSource,
                errorsTraceSource,
                defaultCategory,
                tracingEnabled,
                logWarningsWhenNoCategoriesMatch,
                true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified filters, trace sources, default category, tracing options, and impersonation option.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when the entry categories list of a log entry is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch"><see langword="true"/> to log warnings when a non-matching category is found; otherwise, <see langword="false"/>.</param>
        /// <param name="revertImpersonation"><see langword="true"/> to revert impersonation while logging; otherwise, <see langword="false"/>.</param>
        public LogWriter(
            IEnumerable<ILogFilter> filters,
            IDictionary<string, LogSource> traceSources,
            LogSource allEventsTraceSource,
            LogSource notProcessedTraceSource,
            LogSource errorsTraceSource,
            string defaultCategory,
            bool tracingEnabled,
            bool logWarningsWhenNoCategoriesMatch,
            bool revertImpersonation)
            : this(
                CreateStructureHolder(
                    filters,
                    traceSources,
                    allEventsTraceSource,
                    notProcessedTraceSource,
                    errorsTraceSource,
                    defaultCategory,
                    tracingEnabled,
                    logWarningsWhenNoCategoriesMatch,
                    revertImpersonation))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified filters, trace sources, and default category.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when the entry categories list is empty.</param>
        public LogWriter(IEnumerable<ILogFilter> filters,
                         IEnumerable<LogSource> traceSources,
                         LogSource errorsTraceSource,
                         string defaultCategory)
            : this(filters, CreateTraceSourcesDictionary(traceSources), errorsTraceSource, defaultCategory)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified filters, trace sources, default category, tracing options, and warning options.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when the entry categories list is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch"><see langword="true"/> to log warnings when a non-matching category is found; otherwise, <see langword="false"/>.</param>
        public LogWriter(IEnumerable<ILogFilter> filters,
                         IEnumerable<LogSource> traceSources,
                         LogSource allEventsTraceSource,
                         LogSource notProcessedTraceSource,
                         LogSource errorsTraceSource,
                         string defaultCategory,
                         bool tracingEnabled,
                         bool logWarningsWhenNoCategoriesMatch)
            : this(filters,
                   CreateTraceSourcesDictionary(traceSources),
                   allEventsTraceSource,
                   notProcessedTraceSource,
                   errorsTraceSource,
                   defaultCategory,
                   tracingEnabled,
                   logWarningsWhenNoCategoriesMatch)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the specified logging stack.
        /// </summary>
        /// <param name="structureHolder">The initial implementation of the logging stack</param>
        public LogWriter(LogWriterStructureHolder structureHolder)
        {
            Guard.ArgumentNotNull(structureHolder, "structureHolder");

            this.ReplaceStructureHolder(structureHolder);
        }

        /// <summary>
        /// Gets the <see cref="LogSource"/> mappings available for the <see cref="LogWriter"/>.
        /// </summary>
        public IDictionary<string, LogSource> TraceSources
        {
            get { return structureHolder.TraceSources; }
        }

        private ReaderWriterLockSlim SyncLock
        {
            get
            {
                var currentLock = this.rwSyncLock;
                if (currentLock == null)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return currentLock;
            }
        }

        /// <summary>
        /// Configures the <see cref="LogWriter"/> object.
        /// </summary>
        /// <remarks>
        /// Logging is delayed until the configuration changes are applied.
        /// </remarks>
        /// <param name="configurationScript">An action that configures the log writer.</param>
        public void Configure(Action<LoggingConfiguration> configurationScript)
        {
            this.SyncLock.EnterWriteLock();

            try
            {
                var current = this.structureHolder;

                var config = new LoggingConfiguration(
                                    current.Filters,
                                    current.TraceSources.Select(c => new LogSourceData(c.Value.Name, c.Value.Level, c.Value.AutoFlush, c.Value.Listeners.ToArray())),
                                    SpecialLogSourceData.FromLogSource(current.AllEventsTraceSource),
                                    SpecialLogSourceData.FromLogSource(current.NotProcessedTraceSource),
                                    SpecialLogSourceData.FromLogSource(current.ErrorsTraceSource),
                                    current.DefaultCategory,
                                    current.TracingEnabled,
                                    current.LogWarningsWhenNoCategoriesMatch,
                                    current.RevertImpersonation);

                configurationScript(config);

                var newStructureHolder = CreateStructureHolder(
                                            config.Filters,
                                            config.LogSources.ToDictionary(c => c.Name, c => c.ToLogSource()),
                                            config.SpecialSources.AllEvents.ToLogSource(),
                                            config.SpecialSources.Unprocessed.ToLogSource(),
                                            config.SpecialSources.LoggingErrorsAndWarnings.ToLogSource(),
                                            config.DefaultSource,
                                            config.IsTracingEnabled,
                                            config.LogWarningsWhenNoCategoriesMatch,
                                            !config.UseImpersonation);

                var newList = GetAllListeners(newStructureHolder).ToList();

                foreach (var listener in GetAllListeners(current))
                {
                    // Listeners removed from new collection
                    if (!newList.Contains(listener))
                    {
                        ((IDisposable)listener).Dispose();
                    }
                }

                this.ReplaceStructureHolder(newStructureHolder);
            }
            finally
            {
                this.SyncLock.ExitWriteLock();
            }
        }

        private static void AddTracingCategories(LogEntry log, Stack logicalOperationStack, bool replacementDone)
        {
            if (logicalOperationStack == null)
            {
                return;
            }

            // add tracing categories
            foreach (object logicalOperation in logicalOperationStack)
            {
                // ignore non string objects in the stack
                string category = logicalOperation as string;
                if (category != null)
                {
                    // must take care of logging categories..
                    if (!log.Categories.Contains(category))
                    {
                        if (!replacementDone)
                        {
                            log.Categories = new List<string>(log.Categories);
                            replacementDone = true;
                        }
                        log.Categories.Add(category);
                    }
                }
            }
        }

        private static LogWriterStructureHolder CreateStructureHolder(
            IEnumerable<ILogFilter> filters,
            IDictionary<string, LogSource> traceSources,
            LogSource allEventsTraceSource,
            LogSource notProcessedTraceSource,
            LogSource errorsTraceSource,
            string defaultCategory,
            bool tracingEnabled,
            bool logWarningsWhenNoCategoriesMatch,
            bool revertImpersonation)
        {
            return new LogWriterStructureHolder(
                filters,
                traceSources,
                allEventsTraceSource,
                notProcessedTraceSource,
                errorsTraceSource,
                defaultCategory,
                tracingEnabled,
                logWarningsWhenNoCategoriesMatch,
                revertImpersonation);
        }

        private static IDictionary<string, LogSource> CreateTraceSourcesDictionary(IEnumerable<LogSource> traceSources)
        {
            IDictionary<string, LogSource> result = new Dictionary<string, LogSource>();

            foreach (LogSource source in traceSources)
            {
                result.Add(source.Name, source);
            }

            return result;
        }

        private static IEnumerable<TraceListener> GetAllListeners(LogWriterStructureHolder holder)
        {
            var allSources = holder.TraceSources.Select(s => s.Value).Union(new[] { holder.AllEventsTraceSource, holder.ErrorsTraceSource, holder.NotProcessedTraceSource }).Where(s => s != null);
            var listeners = allSources.SelectMany(c => c.Listeners);
            return listeners.Distinct();
        }

        /// <summary>
        /// Releases the resources used by the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="disposing">"><see langword="true"/> to release managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var currentLock = this.rwSyncLock;
                if (currentLock == null)
                {
                    return;
                }

                try
                {
                    this.rwSyncLock = null;
                    currentLock.EnterWriteLock();
                }
                catch (ObjectDisposedException)
                {
                    // already disposed.
                    return;
                }

                try
                {
                    var listeners = GetAllListeners(this.structureHolder).OfType<IDisposable>().ToList();
                    this.structureHolder = null;
                    listeners.ForEach(l => l.Dispose());
                }
                finally
                {
                    currentLock.ExitWriteLock();

                    try
                    {
                        currentLock.Dispose();
                    }
                    catch (SynchronizationLockException)
                    {
                        // there are consumers still using the LogWriter while it's being disposed.
                        // Ignore. It will eventually be garbage collected.
                    }
                }
            }
        }

        /// <summary>
        /// Releases the resources used by the <see cref="LogWriter"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LogWriter"/> class.
        /// </summary>
        ~LogWriter()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Returns the collection of <see cref="LogSource"/>s that matches the collection of categories provided.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns>The matching <see cref="LogSource"/> objects.</returns>
        IEnumerable<LogSource> DoGetMatchingTraceSources(LogEntry logEntry)
        {
            List<LogSource> matchingTraceSources = new List<LogSource>(logEntry.Categories.Count);
            List<string> missingCategories = new List<string>();

            // match the categories to the receive's trace sources
            foreach (string category in logEntry.Categories)
            {
                LogSource traceSource;
                structureHolder.TraceSources.TryGetValue(category, out traceSource);
                if (traceSource != null)
                {
                    matchingTraceSources.Add(traceSource);
                }
                else
                {
                    missingCategories.Add(category);
                }
            }

            // add the mandatory trace source, if defined
            // otherwise, add the not processed trace source if missing categories were detected
            if (IsValidTraceSource(structureHolder.AllEventsTraceSource))
            {
                matchingTraceSources.Add(structureHolder.AllEventsTraceSource);
            }
            else if (missingCategories.Count > 0)
            {
                if (IsValidTraceSource(structureHolder.NotProcessedTraceSource))
                {
                    matchingTraceSources.Add(structureHolder.NotProcessedTraceSource);
                }
                else if (structureHolder.LogWarningsWhenNoCategoriesMatch)
                {
                    ReportMissingCategories(missingCategories, logEntry);
                }
            }

            return matchingTraceSources;
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Performs any action to handle an error during checking.
        /// </summary>
        /// <param name="ex">The exception that was raised during filter evaluation.</param>
        /// <param name="logEntry">The log entry that was evaluated.</param>
        /// <param name="filter">The filter that raised the exception.</param>
        /// <returns><see langword="true"/>, to indicate that processing should continue.</returns>
        public bool FilterCheckingFailed(Exception ex,
                                         LogEntry logEntry,
                                         ILogFilter filter)
        {
            ReportExceptionCheckingFilters(ex, logEntry, filter);
            return true;
        }

        /// <summary>
        /// Returns the filter of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of filter requiered.</typeparam>
        /// <returns>The instance of <typeparamref name="T"/> in the filters collection, or <see langword="null"/> 
        /// if there is no such instance.</returns>
        public T GetFilter<T>() where T : class, ILogFilter
        {
            return filter.GetFilter<T>();
        }

        /// <summary>
        /// Returns the filter of type <typeparamref name="T"/> named <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of filter required.</typeparam>
        /// <param name="name">The name of the filter required.</param>
        /// <returns>The instance of <typeparamref name="T"/> named <paramref name="name"/> in 
        /// the filters collection, or <see langword="null"/> if there is no such instance.</returns>
        public T GetFilter<T>(string name) where T : class, ILogFilter
        {
            return filter.GetFilter<T>(name);
        }

        /// <summary>
        /// Returns the filter named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the filter required.</param>
        /// <returns>The filter named <paramref name="name"/> in 
        /// the filters collection, or <see langword="null"/> if there is no such filter.</returns>
        public ILogFilter GetFilter(string name)
        {
            return filter.GetFilter(name);
        }

        private static Stack GetLogicalOperationStack()
        {
            try
            {
                return DoGetLogicalOperationStack();
            }
            catch (SecurityException)
            {
                return null;
            }
        }

        // Security review: SecuritySafeCritical because it accesses Trace.CorrelationManager
        // Returns a copy of the correlation manager's logical operation stack.
        [SecuritySafeCritical]
        private static Stack DoGetLogicalOperationStack()
        {
            return (Stack)Trace.CorrelationManager.LogicalOperationStack.Clone();
        }

        /// <summary>
        /// Gets a list of <see cref="LogSource"/> objects for the log entry.
        /// </summary>
        /// <param name="logEntry">The <see cref="LogEntry"/> to get the matching trace sources.</param>
        /// <returns>A collection of <see cref="LogSource"/> objects.</returns>
        public IEnumerable<LogSource> GetMatchingTraceSources(LogEntry logEntry)
        {
            return DoGetMatchingTraceSources(logEntry);
        }

        /// <summary>
        /// Queries whether logging is enabled.
        /// </summary>
        /// <returns><b>true</b> if logging is enabled.</returns>
        public bool IsLoggingEnabled()
        {
            LogEnabledFilter enabledFilter = filter.GetFilter<LogEnabledFilter>();
            return enabledFilter == null || enabledFilter.Enabled;
        }

        /// <summary>
        /// Queries whether tracing is enabled.
        /// </summary>
        /// <returns><b>true</b> if tracing is enabled.</returns>
        public bool IsTracingEnabled()
        {
            return structureHolder.TracingEnabled;
        }

        private static bool IsValidTraceSource(LogSource traceSource)
        {
            return traceSource != null && traceSource.Listeners.Any();
        }

        private void ProcessLog(LogEntry log, TraceEventCache traceEventCache)
        {
            // revert any outstanding impersonation
            using (RevertExistingImpersonation())
            {
                var items = new ContextItems();
                items.ProcessContextItems(log);

                var matchingTraceSources = GetMatchingTraceSources(log);
                var traceListenerFilter = new TraceListenerFilter();

                foreach (LogSource traceSource in matchingTraceSources)
                {
                    traceSource.TraceData(log.Severity, log.EventId, log, traceListenerFilter, traceEventCache, ReportExceptionDuringTracing);
                }
            }
        }

        /// <devdoc>
        /// Checks to determine whether impersonation is in place, and if so, reverts it and returns
        /// the impersonation context that must be used to undo the revert.
        /// </devdoc>
        private WindowsImpersonationContext RevertExistingImpersonation()
        {
            // noop if reverting impersonation is disabled
            if (!structureHolder.RevertImpersonation)
            {
                return null;
            }

            try
            {
                using (WindowsIdentity impersonatedIdentity = WindowsIdentity.GetCurrent(true))
                {
                    if (impersonatedIdentity == null)
                    {
                        return null;
                    }
                }
            }
            catch (SecurityException)
            {
                return null;
            }

            try
            {
                return WindowsIdentity.Impersonate(IntPtr.Zero);    // to be undone by caller
            }
            catch (SecurityException)
            {
                // this shouldn't happen, as GetCurrent() and Impersonate() demand the same CAS permissions.
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        internal void ReplaceStructureHolder(LogWriterStructureHolder newStructureHolder)
        {
            this.structureHolder = newStructureHolder;
            this.filter = new LogFilterHelper(this.structureHolder.Filters, this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Best effort to log exceptions")]
        void ReportExceptionCheckingFilters(Exception exception, object log, ILogFilter logFilter)
        {
            try
            {
                NameValueCollection additionalInfo = new NameValueCollection();
                additionalInfo.Add(ExceptionFormatter.Header,
                                   string.Format(CultureInfo.CurrentCulture, Resources.FilterEvaluationFailed, logFilter.Name));
                additionalInfo.Add(Resources.FilterEvaluationFailed2,
                                   string.Format(CultureInfo.CurrentCulture, Resources.FilterEvaluationFailed3, log));
                ExceptionFormatter formatter =
                    new ExceptionFormatter(additionalInfo, Resources.DistributorEventLoggerDefaultApplicationName);

                LogEntry reportingLogEntry = new LogEntry();
                reportingLogEntry.Severity = TraceEventType.Error;
                reportingLogEntry.Message = formatter.GetMessage(exception);
                reportingLogEntry.EventId = LogWriterFailureEventID;

                structureHolder.ErrorsTraceSource.TraceData(reportingLogEntry.Severity, reportingLogEntry.EventId, reportingLogEntry);
            }
            catch (Exception)
            {
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Best effort to log exceptions")]
        void ReportExceptionDuringTracing(Exception exception, object log, string traceSourceName)
        {
            try
            {
                NameValueCollection additionalInfo = new NameValueCollection();
                additionalInfo.Add(ExceptionFormatter.Header,
                                   string.Format(CultureInfo.CurrentCulture, Resources.TraceSourceFailed, traceSourceName));
                additionalInfo.Add(Resources.TraceSourceFailed2,
                                   string.Format(CultureInfo.CurrentCulture, Resources.TraceSourceFailed3, log));
                ExceptionFormatter formatter =
                    new ExceptionFormatter(additionalInfo, Resources.DistributorEventLoggerDefaultApplicationName);

                LogEntry reportingLogEntry = new LogEntry();
                reportingLogEntry.Severity = TraceEventType.Error;
                reportingLogEntry.Message = formatter.GetMessage(exception);
                reportingLogEntry.EventId = LogWriterFailureEventID;

                structureHolder.ErrorsTraceSource.TraceData(reportingLogEntry.Severity, reportingLogEntry.EventId, reportingLogEntry);
            }
            catch (Exception)
            {
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Best effort to log exceptions")]
        void ReportMissingCategories(ICollection<string> missingCategories,
                                     LogEntry logEntry)
        {
            try
            {
                LogEntry reportingLogEntry = new LogEntry();
                reportingLogEntry.Severity = TraceEventType.Error;
                reportingLogEntry.Message = string.Format(CultureInfo.CurrentCulture, Resources.MissingCategories, TextFormatter.FormatCategoriesCollection(missingCategories), logEntry);
                reportingLogEntry.EventId = LogWriterFailureEventID;

                structureHolder.ErrorsTraceSource.TraceData(reportingLogEntry.Severity, reportingLogEntry.EventId, reportingLogEntry);
            }
            catch (Exception)
            {
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Best effort to log exceptions")]
        void ReportUnknownException(Exception exception,
                                    LogEntry log)
        {
            try
            {
                NameValueCollection additionalInfo = new NameValueCollection();
                additionalInfo.Add(ExceptionFormatter.Header, Resources.ProcessMessageFailed);
                additionalInfo.Add(Resources.ProcessMessageFailed2,
                                   string.Format(CultureInfo.CurrentCulture, Resources.ProcessMessageFailed3, log));
                ExceptionFormatter formatter =
                    new ExceptionFormatter(additionalInfo, Resources.DistributorEventLoggerDefaultApplicationName);

                LogEntry reportingLogEntry = new LogEntry();
                reportingLogEntry.Severity = TraceEventType.Error;
                reportingLogEntry.Message = formatter.GetMessage(exception);
                reportingLogEntry.EventId = LogWriterFailureEventID;

                structureHolder.ErrorsTraceSource.TraceData(reportingLogEntry.Severity, reportingLogEntry.EventId, reportingLogEntry);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Queries whether the specified <see cref="LogEntry"/> should be logged.
        /// </summary>
        /// <param name="log">The log entry to check.</param>
        /// <returns><see langword="true"/> if the entry should be logged; otherwise, <see langword="false"/>.</returns>
        public bool ShouldLog(LogEntry log)
        {
            return filter.CheckFilters(log);
        }

        /// <summary>
        /// Writes the specified log entry.
        /// </summary>
        /// <param name="log">The log entry to write.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is logged")]
        public void Write(LogEntry log)
        {
            Guard.ArgumentNotNull(log, "log");
            var traceEventCache = new TraceEventCache();

            var ignoredActivityId = log.ActivityId;
            var ignoredManagedThreadName = log.ManagedThreadName;

            this.ExecuteReadOperation(() =>
            {
                try
                {
                    bool replacementDone = false;

                    // set default category if necessary
                    if (log.Categories.Count == 0)
                    {
                        log.Categories = new List<string>(1);
                        log.Categories.Add(structureHolder.DefaultCategory);
                        replacementDone = true;
                    }

                    if (structureHolder.TracingEnabled)
                    {
                        var logicalOperationStack = GetLogicalOperationStack();
                        AddTracingCategories(log, logicalOperationStack, replacementDone);
                    }

                    if (ShouldLog(log))
                    {
                        ProcessLog(log, traceEventCache);
                    }
                }
                catch (Exception ex)
                {
                    ReportUnknownException(ex, log);
                }
            });
        }

        private void ExecuteReadOperation(Action readOperation)
        {
            this.SyncLock.EnterReadLock();

            try
            {
                readOperation();
            }
            finally
            {
                this.SyncLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Empties the dictionary of context items.
        /// </summary>
        [SecurityCritical]
        public void FlushContextItems()
        {
            ContextItems items = new ContextItems();
            items.FlushContextItems();
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="System.Runtime.Remoting.Messaging.CallContext"/> dictionary.  
        /// Context items will be recorded with every log entry.
        /// </summary>
        /// <param name="key">Hashtable key</param>
        /// <param name="value">Value.  Objects will be serialized.</param>
        /// <example>The following example demonstrates use of the AddContextItem method.
        /// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
        [SecurityCritical]
        public void SetContextItem(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            ContextItems items = new ContextItems();
            items.SetContextItem(key, value);
        }

        /// <overloads>
        /// Write a new log entry to the default category.
        /// </overloads>
        /// <summary>
        /// Write a new log entry to the default category.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        public void Write(object message)
        {
            this.Write(
                message,
                emptyCategoriesList,
                DefaultPriority,
                DefaultEventId,
                DefaultSeverity,
                DefaultTitle,
                null);
        }

        /// <summary>
        /// Write a new log entry to a specific category.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        public void Write(object message, string category)
        {
            this.Write(message, category, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category and priority.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        public void Write(object message, string category, int priority)
        {
            this.Write(message, category, priority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority and event id.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        public void Write(object message, string category, int priority, int eventId)
        {
            this.Write(message, category, priority, eventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event id and severity.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        public void Write(object message, string category, int priority, int eventId, TraceEventType severity)
        {
            this.Write(message, category, priority, eventId, severity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event id, severity
        /// and title.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message</param>
        public void Write(
            object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            string title)
        {
            this.Write(message, category, priority, eventId, severity, title, null);
        }

        /// <summary>
        /// Write a new log entry and a dictionary of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(object message, IDictionary<string, object> properties)
        {
            this.Write(
                message,
                emptyCategoriesList,
                DefaultPriority,
                DefaultEventId,
                DefaultSeverity,
                DefaultTitle,
                properties);
        }

        /// <summary>
        /// Write a new log entry to a specific category with a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(object message, string category, IDictionary<string, object> properties)
        {
            this.Write(
                message,
                category,
                DefaultPriority,
                DefaultEventId,
                DefaultSeverity,
                DefaultTitle,
                properties);
        }

        /// <summary>
        /// Write a new log entry to with a specific category, priority and a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(object message, string category, int priority, IDictionary<string, object> properties)
        {
            this.Write(message, category, priority, DefaultEventId, DefaultSeverity, DefaultTitle, properties);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event Id, severity
        /// title and dictionary of extended properties.
        /// </summary>
        /// <example>The following example demonstrates use of the Write method with
        /// a full set of parameters.
        /// <code></code></example>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(
            object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            string title,
            IDictionary<string, object> properties)
        {
            this.Write(message, new string[] { category }, priority, eventId, severity, title, properties);
        }

        /// <summary>
        /// Write a new log entry to a specific collection of categories.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        public void Write(object message, IEnumerable<string> categories)
        {
            this.Write(message, categories, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories and priority.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        public void Write(object message, IEnumerable<string> categories, int priority)
        {
            this.Write(message, categories, priority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories, priority and event id.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        public void Write(object message, IEnumerable<string> categories, int priority, int eventId)
        {
            this.Write(message, categories, priority, eventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories, priority, event id and severity.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration. 
        /// (Unspecified, Information, Warning or Error).</param>
        public void Write(
            object message,
            IEnumerable<string> categories,
            int priority,
            int eventId,
            TraceEventType severity)
        {
            this.Write(message, categories, priority, eventId, severity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories, priority, event id, severity
        /// and title.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message</param>
        public void Write(
            object message,
            IEnumerable<string> categories,
            int priority,
            int eventId,
            TraceEventType severity,
            string title)
        {
            this.Write(message, categories, priority, eventId, severity, title, null);
        }

        /// <summary>
        /// Write a new log entry to a specific collection of categories with a dictionary of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(object message, IEnumerable<string> categories, IDictionary<string, object> properties)
        {
            this.Write(message, categories, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle, properties);
        }

        /// <summary>
        /// Write a new log entry to with a specific collection of categories, priority and a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(
            object message,
            IEnumerable<string> categories,
            int priority,
            IDictionary<string,
            object> properties)
        {
            this.Write(message, categories, priority, DefaultEventId, DefaultSeverity, DefaultTitle, properties);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event Id, severity
        /// title and dictionary of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public void Write(
            object message,
            IEnumerable<string> categories,
            int priority,
            int eventId,
            TraceEventType severity,
            string title,
            IDictionary<string, object> properties)
        {
            LogEntry log = new LogEntry();
            log.Message = message.ToString();
            log.Categories = categories.ToArray();
            log.Priority = priority;
            log.EventId = eventId;
            log.Severity = severity;
            log.Title = title;
            log.ExtendedProperties = properties;

            this.Write(log);
        }
    }
}
