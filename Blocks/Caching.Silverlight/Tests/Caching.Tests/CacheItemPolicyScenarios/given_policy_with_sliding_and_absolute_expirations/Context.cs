//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_sliding_and_absolute_expirations
{
    public abstract class Context : ArrangeActAssert
    {
        protected CacheItemPolicy CacheItemPolicy;

        protected DateTimeOffset AbsoluteItemExpiration = new DateTimeOffset(2011, 1, 1, 10, 10, 0,
            TimeSpan.FromHours(0));

        protected TimeSpan SlidingItemExpiration = TimeSpan.FromMinutes(10);

        protected override void Arrange()
        {
            base.Arrange();

            CacheItemPolicy = new CacheItemPolicy()
                                {
                                    AbsoluteExpiration = AbsoluteItemExpiration,
                                    SlidingExpiration = SlidingItemExpiration,
                                    Priority = CacheItemPriority.Default
                                };
        }

        protected override void Teardown()
        {
            CachingTimeProvider.ResetTimeProvider();
            base.Teardown();
        }
    }
}
