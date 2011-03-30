using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.AsyncConfigurationScenarios.given_a_null_current_container
{
    public abstract class Context : ArrangeActAssert
    {
        protected EnterpriseLibraryConfigurationCompletedEventArgs EventArgs;

        protected override void Arrange()
        {
            base.Arrange();

            Common.Configuration.EnterpriseLibraryContainer.Current = null;

            Common.Configuration.EnterpriseLibraryContainer.EnterpriseLibraryConfigurationCompleted +=
              (o, e) => this.EventArgs = e;

        }

        protected override void Teardown()
        {
            Common.Configuration.EnterpriseLibraryContainer.Current = null;

            base.Teardown();
        }
    }
}
