//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	/// <summary>
	/// The instrumentation gateway when no instances of the objects from the block are involved.
	/// </summary>
	[EventLogDefinition("Application", "Enterprise Library Data")]
	[CustomFactory(typeof(DefaultDataEventLoggerCustomFactory))]
	public class DefaultDataEventLogger : InstrumentationListener
	{
		private readonly IEventLogEntryFormatter eventLogEntryFormatter;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultDataEventLogger"/> class, specifying whether 
		/// logging to the event log and firing WMI events is allowed.
		/// </summary>
		/// <param name="eventLoggingEnabled"><b>true</b> if writing to the event log is allowed, <b>false</b> otherwise.</param>
		/// <param name="wmiEnabled"><b>true</b> if firing WMI events is allowed, <b>false</b> otherwise.</param>
		public DefaultDataEventLogger(bool eventLoggingEnabled, bool wmiEnabled)
			: base((string)null, false, eventLoggingEnabled, wmiEnabled, null)
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}


		/// <summary>
		/// Logs the occurrence of a configuration error for the Enterprise Library Data Access Application Block through the 
		/// available instrumentation mechanisms.
		/// </summary>
		/// <param name="instanceName">Name of the <see cref="Database"/> instance in which the configuration error was detected.</param>
		/// <param name="exception">The exception raised for the configuration error.</param>
		public void LogConfigurationError(Exception exception, string instanceName)
		{
			if (WmiEnabled) FireManagementInstrumentation(new DataConfigurationFailureEvent(instanceName, exception.ToString()));
			if (EventLoggingEnabled)
			{
				string eventLogMessage
					= string.Format(
						Resources.Culture,
						Resources.ConfigurationFailureCreatingDatabase,
						instanceName);
				string entryText = eventLogEntryFormatter.GetEntryText(eventLogMessage, exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
	}
}
