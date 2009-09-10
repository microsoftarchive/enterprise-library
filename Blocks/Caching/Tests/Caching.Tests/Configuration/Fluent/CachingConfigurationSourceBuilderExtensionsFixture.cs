//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    public abstract class Given_ConfigurationSourceBuilder : ArrangeActAssert
    {
        protected ConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        public IConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }
    }

    [TestClass]
    public class When_ConfiguringCachingOnConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigurationSourceBuilder.ConfigureCaching();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsCachingConfiguration()
        {
            var configurationSource = GetConfigurationSource();
            Assert.IsNotNull(configurationSource.GetSection(CacheManagerSettings.SectionName));
        }

        //as long as this compiles we should be good
        public void Then_CanComposeFluentInterfaces()
        {
            ConfigurationSourceBuilder.ConfigureCaching()
                   .ForCacheManagerNamed("cache manager")
                        .UseAsDefaultCache()
                        .StoreInIsolatedStorage("store")
                            .EncryptUsing.SharedEncryptionProviderNamed("shared encryption provider")

                   .ForCustomCacheManagerNamed("another provider", typeof(CustomCacheManager))
                        .UseAsDefaultCache();
                    
                    
        }
    }

    public abstract class Given_CachingSettingsInConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected ICachingConfiguration CachingConfiguration;

        protected override void Arrange()
        {
            base.Arrange();

            CachingConfiguration = base.ConfigurationSourceBuilder.ConfigureCaching();
        }

        protected CacheManagerSettings GetCacheManagerSettings()
        {
            var configurationSource = GetConfigurationSource();
            return (CacheManagerSettings) configurationSource.GetSection(CacheManagerSettings.SectionName);
        }
    }
}
