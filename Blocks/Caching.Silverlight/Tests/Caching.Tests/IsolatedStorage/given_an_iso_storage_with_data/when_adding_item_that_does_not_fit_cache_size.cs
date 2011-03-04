using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_item_that_does_not_fit_cache_size : Context
    {
        protected override long MaxSize
        {
            get { return 8 * 1024; }
        }

        private bool added;

        protected override void Act()
        {
            base.Act();

            added = Cache.Add("newIem", new byte[9000], new CacheItemPolicy());

            base.RefreshCache();
        }

        [TestMethod]
        public void then_returns_true_on_addition_bacause_it_fails_silently()
        {
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void then_item_is_null()
        {
            Assert.IsNull(Cache["newItem"]);
        }
    }
}
