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
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.Unity.Utility;
using System.Globalization;
#if !SILVERLIGHT
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Stack = System.Collections.Generic.Stack<object>;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Instance based class to write log messages based on a given configuration.
    /// Messages are routed based on category.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The LogWriterImpl works as an entry point to the <see cref="System.Diagnostics"/> trace listeners. 
    /// It will trace the <see cref="LogEntry"/> through the <see cref="TraceListeners"/>s associated with the <see cref="LogSource"/>s 
    /// for all the matching categories in the elements of the <see cref="LogEntry.Categories"/> property of the log entry. 
    /// If the "all events" special log source is configured, the log entry will be traced through the log source regardless of other categories 
    /// that might have matched.
    /// If the "all events" special log source is not configured and the "unprocessed categories" special log source is configured,
    /// and the category specified in the logEntry being logged is not defined, then the logEntry will be logged to the "unprocessed categories"
    /// special log source.
    /// If both the "all events" and "unprocessed categories" special log sources are not configured and the property LogWarningsWhenNoCategoriesMatch
    /// is set to true, then the logEntry is logged to the "logging errors and warnings" special log source.
    /// </para>
    /// </remarks>
    partial class LogWriterImpl
    {
        private readonly ILoggingInstrumentationProvider instrumentationProvider;
        LogWriterStructureHolder structureHolder;
        LogFilterHelper filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="structureHolder">The initial implementation of the logging stack</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public LogWriterImpl(
            LogWriterStructureHolder structureHolder,
            ILoggingInstrumentationProvider instrumentationProvider)
        {
            Guard.ArgumentNotNull(structureHolder, "structureHolder");
            Guard.ArgumentNotNull(instrumentationProvider, "instrumentationProvider");

            this.instrumentationProvider = instrumentationProvider;
            this.ReplaceStructureHolder(structureHolder);
        }

        /// <summary>
        /// Gets the <see cref="LogSource"/> mappings available for the <see cref="LogWriterImpl"/>.
        /// </summary>
        public override IDictionary<string, LogSource> TraceSources
        {
            get { return structureHolder.TraceSources; }
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

        static IDictionary<string, LogSource> CreateTraceSourcesDictionary(IEnumerable<LogSource> traceSources)
        {
            IDictionary<string, LogSource> result = new Dictionary<string, LogSource>();

            foreach (LogSource source in traceSources)
            {
                result.Add(source.Name, source);
            }

            return result;
        }

        /// <summary>
        /// Returns the collection of <see cref="LogSource"/>s that matches the collection of categories provided.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns>The matching <see cref="LogSource"/>s</returns>
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
        /// <param name="ex">The exception raised during filter evaluation.</param>
        /// <param name="logEntry">The log entry being evaluated.</param>
        /// <param name="filter">The filter that raised the exception.</param>
        /// <returns>True signaling processing should continue.</returns>
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
        /// <typeparam name="T">The type of filter required.</typeparam>
        /// <returns>The instance of <typeparamref name="T"/> in the filters collection, or <see langword="null"/> 
        /// if there is no such instance.</returns>
        public override T GetFilter<T>()
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
        public override T GetFilter<T>(string name)
        {
            return filter.GetFilter<T>(name);
        }

        /// <summary>
        /// Returns the filter named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the filter required.</param>
        /// <returns>The filter named <paramref name="name"/> in 
        /// the filters collection, or <see langword="null"/> if there is no such filter.</returns>
        public override ILogFilter GetFilter(string name)
        {
            return filter.GetFilter(name);
        }

        private static Stack GetLogicalOperationStack()
        {
            if (!Tracer.IsTracingAvailable())
            {
                return null;
            }

            try
            {
                var stack = Trace.CorrelationManager.LogicalOperationStack;
#if !SILVERLIGHT
                return (Stack)stack.Clone();
#else
                return stack;
#endif
            }
            catch (System.Security.SecurityException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="LogSource"/> objects for the log entry.
        /// </summary>
        /// <param name="logEntry">The <see cref="LogEntry"/> to get the matching trace sources.</param>
        /// <returns>A collection of <see cref="LogSource"/> objects.</returns>
        public override IEnumerable<LogSource> GetMatchingTraceSources(LogEntry logEntry)
        {
            return DoGetMatchingTraceSources(logEntry);
        }

        /// <summary>
        /// Queries whether logging is enabled.
        /// </summary>
        /// <returns><b>true</b> if logging is enabled.</returns>
        public override bool IsLoggingEnabled()
        {
            LogEnabledFilter enabledFilter = filter.GetFilter<LogEnabledFilter>();
            return enabledFilter == null || enabledFilter.Enabled;
        }

        /// <summary>
        /// Queries whether tracing is enabled.
        /// </summary>
        /// <returns><b>true</b> if tracing is enabled.</returns>
        public override bool IsTracingEnabled()
        {
            return structureHolder.TracingEnabled;
        }

        private static bool IsValidTraceSource(LogSource traceSource)
        {
            return traceSource != null && traceSource.Listeners.Count > 0;
        }

        private void ProcessLog(LogEntry log, TraceEventCache traceEventCache)
        {
#if !SILVERLIGHT
            // revert any outstanding impersonation
            using (RevertExistingImpersonation())
            {
                var items = new ContextItems();
                items.ProcessContextItems(log);
#endif

            var matchingTraceSources = GetMatchingTraceSources(log);
            var traceListenerFilter = new TraceListenerFilter();

            foreach (LogSource traceSource in matchingTraceSources)
            {
                try
                {
                    traceSource.TraceData(log.Severity, log.EventId, log, traceListenerFilter, traceEventCache);
                }
                catch (Exception ex)
                {
                    ReportExceptionDuringTracing(ex, log, traceSource);
                }
            }
#if !SILVERLIGHT
            }
#endif
        }

        internal void ReplaceStructureHolder(LogWriterStructureHolder newStructureHolder)
        {
            structureHolder = newStructureHolder;
            filter = new LogFilterHelper(structureHolder.Filters, this);
        }

        void ReportExceptionCheckingFilters(Exception exception,
                                            LogEntry log,
                                            ILogFilter logFilter)
        {
            try
            {
                IDictionary<string, string> additionalInfo = new Dictionary<string, string>();
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
            catch (Exception ex)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.FailureWhileCheckingFilters, ex);
            }
        }

        void ReportExceptionDuringTracing(Exception exception,
                                          LogEntry log,
                                          LogSource traceSource)
        {
            ReportExceptionDuringTracing(exception, log, traceSource.Name);
        }

        private void ReportExceptionDuringTracing(Exception exception, LogEntry log, string traceSourceName)
        {
            try
            {
                IDictionary<string, string> additionalInfo = new Dictionary<string, string>();
                additionalInfo.Add(ExceptionFormatter.Header,
                                   string.Format(CultureInfo.CurrentCulture, Resources.TraceSourceFailed, traceSourceName));
                additionalInfo.Add(Resources.TraceSourceFailed2,
                                   string.Format(CultureInfo.CurrentCulture, Resources.TraceSourceFailed3, log));
                ExceptionFormatter formatter =
                    new ExceptionFormatter(additionalInfo, Resources.DistributorEventLoggerDefaultApplicationName);

                ReportErrorDuringTracing(formatter.GetMessage(exception));
            }
            catch (Exception ex)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.FailureWhileTracing, ex);
            }
        }

        private void ReportErrorDuringTracing(string message)
        {
            try
            {
                LogEntry reportingLogEntry = new LogEntry();
                reportingLogEntry.Severity = TraceEventType.Error;
                reportingLogEntry.Message = message;
                reportingLogEntry.EventId = LogWriterFailureEventID;

                structureHolder.ErrorsTraceSource.TraceData(reportingLogEntry.Severity, reportingLogEntry.EventId, reportingLogEntry);
            }
            catch (Exception ex)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.FailureWhileTracing, ex);
            }
        }

        void ReportMissingCategories(ICollection<string> missingCategories,
                                     LogEntry logEntry)
        {
            try
            {
                LogEntry reportingLogEntry = new LogEntry();
                reportingLogEntry.Severity = TraceEventType.Error;
                reportingLogEntry.Message = string.Format(
                    CultureInfo.CurrentCulture, 
                    Resources.MissingCategories, 
#if !SILVERLIGHT
                    TextFormatter.FormatCategoriesCollection(missingCategories),
#else
                    string.Join(", ", missingCategories),
#endif
                    logEntry);
                reportingLogEntry.EventId = LogWriterFailureEventID;

                structureHolder.ErrorsTraceSource.TraceData(reportingLogEntry.Severity, reportingLogEntry.EventId, reportingLogEntry);
            }
            catch (Exception ex)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.FailureWhileReportingMissingCategories, ex);
            }
        }

        void ReportUnknownException(Exception exception,
                                    LogEntry log)
        {
            try
            {
                IDictionary<string, string> additionalInfo = new Dictionary<string, string>();
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
            catch (Exception ex)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.UnknownFailure, ex);
            }
        }

        /// <summary>
        /// Queries whether a <see cref="LogEntry"/> should be logged.
        /// </summary>
        /// <param name="log">The log entry to check.</param>
        /// <returns><b>true</b> if the entry should be logged.</returns>
        public override bool ShouldLog(LogEntry log)
        {
            return filter.CheckFilters(log);
        }

        /// <summary>
        /// Writes a new log entry as defined in the <see cref="LogEntry"/> parameter.
        /// </summary>
        /// <param name="log">Log entry object to write.</param>
        public override void Write(LogEntry log)
        {
            if (log == null) throw new ArgumentNullException("log");
            var traceEventCache = new TraceEventCache();

            var ignoredActivityId = log.ActivityId;
            var ignoredManagedThreadName = log.ManagedThreadName;

#if !SILVERLIGHT
            this.updateCoordinator.ExecuteReadOperation(() =>
#endif
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
                        instrumentationProvider.FireLogEventRaised();
                    }
                }
                catch (Exception ex)
                {
                    ReportUnknownException(ex, log);
                }
#if !SILVERLIGHT
            });
#else
            }
#endif
        }
    }
}
