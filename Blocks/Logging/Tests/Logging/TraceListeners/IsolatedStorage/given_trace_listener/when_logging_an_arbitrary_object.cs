using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_trace_listener
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_logging_an_arbitrary_object : Context
    {
        private LogEntry logEntry;

        protected override void Act()
        {
            this.repositoryMock.Setup(r => r.Add(It.IsAny<LogEntry>())).Callback<LogEntry>(le => this.logEntry = le);

            this.traceListener.TraceData(new Diagnostics.TraceEventCache(), "test", Diagnostics.TraceEventType.Error, 100, "some object");
        }

        [TestMethod]
        public void then_entry_is_logged_to_the_repository()
        {
            LogEntryAssert.AreEqual(
                new LogEntry
                {
                    Message = "some object",
                    Categories = new[] { "test" },
                    Severity = Diagnostics.TraceEventType.Error,
                    EventId = 100
                },
                this.logEntry);
        }
    }
}
