using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceDispatcherScenarios.given_default_trace_dispatcher
{
    [TestClass]
    public class when_trace_data_is_received : Context
    {
        private object Sender;
        private TraceReceivedEventArgs EventArgs;

        protected override void Arrange()
        {
            base.Arrange();

            TraceDispatcher.TraceReceived += (s, e) =>
            {
                Sender = s;
                EventArgs = e;
            };

        }
        protected override void Act()
        {
            TraceDispatcher.ReceiveTrace(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry, TestTag);
        }

        [TestMethod]
        public void then_trace_received_event_is_properly_raised()
        {
            Assert.AreSame(Sender, TraceDispatcher);
            Assert.AreSame(EventArgs.Data, TestLogEntry);
            Assert.AreEqual(EventArgs.EventType, TestTraceEventType);
            Assert.AreEqual(EventArgs.Id, TestId);
            Assert.AreEqual(EventArgs.Source, TestSource);
            Assert.AreEqual(EventArgs.Tag, TestTag);
            Assert.AreSame(EventArgs.TraceEventCache, TestTraceEventCache);
        }
    }
}
