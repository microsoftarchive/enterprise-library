using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_trace_listener
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_flushing_the_listener : Context
    {
        protected override void Act()
        {
            this.traceListener.Flush();
        }

        [TestMethod]
        public void then_repository_is_flushed()
        {
            this.repositoryMock.Verify(r => r.Flush(), Times.Once());
        }
    }
}
