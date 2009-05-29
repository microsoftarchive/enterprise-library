//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class CustomHashProviderFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanBuildCustomHashProviderFromGivenConfiguration()
        {
            CustomHashProviderData customData
                = new CustomHashProviderData("custom", typeof(MockCustomHashProvider));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            CryptographySettings settings = new CryptographySettings();
            settings.HashProviders.Add(customData);
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CryptographySettings.SectionName, settings);

            IHashProvider custom = EnterpriseLibraryContainer
                .CreateDefaultContainer(configurationSource)
                .GetInstance<IHashProvider>("custom");
                
            Assert.IsNotNull(custom);
            Assert.AreSame(typeof(MockCustomHashProvider), custom.GetType());
            Assert.AreEqual("value1", ((MockCustomHashProvider)custom).customValue);
        }

        [TestMethod]
        public void CanBuildCustomHashProviderFromSavedConfiguration()
        {
            CustomHashProviderData customData
                = new CustomHashProviderData("custom", typeof(MockCustomHashProvider));
            customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            CryptographySettings settings = new CryptographySettings();
            settings.HashProviders.Add(customData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
            sections[CryptographySettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            IHashProvider custom = EnterpriseLibraryContainer
                .CreateDefaultContainer(configurationSource)
                .GetInstance<IHashProvider>("custom");

            Assert.IsNotNull(custom);
            Assert.AreSame(typeof(MockCustomHashProvider), custom.GetType());
            Assert.AreEqual("value1", ((MockCustomHashProvider)custom).customValue);
        }
    }
}
