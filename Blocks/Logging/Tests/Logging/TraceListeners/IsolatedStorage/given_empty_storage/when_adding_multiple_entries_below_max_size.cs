using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_multiple_entries_below_max_size : Context
    {
        private byte[] entry1;
        private byte[] entry2;
        private byte[] entry3;

        protected override void Act()
        {
            base.Act();

            this.entry1 = Enumerable.Repeat<byte>(65, 256).ToArray();
            this.entry2 = Enumerable.Repeat<byte>(66, 128).ToArray();
            this.entry3 = Enumerable.Repeat<byte>(67, 512).ToArray();

            this.storage.Add(this.entry1);
            this.storage.Add(this.entry2);
            this.storage.Add(this.entry3);
        }

        [TestMethod]
        public void then_header_points_to_first_entry()
        {
            Assert.AreEqual(0, this.storage.Head);
        }

        [TestMethod]
        public void then_tail_points_to_end_of_last_entry()
        {
            Assert.AreEqual(
                this.storage.Head
                    + sizeof(byte) * 2 + sizeof(int) + this.entry1.Length
                    + sizeof(byte) * 2 + sizeof(int) + this.entry2.Length
                    + sizeof(byte) * 2 + sizeof(int) + this.entry3.Length,
                this.storage.Tail);
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
                new[] { this.entry1, this.entry2, this.entry3 },
                entries.ToArray(),
                new EntryComparer());
        }
    }
}
