using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.IsolatedStorageCacheDataScenarios.given_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected IsolatedStorageCacheData data;

        protected override void Arrange()
        {
            base.Arrange();

            this.data = new IsolatedStorageCacheData { Name = "test name", MaxSize = 100L };
        }
    }
}
