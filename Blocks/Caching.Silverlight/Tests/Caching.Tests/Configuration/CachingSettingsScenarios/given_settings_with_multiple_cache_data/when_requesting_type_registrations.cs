using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.CachingSettingsScenarios.given_settings_with_multiple_cache_data
{
    [TestClass]
    public class when_requesting_type_registrations : Context
    {
        private IEnumerable<TypeRegistration> registrations;

        protected override void Act()
        {
            base.Act();

            this.registrations = this.settings.GetRegistrations(Mock.Of<IConfigurationSource>());
        }

        [TestMethod]
        public void then_gets_registrations_from_cache_data()
        {
            Assert.AreEqual(3, this.registrations.Count());
        }

        [TestMethod]
        public void then_registrations_for_non_default_caches_are_not_default()
        {
            Assert.IsFalse(this.registrations.First(tr => tr.Name == "cache1").IsDefault);
            Assert.IsFalse(this.registrations.First(tr => tr.Name == "cache3").IsDefault);
        }

        [TestMethod]
        public void then_registrations_for_default_caches_is_default()
        {
            Assert.IsTrue(this.registrations.First(tr => tr.Name == "cache2").IsDefault);
        }
    }
}
