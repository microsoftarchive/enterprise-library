using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_log_entry_serializer
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntrySerializer serializer;

        protected override void Arrange()
        {
            base.Arrange();

            this.serializer = new LogEntrySerializer();
        }
    }
}
