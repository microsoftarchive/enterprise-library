using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceDispatcherScenarios.given_default_trace_dispatcher
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry TestLogEntry;

        protected const string TestTag = "tag";
        protected const string TestSource = "source";
        protected const string TestMessage = "message";
        protected const int TestId = 100;
        protected readonly ICollection<string> TestCategories = new Collection<string> { "cat1", "cat2" };
        protected const TraceEventType TestTraceEventType = Diagnostics.TraceEventType.Error;
        protected readonly TraceEventCache TestTraceEventCache = new Diagnostics.TraceEventCache();

        protected DefaultTraceDispatcher TraceDispatcher;

        protected override void Arrange()
        {
            base.Arrange();

            TestLogEntry = new LogEntry { Message = TestMessage, Categories = TestCategories, Severity = TestTraceEventType };

            TraceDispatcher = new DefaultTraceDispatcher();
        }

        protected override void Teardown()
        {
            base.Teardown();
        }

    }
}
