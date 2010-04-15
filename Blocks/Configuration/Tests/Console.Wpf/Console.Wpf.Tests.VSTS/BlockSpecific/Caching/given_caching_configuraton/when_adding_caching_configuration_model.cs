//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    [TestClass]
    public class when_setting_protection_provider : CachingConfigurationContext
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

            CacheManagerSettings savedSettings = (CacheManagerSettings)saveSource.GetSection(CacheManagerSettings.SectionName);
            Assert.AreEqual("RsaProtectedConfigurationProvider", saveSource.protectionProviderNameOnLastCall);
        }
    }

    [TestClass]
    public class when_setting_require_permission : CachingConfigurationContext
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

            CacheManagerSettings savedSettings = (CacheManagerSettings)saveSource.GetSection(CacheManagerSettings.SectionName);
            Assert.IsFalse(savedSettings.SectionInformation.RequirePermission);
        }
    }

    [TestClass]
    public class when_creating_model_for_section_that_requires_permission : CachingConfigurationContext
    {
        protected override void Act()
        {
            CachingConfiguration.SectionInformation.RequirePermission = true;
            
            CachingViewModel = SectionViewModel.CreateSection(Container, CacheManagerSettings.SectionName, CachingConfiguration);
        }

        [TestMethod]
        public void then_require_permission_is_set_to_true()
        {
            Assert.IsTrue((bool)CachingViewModel.Property("Require Permission").Value);
        }
    }

    [TestClass]
    public class when_updating_caching_configuration_model_layout : CachingConfigurationContext
    {

        [TestMethod]
        public void then_related_elements_for_cache_manager_contains_backing_store()
        {
            var cacheManager2 = CachingViewModel.DescendentElements().Where(x => x.Name == "Cache Manager 2").FirstOrDefault();
            var relatedElements = CachingViewModel.GetRelatedElements(cacheManager2);

            Assert.IsTrue(relatedElements.Where(x => x.Name == "database store").Any());
        }

        [TestMethod]
        public void then_related_elements_for_backing_store_contains_cache_manager_and_encryption_provider()
        {
            var cacheManager2 = CachingViewModel.DescendentElements().Where(x => x.Name == "database store").FirstOrDefault();
            var relatedElements = CachingViewModel.GetRelatedElements(cacheManager2);

            Assert.IsTrue(relatedElements.Where(x => x.Name == "Cache Manager 2").Any());
            Assert.IsTrue(relatedElements.Where(x => x.Name == "Cache Manager 3").Any());
            Assert.IsTrue(relatedElements.Where(x => x.Name == "crypto thingy").Any());
        }

        [TestMethod]
        public void then_null_cache_storage_is_not_shown()
        {
            Assert.IsNull(CachingViewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(CacheStorageData)).FirstOrDefault());
        }
        
    }

    [TestClass]
    public class when_accessing_encryption_providers_collection : CachingConfigurationContext
    {
        ElementCollectionViewModel encryptionProvidersCollection;

        protected override void Act()
        {
            encryptionProvidersCollection = (ElementCollectionViewModel)CachingViewModel.GetDescendentsOfType<NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>>().First();
        }

        [TestMethod]
        public void then_encryption_providers_down_return_base_storage_encryption_provider_in_polymorphic_elements()
        {
            Assert.IsTrue(encryptionProvidersCollection.IsPolymorphicCollection);
            Assert.IsFalse(encryptionProvidersCollection.PolymorphicCollectionElementTypes.Contains(typeof(StorageEncryptionProviderData)));
        }

        [TestMethod]
        public void then_encryption_providers_down_returns_symmetric_instance_encryption_provider()
        {
            Assert.IsTrue(encryptionProvidersCollection.IsPolymorphicCollection);
            Assert.IsTrue(encryptionProvidersCollection.PolymorphicCollectionElementTypes.Contains(typeof(SymmetricStorageEncryptionProviderData)));
        }
    }
}
