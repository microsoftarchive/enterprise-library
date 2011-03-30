using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_getting_entries : Context
    {
        private IEnumerable<byte[]> entries;

        protected override void Act()
        {
            base.Act();

            this.entries = this.storage.RetrieveEntries();
        }

        [TestMethod]
        public void then_gets_empty_entries()
        {
            Assert.AreEqual(0, this.entries.Count());
        }
    }
}
