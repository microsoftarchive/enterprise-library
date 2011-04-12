using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_trace_data
{
    [TestClass]
    public class when_there_is_an_unknown_proxy_error_on_end : Context
    {
        protected override void Act()
        {
            LoggingServiceMock.Setup(x => x.EndAdd(It.IsAny<IAsyncResult>())).Throws<TestException>();

            Listener.TraceData(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry);
            DoWork();
        }

        [TestMethod]
        public void then_error_is_notified_to_async_reporter()
        {
            AsyncTracingErrorReporterMock.Verify(x => x.ReportErrorDuringTracing(It.Is<string>(m => m.Contains(TestMessage))), Times.Once());
        }

        [TestMethod]
        public void then_entries_are_not_resent_on_next_timer_tick()
        {
            Assert.AreEqual(1, SendLogEntriesMessages.Count);

            DoWork();

            Assert.AreEqual(1, SendLogEntriesMessages.Count);
        }

        public class TestException : Exception
        {
        }
    }
}
