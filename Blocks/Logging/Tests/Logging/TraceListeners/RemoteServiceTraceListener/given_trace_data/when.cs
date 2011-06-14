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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_trace_data
{
    [TestClass]
    public class when : Context
    {
        [TestMethod]
        public void then_listener_is_thread_safe()
        {
            Assert.IsTrue(Listener.IsThreadSafe);
        }

        [TestMethod]
        public void then_adds_timer_subscription()
        {
            Assert.IsNotNull(DoWork);
            TimerMock.Verify(x => x.Start(), Times.Once());
        }

        [TestMethod]
        public void then_forces_tick_on_scheduler()
        {
            TimerMock.Verify(x => x.ForceDoWork(), Times.Once());
        }
    }
}
