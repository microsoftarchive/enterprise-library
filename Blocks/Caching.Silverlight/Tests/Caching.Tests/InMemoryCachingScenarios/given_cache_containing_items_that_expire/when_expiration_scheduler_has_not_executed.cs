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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_cache_containing_items_that_expire
{
    [TestClass]
    public class when_expiration_scheduler_has_not_executed : Context
    {
        protected override void Act()
        {
            base.Act();
        }

        [TestMethod]
        public void then_returns_null_item_on_get()
        {
            Assert.IsNull(Cache.Get(ExpiredKey));
        }

        [TestMethod]
        public void then_enumeration_does_not_yield_expired_item()
        {
            var items = Cache.ToList();

            Assert.AreEqual(1, items.Count);
            Assert.IsFalse(items.Any(x => x.Key == ExpiredKey));
        }

        [TestMethod]
        public void then_getvalues_does_not_yield_expired_item()
        {
            var items = Cache.GetValues(new[] { ExpiredKey, UnexpiredKey });

            Assert.AreEqual(1, items.Count);
            Assert.IsFalse(items.Any(x => x.Key == ExpiredKey));
        }

        [TestMethod]
        public void then_get_raises_removed_callback()
        {
            CacheEntryRemovedArguments removedArguments = null;
            this.RemovedCallback += (args) => removedArguments = args;
            Assert.IsNull(removedArguments);

            var result = Cache.Get(ExpiredKeyWithCallback);

            Assert.IsNull(result);
            Assert.IsNotNull(removedArguments);
            Assert.AreEqual(ExpiredKeyWithCallback, removedArguments.CacheItem.Key);
            Assert.AreEqual("value", removedArguments.CacheItem.Value);
            Assert.AreEqual(Cache, removedArguments.Source);
            Assert.AreEqual(CacheEntryRemovedReason.Expired, removedArguments.RemovedReason);
        }
    }
}
