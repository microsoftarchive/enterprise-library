//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport
{
    /// <summary>
    /// A helper class that encapsulates the gyrations needed to get new
    /// items out of an event log. It marks the current time and returns
    /// entries that have occurred since that time.
    /// </summary>
    public class EventLogTracker : IDisposable
    {
        private DateTime startTime;
        private EventLog eventLog;

        public EventLogTracker(string logName)
            : this(new EventLog(logName))
        {
        }
        
        public EventLogTracker(EventLog eventLog)
        {
            this.eventLog = eventLog;
            startTime = DateTime.Now;

            // Event log granularity is down to the second.
            // Wait one second to ensure any new log messages
            // have an actual newer timestamp.
            Thread.Sleep(1000);
        }

        public void Dispose()
        {
            if(eventLog != null)
            {
                eventLog.Dispose();
                eventLog = null;
            }
        }

        /// <summary>
        /// Return new event log entries created since this
        /// object was instantiated. Returns the newest entries
        /// first, proceeding backwards.
        /// </summary>
        /// <returns>The sequence of <see cref="EventLogEntry"/>
        /// objects that are newer than the time this object was
        /// created.</returns>
        public IEnumerable<EventLogEntry> NewEntries()
        {
            return eventLog.GetEntriesSince(startTime);
        }
    }
}
