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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class CustomCacheManagerFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanBuildCustomCacheManagerFromGivenConfiguration()
        {
            CustomCacheManagerData customData
                = new CustomCacheManagerData("custom", typeof(CustomCacheManager));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");

            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(customData);
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, settings);

            ICacheManager custom
                = EnterpriseLibraryFactory.BuildUp<ICacheManager>("custom", configurationSource);

            Assert.IsNotNull(custom);
            Assert.AreSame(typeof(CustomCacheManager), custom.GetType());
            Assert.AreEqual("value1", ((CustomCacheManager)custom).customValue);
        }

        [TestMethod]
        public void CanBuildCustomCacheManagerFromSavedConfiguration()
        {
            CustomCacheManagerData customData
                = new CustomCacheManagerData("custom", typeof(CustomCacheManager));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(customData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
            sections[CacheManagerSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            ICacheManager custom
                = EnterpriseLibraryFactory.BuildUp<ICacheManager>("custom", configurationSource);

            Assert.IsNotNull(custom);
            Assert.AreSame(typeof(CustomCacheManager), custom.GetType());
            Assert.AreEqual("value1", ((CustomCacheManager)custom).customValue);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            CustomCacheManagerData customData
                = new CustomCacheManagerData("custom", typeof(CustomCacheManager));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            CacheManagerSettings settings = new CacheManagerSettings();
            settings.CacheManagers.Add(customData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
            sections[CacheManagerSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            CacheManagerSettings roSettigs = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(1, roSettigs.CacheManagers.Count);

            Assert.IsNotNull(roSettigs.CacheManagers.Get("custom"));
            Assert.AreSame(typeof(CustomCacheManagerData), roSettigs.CacheManagers.Get("custom").GetType());
            Assert.AreEqual("custom", ((CustomCacheManagerData)roSettigs.CacheManagers.Get("custom")).Name);
            Assert.AreEqual(typeof(CustomCacheManager), ((CustomCacheManagerData)roSettigs.CacheManagers.Get("custom")).Type);
            Assert.AreEqual("value1", ((CustomCacheManagerData)roSettigs.CacheManagers.Get("custom")).Attributes[MockCustomProviderBase.AttributeKey]);
        }
    }
}
