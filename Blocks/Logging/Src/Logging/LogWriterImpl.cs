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
using System.Security;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Instance based class to write log messages based on a given configuration.
    /// Messages are routed based on category.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To write log messages to the default configuration, use the <see cref="Logger"/> facade.  
    /// Only create an instance of a LogWriterImpl if you need to write log messages using a custom configuration.
    /// </para>
    /// <para>
    /// The LogWriterImpl works as an entry point to the <see cref="System.Diagnostics"/> trace listeners. 
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
    public class LogWriterImpl : LogWriter, ILogFilterErrorHandler, ILoggingUpdateHandler
    {
        private readonly ILoggingUpdateCoordinator updateCoordinator;
        private readonly ILoggingInstrumentationProvider instrumentationProvider;
        LogWriterStructureHolder structureHolder;
        LogFilterHelper filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list is empty.</param>
        public LogWriterImpl(IEnumerable<ILogFilter> filters,
                         IDictionary<string, LogSource> traceSources,
                         LogSource errorsTraceSource,
                         string defaultCategory)
            : this(filters, traceSources, null, null, errorsTraceSource, defaultCategory, false, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list is empty.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public LogWriterImpl(IEnumerable<ILogFilter> filters,
                         IDictionary<string, LogSource> traceSources,
                         LogSource errorsTraceSource,
                         string defaultCategory,
                         ILoggingInstrumentationProvider instrumentationProvider)
            : this(filters, traceSources, null, null, errorsTraceSource, defaultCategory, false, false, true, instrumentationProvider)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list of a log entry is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch">true if warnings should be logged when a non-matching category is found.</param>
        public LogWriterImpl(
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
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list of a log entry is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch">true if warnings should be logged when a non-matching category is found.</param>
        /// <param name="revertImpersonation">true if impersonation should be reverted while logging.</param>
        public LogWriterImpl(
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
                    revertImpersonation),
                new NullLoggingInstrumentationProvider(),
                new LoggingUpdateCoordinator(null))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list of a log entry is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch">true if warnings should be logged when a non-matching category is found.</param>
        /// <param name="revertImpersonation">true if impersonation should be reverted while logging.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public LogWriterImpl(
            IEnumerable<ILogFilter> filters,
            IDictionary<string, LogSource> traceSources,
            LogSource allEventsTraceSource,
            LogSource notProcessedTraceSource,
            LogSource errorsTraceSource,
            string defaultCategory,
            bool tracingEnabled,
            bool logWarningsWhenNoCategoriesMatch,
            bool revertImpersonation,
            ILoggingInstrumentationProvider instrumentationProvider)
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
                    revertImpersonation),
                instrumentationProvider,
                new LoggingUpdateCoordinator(null))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list is empty.</param>
        public LogWriterImpl(IEnumerable<ILogFilter> filters,
                         IEnumerable<LogSource> traceSources,
                         LogSource errorsTraceSource,
                         string defaultCategory)
            : this(filters, CreateTraceSourcesDictionary(traceSources), errorsTraceSource, defaultCategory)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch">true if warnings should be logged when a non-matching category is found.</param>
        public LogWriterImpl(IEnumerable<ILogFilter> filters,
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
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="filters">The collection of filters to use when processing an entry.</param>
        /// <param name="traceSources">The trace sources to dispatch entries to.</param>
        /// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
        /// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
        /// <param name="defaultCategory">The default category to set when entry categories list is empty.</param>
        /// <param name="tracingEnabled">The tracing status.</param>
        /// <param name="logWarningsWhenNoCategoriesMatch">true if warnings should be logged when a non-matching category is found.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public LogWriterImpl(IEnumerable<ILogFilter> filters,
                         IEnumerable<LogSource> traceSources,
                         LogSource allEventsTraceSource,
                         LogSource notProcessedTraceSource,
                         LogSource errorsTraceSource,
                         string defaultCategory,
                         bool tracingEnabled,
                         bool logWarningsWhenNoCategoriesMatch,
                         ILoggingInstrumentationProvider instrumentationProvider)
            : this(filters,
                   CreateTraceSourcesDictionary(traceSources),
                   allEventsTraceSource,
                   notProcessedTraceSource,
                   errorsTraceSource,
                   defaultCategory,
                   tracingEnabled,
                   logWarningsWhenNoCategoriesMatch,
                   true,
                   instrumentationProvider)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="structureHolder">The initial implementation of the logging stack</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        /// <param name="updateCoordinator">The coordinator for logging operations.</param>
        public LogWriterImpl(
            LogWriterStructureHolder structureHolder,
            ILoggingInstrumentationProvider instrumentationProvider,
            ILoggingUpdateCoordinator updateCoordinator)
        {
            Guard.ArgumentNotNull(structureHolder, "structureHolder");
            Guard.ArgumentNotNull(instrumentationProvider, "instrumentationProvider");
            Guard.ArgumentNotNull(updateCoordinator, "updateCoordinator");

            this.instrumentationProvider = instrumentationProvider;
            this.ReplaceStructureHolder(structureHolder);

            this.updateCoordinator = updateCoordinator;
            this.updateCoordinator.RegisterLoggingUpdateHandler(this);
        }

        ///<summary>
        /// Prepares to update it's internal state, but does not commit this until <see cref="ILoggingUpdateHandler.CommitUpdate"/>
        ///</summary>
        object ILoggingUpdateHandler.PrepareForUpdate(IServiceLocator serviceLocator)
        {
            var newStructureHolder = serviceLocator.GetInstance<LogWriterStructureHolder>();
            return newStructureHolder;
        }

        ///<summary>
        /// Commits the update of internal state.
        ///</summary>
        void ILoggingUpdateHandler.CommitUpdate(object context)
        {
            this.ReplaceStructureHolder((LogWriterStructureHolder)context);
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
        /// Releases the resources used by the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> when disposing, <see langword="false"/> otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.structureHolder.Dispose();
                this.updateCoordinator.UnregisterLoggingUpdateHandler(this);
            }
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
        /// <param name="filter">The fiter that raised the exception.</param>
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
        /// <typeparam name="T">The type of filter requiered.</typeparam>
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
                return (Stack)Trace.CorrelationManager.LogicalOperationStack.Clone();
            }
            catch (SecurityException)
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
            // revert any outstanding impersonation
            using (RevertExistingImpersonation())
            {
                var items = new ContextItems();
                items.ProcessContextItems(log);

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
            }
        }

        /// <devdoc>
        /// Checks to determine whether impersonation is in place, and if it is then it reverts it returning
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
            catch (SecurityException e)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.ExceptionCannotCheckImpersonatedIdentity, e);
                return null;
            }

            try
            {
                return WindowsIdentity.Impersonate(IntPtr.Zero);    // to be undone by caller
            }
            catch (SecurityException e)
            {
                // this shouldn't happen, as GetCurrent() and Impersonate() demand the same CAS permissions.
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.ExceptionCannotRevertImpersonatedIdentity, e);
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources.ExceptionCannotRevertImpersonatedIdentity, e);
                return null;
            }
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
                NameValueCollection additionalInfo = new NameValueCollection();
                additionalInfo.Add(ExceptionFormatter.Header,
                                   string.Format(Resources.Culture, Resources.FilterEvaluationFailed, logFilter.Name));
                additionalInfo.Add(Resources.FilterEvaluationFailed2,
                                   string.Format(Resources.Culture, Resources.FilterEvaluationFailed3, log));
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
            try
            {
                NameValueCollection additionalInfo = new NameValueCollection();
                additionalInfo.Add(ExceptionFormatter.Header,
                                   string.Format(Resources.Culture, Resources.TraceSourceFailed, traceSource.Name));
                additionalInfo.Add(Resources.TraceSourceFailed2,
                                   string.Format(Resources.Culture, Resources.TraceSourceFailed3, log));
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
                reportingLogEntry.Message = string.Format(Resources.Culture, Resources.MissingCategories, TextFormatter.FormatCategoriesCollection(missingCategories), logEntry);
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
                NameValueCollection additionalInfo = new NameValueCollection();
                additionalInfo.Add(ExceptionFormatter.Header, Resources.ProcessMessageFailed);
                additionalInfo.Add(Resources.ProcessMessageFailed2,
                                   string.Format(Resources.Culture, Resources.ProcessMessageFailed3, log));
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
        /// Queries whether a <see cref="LogEntry"/> shold be logged.
        /// </summary>
        /// <param name="log">The log entry to check.</param>
        /// <returns><b>true</b> if the entry should be logged.</returns>
        public override bool ShouldLog(LogEntry log)
        {
            return filter.CheckFilters(log);
        }

        void TryLogLockAcquisitionFailure(string message)
        {
            instrumentationProvider.FireLockAcquisitionError(message);
        }

        /// <summary>
        /// Writes a new log entry as defined in the <see cref="LogEntry"/> parameter.
        /// </summary>
        /// <param name="log">Log entry object to write.</param>
        public override void Write(LogEntry log)
        {
            var traceEventCache = new TraceEventCache();

            var ignoredActivityId = log.ActivityId;
            var ignoredManagedThreadName = log.ManagedThreadName;

            this.updateCoordinator.ExecuteReadOperation(() =>
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
            });
        }
    }
}
