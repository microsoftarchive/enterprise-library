using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_creating_second_instance_cache : Context
    {
        protected IsolatedStorageCache SecondCache;

        protected override void Act()
        {
            base.Act();

            SecondCache = new IsolatedStorageCache(CacheName, MaxSize, QuotaUsedBeforeScavenging, QuotaUsedAfterScavenging, PollingInterval, new IsolatedStorageCacheEntrySerializer());
            SecondCache["new item"] = 37;
        }

        protected override void Teardown()
        {
            base.Teardown();
            using (SecondCache) { }
        }

        [TestMethod]
        public void then_contains_first_instances_items()
        {
            Assert.AreEqual("value", SecondCache["key"]);
            Assert.AreEqual(5000, (SecondCache["largeData"] as List<int>).Count);
        }

        [TestMethod]
        public void then_contains_newly_added_items()
        {
            Assert.AreEqual(37, SecondCache["new item"]);
        }

        [TestMethod]
        public void then_new_instance_does_not_affect_first()
        {
            Assert.IsFalse(Cache.Contains("new item"));

            this.RefreshCache();

            Assert.IsFalse(Cache.Contains("new item"));
        }
    }
}
