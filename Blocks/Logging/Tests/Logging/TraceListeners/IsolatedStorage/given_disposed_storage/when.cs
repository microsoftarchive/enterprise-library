using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_disposed_storage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when : Context
    {
        [TestMethod]
        public void then_stream_is_disposed()
        {
            Assert.IsFalse(this.stream.CanRead);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void then_adding_a_new_entry_throws()
        {
            this.storage.Add(new byte[100]);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void then_retrieving_entries_throws()
        {
            this.storage.RetrieveEntries();
        }
    }
}
