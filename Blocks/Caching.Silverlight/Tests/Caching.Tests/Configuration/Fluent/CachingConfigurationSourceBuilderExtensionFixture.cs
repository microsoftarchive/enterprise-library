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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    public abstract class Given_ConfigurationSourceBuilder : ArrangeActAssert
    {
        protected IConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }


        protected IConfigurationSource GetConfigurationSource()
        {
            IConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configurationSource);

            return configurationSource;
        }
    }

    [TestClass]
    public class When_ConfiguringCachingSettingsOnConfiguration : Given_ConfigurationSourceBuilder
    {
        IConfigureCaching ConfigureCaching;

        protected override void Act()
        {
            ConfigureCaching = base.ConfigurationSourceBuilder.ConfigureCaching();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsCachingSettings()
        {
            var configurationSource = GetConfigurationSource();
            Assert.IsNotNull(configurationSource.GetSection(CachingSettings.SectionName));
        }
    }

    public abstract class Given_CachingSettingsInConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected IConfigureCaching ConfigureCaching;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureCaching = ConfigurationSourceBuilder.ConfigureCaching();
        }

        protected CachingSettings GetCachingSettings()
        {
            var configurationSource = GetConfigurationSource();
            return (CachingSettings)configurationSource.GetSection(CachingSettings.SectionName);
        }
    }
}
