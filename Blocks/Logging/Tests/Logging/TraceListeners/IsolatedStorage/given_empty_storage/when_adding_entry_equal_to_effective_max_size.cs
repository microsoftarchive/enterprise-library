using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_entry_equal_to_effective_max_size : Context
    {
        private byte[] entry;

        protected override void Act()
        {
            this.entry =
                Enumerable.Range(0, this.storage.EffectiveMaxSizeInBytes - BoundedStreamStorage.EntryHeaderSize)
                    .Select(i => (byte)i)
                    .ToArray();

            this.storage.Add(entry);
        }

        [TestMethod]
        public void then_entry_is_added_to_the_repository()
        {
            var entries = this.storage.RetrieveEntries();

            Assert.AreEqual(1, entries.Count());
            CollectionAssert.AreEqual(this.entry, entries.ElementAt(0));
        }

        [TestMethod]
        public void then_header_is_zero()
        {
            Assert.AreEqual(0, this.storage.Head);
        }

        [TestMethod]
        public void then_tail_is_zero()
        {
            Assert.AreEqual(0, this.storage.Tail);
        }
    }
}
