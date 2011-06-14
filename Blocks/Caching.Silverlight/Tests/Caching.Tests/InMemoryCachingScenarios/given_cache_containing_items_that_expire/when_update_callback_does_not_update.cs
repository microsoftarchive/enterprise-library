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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_cache_containing_items_that_expire
{
    [TestClass]
    public class when_update_callback_does_not_update : Context
    {
        private const string KeyWithUpdateAndExpired = "key with update callback and expired";
        private List<CacheEntryUpdateArguments> updateArgumentsList;

        protected override void Arrange()
        {
            this.updateArgumentsList = new List<CacheEntryUpdateArguments>();

            base.Arrange();

            Cache.Add(
                KeyWithUpdateAndExpired, 
                "original value",
                new CacheItemPolicy { UpdateCallback = OnUpdateCallback, AbsoluteExpiration = NowForTest - TimeSpan.FromHours(2) }
            );
        }

        protected override void Act()
        {
            base.Act();

            DoExpirations();
        }

        private void OnUpdateCallback(CacheEntryUpdateArguments arguments)
        {
            arguments.UpdatedCacheItem = null;
            this.updateArgumentsList.Add(arguments);
        }

        [TestMethod]
        public void then_updated_item_is_not_present()
        {
            Assert.IsNull(Cache[KeyWithUpdateAndExpired]);
        }

        [TestMethod]
        public void then_update_reason_is_because_item_expired()
        {
            Assert.AreEqual(CacheEntryRemovedReason.Expired, this.updateArgumentsList.Single(x => x.Key == KeyWithUpdateAndExpired).RemovedReason);
        }

        [TestMethod]
        public void then_additional_expirations_do_not_call_back()
        {
            DoExpirations();
            DoExpirations();

            Assert.AreEqual(1, this.updateArgumentsList.Count);
        }
    }
}
