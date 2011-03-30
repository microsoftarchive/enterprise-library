using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.NetworkTraceListenerScenarios.given_trace_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry TestLogEntry;
        protected NetworkTraceListener Listener;
        protected Mock<ILoggingService> LoggingServiceMock;
        protected IList<LogEntryMessage[]> SendLogEntriesMessages;

        protected const string TestListenerName = "listenerName";
        protected const string TestSource = "source";
        protected const string TestMessage = "message";
        protected const int TestId = 100;
        protected readonly string[] TestCategories = new[] { "cat1", "cat2" };
        protected const TraceEventType TestTraceEventType = Diagnostics.TraceEventType.Error;
        protected readonly TraceEventCache TestTraceEventCache = new Diagnostics.TraceEventCache();

        protected override void Arrange()
        {
            base.Arrange();

            TestLogEntry = new LogEntry { Message = TestMessage, Categories = TestCategories, Severity = TestTraceEventType };

            SendLogEntriesMessages = new List<LogEntryMessage[]>();
            LoggingServiceMock = new Mock<ILoggingService>();
            LoggingServiceMock
                .Setup(x => x.BeginSendLogEntries(It.IsAny<LogEntryMessage[]>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns<LogEntryMessage[], AsyncCallback, object>((m, c, s) =>
                {
                    SendLogEntriesMessages.Add(m);
                    return Mock.Of<IAsyncResult>();
                });
            Listener = new NetworkTraceListener(() => LoggingServiceMock.Object) { Name = TestListenerName };
        }

        protected override void Teardown()
        {
            base.Teardown();
        }
    }
}
