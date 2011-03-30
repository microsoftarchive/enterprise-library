using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_single_log_entry : Context
    {
        private LogEntry logEntry;

        protected override void Act()
        {
            base.Act();

            this.logEntry =
                new LogEntry
                {
                    Message = "some message",
                    Categories = new[] { "category1", "category2" }
                };

            this.repository.Add(this.logEntry);
        }

        [TestMethod]
        public void then_can_retrieve_the_entry()
        {
            var actualEntries = this.repository.RetrieveEntries();

            Assert.AreEqual(1, actualEntries.Count());
            LogEntryAssert.AreEqual(this.logEntry, actualEntries.ElementAt(0));
        }

    }
}
