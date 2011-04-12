using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching
{
    [TestClass]
    public class DefaultConfigurationFixture
    {
        [TestMethod]
        public void InMemoryCacheDataHasProperDefaultValues()
        {
            var data = new InMemoryCacheData();
            Assert.AreEqual(typeof(InMemoryCache), data.Type);
            Assert.AreEqual(TimeSpan.FromMinutes(2), data.ExpirationPollingInterval);
            Assert.AreEqual(80, data.ItemsLeftAfterScavenging);
            Assert.AreEqual(200, data.MaxItemsBeforeScavenging);
        }

        [TestMethod]
        public void IsolatedStorageCacheDataHasProperDefaultValues()
        {
            var data = new IsolatedStorageCacheData();
            Assert.AreEqual(typeof(IsolatedStorageCache), data.Type);
            Assert.AreEqual(TimeSpan.FromMinutes(2), data.ExpirationPollingInterval);
            Assert.AreEqual(60, data.PercentOfQuotaUsedAfterScavenging);
            Assert.AreEqual(80, data.PercentOfQuotaUsedBeforeScavenging);
        }
    }
}
