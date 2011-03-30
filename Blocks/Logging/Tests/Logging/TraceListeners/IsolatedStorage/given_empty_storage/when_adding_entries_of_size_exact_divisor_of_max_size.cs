using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_entries_of_size_equal_to_a_fourth_of_the_max_size : Context
    {
        private List<byte[]> entries;

        protected override void Arrange()
        {
            base.Arrange();

            var entrySize = (this.storage.ActualMaxSizeInBytes - BoundedStreamStorage.StreamHeaderSize) / 4 - BoundedStreamStorage.EntryHeaderSize;

            this.entries =
                Enumerable.Range(0, 6)
                    .Select(i => Enumerable.Repeat((byte)i, entrySize).ToArray())
                    .ToList();
        }

        protected override void Act()
        {
            base.Act();

            foreach (var entry in this.entries)
            {
                this.storage.Add(entry);
            }
        }

        [TestMethod]
        public void then_four_last_entries_are_available()
        {
            var actualEntries = this.storage.RetrieveEntries();

            Assert.AreEqual(4, actualEntries.Count());
            CollectionAssert.AreEqual(
                this.entries.Skip(2).ToList(),
                actualEntries.ToList(),
                new EntryComparer());
        }
    }
}
