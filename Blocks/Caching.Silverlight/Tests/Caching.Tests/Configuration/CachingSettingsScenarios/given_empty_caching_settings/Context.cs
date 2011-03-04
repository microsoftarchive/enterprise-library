using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.CachingSettingsScenarios.given_empty_caching_settings
{
    public abstract class Context : ArrangeActAssert
    {
        protected CachingSettings settings;

        protected override void Arrange()
        {
            base.Arrange();

            this.settings = new CachingSettings { };
        }
    }
}
