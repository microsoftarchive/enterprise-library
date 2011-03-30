using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service.Tests.LoggingService.given_an_incoming_log_entry
{
    [TestClass]
    public class when_data_is_traced : Context
    {
        protected override void Act()
        {
            LoggingService.SendLogEntries(new[] { TestLogEntry });
        }

        [TestMethod]
        public void then_trace_is_properly_sent_through_channel()
        {
            var loggedEntry = LogWriterEntries.Single();
            Assert.AreEqual(1, LogWriterEntries.Count);
            Assert.AreEqual(TestLogEntry.Message, loggedEntry.Message);
            Assert.AreEqual(TestLogEntry.Severity, loggedEntry.Severity);
            Assert.AreEqual(TestCategories.Length, loggedEntry.Categories.Count);
            Assert.AreEqual(TestCategories[0], loggedEntry.Categories.ElementAt(0));
            Assert.AreEqual(TestCategories[1], loggedEntry.Categories.ElementAt(1));
        }
    }
}
