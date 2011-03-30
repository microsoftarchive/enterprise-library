using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_cache_containing_items_that_expire
{
    [TestClass]
    public class when_specifying_an_update_callback : Context
    {
        private const string KeyWithUpdateAndExpired = "key with update callback and expired";
        private const string KeyWithUpdateNotExpired = "key with update callback with no expiration";
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

            Cache.Add(
                KeyWithUpdateNotExpired,
                "original value",
                new CacheItemPolicy { UpdateCallback = OnUpdateCallback, AbsoluteExpiration = NowForTest + TimeSpan.FromHours(2) }
            );
        }

        protected override void Act()
        {
            base.Act();

            DoExpirations();
        }

        private void OnUpdateCallback(CacheEntryUpdateArguments arguments)
        {
            arguments.UpdatedCacheItem = new CacheItem(arguments.Key, "updated value");
            this.updateArgumentsList.Add(arguments);
        }

        [TestMethod]
        public void then_updated_item_is_present()
        {
            Assert.AreEqual("updated value", Cache[KeyWithUpdateAndExpired]);
        }

        [TestMethod]
        public void then_update_reason_is_because_item_expired()
        {
            Assert.AreEqual(CacheEntryRemovedReason.Expired, this.updateArgumentsList.First(x => x.Key == KeyWithUpdateAndExpired).RemovedReason);
        }

        [TestMethod]
        public void then_when_explicitly_removing_item_does_not_invoke_update_callback()
        {
            Assert.IsNotNull(Cache[KeyWithUpdateNotExpired]);

            Cache.Remove(KeyWithUpdateNotExpired);

            Assert.IsNull(Cache[KeyWithUpdateNotExpired]);
            Assert.IsFalse(this.updateArgumentsList.Any(x => x.Key == KeyWithUpdateNotExpired));
        }

        [TestMethod]
        public void then_when_overwriting_item_does_not_invoke_update_callback()
        {
            Assert.IsNotNull(Cache[KeyWithUpdateNotExpired]);

            Cache[KeyWithUpdateNotExpired] = "overwritten";

            Assert.AreEqual("overwritten", Cache[KeyWithUpdateNotExpired]);
            Assert.IsFalse(this.updateArgumentsList.Any(x => x.Key == KeyWithUpdateNotExpired));
        }
    }
}
