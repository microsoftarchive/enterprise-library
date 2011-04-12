using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_unavailable_network
{
    [TestClass]
    public class when_data_is_traced : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new Diagnostics.TraceEventType(), 100, TestLogEntry1);
            if (this.IsTimerStarted) DoWork();
        }

        [TestMethod]
        public void then_does_not_try_to_send()
        {
            Assert.IsNull(base.initializeException, base.initializeException != null ? base.initializeException.Message : null);
            Assert.AreEqual(0, SendLogEntriesMessages.Count);
        }
    }
}
