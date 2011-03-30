using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.give_storage_with_entries
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_entry_requiring_two_oldest_entries_to_be_removed : Context
    {
        private byte[] entry1;

        protected override void Act()
        {
            base.Act();

            this.entry1 = Enumerable.Range(0, 512).Select(i => (byte)i).ToArray();

            this.storage.Add(this.entry1);
        }

        [TestMethod]
        public void then_header_points_to_third_entry()
        {
            Assert.AreEqual(
                sizeof(byte) * 2 + sizeof(int) + this.existingEntry1.Length
                + sizeof(byte) * 2 + sizeof(int) + this.existingEntry2.Length,
                this.storage.Head);
        }

        [TestMethod]
        public void then_tail_points_to_end_of_last_entry()
        {
            Assert.AreEqual(
                (sizeof(byte) * 2 + sizeof(int)) * 4
                    + this.existingEntry1.Length
                    + this.existingEntry2.Length
                    + this.existingEntry3.Length
                    + this.entry1.Length
                    - (1024 - BoundedStreamStorage.StreamHeaderSize),
                this.storage.Tail);
        }

        [TestMethod]
        public void then_tail_is_lower_than_header()
        {
            Assert.IsTrue(this.storage.Head > this.storage.Tail);
        }

        [TestMethod]
        public void then_head_and_tail_are_updated_in_stream()
        {
            var bytes = this.stream.ToArray();

            var head = BitConverter.ToInt32(bytes, sizeof(byte) * 2 + sizeof(int) * 2);
            var tail = BitConverter.ToInt32(bytes, sizeof(byte) * 2 + sizeof(int) * 2 + sizeof(int));

            Assert.AreEqual(this.storage.Head, head);
            Assert.AreEqual(this.storage.Tail, tail);
        }

        [TestMethod]
        public void then_can_read_entries_from_storage()
        {
            var entries = this.storage.RetrieveEntries();

            CollectionAssert.AreEqual(
                new[] { this.existingEntry3, this.entry1 },
                entries.ToArray(),
                new EntryComparer());
        }
    }
}
