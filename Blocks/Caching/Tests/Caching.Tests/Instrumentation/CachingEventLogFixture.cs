//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestClass]
    public class CachingEventLogFixture
    {
        const string instanceName = "test";
        const string exceptionMessage = "exception message";
        const string errorMessage = "error message";
        const string key = "key";

        NoPrefixNameFormatter formatter;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new NoPrefixNameFormatter();
        }

        [TestMethod]
        public void CacheFailureWithInstrumentationDisabledDoesNotWriteToEventLog()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            CacheFailureEventArgs args = new CacheFailureEventArgs(errorMessage, exception);

            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;

                listener.CacheFailed(null, args);

                Assert.AreEqual(eventCount, eventLog.Entries.Count);
            }
        }

        [TestMethod]
        public void CacheFailureWithInstrumentationEnabledDoesWriteToEventLog()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, true, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            CacheFailureEventArgs args = new CacheFailureEventArgs(errorMessage, exception);

            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;

                listener.CacheFailed(null, args);

                Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
                Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(exceptionMessage) > -1);
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationDisabledDoesNotWriteToEventLog()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            CacheCallbackFailureEventArgs args = new CacheCallbackFailureEventArgs(errorMessage, exception);

            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;

                listener.CacheCallbackFailed(null, args);

                Assert.AreEqual(eventCount, eventLog.Entries.Count);
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationEnabledDoesWriteToEventLog()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, true, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            CacheCallbackFailureEventArgs args = new CacheCallbackFailureEventArgs(key, exception);

            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;

                listener.CacheCallbackFailed(null, args);

                Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
                Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(exceptionMessage) > -1);
            }
        }

        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", CachingInstrumentationListener.EventLogSourceName);
        }
    }
}
