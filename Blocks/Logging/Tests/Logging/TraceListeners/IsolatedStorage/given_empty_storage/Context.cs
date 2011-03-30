using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_empty_storage
{
    public abstract class Context : ArrangeActAssert
    {
        protected MemoryStream stream;
        protected BoundedStreamStorage storage;

        protected override void Arrange()
        {
            base.Arrange();

            this.stream = new MemoryStream();
            BoundedStreamStorage.Initialize(this.stream, 1000 + BoundedStreamStorage.StreamHeaderSize, 1000 + BoundedStreamStorage.StreamHeaderSize);

            this.storage = new BoundedStreamStorage(this.stream);
        }

        protected override void Teardown()
        {
            base.Teardown();

            this.stream.Dispose();
        }
    }
}
