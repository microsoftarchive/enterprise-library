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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.Contexts
{
    public abstract class CachingConfigurationContext : ContainerContext
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
            
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            CachingViewModel = sourceModel.AddSection(CacheManagerSettings.SectionName, CachingConfiguration);
        }
    }
}
