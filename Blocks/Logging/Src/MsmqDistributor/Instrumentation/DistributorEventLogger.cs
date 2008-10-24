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
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
	/// <summary>
	/// Event logger for distributor windows service.
	/// This class writes event log entries 
	/// </summary>
	public class DistributorEventLogger
	{
		internal static string Header = ExceptionFormatter.Header;

		private static readonly string DefaultApplicationName = Resources.DistributorEventLoggerDefaultApplicationName;
		private const string DefaultLogName = "Application";
		private string logName = null;
		private string applicationName = null;
		private NameValueCollection additionalInfo = new NameValueCollection();

		/// <summary>
		/// The Event logger used to back up configured logging sinks in the event of problems.
		/// Used to write diagnostic messages to the event log.
		/// </summary>
		public DistributorEventLogger()
		{
			this.ApplicationName = DefaultApplicationName;
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="DistributorEventLogger"/>.
		/// </summary>
		/// <param name="logName">The name of the Event Log</param>
		public DistributorEventLogger(string logName)
		{
			this.EventLogName = logName;
			this.ApplicationName = applicationName;
		}

		/// <summary>
		/// Name of the windows service.
		/// </summary>
		public string ApplicationName
		{
			get { return ((null == applicationName) ? DefaultApplicationName : applicationName); }
			set { applicationName = value; }
		}

		/// <summary>
		/// The Event Log name (i.e. the name of the event log to write to; e.g. "Application").
		/// </summary>
		public string EventLogName
		{
			get { return ((null == logName) ? DefaultLogName : logName); }
			set { logName = value; }
		}

		/// <summary>
		/// Add a message to the additional information name value collection.
		/// </summary>
		/// <param name="message">The message key</param>
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
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServiceStartComplete, this.ApplicationName), true);
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Performs instrumentation for the resume event.
		/// </summary>
		public void LogServiceResumed()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServiceResumeComplete, this.ApplicationName), true);
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Performs instrumentation for the stop event.
		/// </summary>
		public void LogServiceStopped()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServiceStopComplete, this.ApplicationName), false);
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Performs instrumentation for the pause event.
		/// </summary>
		public void LogServicePaused()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServicePausedSuccess, this.ApplicationName), false);
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

			try
			{
				System.Management.Instrumentation.Instrumentation.Fire(new DistributorServiceFailureEvent(this.GetMessage(exception), exception));
			}
			catch
			{ }
			this.WriteToLog(exception, eventType);
		}

		private string GetMessage(Exception exception)
		{
			ExceptionFormatter exFormatter = new ExceptionFormatter(additionalInfo, this.ApplicationName);
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

			EventLog.WriteEntry(this.ApplicationName, finalMessage, GetEventLogEntryType(severity));
		}

		private void LogServiceLifecycleEvent(string message, bool started)
		{
			this.AddMessage(DistributorEventLogger.Header, message);

			try
			{
				System.Management.Instrumentation.Instrumentation.Fire(new DistributorServiceLifecycleEvent(this.GetMessage(null), started));
			}
			catch
			{ }
			this.WriteToLog(null, TraceEventType.Information);
		}
	}
}
