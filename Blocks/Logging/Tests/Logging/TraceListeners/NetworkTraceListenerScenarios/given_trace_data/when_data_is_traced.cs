using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.NetworkTraceListenerScenarios.given_trace_data
{
    [TestClass]
    public class when_data_is_traced : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry);
        }

        [TestMethod]
        public void then_trace_is_properly_sent_through_channel()
        {
            Assert.AreEqual(1, SendLogEntriesMessages.Count);
            Assert.AreEqual(1, SendLogEntriesMessages.First().Length);
            Assert.AreEqual(TestLogEntry.Message, SendLogEntriesMessages.First()[0].Message);
            Assert.AreEqual(TestLogEntry.Severity, SendLogEntriesMessages.First()[0].Severity);
            Assert.AreEqual(TestCategories.Length, SendLogEntriesMessages.First()[0].Categories.Length);
            Assert.AreEqual(TestCategories[0], SendLogEntriesMessages.First()[0].Categories[0]);
            Assert.AreEqual(TestCategories[1], SendLogEntriesMessages.First()[0].Categories[1]);
        }
    }
}
