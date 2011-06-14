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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_nonremovable_priority_and_expiration
{
    [TestClass]
    public class when_expiring : Context
    {
        protected override void Act()
        {
            base.Act();
            CachingTimeProvider.SetTimeProviderForTests(() => ExpirationTime + TimeSpan.FromHours(2));
        }

        [TestMethod]
        public void then_item_expires()
        {
            Assert.IsTrue(Policy.IsExpired(ExpirationTime + TimeSpan.FromMinutes(2)));
        }
    }
}
