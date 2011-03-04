using System.Collections.Generic;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_reopening_cache : Context
    {
        protected override void Act()
        {
            base.Act();

            base.RefreshCache();
        }

        [TestMethod]
        public void then_count_of_items_is_correct()
        {
            Assert.AreEqual(2, Cache.GetCount());
        }

        [TestMethod]
        public void then_string_item_can_be_retrieved_by_indexer()
        {
            Assert.AreEqual("value", Cache["key"]);
        }

        [TestMethod]
        public void then_collection_item_can_be_retrieved_by_indexer()
        {
            var value = Cache["largeData"] as List<int>;
            
            Assert.IsNotNull(value);
            CollectionAssert.AreEquivalent(Enumerable.Range(0, 5000).ToList(), value);
        }
    }
}
