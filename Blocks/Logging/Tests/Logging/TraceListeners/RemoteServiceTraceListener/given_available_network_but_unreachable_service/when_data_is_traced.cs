using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_available_network_but_unreachable_service
{
    [TestClass]
    public class when_data_is_traced : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new Diagnostics.TraceEventType(), 100, TestLogEntry1);
            DoWork();
        }

        [TestMethod]
        public void then_tries_to_send_entry_without_failing()
        {
            Assert.IsNull(base.initializeException, base.initializeException != null ? base.initializeException.Message : null);
            Assert.AreEqual(1, SendLogEntriesMessages.Count);
            Assert.AreEqual(1, SendLogEntriesMessages.First().Length);
            Assert.AreEqual(TestLogEntry1.Message, SendLogEntriesMessages.First()[0].Message);
        }

        [TestMethod]
        public void then_logging_service_is_disposed()
        {
            LoggingServiceMock.As<IDisposable>().Verify(x => x.Dispose(), Times.Once());
        }
    }
}
