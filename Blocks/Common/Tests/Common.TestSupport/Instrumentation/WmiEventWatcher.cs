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
using System.Management;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation
{
    public class WmiEventWatcher : IDisposable
    {
        readonly object eventsCollectionLock = new object();
        readonly List<ManagementBaseObject> eventsReceived = new List<ManagementBaseObject>();
        readonly ManagementEventWatcher eventWatcher;
        readonly int numberOfEventsToWatchFor;

        public WmiEventWatcher(int numberOfEventsToWatchFor)
            : this(numberOfEventsToWatchFor, "BaseWmiEvent") {}

        public WmiEventWatcher(int numberOfEventsToWatchFor,
                               string query)
        {
            this.numberOfEventsToWatchFor = numberOfEventsToWatchFor;

            WqlEventQuery eventQuery = new WqlEventQuery(query);
            ManagementScope scope = new ManagementScope(@"\\.\root\EnterpriseLibrary");

            eventWatcher = new ManagementEventWatcher(scope, eventQuery);
            eventWatcher.EventArrived += delegate_EventArrived;

            eventWatcher.Start();
        }

        public List<ManagementBaseObject> EventsReceived
        {
            get { return eventsReceived; }
        }

        public void delegate_EventArrived(object sender,
                                          EventArrivedEventArgs e)
        {
            lock (eventsCollectionLock)
            {
                eventsReceived.Add(e.NewEvent);
            }
        }

        public void Dispose()
        {
            eventWatcher.Stop();
            eventWatcher.Dispose();
        }

        public void WaitForEvents()
        {
            for (int i = 0; i < numberOfEventsToWatchFor * 2; i++)
            {
                Thread.Sleep(100);
                lock (eventsCollectionLock)
                {
                    if (eventsReceived.Count == numberOfEventsToWatchFor) break;
                }
            }
        }
    }
}
