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
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_sliding_and_absolute_expirations
{
    [TestClass]
    public class when_time_is_within_expiration_and_last_usage_is_in_sliding_window : Context
    {
        private DateTimeOffset lastAccessedTime;

        protected override void Act()
        {
            base.Act();
            var currentTime = AbsoluteItemExpiration - TimeSpan.FromHours(2);

            CachingTimeProvider.SetTimeProviderForTests(() => currentTime);
            lastAccessedTime = currentTime - TimeSpan.FromMinutes(2);
        }

        [TestMethod]
        public void then_item_is_not_expired()
        {
            Assert.IsFalse(CacheItemPolicy.IsExpired(lastAccessedTime));
        }
    }
}
