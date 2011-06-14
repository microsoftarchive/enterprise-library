//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// Is the instrumentation gateway when no instances of the objects from the block are involved.
    /// </summary>
    [EventLogDefinition("Application", "Enterprise Library ExceptionHandling")]
    public class DefaultExceptionHandlingEventLogger : InstrumentationListener, IDefaultExceptionHandlingInstrumentationProvider
    {
        private readonly IEventLogEntryFormatter eventLogEntryFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExceptionHandlingEventLogger"/> class.
        /// </summary>
        /// <param name="performanceCountersEnabled"><code>true</code> if updating performance counters is allowed, <code>false</code> otherwise.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if writing to the event log is allowed, <code>false</code> otherwise.</param>
        /// <param name="applicationInstanceName">The application name to use with performance counters.</param>
        public DefaultExceptionHandlingEventLogger(
            bool performanceCountersEnabled,
            bool eventLoggingEnabled,
            string applicationInstanceName)
            : base(performanceCountersEnabled, eventLoggingEnabled, new AppDomainNameFormatter(applicationInstanceName))
        {
            eventLogEntryFormatter = new EventLogEntryFormatter(Resources_Desktop.BlockName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExceptionHandlingEventLogger"/> class, specifying whether 
        /// logging to the event log is allowed.
        /// </summary>
        /// <param name="eventLoggingEnabled"><code>true</code> if writing to the event log is allowed, <code>false</code> otherwise.</param>
        public DefaultExceptionHandlingEventLogger(bool eventLoggingEnabled)
            : this(false, eventLoggingEnabled, null)
        { }

        /// <summary>
        /// Logs the occurrence of a configuration error for the Enterprise Library Exception Handling Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="exception">The exception raised for the configuration error.</param>
        /// <param name="policyName">The name of the Exception policy in which the configuration error was detected.</param>
        public void LogConfigurationError(Exception exception, string policyName)
        {
            if(exception == null) throw new ArgumentNullException("exception");
            if (EventLoggingEnabled)
            {
                string eventLogMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources_Desktop.ConfigurationFailureCreatingPolicy,
                        policyName);
                string entryText = eventLogEntryFormatter.GetEntryText(eventLogMessage, exception);
                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Logs the occurrence of an internal error for the Enterprise Library Exception Handling Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="policyName">The name of the Exception policy in which the error was occurred.</param>
        /// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
        public void LogInternalError(string policyName, string exceptionMessage)
        {
            if (EventLoggingEnabled)
            {
                string entryText = eventLogEntryFormatter.GetEntryText(exceptionMessage);
                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Fires the ExceptionHandlingErrorOccurred"/> event.
        /// </summary>
        /// <param name="policyName">The name of the policy involved with the error.</param>
        /// <param name="message">The message that describes the failure.</param>
        public void FireExceptionHandlingErrorOccurred(string policyName, string message)
        {
            LogInternalError(policyName, message);
        }
    }
}
