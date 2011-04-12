using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_unavailable_network
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry TestLogEntry1;
        protected LogEntry TestLogEntry2;
        protected Logging.TraceListeners.RemoteServiceTraceListener Listener;
        protected Mock<ILoggingService> LoggingServiceMock;
        protected Mock<IAsyncTracingErrorReporter> AsyncTracingErrorReporterMock;
        protected Mock<INetworkStatus> NetworkStatusMock;
        protected IList<LogEntryMessage[]> SendLogEntriesMessages;
        protected Action DoWork;

        protected const string TestListenerName = "listenerName";
        protected const string TestSource = "source";
        protected const string TestMessage1 = "message1";
        protected const string TestMessage2 = "message2";
        protected const int BufferSize = 10;
        protected readonly TraceEventCache TestTraceEventCache = new TraceEventCache();
        protected bool IsTimerStarted;

        protected override void Arrange()
        {
            base.Arrange();
            this.DoWork = null;
            this.IsTimerStarted = false;

            TestLogEntry1 = new LogEntry { Message = TestMessage1, Categories = new[] { "cat1", "cat2" }, Severity = TraceEventType.Error };
            TestLogEntry2 = new LogEntry { Message = TestMessage2, Categories = new[] { "cat2" }, Severity = TraceEventType.Warning };

            SendLogEntriesMessages = new List<LogEntryMessage[]>();
            LoggingServiceMock = new Mock<ILoggingService>();
            LoggingServiceMock
                .Setup(x => x.BeginAdd(It.IsAny<LogEntryMessage[]>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns<LogEntryMessage[], AsyncCallback, object>((m, c, s) =>
                {
                    SendLogEntriesMessages.Add(m);
                    var result = Mock.Of<IAsyncResult>(x => x.AsyncState == s && x.IsCompleted == false);
                    c.Invoke(result);
                    return result;
                });

            LoggingServiceMock.As<IDisposable>();

            var timerMock = new Mock<IRecurringWorkScheduler>();
            timerMock.Setup(x => x.SetAction(It.IsAny<Action>())).Callback<Action>(x => DoWork = x);
            timerMock.Setup(x => x.Start()).Callback(() => this.IsTimerStarted = true);
            timerMock.Setup(x => x.Stop()).Callback(() => this.IsTimerStarted = false);
            timerMock.Setup(x => x.ForceDoWork()).Callback(() => DoWork());
            AsyncTracingErrorReporterMock = new Mock<IAsyncTracingErrorReporter>();
            NetworkStatusMock = new Mock<INetworkStatus>();
            NetworkStatusMock.Setup(x => x.GetIsNetworkAvailable()).Returns(false);
            Listener = new Logging.TraceListeners.RemoteServiceTraceListener(
                true,
                () => this.LoggingServiceMock.Object, 
                timerMock.Object,
                new LogEntryMessageStore(TestListenerName, BufferSize, 0),
                AsyncTracingErrorReporterMock.Object,
                NetworkStatusMock.Object)
                {
                    Name = TestListenerName
                };
        }
    }
}
