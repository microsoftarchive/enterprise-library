using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_cache_containing_items_that_expire
{
    [TestClass]
    public class when_manually_running_expirations : Context
    {
        protected override void Act()
        {
            base.Act();

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
    }
}
