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

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                provider.FireCacheFailed(errorMessage, exception);

                Assert.AreEqual(0, eventLog.NewEntries().Count());
            }
        }

        [TestMethod]
        public void CacheFailureWithInstrumentationEnabledDoesWriteToEventLog()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, true, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                provider.FireCacheFailed(errorMessage, exception);

                var newEntries = from entry in eventLog.NewEntries()
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(1, newEntries.Count());
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationDisabledDoesNotWriteToEventLog()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                provider.FireCacheCallbackFailed(errorMessage, exception);

                var newEntries = from entry in eventLog.NewEntries()
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(0, newEntries.Count());
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationEnabledDoesWriteToEventLog()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, true, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                provider.FireCacheCallbackFailed(key, exception);

                var newEntries = from entry in eventLog.NewEntries()
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(1, newEntries.Count());
            }
        }

        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", CachingInstrumentationProvider.EventLogSourceName);
        }
    }
}
