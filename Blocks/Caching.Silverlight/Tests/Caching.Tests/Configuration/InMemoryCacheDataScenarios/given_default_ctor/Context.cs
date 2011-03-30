using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.InMemoryCacheDataScenarios.given_default_ctor
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCacheData Data;

        protected override void Arrange()
        {
            base.Arrange();

        }
    }
}
