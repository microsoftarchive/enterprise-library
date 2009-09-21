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

namespace Console.Wpf.Tests.VSTS.DevTests.given_caching_configuraton
{
    public abstract class given_caching_configuration : ArrangeActAssert
    {
        protected CacheManagerSettings CachingConfiguration;

        protected override void Arrange()
        {
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
        }
    }

    [TestClass]
    public class when_adding_caching_configuration_model : given_caching_configuration
    {
        SectionViewModel cachingModel;
        ServiceContainer container;

        protected override void Act()
        {
            container = new ServiceContainer();
            cachingModel = SectionViewModel.CreateSection(container,  CachingConfiguration);
            cachingModel.UpdateLayout();
        }

        [TestMethod]
        public void then_cache_managers_are_contained_in_first_column()
        {
            var cacheManagers = cachingModel.DescendentElements().Where(x => typeof(CacheManagerData).IsAssignableFrom(x.ConfigurationType));

            Assert.AreNotEqual(0, cacheManagers.Count());
            Assert.IsFalse(cacheManagers.Where(x => x.Column != 0).Any());
        }

        [TestMethod]
        public void then_backingstores_are_contained_in_first_column()
        {
            var backingStores = cachingModel.DescendentElements().Where(x => typeof(CacheStorageData).IsAssignableFrom(x.ConfigurationType));

            Assert.AreNotEqual(0, backingStores.Count());
            Assert.IsFalse(backingStores.Where(x => x.Column != 1).Any());
        }

        [TestMethod]
        public void then_encryption_providers_are_contained_in_first_column()
        {
            var encryptionProviders = cachingModel.DescendentElements().Where(x => typeof(StorageEncryptionProviderData).IsAssignableFrom(x.ConfigurationType));

            Assert.AreNotEqual(0, encryptionProviders.Count());
            Assert.IsFalse(encryptionProviders.Where(x => x.Column != 2).Any());
        }

        [TestMethod]
        public void then_related_elements_for_cache_manager_contains_backing_store()
        {
            var cacheManager2 = cachingModel.DescendentElements().Where(x => x.Name == "Cache Manager 2").FirstOrDefault();
            var relatedElements = cachingModel.GetRelatedElements(cacheManager2);

            Assert.IsTrue(relatedElements.Where(x => x.Name == "database store").Any());
        }

        [TestMethod]
        public void then_related_elements_for_backing_store_contains_cache_manager_and_encryption_provider()
        {
            var cacheManager2 = cachingModel.DescendentElements().Where(x => x.Name == "database store").FirstOrDefault();
            var relatedElements = cachingModel.GetRelatedElements(cacheManager2);

            Assert.IsTrue(relatedElements.Where(x => x.Name == "Cache Manager 2").Any());
            Assert.IsTrue(relatedElements.Where(x => x.Name == "Cache Manager 3").Any());
            Assert.IsTrue(relatedElements.Where(x => x.Name == "crypto thingy").Any());
        }
    }
}
