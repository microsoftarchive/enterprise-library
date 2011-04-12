using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Used to report logging errors which have occurred during asynchronous logging operations.
    /// </summary>
    public class AsyncTracingErrorReporter : IAsyncTracingErrorReporter
    {
        private Action<Exception, LogEntry, string> logEntryExceptionReportingAction;
        private Action<string> errorMessageReportingAction;

        /// <summary>
        /// Reports an exception during asynchronous tracing.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="log">The log entry that was being logged when the exception occurred.</param>
        /// <param name="traceSourceName">The name of the trace source for which the entry was being logged.</param>
        public void ReportExceptionDuringTracing(Exception exception, LogEntry log, string traceSourceName)
        {
            var action = this.logEntryExceptionReportingAction;
            if (action != null)
            {
                action(exception, log, traceSourceName);
            }
        }

        /// <summary>
        /// Reports an error during asynchronous tracing.
        /// </summary>
        /// <param name="message">The error message to trace.</param>
        public void ReportErrorDuringTracing(string message)
        {
            var action = this.errorMessageReportingAction;
            if (action != null)
            {
                action(message);
            }
        }

        internal void SetLogEntryExceptionReportingAction(Action<Exception, LogEntry, string> logEntryExceptionReportingAction)
        {
            this.logEntryExceptionReportingAction = logEntryExceptionReportingAction;
        }

        internal void SetErrorMessageReportingAction(Action<string> errorMessageReportingAction)
        {
            this.errorMessageReportingAction = errorMessageReportingAction;
        }
    }
}
