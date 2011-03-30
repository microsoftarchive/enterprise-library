using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.NotificationTraceListenerScenarios.given_trace_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry TestLogEntry;
        protected NotificationTraceListener Listener;
        protected Mock<ITraceDispatcher> TraceDispatcherMock;

        protected const string TestListenerName = "listenerName";
        protected const string TestSource = "source";
        protected const string TestMessage = "message";
        protected const int TestId = 100;
        protected readonly ICollection<string> TestCategories = new Collection<string> { "cat1", "cat2" };
        protected const TraceEventType TestTraceEventType = Diagnostics.TraceEventType.Error;
        protected readonly TraceEventCache TestTraceEventCache = new Diagnostics.TraceEventCache();

        protected override void Arrange()
        {
            base.Arrange();

            TestLogEntry = new LogEntry { Message = TestMessage, Categories = TestCategories, Severity = TestTraceEventType };

            TraceDispatcherMock = new Mock<ITraceDispatcher>();

            Listener = new NotificationTraceListener(TraceDispatcherMock.Object) { Name = TestListenerName };
        }

        protected override void Teardown()
        {
            base.Teardown();
        }

    }
}
