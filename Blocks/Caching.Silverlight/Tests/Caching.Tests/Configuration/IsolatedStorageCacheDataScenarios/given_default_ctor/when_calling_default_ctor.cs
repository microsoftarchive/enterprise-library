using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.IsolatedStorageCacheDataScenarios.given_default_ctor
{
    [TestClass]
    public class when_calling_default_ctor : Context
    {
        protected override void Act()
        {
            base.Act();

            Data = new IsolatedStorageCacheData();
        }

        [TestMethod]
        public void then_default_values_are_setted_properly()
        {
            Assert.AreNotEqual(0, Data.MaxSizeInKiloBytes);
            Assert.AreNotEqual(0, Data.PercentOfQuotaUsedBeforeScavenging);
            Assert.AreNotEqual(0, Data.PercentOfQuotaUsedAfterScavenging);
            Assert.IsNotNull(Data.ExpirationPollingInterval);
            Assert.IsNotNull(Data.SerializerType);
        }
    }
}
