using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.NotificationTraceListenerScenarios.given_notification_trace_with_trace_tag
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry ActualLogEntry;
        protected LogEntry NotificationTraceLogEntry;

        protected const string DefaultTraceTag = "defaultTraceTag";
        protected string TraceTag;

        protected NotificationTraceListener Listener;

        //protected LoggingSettings Settings;

        protected override void Arrange()
        {
            base.Arrange();

            var notificationMock = new Mock<INotificationTrace>();

            notificationMock
                .Setup(n => n.NewTrace(new TraceEventCache(), "source", TraceEventType.Error, 1, It.IsAny<LogEntry>(), It.IsAny<string>()))
                .Callback((LogEntry le, string tag) =>
                              {
                                  NotificationTraceLogEntry = le;
                                  TraceTag = tag;
                              });

            ActualLogEntry = new LogEntry { Message = "message", Categories = { "cat1", "cat2" }, Severity = Diagnostics.TraceEventType.Error };

            Listener = new NotificationTraceListener(notificationMock.Object, DefaultTraceTag) { Name = "listener" };

            //Settings = new LoggingSettings(null);
        }

        protected override void Teardown()
        {
            base.Teardown();
        }

    }
}
