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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_absolute_expiration
{
    [TestClass]
    public class when_time_is_after_expiration_time : Context
    {
        protected override void Act()
        {
            base.Act();

            CachingTimeProvider.SetTimeProviderForTests(() => AbsoluteExpirationTime.AddMinutes(1));
        }

        [TestMethod]
        public void then_item_is_expired()
        {
            Assert.IsTrue(CacheItemPolicy.IsExpired(DateTimeOffset.Now));
        }
    }
}
