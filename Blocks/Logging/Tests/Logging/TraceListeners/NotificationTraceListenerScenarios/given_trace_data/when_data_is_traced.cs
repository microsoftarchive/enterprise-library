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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.NotificationTraceListenerScenarios.given_trace_data
{
    [TestClass]
    public class when_data_is_traced : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry);
        }

        [TestMethod]
        public void then_trace_is_properly_sent_to_handler()
        {
            TraceDispatcherMock.Verify(th => th.ReceiveTrace(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry, TestListenerName), Times.Once());
        }

    }
}
