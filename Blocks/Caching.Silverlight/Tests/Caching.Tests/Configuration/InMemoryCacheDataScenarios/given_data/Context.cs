using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.InMemoryCacheDataScenarios.given_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCacheData data;

        protected override void Arrange()
        {
            base.Arrange();

            this.data =
                new InMemoryCacheData
                {
                    Name = "test name",
                    MaxItemsBeforeScavenging = 500,
                    ItemsLeftAfterScavenging = 300,
                    PollInterval = TimeSpan.FromSeconds(15)
                };
        }
    }
}
