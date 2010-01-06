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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability.Tests
{
    [TestClass]
    public class CachingStoreProviderDataManageabilityProviderFixture
    {
        CachingStoreProviderDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        CachingStoreProviderData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new CachingStoreProviderDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new CachingStoreProviderData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(CachingStoreProviderDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(CachingStoreProviderDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(SecuritySettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(CachingStoreProviderData), selectedAttribute.TargetType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPolicies(new TestsConfigurationSection(), true, machineKey, userKey);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            configurationObject.CacheManager = "cache manager";
            configurationObject.AbsoluteExpiration = 100;
            configurationObject.SlidingExpiration = 200;

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual("cache manager", configurationObject.CacheManager);
            Assert.AreEqual(100, configurationObject.AbsoluteExpiration);
            Assert.AreEqual(200, configurationObject.SlidingExpiration);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.CacheManager = "cache manager";
            configurationObject.AbsoluteExpiration = 100;
            configurationObject.SlidingExpiration = 200;

            machineKey.AddStringValue(CachingStoreProviderDataManageabilityProvider.CacheManagerPropertyName, "machine cache manager");
            machineKey.AddIntValue(CachingStoreProviderDataManageabilityProvider.AbsoluteExpirationPropertyName, 150);
            machineKey.AddIntValue(CachingStoreProviderDataManageabilityProvider.SlidingExpirationPropertyName, 250);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, userKey);

            Assert.AreEqual("machine cache manager", configurationObject.CacheManager);
            Assert.AreEqual(150, configurationObject.AbsoluteExpiration);
            Assert.AreEqual(250, configurationObject.SlidingExpiration);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.CacheManager = "cache manager";
            configurationObject.AbsoluteExpiration = 100;
            configurationObject.SlidingExpiration = 200;

            userKey.AddStringValue(CachingStoreProviderDataManageabilityProvider.CacheManagerPropertyName, "user cache manager");
            userKey.AddIntValue(CachingStoreProviderDataManageabilityProvider.AbsoluteExpirationPropertyName, 150);
            userKey.AddIntValue(CachingStoreProviderDataManageabilityProvider.SlidingExpirationPropertyName, 250);

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual("user cache manager", configurationObject.CacheManager);
            Assert.AreEqual(150, configurationObject.AbsoluteExpiration);
            Assert.AreEqual(250, configurationObject.SlidingExpiration);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.CacheManager = "cache manager";
            configurationObject.AbsoluteExpiration = 100;
            configurationObject.SlidingExpiration = 200;

            machineKey.AddStringValue(CachingStoreProviderDataManageabilityProvider.CacheManagerPropertyName, "machine cache manager");
            machineKey.AddIntValue(CachingStoreProviderDataManageabilityProvider.AbsoluteExpirationPropertyName, 150);
            machineKey.AddIntValue(CachingStoreProviderDataManageabilityProvider.SlidingExpirationPropertyName, 250);

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, userKey);

            Assert.AreEqual("cache manager", configurationObject.CacheManager);
            Assert.AreEqual(100, configurationObject.AbsoluteExpiration);
            Assert.AreEqual(200, configurationObject.SlidingExpiration);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            CacheManagerSettings cacheSettings = new CacheManagerSettings();
            configurationSource.Add(CacheManagerSettings.SectionName, cacheSettings);
            cacheSettings.CacheManagers.Add(new CacheManagerData("manager", 0, 0, 0, "storage"));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            contentBuilder.StartCategory("category");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndCategory();

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());

            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(CachingStoreProviderDataManageabilityProvider.CacheManagerPropertyName,
                            ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(((AdmNumericPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(CachingStoreProviderDataManageabilityProvider.AbsoluteExpirationPropertyName,
                            ((AdmNumericPart)partsEnumerator.Current).ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(((AdmNumericPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(CachingStoreProviderDataManageabilityProvider.SlidingExpirationPropertyName,
                            ((AdmNumericPart)partsEnumerator.Current).ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
