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
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_unavailable_network
{
    [TestClass]
    public class when_network_becomes_available : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new TraceEventType(), 10, TestLogEntry1);
            if (this.IsTimerStarted) DoWork();

            Assert.AreEqual(0, SendLogEntriesMessages.Count);

            NetworkStatusMock.Setup(x => x.GetIsNetworkAvailable()).Returns(true);
            NetworkStatusMock.Raise(x => x.NetworkStatusUpdated += null, EventArgs.Empty);
        }

        [TestMethod]
        public void then_all_buffered_entries_are_sent_immediately()
        {
            Assert.IsNull(base.initializeException, base.initializeException != null ? base.initializeException.Message : null);

            Assert.AreEqual(1, SendLogEntriesMessages.Count);
            Assert.AreEqual(TestLogEntry1.Message, SendLogEntriesMessages[0][0].Message);
        }

        [TestMethod]
        public void then_future_ticks_do_not_resend_entries()
        {
            DoWork();
            Assert.AreEqual(1, SendLogEntriesMessages.Count);
        }

        [TestMethod]
        public void then_serveral_ticks_do_not_resend_entries()
        {
            DoWork();
            DoWork();
            DoWork();
            Assert.AreEqual(1, SendLogEntriesMessages.Count);
        }
    }
}
