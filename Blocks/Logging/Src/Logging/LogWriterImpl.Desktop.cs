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
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.Unity.Utility;
using System.Security;
using System.Security.Principal;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    public partial class LogWriterImpl : LogWriter, ILogFilterErrorHandler, ILoggingUpdateHandler
    {
        private readonly ILoggingUpdateCoordinator updateCoordinator;

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
            : this(structureHolder, instrumentationProvider)
        {
            Guard.ArgumentNotNull(updateCoordinator, "updateCoordinator");

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
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources_Desktop.ExceptionCannotCheckImpersonatedIdentity, e);
                return null;
            }

            try
            {
                return WindowsIdentity.Impersonate(IntPtr.Zero);    // to be undone by caller
            }
            catch (SecurityException e)
            {
                // this shouldn't happen, as GetCurrent() and Impersonate() demand the same CAS permissions.
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources_Desktop.ExceptionCannotRevertImpersonatedIdentity, e);
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                instrumentationProvider.FireFailureLoggingErrorEvent(Resources_Desktop.ExceptionCannotRevertImpersonatedIdentity, e);
                return null;
            }
        }
    }
}
