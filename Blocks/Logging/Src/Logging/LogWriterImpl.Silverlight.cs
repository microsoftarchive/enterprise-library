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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    public partial class LogWriterImpl : LogWriter, ILogFilterErrorHandler
    {
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
            : this(filters, traceSources, null, null, errorsTraceSource, defaultCategory, false, false, true, instrumentationProvider, new AsyncTracingErrorReporter())
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
                filters,
                traceSources,
                allEventsTraceSource,
                notProcessedTraceSource,
                errorsTraceSource,
                defaultCategory,
                tracingEnabled,
                logWarningsWhenNoCategoriesMatch,
                revertImpersonation,
                new NullLoggingInstrumentationProvider(),
                new AsyncTracingErrorReporter())
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
        /// <param name="asyncTracingErrorReporter">The async tracing error reporter.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
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
            ILoggingInstrumentationProvider instrumentationProvider,
            IAsyncTracingErrorReporter asyncTracingErrorReporter)
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
                asyncTracingErrorReporter)
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
                   instrumentationProvider,
                   new AsyncTracingErrorReporter())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterImpl"/> class.
        /// </summary>
        /// <param name="structureHolder">The initial implementation of the logging stack</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        /// <param name="asyncTracingErrorReporter">The async tracing error reporter.</param>
        public LogWriterImpl(
            LogWriterStructureHolder structureHolder,
            ILoggingInstrumentationProvider instrumentationProvider,
            IAsyncTracingErrorReporter asyncTracingErrorReporter)
            : this(structureHolder, instrumentationProvider)
        {
            Guard.ArgumentNotNull(asyncTracingErrorReporter, "asyncTracingErrorReporter");

            var reporter = asyncTracingErrorReporter as AsyncTracingErrorReporter;
            if (reporter != null)
            {
                reporter.SetLogEntryExceptionReportingAction(this.ReportExceptionDuringTracing);
                reporter.SetErrorMessageReportingAction(this.ReportErrorDuringTracing);
            }
        }

        /// <summary>
        /// Starts a new tracing operation.
        /// </summary>
        /// <param name="operation">The operation id.</param>
        /// <returns>A <see cref="Tracer"/> representing the tracing operation.</returns>
        public override Tracer StartTrace(string operation)
        {
            return new Tracer(operation, this);
        }

        /// <summary>
        /// Starts a new tracing operation.
        /// </summary>
        /// <param name="operation">The operation id.</param>
        /// <param name="activityId">The activity id.</param>
        /// <returns>A <see cref="Tracer"/> representing the tracing operation.</returns>
        public override Tracer StartTrace(string operation, Guid activityId)
        {
            return new Tracer(operation, activityId, this);
        }

        /// <summary>
        /// Sets the tracing status.
        /// </summary>
        /// <param name="enabled">The new status.</param>
        public void SetTracingEnabled(bool enabled)
        {
            this.structureHolder.SetTracingEnabled(enabled);
        }

        #region LogWriter Update API
        private readonly object updateContextLockObject = new object();
        private ILogWriterUpdateContext activeUpdateContext;

        /// <summary>
        /// Provides an update context to batch change requests to the <see cref="LogWriterImpl"/> configuration,
        /// and apply all the changes in a single call <see cref="ILogWriterUpdateContext.ApplyChanges"/>.
        /// </summary>
        /// <returns>Returns an <see cref="ILogWriterUpdateContext"/> instance that can be used to apply the configuration changes.</returns>
        public override ILogWriterUpdateContext GetUpdateContext()
        {
            lock (this.updateContextLockObject)
            {
                this.activeUpdateContext = new LogWriterUpdateContext(this);
                return this.activeUpdateContext;
            }
        }

        /// <summary>
        /// Gets all the available <see cref="TraceListener"/>s in the application.
        /// </summary>
        /// <returns>A collection of all the available <see cref="TraceListener"/>s in the application.</returns>
        protected ICollection<TraceListener> GetTraceListeners()
        {
            return this.TraceSources.SelectMany(x => x.Value.Listeners).Distinct().ToList().AsReadOnly();
        }

        /// <summary>
        /// Provides an update context for changing the <see cref="LogWriterImpl"/> settings.
        /// </summary>
        protected class LogWriterUpdateContext : ILogWriterUpdateContext
        {
            /// <summary>
            /// Initializes a new instance of <see cref="LogWriterUpdateContext"/>.
            /// </summary>
            /// <param name="logWriter">The <see cref="LogWriterImpl"/> being configured.</param>
            public LogWriterUpdateContext(LogWriterImpl logWriter)
            {
                this.LogWriter = logWriter;
                this.IsLoggingEnabled = logWriter.IsLoggingEnabled();

                var traceListeners = logWriter.GetTraceListeners();
                this.Categories = logWriter.TraceSources
                    .Select(x => x.Value.GetUpdateContext(traceListeners))
                    .Where(c => c != null)
                    .ToList()
                    .AsReadOnly();
                this.Listeners = traceListeners
                    .Select(x => x.GetUpdateContext())
                    .Where(c => c != null)
                    .ToList()
                    .AsReadOnly();

                if (logWriter.structureHolder.AllEventsTraceSource != null)
                {
                    this.AllEventsCategory = logWriter.structureHolder.AllEventsTraceSource.GetUpdateContext(traceListeners);
                }

                if (logWriter.structureHolder.NotProcessedTraceSource != null)
                {
                    this.NotProcessedCategory = logWriter.structureHolder.NotProcessedTraceSource.GetUpdateContext(traceListeners);
                }

                if (logWriter.structureHolder.ErrorsTraceSource != null)
                {
                    this.ErrorsCategory = logWriter.structureHolder.ErrorsTraceSource.GetUpdateContext(traceListeners);
                }
            }

            /// <summary>
            /// Gets the update contexts for all the configured categories.
            /// </summary>
            /// <seealso cref="LogSource"/>
            public ICollection<ILogSourceUpdateContext> Categories { get; private set; }

            /// <summary>
            /// Gets or sets if logging is enabled.
            /// </summary>
            /// <returns><see langword="true"/> if logging is enabled.</returns>
            public bool IsLoggingEnabled { get; set; }

            /// <summary>
            /// The <see cref="LogWriterImpl"/> being configured.
            /// </summary>
            protected LogWriterImpl LogWriter { get; private set; }

            /// <summary>
            /// Gets the update contexts for all the configured <see cref="TraceListener"/>s.
            /// </summary>
            public ICollection<ITraceListenerUpdateContext> Listeners { get; private set; }

            /// <summary>
            /// Gets the update context for configured 'All Events' special category.
            /// </summary>
            /// <seealso cref="LogSource"/>
            public ILogSourceUpdateContext AllEventsCategory { get; private set; }

            /// <summary>
            /// Gets the update context for configured 'Not Processed' special category.
            /// </summary>
            /// <seealso cref="LogSource"/>
            public ILogSourceUpdateContext NotProcessedCategory { get; private set; }

            /// <summary>
            /// Gets the update context for configured 'Errors' special category.
            /// </summary>
            /// <seealso cref="LogSource"/>
            public ILogSourceUpdateContext ErrorsCategory { get; private set; }

            /// <summary>
            /// Commits the changes.
            /// </summary>
            public virtual void ApplyChanges()
            {
                lock (this.LogWriter.updateContextLockObject)
                {
                    if (this.LogWriter.activeUpdateContext != this)
                    {
                        throw new InvalidOperationException(Resources.LogWriterUpdateContext_IsNotCurrent);
                    }

                    if (this.IsLoggingEnabled != this.LogWriter.IsLoggingEnabled())
                    {
                        var enabledFilter = this.LogWriter.GetFilter<LogEnabledFilter>();
                        if (enabledFilter == null)
                        {
                            enabledFilter = new LogEnabledFilter("LogWriterUpdateContext_IsLoggingEnabled", this.IsLoggingEnabled);
                            var previousHolder = this.LogWriter.structureHolder;
                            var filters = previousHolder.Filters.ToList();
                            filters.Add(enabledFilter);
                            var newHolder = new LogWriterStructureHolder(
                                filters,
                                previousHolder.TraceSources,
                                previousHolder.AllEventsTraceSource,
                                previousHolder.NotProcessedTraceSource,
                                previousHolder.ErrorsTraceSource,
                                previousHolder.DefaultCategory,
                                previousHolder.TracingEnabled,
                                previousHolder.LogWarningsWhenNoCategoriesMatch,
                                previousHolder.RevertImpersonation);

                            this.LogWriter.ReplaceStructureHolder(newHolder);
                        }
                        else
                        {
                            enabledFilter.Enabled = this.IsLoggingEnabled;
                        }
                    }

                    foreach (var sourceUpdateContext in this.Categories.OfType<ICommitable>())
                    {
                        sourceUpdateContext.Commit();
                    }

                    foreach (var listenerUpdateContext in this.Listeners.OfType<ICommitable>())
                    {
                        listenerUpdateContext.Commit();
                    }

                    if(this.AllEventsCategory != null)
                    {
                        ((ICommitable)this.AllEventsCategory).Commit();
                    }

                    if (this.NotProcessedCategory != null)
                    {
                        ((ICommitable)this.NotProcessedCategory).Commit();
                    }

                    if (this.ErrorsCategory != null)
                    {
                        ((ICommitable) this.ErrorsCategory).Commit();
                    }

                    this.LogWriter.activeUpdateContext = null;
                }
            }
        }
        #endregion LogWriter Update API

        /// <summary>
        /// Releases the resources used by the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> when disposing, <see langword="false"/> otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.structureHolder.Dispose();
            }
        }
    }
}
