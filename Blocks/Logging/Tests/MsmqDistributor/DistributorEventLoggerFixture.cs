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
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestClass]
    public class DistributorEventLoggerFixture
    {
        public const string TestEventLogName = "Application"; //"Test Event Log";
        public const string TestEventSource = "Test Log Distributor";
        public const string message = "message";

        [TestMethod]
        public void CanCreateDistributorEventLogger()
        {
            DistributorEventLogger logger = new DistributorEventLogger();
        }

        [TestMethod]
        public void ServiceStartedWritesToEventLog()
        {
            DistributorEventLogger logger = new DistributorEventLogger();

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServiceStarted();

                Assert.AreEqual(1,
                    eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));
            }
        }

        [TestMethod]
        public void ServiceStoppedWritesToEventLog()
        {
            DistributorEventLogger logger = new DistributorEventLogger();
            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServiceStopped();

                Assert.AreEqual(1, eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));
            }
        }

        [TestMethod]
        public void ServicePausedWritesToEventLog()
        {
            DistributorEventLogger logger = new DistributorEventLogger();

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServicePaused();

                Assert.AreEqual(1, eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));
            }

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServicePaused();

                Assert.AreEqual(1, eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));
            }
        }

        [TestMethod]
        public void ServiceResumedWritesToEventLog()
        {
            DistributorEventLogger logger = new DistributorEventLogger(TestEventSource);

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServiceResumed();

                Assert.AreEqual(1, eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));
            }
        }

        [TestMethod]
        public void ServiceFailureWithoutExceptionWritesToEventLog()
        {
            DistributorEventLogger logger = new DistributorEventLogger(TestEventSource);
            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServiceFailure(message, null, TraceEventType.Error);

                Assert.AreEqual(1,
                    eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));

            }
        }

        [TestMethod]
        public void ServiceFailureWithExceptionWritesToEventLog()
        {
            DistributorEventLogger logger = new DistributorEventLogger(TestEventSource);

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogServiceFailure(message, GetException(), TraceEventType.Error);

                Assert.AreEqual(1,
                    eventLog.NewEntries().Count(ev => EventIsFromLogger(ev, logger)));
            }
        }

        static EventLog GetEventLog()
        {
            if (!EventLog.Exists(TestEventLogName))
            {
                using (EventLog log = new EventLog(TestEventLogName, ".", TestEventSource))
                {
                    log.WriteEntry("Event Log Created");
                }
            }
            return new EventLog(TestEventLogName);
        }

        Exception GetException()
        {
            Exception exception = null;
            try
            {
                throw new ArgumentException("argument");
            }
            catch (Exception e)
            {
                exception = e;
            }
            return exception;
        }

        private static bool EventIsFromLogger(EventLogEntry entry, DistributorEventLogger logger)
        {
            return entry.Source == logger.EventSource;
        }
    }
}
