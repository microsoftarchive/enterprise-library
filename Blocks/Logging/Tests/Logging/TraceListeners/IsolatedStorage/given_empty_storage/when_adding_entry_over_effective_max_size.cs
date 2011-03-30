using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
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
        public void then_repository_remains_empty()
        {
            CollectionAssert.AreEqual(new byte[0][], this.storage.RetrieveEntries().ToArray());
        }
    }
}
