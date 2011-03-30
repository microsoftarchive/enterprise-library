using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_log_entry_serializer
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_serializing_log_entry_with_categories : Context
    {
        private LogEntry logEntry;
        private byte[] serializedEntry;

        protected override void Act()
        {
            base.Act();

            this.logEntry =
                new LogEntry
                {
                    Message = "message",
                    EventId = 100,
                    Priority = 200,
                    Severity = Diagnostics.TraceEventType.Error,
                    Title = "title",
                    Categories = new[] { "category1", "category2" }
                };
            this.serializedEntry = this.serializer.Serialize(this.logEntry);
        }

        [TestMethod]
        public void then_can_deserialize_the_entry()
        {
            var actualEntry = this.serializer.Deserialize(this.serializedEntry);

            LogEntryAssert.AreEqual(this.logEntry, actualEntry);
        }
    }
}
