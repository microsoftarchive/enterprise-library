using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.ContainerModelScenarios.given_configuration_source_with_cache_settings
{
    [TestClass]
    public class when_configuring_a_container : Context
    {
        private IServiceLocator container;

        protected override void Act()
        {
            base.Act();

            this.container = EnterpriseLibraryContainer.CreateDefaultContainer(this.configurationSource);
        }

        [TestMethod]
        public void then_can_resolve_registered_caches_by_name()
        {
            var cache1 = this.container.GetInstance<ObjectCache>("cache1");
            var cache2 = this.container.GetInstance<ObjectCache>("cache2");

            Assert.IsNotNull(cache1);
            Assert.IsInstanceOfType(cache1, typeof(InMemoryCache));
            Assert.AreEqual("cache1", cache1.Name);
            Assert.IsNotNull(cache2);
            Assert.IsInstanceOfType(cache2, typeof(IsolatedStorageCache));
            Assert.AreEqual("cache2", cache2.Name);
        }

        [TestMethod]
        public void then_can_resolve_default_cache()
        {
            var cache = this.container.GetInstance<ObjectCache>();

            Assert.IsNotNull(cache);
            Assert.IsInstanceOfType(cache, typeof(IsolatedStorageCache));
            Assert.AreEqual("cache2", cache.Name);
        }

        protected override void Teardown()
        {
            base.Teardown();

            using (this.container as IDisposable) { }
        }
    }
}
