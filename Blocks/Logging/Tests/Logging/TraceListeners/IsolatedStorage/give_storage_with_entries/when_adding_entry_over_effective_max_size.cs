using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.give_storage_with_entries
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_entry_over_effective_max_size : Context
    {
        private Exception exception;

        protected override void Act()
        {
            try
            {
                this.storage.Add(new byte[this.storage.EffectiveMaxSizeInBytes - BoundedStreamStorage.EntryHeaderSize + 1]);
            }
            catch (ArgumentOutOfRangeException e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_exception_is_thrown()
        {
            Assert.IsNotNull(this.exception);
        }

        [TestMethod]
        public void then_repository_keeps_the_previous_entries()
        {
            CollectionAssert.AreEqual(
                new[] { this.existingEntry1, this.existingEntry2, this.existingEntry3 },
                this.storage.RetrieveEntries().ToArray(),
                new EntryComparer());
        }
    }
}
