using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_sliding_and_absolute_expirations
{
    public abstract class Context : ArrangeActAssert
    {
        protected IExtendedCacheItemPolicy CacheItemPolicy;

        protected DateTimeOffset AbsoluteItemExpiration = new DateTimeOffset(2011, 1, 1, 10, 10, 0,
            TimeSpan.FromHours(0));

        protected TimeSpan SlidingItemExpiration = TimeSpan.FromMinutes(10);

        protected override void Arrange()
        {
            base.Arrange();

            CacheItemPolicy = new DefaultExtendedCacheItemPolicy(
                new CacheItemPolicy()
                {
                    AbsoluteExpiration = AbsoluteItemExpiration,
                    SlidingExpiration = SlidingItemExpiration,
                    Priority = Caching.Runtime.Caching.CacheItemPriority.Default
                });
        }
    }
}
