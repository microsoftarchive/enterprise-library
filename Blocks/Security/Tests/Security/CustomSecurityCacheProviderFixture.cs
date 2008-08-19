//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class CustomSecurityCacheProviderFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanBuildCustomSecurityCacheProviderFromGivenConfiguration()
        {
            CustomSecurityCacheProviderData customData
                = new CustomSecurityCacheProviderData("custom", typeof(MockCustomSecurityCacheProvider));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            SecuritySettings settings = new SecuritySettings();
            settings.SecurityCacheProviders.Add(customData);
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(SecuritySettings.SectionName, settings);

            ISecurityCacheProvider custom
                = EnterpriseLibraryFactory.BuildUp<ISecurityCacheProvider>("custom", configurationSource);

            Assert.IsNotNull(custom);
            Assert.AreSame(typeof(MockCustomSecurityCacheProvider), custom.GetType());
            Assert.AreEqual("value1", ((MockCustomSecurityCacheProvider)custom).customValue);
        }

        [TestMethod]
        public void CanBuildCustomSecurityCacheProviderFromSavedConfiguration()
        {
            CustomSecurityCacheProviderData customData
                = new CustomSecurityCacheProviderData("custom", typeof(MockCustomSecurityCacheProvider));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            SecuritySettings settings = new SecuritySettings();
            settings.SecurityCacheProviders.Add(customData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
            sections[SecuritySettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            ISecurityCacheProvider custom
                = EnterpriseLibraryFactory.BuildUp<ISecurityCacheProvider>("custom", configurationSource);

            Assert.IsNotNull(custom);
            Assert.AreSame(typeof(MockCustomSecurityCacheProvider), custom.GetType());
            Assert.AreEqual("value1", ((MockCustomSecurityCacheProvider)custom).customValue);
        }
    }
}