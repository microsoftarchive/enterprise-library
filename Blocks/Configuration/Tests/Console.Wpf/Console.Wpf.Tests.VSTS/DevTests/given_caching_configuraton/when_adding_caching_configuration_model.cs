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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.ViewModel;
using System.ComponentModel.Design;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using System.Configuration;
using System.Configuration.Provider;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_caching_configuraton
{
    public abstract class given_caching_configuration : Contexts.ContainerContext
    {
        protected CacheManagerSettings CachingConfiguration;
        protected SectionViewModel CachingViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder sourceBuilder = new ConfigurationSourceBuilder();

            sourceBuilder.ConfigureCaching()
                            .ForCacheManagerNamed("Cache Manager 1")
                                .StoreInMemory()
                            .ForCacheManagerNamed("Cache Manager 2")
                                .StoreInSharedBackingStore("database store")
                            .ForCacheManagerNamed("Cache Manager 3")
                                .StoreCacheMangerItemsInDatabase("database store")
                                    .EncryptUsing.SymmetricEncryptionProviderNamed("crypto thingy")
                                        .UsingSharedSymmetricEncryptionInstanceNamed("symm instance");

            sourceBuilder.UpdateConfigurationWithReplace(source);
            CachingConfiguration = (CacheManagerSettings)source.GetSection(CacheManagerSettings.SectionName);
            CachingViewModel = SectionViewModel.CreateSection(Container, CacheManagerSettings.SectionName, CachingConfiguration);
            
        }
    }

    [TestClass]
    public class when_setting_protection_provider : given_caching_configuration
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

    //TODO: Requires saving the configuration to disk....
    //[TestClass]
    //public class when_creating_model_for_protected_section : given_caching_configuration
    //{
    //    protected override void Act()
    //    {
    //        CachingConfiguration.SectionInformation.ProtectSection(ProtectedConfiguration.DefaultProvider);

    //        CachingViewModel = SectionViewModel.CreateSection(Container, CacheManagerSettings.SectionName, CachingConfiguration);
    //    }

    //    [TestMethod]
    //    public void then_cache_manager_settings_have_protection_provider()
    //    {
    //        Assert.AreEqual(ProtectedConfiguration.DefaultProvider, (string) CachingViewModel.Property("Protection Provider").Value);
    //    }
    //}


    [TestClass]
    public class when_setting_require_permission : given_caching_configuration
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
    public class when_creating_model_for_section_that_requires_permission : given_caching_configuration
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
    public class when_updating_caching_configuration_model_layout : given_caching_configuration
    {
        protected override void Act()
        {
            CachingViewModel.UpdateLayout();
        }

        [TestMethod]
        public void then_cache_managers_are_contained_in_first_column()
        {
            var cacheManagers = CachingViewModel.DescendentElements().Where(x => typeof(CacheManagerData).IsAssignableFrom(x.ConfigurationType));

            Assert.AreNotEqual(0, cacheManagers.Count());
            Assert.IsFalse(cacheManagers.Where(x => x.Column != 0).Any());
        }

        [TestMethod]
        public void then_backingstores_are_contained_in_first_column()
        {
            var backingStores = CachingViewModel.DescendentElements().Where(x => typeof(CacheStorageData).IsAssignableFrom(x.ConfigurationType));

            Assert.AreNotEqual(0, backingStores.Count());
            Assert.IsFalse(backingStores.Where(x => x.Column != 1).Any());
        }

        [TestMethod]
        public void then_encryption_providers_are_contained_in_first_column()
        {
            var encryptionProviders = CachingViewModel.DescendentElements().Where(x => typeof(StorageEncryptionProviderData).IsAssignableFrom(x.ConfigurationType));

            Assert.AreNotEqual(0, encryptionProviders.Count());
            Assert.IsFalse(encryptionProviders.Where(x => x.Column != 2).Any());
        }

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
    public class when_accessing_encryption_providers_collection : given_caching_configuration
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
