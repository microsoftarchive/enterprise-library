using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service.Tests.LoggingService.given_an_incoming_log_entry
{
    [TestClass]
    public class when_adding : Context
    {
        protected override void Act()
        {
            LoggingService.Add(new[] { TestLogEntry });
        }

        [TestMethod]
        public void then_trace_is_properly_sent_through_channel()
        {
            Assert.AreEqual(1, LogWriterEntries.Count);
            var loggedEntry = LogWriterEntries.Single();
            Assert.AreEqual(TestLogEntry.Message, loggedEntry.Message);
            Assert.AreEqual(TestLogEntry.Severity, loggedEntry.Severity);
            Assert.AreEqual(TestCategories.Length, loggedEntry.Categories.Count);
            Assert.AreEqual(TestCategories[0], loggedEntry.Categories.ElementAt(0));
            Assert.AreEqual(TestCategories[1], loggedEntry.Categories.ElementAt(1));
            Assert.AreEqual(TestTimeStamp, loggedEntry.TimeStamp);
        }

        [TestMethod]
        public void then_extended_properties_are_correctly_translated()
        {
            var loggedEntry = LogWriterEntries.Single();
            Assert.AreEqual(2, loggedEntry.ExtendedProperties.Count);
            Assert.AreEqual(TestExtendedPropertiesValues[0], loggedEntry.ExtendedProperties[TestExtendedPropertiesKeys[0]]);
            Assert.AreEqual(TestExtendedPropertiesValues[1], loggedEntry.ExtendedProperties[TestExtendedPropertiesKeys[1]]);
        }
    }
}
