using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_available_network_but_unreachable_service
{
    [TestClass]
    public class when_data_is_traced_and_connectivity_is_restored : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new Diagnostics.TraceEventType(), 100, TestLogEntry1);
            DoWork();

            LoggingServiceMock.Setup(x => x.EndAdd(It.IsAny<IAsyncResult>()));
            DoWork();
        }

        [TestMethod]
        public void then_on_timer_tick_retries_send()
        {
            Assert.IsNull(base.initializeException, base.initializeException != null ? base.initializeException.Message : null);
            Assert.AreEqual(2, SendLogEntriesMessages.Count);
            Assert.AreEqual(1, SendLogEntriesMessages.ElementAt(1).Length);
            Assert.AreEqual(TestLogEntry1.Message, SendLogEntriesMessages.ElementAt(1)[0].Message);
        }

        [TestMethod]
        public void then_future_ticks_do_not_resend_entry()
        {
            DoWork();
            Assert.AreEqual(2, SendLogEntriesMessages.Count);
        }

        [TestMethod]
        public void then_serveral_ticks_do_not_resend_entry()
        {
            DoWork();
            DoWork();
            DoWork();
            DoWork();
            Assert.AreEqual(2, SendLogEntriesMessages.Count);
        }
    }
}
