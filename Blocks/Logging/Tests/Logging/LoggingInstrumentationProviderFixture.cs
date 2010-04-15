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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class GivenALoggingInstrumentationProviderWithEventLogEnabled
    {
        private LoggingInstrumentationProvider provider;

        [TestInitialize]
        public void Setup()
        {
            this.provider = new LoggingInstrumentationProvider(false, true, "application");
        }

        [TestMethod]
        public void WhenAReconfigurationErrorIsNotified_ThenItIsLoggedThroughWmi()
        {
            DateTime testStartTime = DateTime.Now;
            Thread.Sleep(1500); // Log granularity is to the second, force us to the next second

            using (var eventLog = new EventLog("Application", ".", "Enterprise Library Logging"))
            {
                this.provider.FireReconfigurationErrorEvent(new Exception("test message"));

                var entries =
                    eventLog.GetEntriesSince(testStartTime).Where(entry => entry.Message.IndexOf("test message") > -1);

                Assert.AreEqual(1, entries.Count());
            }
        }
    }

    [TestClass]
    public class GivenALoggingInstrumentationProviderWithEventLogDisabled
    {
        private LoggingInstrumentationProvider provider;

        [TestInitialize]
        public void Setup()
        {
            this.provider = new LoggingInstrumentationProvider(false, false, "application");
        }

        [TestMethod]
        public void WhenAReconfigurationErrorIsNotified_ThenItIsLoggedThroughWmi()
        {
            DateTime testStartTime = DateTime.Now;
            Thread.Sleep(1500); // Log granularity is to the second, force us to the next second

            using (var eventLog = new EventLog("Application", ".", "Enterprise Library Logging"))
            {
                this.provider.FireReconfigurationErrorEvent(new Exception("test message"));

                var entries =
                    eventLog.GetEntriesSince(testStartTime).Where(entry => entry.Message.IndexOf("test message") > -1);

                Assert.AreEqual(0, entries.Count());
            }
        }
    }
}
