using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_trace_data
{
    [TestClass]
    public class when_there_is_an_unknown_proxy_error_on_begin : Context
    {
        protected override void Act()
        {
            LoggingServiceMock.Setup(x => x.BeginAdd(It.IsAny<LogEntryMessage[]>(), It.IsAny<AsyncCallback>(), It.IsAny<object>())).Throws<TestException>();

            Listener.TraceData(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry);
            DoWork();
        }

        [TestMethod]
        public void then_error_is_notified_to_async_reporter()
        {
            AsyncTracingErrorReporterMock.Verify(x => x.ReportErrorDuringTracing(It.Is<string>(m => m.Contains(typeof(TestException).FullName))), Times.Once());
        }

        [TestMethod]
        public void then_entries_are_resent_on_next_timer_tick()
        {
            DoWork();

            AsyncTracingErrorReporterMock.Verify(x => x.ReportErrorDuringTracing(It.Is<string>(m => m.Contains(typeof(TestException).FullName))), Times.Exactly(2));
        }

        public class TestException : Exception
        {
        }
    }
}
