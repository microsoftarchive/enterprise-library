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

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Used to report logging errors which have occurred during asynchronous logging operations.
    /// </summary>
    /// <remarks>
    /// Trace listeners which perform asynchronous operations must receive a reference to this interface and report 
    /// errors in asynchronous operations to it.
    /// </remarks>
    public interface IAsyncTracingErrorReporter
    {
        /// <summary>
        /// Reports an exception during asynchronous tracing.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="log">The log entry that was being logged when the exception occurred.</param>
        /// <param name="traceSourceName">The name of the trace source for which the entry was being logged.</param>
        void ReportExceptionDuringTracing(Exception exception, LogEntry log, string traceSourceName);

        /// <summary>
        /// Reports an error during asynchronous tracing.
        /// </summary>
        /// <param name="message">The error message to trace.</param>
        void ReportErrorDuringTracing(string message);
    }
}
