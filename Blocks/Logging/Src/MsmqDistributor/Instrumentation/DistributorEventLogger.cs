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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
    /// <summary>
    /// Represents an event logger for the distributor Windows service.
    /// This class writes event log entries.
    /// </summary>
    public class DistributorEventLogger
    {
        internal static string Header = ExceptionFormatter.Header;
        private static readonly string DefaultEventSource = Resources.DistributorEventLoggerDefaultApplicationName;
        private string eventSource = null;
        private readonly NameValueCollection additionalInfo = new NameValueCollection();

        /// <summary>
        /// The Event logger used to back up configured logging sinks in the event of problems.
        /// Used to write diagnostic messages to the event log.
        /// </summary>
        public DistributorEventLogger()
            : this(DefaultEventSource)
        {
        }

        ///<summary>
        /// The Event logger used to back up configured logging sinks in the event of problems.
        /// Used to write diagnostic messages to the event log.
        ///</summary>
        ///<param name="eventSource">The name of the <see cref="EventLog.Source"/> use when logging.</param>
        public DistributorEventLogger(string eventSource)
        {
            if (string.IsNullOrEmpty(eventSource)) throw new ArgumentNullException("eventSource");

            this.EventSource = eventSource;
        }

        /// <summary>
        /// Gets or sets the name of the Windows service.
        /// </summary>
        public string EventSource
        {
            get { return (eventSource ?? DefaultEventSource); }
            set { eventSource = value; }
        }

        /// <summary>
        /// Adds the specified message to the additional information name/value collection.
        /// </summary>
        /// <param name="message">The message key.</param>
        /// <param name="value">The actual message (this value will be shown in the event log)</param>
        public void AddMessage(string message, string value)
        {
            additionalInfo.Add(message, value);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Performs instrumentation for the start event.
        /// </summary>
        public void LogServiceStarted()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServiceStartComplete, this.EventSource), true);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Performs instrumentation for the resume event.
        /// </summary>
        public void LogServiceResumed()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServiceResumeComplete, this.EventSource), true);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Performs instrumentation for the stop event.
        /// </summary>
        public void LogServiceStopped()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServiceStopComplete, this.EventSource), false);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Performs instrumentation for the pause event.
        /// </summary>
        public void LogServicePaused()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServicePausedSuccess, this.EventSource), false);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Performs instrumentation for a failure.
        /// </summary>
        /// <param name="message">The message that describes the failure.</param>
        /// <param name="exception">The exception thrown during the failure, or null.</param>
        /// <param name="eventType">The type of event to instrument.</param>
        public void LogServiceFailure(string message, Exception exception, TraceEventType eventType)
        {
            this.AddMessage(DistributorEventLogger.Header, message);
            this.WriteToLog(exception, eventType);
        }

        private string GetMessage(Exception exception)
        {
            ExceptionFormatter exFormatter = new ExceptionFormatter(additionalInfo, this.EventSource);
            return exFormatter.GetMessage(exception);
        }

        private EventLogEntryType GetEventLogEntryType(TraceEventType severity)
        {
            if ((severity == TraceEventType.Error) || (severity == TraceEventType.Critical))
            {
                return EventLogEntryType.Error;
            }
            if (severity == TraceEventType.Warning)
            {
                return EventLogEntryType.Warning;
            }
            return EventLogEntryType.Information;
        }

        private void WriteToLog(Exception exception, TraceEventType severity)
        {
            string finalMessage = String.Empty;

            finalMessage = GetMessage(exception);
            additionalInfo.Clear();

            EventLog.WriteEntry(this.EventSource, finalMessage, GetEventLogEntryType(severity));
        }

        private void LogServiceLifecycleEvent(string message, bool started)
        {
            this.AddMessage(DistributorEventLogger.Header, message);
            this.WriteToLog(null, TraceEventType.Information);
        }
    }
}
