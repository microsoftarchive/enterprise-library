using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_trace_listener
{
    public abstract class Context : ArrangeActAssert
    {
        protected Mock<ILogEntryRepository> repositoryMock;
        protected IsolatedStorageTraceListener traceListener;

        protected override void Arrange()
        {
            base.Arrange();

            this.repositoryMock = new Mock<ILogEntryRepository>();
            this.traceListener = new IsolatedStorageTraceListener(this.repositoryMock.Object);
        }
    }
}
