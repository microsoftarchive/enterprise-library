using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_initialized_stream
{
    public abstract class Context : ArrangeActAssert
    {
        protected Stream stream;
        protected byte[] buffer;

        protected override void Arrange()
        {
            base.Arrange();

            this.buffer = new byte[1024];
            this.stream = new MemoryStream(this.buffer);
            BoundedStreamStorage.Initialize(this.stream, 1024, 512);
        }

        protected override void Teardown()
        {
            base.Teardown();

            this.stream.Dispose();
        }
    }
}
