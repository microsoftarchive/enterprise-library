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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Tests
{
    [TestClass]
    public class SecurityCacheConfigurationSerializationFixture
    {
        const string securityCacheName1 = "authorization1";

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            SecuritySettings settings = new SecuritySettings();

            CustomSecurityCacheProviderData securityCacheData1 = new CustomSecurityCacheProviderData(securityCacheName1, typeof(MockCustomSecurityCacheProvider));

            settings.SecurityCacheProviders.Add(securityCacheData1);
            settings.DefaultSecurityCacheProviderName = securityCacheName1;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[SecuritySettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            SecuritySettings roSettigs = (SecuritySettings)configurationSource.GetSection(SecuritySettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(1, roSettigs.SecurityCacheProviders.Count);

            Assert.IsNotNull(roSettigs.SecurityCacheProviders.Get(securityCacheName1));
            Assert.AreSame(typeof(CustomSecurityCacheProviderData), roSettigs.SecurityCacheProviders.Get(securityCacheName1).GetType());
            Assert.AreSame(typeof(MockCustomSecurityCacheProvider), ((CustomSecurityCacheProviderData)roSettigs.SecurityCacheProviders.Get(securityCacheName1)).Type);
        }
    }
}
