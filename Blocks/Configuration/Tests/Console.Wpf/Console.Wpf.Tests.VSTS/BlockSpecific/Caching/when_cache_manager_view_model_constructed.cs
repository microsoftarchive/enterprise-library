using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Caching
{
    [TestClass]
    public class when_cache_manager_view_model_constructed : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var source = new DesignDictionaryConfigurationSource();
            new TestConfigurationBuilder().AddCachingSettings().Build(source);

            var sourceModel = this.Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            CacheSectionViewModel =
                sourceModel.Sections.Where(x => x.ConfigurationType == typeof (CacheManagerSettings)).Single();

            CacheManager = CacheSectionViewModel.GetDescendentsOfType<CacheManagerData>().FirstOrDefault();
            
        }

        protected ElementViewModel CacheManager { get; private set; }
        protected SectionViewModel CacheSectionViewModel { get; private set; }

        [TestMethod]
        public void then_backing_store_custom_property_suggested_values_includes_empty_value()
        {
            Assert.IsTrue(CacheManager.Property("CacheStorage").SuggestedValues.Contains(string.Empty));
        }

        [TestMethod]
        public void then_backing_store_custom_property_bindable_includes_none_option()
        {
            Assert.IsTrue(((SuggestedValuesBindableProperty)CacheManager.Property("CacheStorage").BindableProperty).BindableSuggestedValues.Contains("<none>"));
        }
    }
}
