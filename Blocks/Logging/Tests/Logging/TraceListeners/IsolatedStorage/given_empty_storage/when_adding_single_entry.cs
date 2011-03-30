using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_single_entry : Context
    {
        private byte[] entry;

        protected override void Act()
        {
            base.Act();

            this.entry = Enumerable.Repeat<byte>(65, 256).ToArray();

            this.storage.Add(this.entry);
        }

        [TestMethod]
        public void then_header_points_to_entry()
        {
            Assert.AreEqual(0, this.storage.Head);
        }

        [TestMethod]
        public void then_tail_points_to_end_of_entry()
        {
            Assert.AreEqual(this.storage.Head + sizeof(byte) * 2 + sizeof(int) + this.entry.Length, this.storage.Tail);
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
        public void then_entry_is_stored_in_stream()
        {
            var bytes = this.stream.ToArray();

            var actualSize = BitConverter.ToInt32(bytes, (int)BoundedStreamStorage.StreamHeaderSize + this.storage.Head + sizeof(byte) * 2);
            var actualEntry = new byte[this.entry.Length];
            Array.Copy(
                bytes, 
                (int)BoundedStreamStorage.StreamHeaderSize + this.storage.Head + sizeof(byte) * 2 + sizeof(int), 
                actualEntry, 
                0, 
                actualEntry.Length);

            Assert.AreEqual(this.entry.Length, actualSize);
            CollectionAssert.AreEqual(this.entry, actualEntry);
        }

        [TestMethod]
        public void then_can_read_entry_from_storage()
        {
            var entries = this.storage.RetrieveEntries();

            CollectionAssert.AreEqual(new[] { this.entry }, entries.ToArray(), new EntryComparer());
        }
    }
}
