using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_initialized_stream
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_storage_is_initialized : Context
    {
        private BoundedStreamStorage storage;

        protected override void Act()
        {
            base.Act();

            this.storage = new BoundedStreamStorage(this.stream);
        }

        [TestMethod]
        public void then_storage_reads_initial_properties()
        {
            Assert.AreEqual(1024, this.storage.MaxSizeInBytes);
            Assert.AreEqual(512, this.storage.ActualMaxSizeInBytes);
            Assert.AreEqual(-1, this.storage.Head);
            Assert.AreEqual(0, this.storage.Tail);
        }
    }
}
