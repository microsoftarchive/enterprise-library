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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// The instrumentation gateway when no instances of the objects from the block are involved.
    /// </summary>
    [EventLogDefinition("Application", "Enterprise Library Data")]
    public class DefaultDataEventLogger : InstrumentationListener
    {
        private readonly IEventLogEntryFormatter eventLogEntryFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDataEventLogger"/> class, specifying whether 
        /// logging to the event log is allowed.
        /// </summary>
        /// <param name="eventLoggingEnabled"><b>true</b> if writing to the event log is allowed, <b>false</b> otherwise.</param>
        public DefaultDataEventLogger(bool eventLoggingEnabled)
            : base((string)null, false, eventLoggingEnabled, null)
        {
            eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
        }


        /// <summary>
        /// Logs the occurrence of a configuration error for the Enterprise Library Data Access Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="instanceName">Name of the <see cref="Database"/> instance in which the configuration error was detected.</param>
        /// <param name="exception">The exception raised for the configuration error.</param>
        public void LogConfigurationError(Exception exception, string instanceName)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            if (EventLoggingEnabled)
            {
                string eventLogMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ConfigurationFailureCreatingDatabase,
                        instanceName);
                string entryText = eventLogEntryFormatter.GetEntryText(eventLogMessage, exception);
                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }
    }
}
