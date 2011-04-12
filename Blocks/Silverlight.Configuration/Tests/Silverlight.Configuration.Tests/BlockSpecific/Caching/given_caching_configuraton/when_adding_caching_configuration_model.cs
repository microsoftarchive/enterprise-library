using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    [TestClass]
    public class when_setting_protection_provider : Context
    {
        protected override void Act()
        {
            CachingViewModel.Property("Protection Provider").Value = "RsaProtectedConfigurationProvider";
        }

        [TestMethod]
        public void then_cache_manager_settings_are_saved_encrypted()
        {
            ProtectedConfigurationSource saveSource = new ProtectedConfigurationSource();
            CachingViewModel.Save(saveSource);

            CachingSettings savedSettings = (CachingSettings)saveSource.GetSection(CachingSettings.SectionName);
            Assert.AreEqual("RsaProtectedConfigurationProvider", saveSource.protectionProviderNameOnLastCall);
        }
    }

    [TestClass]
    public class when_setting_require_permission : Context
    {
        protected override void Act()
        {
            CachingViewModel.Property("Require Permission").Value = false;
        }

        [TestMethod]
        public void then_section_is_saved_with_require_permission()
        {
            DesignDictionaryConfigurationSource saveSource = new DesignDictionaryConfigurationSource();
            CachingViewModel.Save(saveSource);

            CachingSettings savedSettings = (CachingSettings)saveSource.GetSection(CachingSettings.SectionName);
            Assert.IsFalse(savedSettings.SectionInformation.RequirePermission);
        }
    }

    [TestClass]
    public class when_creating_model_for_section_that_requires_permission : Context
    {
        protected override void Act()
        {
            TestCachingSettings.SectionInformation.RequirePermission = true;
            
            CachingViewModel = SectionViewModel.CreateSection(Container, CachingSettings.SectionName, TestCachingSettings);
        }

        [TestMethod]
        public void then_require_permission_is_set_to_true()
        {
            Assert.IsTrue((bool)CachingViewModel.Property("Require Permission").Value);
        }
    }
    }
