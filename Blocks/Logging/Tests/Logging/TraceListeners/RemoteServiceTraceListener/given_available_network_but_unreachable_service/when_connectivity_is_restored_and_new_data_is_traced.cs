using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_available_network_but_unreachable_service
{
    [TestClass]
    public class when_connectivity_is_restored_and_new_data_is_traced : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new Diagnostics.TraceEventType(), 10, TestLogEntry1);
            DoWork();

            LoggingServiceMock.Setup(x => x.EndAdd(It.IsAny<IAsyncResult>()));

            Listener.TraceData(TestTraceEventCache, TestSource, new Diagnostics.TraceEventType(), 20, TestLogEntry2);
            DoWork();
        }

        [TestMethod]
        public void then_all_buffered_entries_are_sent_immediately()
        {
            Assert.IsNull(base.initializeException, base.initializeException != null ? base.initializeException.Message : null);

            Assert.AreEqual(2, SendLogEntriesMessages.Count);
            Assert.AreEqual(1, SendLogEntriesMessages[0].Length);
            Assert.AreEqual(2, SendLogEntriesMessages[1].Length);
            Assert.AreEqual(TestLogEntry1.Message, SendLogEntriesMessages.ElementAt(1)[0].Message);
            Assert.AreEqual(TestLogEntry2.Message, SendLogEntriesMessages.ElementAt(1)[1].Message);
        }

        [TestMethod]
        public void then_future_ticks_do_not_resend_entries()
        {
            DoWork();
            Assert.AreEqual(2, SendLogEntriesMessages.Count);
        }

        [TestMethod]
        public void then_serveral_ticks_do_not_resend_entries()
        {
            DoWork();
            DoWork();
            DoWork();
            Assert.AreEqual(2, SendLogEntriesMessages.Count);
        }
    }
}
