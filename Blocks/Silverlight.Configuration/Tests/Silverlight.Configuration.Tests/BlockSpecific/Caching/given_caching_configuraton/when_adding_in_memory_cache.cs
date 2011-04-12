
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    [TestClass]
    public class when_adding_in_memory_cache : Context
    {
        protected override void Act()
        {
            Assert.AreEqual(0, TestCachingSettings.Caches.Count);
            AddNewInMemoryCache.Execute(null);
        }

        [TestMethod]
        public void then_newly_in_memory_cache_added()
        {
            Assert.AreEqual(1, TestCachingSettings.Caches.Count);
            Assert.IsFalse(string.IsNullOrEmpty((string)AddNewInMemoryCache.AddedElementViewModel.Property("Name").Value));
        }

    }
}
