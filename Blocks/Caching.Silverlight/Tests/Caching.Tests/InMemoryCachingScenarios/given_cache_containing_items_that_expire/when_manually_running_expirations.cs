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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_cache_containing_items_that_expire
{
    [TestClass]
    public class when_manually_running_expirations : Context
    {
        protected CacheEntryRemovedArguments removedArguments;

        protected override void Act()
        {
            base.Act();

            this.RemovedCallback += (args) => removedArguments = args;

            DoExpirations();
        }

        [TestMethod]
        public void then_expiration_scheduler_was_initialized_correctly()
        {
            ExpirationMock.Verify();
        }

        [TestMethod]
        public void then_expired_items_are_removed()
        {
            Assert.AreEqual(1, Cache.GetCount());
        }

        [TestMethod]
        public void then_cache_still_contains_unexpired_item()
        {
            Assert.IsTrue(Cache.Contains(UnexpiredKey));
        }

        [TestMethod]
        public void then_cache_called_removed_callback()
        {
            Assert.IsNotNull(removedArguments);
            Assert.AreEqual(ExpiredKeyWithCallback, removedArguments.CacheItem.Key);
            Assert.AreEqual("value", removedArguments.CacheItem.Value);
            Assert.AreEqual(Cache, removedArguments.Source);
            Assert.AreEqual(CacheEntryRemovedReason.Expired, removedArguments.RemovedReason);
        }
    }
}
