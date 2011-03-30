using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.IsolatedStorageCacheDataScenarios.given_default_ctor
{
    public abstract class Context : ArrangeActAssert
    {
        protected IsolatedStorageCacheData Data;

        protected override void Arrange()
        {
            base.Arrange();

        }
    }
}
