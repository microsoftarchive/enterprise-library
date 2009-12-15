using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Console.Wpf.Tests.VSTS.DevTests.given_invalid_model
{
    public abstract class CachingElementModelContext : CachingConfigurationContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            var source = new DesignDictionaryConfigurationSource();
            source.Add(BlockSectionNames.Caching, CachingSettings);
            configurationSourceModel.Load(source);

            CachingSettingsViewModel =
                configurationSourceModel.Sections
                    .Where(x => x.ConfigurationType == typeof(CacheManagerSettings)).Single();
        }

        protected SectionViewModel CachingSettingsViewModel { get; private set; }
    }

    [TestClass]
    public class when_invalidating_caching_section : CachingElementModelContext
    {
        private ValidationModel validationModel;
        private ElementViewModel cacheManager;

        protected override void Arrange()
        {
            base.Arrange();
            validationModel = Container.Resolve<ValidationModel>();
            cacheManager = CachingSettingsViewModel.DescendentElements(x => x.ConfigurationType == typeof(CacheManagerData)).Single();
        }

        protected override void Act()
        {
            var bindableProperty = cacheManager.Property("ExpirationPollFrequencyInSeconds").BindableProperty;
            bindableProperty.BindableValue = "Invalid";
        }

        [TestMethod]
        public void then_only_one_validation_error_message()
        {
            Assert.AreEqual(1, validationModel.ValidationErrors.Count());
        }
    }

    [TestClass]
    public class when_adding_additional_elements_to_invalid_mdoel : CachingElementModelContext
    {
        private ValidationModel validationModel;
        private ElementCollectionViewModel cacheManagerCollection;
        private int originalCount;

        protected override void Arrange()
        {
            base.Arrange();
            validationModel = Container.Resolve<ValidationModel>();

            cacheManagerCollection = CachingSettingsViewModel.GetDescendentsOfType<NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>>().OfType<ElementCollectionViewModel>().Single();
            var newItem = cacheManagerCollection.AddNewCollectionElement(typeof(CacheManagerData));
            newItem.Property("Name").BindableProperty.BindableValue = "";

            originalCount = validationModel.ValidationErrors.Count();
        }

        protected override void Act()
        {
            var newItem = cacheManagerCollection.AddNewCollectionElement(typeof(CacheManagerData));
        }

        [TestMethod]
        public void then_validation_model_retains_errors()
        {
            Assert.AreEqual(originalCount, validationModel.ValidationErrors.Count());
        }
    }
}