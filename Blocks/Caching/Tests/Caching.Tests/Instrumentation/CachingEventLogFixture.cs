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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
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
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;

                provider.FireCacheFailed(errorMessage, exception);

                Assert.AreEqual(eventCount, eventLog.Entries.Count);
            }
        }

        [TestMethod]
        public void CacheFailureWithInstrumentationEnabledDoesWriteToEventLog()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, true, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (EventLog eventLog = GetEventLog())
            {
                int originalEventCount = eventLog.Entries.Count;

                provider.FireCacheFailed(errorMessage, exception);

                int newEventCount = eventLog.Entries.Count;
                Assert.IsTrue(originalEventCount < newEventCount);
                var newEntries = from entry in eventLog.GetNewEntries(originalEventCount)
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(1, newEntries.ToList().Count());
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationDisabledDoesNotWriteToEventLog()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (EventLog eventLog = GetEventLog())
            {
                int originalEventCount = eventLog.Entries.Count;

                provider.FireCacheCallbackFailed(errorMessage, exception);

                var newEntries = from entry in eventLog.GetNewEntries(originalEventCount)
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(0, newEntries.ToList().Count);
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationEnabledDoesWriteToEventLog()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, true, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (EventLog eventLog = GetEventLog())
            {
                int originalEventCount = eventLog.Entries.Count;

                provider.FireCacheCallbackFailed(key, exception);

                var newEntries = from entry in eventLog.GetNewEntries(originalEventCount)
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(1, newEntries.ToList().Count);
            }
        }

        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", CachingInstrumentationProvider.EventLogSourceName);
        }
    }
}
