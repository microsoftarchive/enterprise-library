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
using System.Configuration;
using System.Diagnostics;
using System.Text;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	/// <summary>
	/// Provides the concrete instrumentation for the logical events raised by a <see cref="LoggingInstrumentationProvider"/> object.
	/// </summary>
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "LoggingCountersHelpResource")]
	[EventLogDefinition("Application", "Enterprise Library Logging")]
	public class LoggingInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();
        private const string TotalLoggingEventsRaised = "Total Logging Events Raised";
        private const string TotalTraceListenerEntriesWritten = "Total Trace Listener Entries Written";

		[PerformanceCounter("Logging Events Raised/sec", "LoggingEventRaisedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private EnterpriseLibraryPerformanceCounter logEventRaised;

        [PerformanceCounter(TotalLoggingEventsRaised, "TotalLoggingEventsRaisedHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter totalLoggingEventsRaised;

		[PerformanceCounter("Trace Listener Entries Written/sec", "TraceListenerEntryWrittenHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private EnterpriseLibraryPerformanceCounter traceListenerEntryWritten;

        [PerformanceCounter(TotalTraceListenerEntriesWritten, "TotalTraceListenerEntriesWrittenHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter totalTraceListenerEntriesWritten;

		private const string counterCategoryName = "Enterprise Library Logging Counters";
		private IEventLogEntryFormatter eventLogEntryFormatter;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingInstrumentationListener"/> class.
		/// </summary>
		/// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
		public LoggingInstrumentationListener(bool performanceCountersEnabled,
											  bool eventLoggingEnabled,
											  bool wmiEnabled,
                                              string applicationInstanceName)
            : base(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingInstrumentationListener"/> class.
		/// </summary>
		/// <param name="instanceName">The instance name.</param>
		/// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
		public LoggingInstrumentationListener(string instanceName,
											  bool performanceCountersEnabled,
											  bool eventLoggingEnabled,
											  bool wmiEnabled,
                                              string applicationInstanceName)
            : base(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="LoggingInstrumentationProvider.failureLoggingError"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("FailureLoggingError")]
		public void FailureLoggingError(object sender, FailureLoggingErrorEventArgs e)
		{
			if (WmiEnabled) FireManagementInstrumentation(new LoggingFailureLoggingErrorEvent(e.ErrorMessage, e.Exception.ToString()));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(e.ErrorMessage, e.Exception);

				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="LoggingInstrumentationProvider.logEventRaised"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("LoggingEventRaised")]
		public void LoggingEventRaised(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                logEventRaised.Increment();
                totalLoggingEventsRaised.Increment();
            }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="LoggingInstrumentationProvider.traceListenerEntryWritten"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("TraceListenerEntryWritten")]
		public void TraceListenerEntryWritten(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                traceListenerEntryWritten.Increment();
                totalTraceListenerEntriesWritten.Increment();
            }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="LoggingInstrumentationProvider.configurationFailure"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ConfigurationFailure")]
		public void ConfigurationFailure(object sender, LoggingConfigurationFailureEventArgs e)
		{
			if (WmiEnabled) FireManagementInstrumentation(new LoggingConfigurationFailureEvent(e.Exception.Message));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(Resources.ConfigurationFailureUpdating, e.Exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="LoggingInstrumentationProvider.lockAcquisitionError"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("LockAcquisitionError")]
		public void LockAcquisitionError(object sender, LockAcquisitionErrorEventArgs e)
		{
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(e.ErrorMessage);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// Creates the performance counters to instrument the logging events to the instance names.
		/// </summary>
		/// <param name="instanceNames">The instance names for the performance counters.</param>
		protected override void CreatePerformanceCounters(string[] instanceNames)
		{
			logEventRaised = factory.CreateCounter(counterCategoryName, "Logging Events Raised/sec", instanceNames);
			traceListenerEntryWritten = factory.CreateCounter(counterCategoryName, "Trace Listener Entries Written/sec", instanceNames);
            totalLoggingEventsRaised = factory.CreateCounter(counterCategoryName, TotalLoggingEventsRaised, instanceNames);
            totalTraceListenerEntriesWritten = factory.CreateCounter(counterCategoryName, TotalTraceListenerEntriesWritten, instanceNames);
		}
	}
}
