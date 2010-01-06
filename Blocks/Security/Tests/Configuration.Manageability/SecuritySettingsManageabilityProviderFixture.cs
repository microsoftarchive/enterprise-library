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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Security.Configuration.Manageability.Tests
{
    [TestClass]
    public class SecuritySettingsManageabilityProviderFixture
    {
        SecuritySettingsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        SecuritySettings section;
        DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            provider = new SecuritySettingsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            section = new SecuritySettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(SecuritySettings.SectionName, section);
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(SecuritySettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(SecuritySettings.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(SecuritySettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPolicies(new TestsConfigurationSection(), true, machineKey, userKey);
        }

        [TestMethod]
        public void SectionIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("default authorization", section.DefaultAuthorizationProviderName);
            Assert.AreEqual("default securitycache", section.DefaultSecurityCacheProviderName);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfMachineKeyIsNull()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            provider.OverrideWithGroupPolicies(section, true, null, userKey);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfUserKeyIsNull()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            provider.OverrideWithGroupPolicies(section, true, machineKey, null);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromMachineKey()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            machineKey.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultAuthorizationProviderPropertyName, "machine authorization");
            machineKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultSecurityCacheProviderPropertyName, "machine securitycache");

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("machine authorization", section.DefaultAuthorizationProviderName);
            Assert.AreEqual("machine securitycache", section.DefaultSecurityCacheProviderName);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromUserKey()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            userKey.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultAuthorizationProviderPropertyName, "user authorization");
            userKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultSecurityCacheProviderPropertyName, "user securitycache");

            provider.OverrideWithGroupPolicies(section, true, userKey, userKey);

            Assert.AreEqual("user authorization", section.DefaultAuthorizationProviderName);
            Assert.AreEqual("user securitycache", section.DefaultSecurityCacheProviderName);
        }

        [TestMethod]
        public void DefaultAuthorizationProviderIsOverridenWithListItemNone()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            userKey.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultAuthorizationProviderPropertyName, AdmContentBuilder.NoneListItem);
            userKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultSecurityCacheProviderPropertyName, "user securitycache");

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("", section.DefaultAuthorizationProviderName);
            Assert.AreEqual("user securitycache", section.DefaultSecurityCacheProviderName);
        }

        [TestMethod]
        public void DefaultSecurityCacheProviderIsOverridenWithListItemNone()
        {
            section.DefaultAuthorizationProviderName = "default authorization";
            section.DefaultSecurityCacheProviderName = "default securitycache";

            userKey.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultAuthorizationProviderPropertyName, "user authorization");
            userKey.AddStringValue(SecuritySettingsManageabilityProvider.DefaultSecurityCacheProviderPropertyName, AdmContentBuilder.NoneListItem);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("user authorization", section.DefaultAuthorizationProviderName);
            Assert.AreEqual("", section.DefaultSecurityCacheProviderName);
        }

        [TestMethod]
        public void AuthorizationProviderWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(AuthorizationProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            AuthorizationProviderData authorizationProvider1Data = new AuthorizationProviderData("authorizationProvider1", typeof(Object));
            section.AuthorizationProviders.Add(authorizationProvider1Data);
            AuthorizationProviderData authorizationProvider2Data = new AuthorizationProviderData("authorizationProvider2", typeof(Object));
            section.AuthorizationProviders.Add(authorizationProvider2Data);

            MockRegistryKey machineAuthorizationProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(SecuritySettingsManageabilityProvider.AuthorizationProvidersKeyName, machineAuthorizationProvidersKey);
            MockRegistryKey machineAuthorizationProvider2Key = new MockRegistryKey(false);
            machineAuthorizationProvidersKey.AddSubKey("authorizationProvider2", machineAuthorizationProvider2Key);
            machineAuthorizationProvider2Key.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual(1, section.AuthorizationProviders.Count);
            Assert.IsNotNull(section.AuthorizationProviders.Get("authorizationProvider1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineAuthorizationProvidersKey, machineAuthorizationProvider2Key));
        }

        [TestMethod]
        public void AuthorizationProviderWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(AuthorizationProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            AuthorizationProviderData authorizationProvider1Data = new AuthorizationProviderData("authorizationProvider1", typeof(Object));
            section.AuthorizationProviders.Add(authorizationProvider1Data);
            AuthorizationProviderData authorizationProvider2Data = new AuthorizationProviderData("authorizationProvider2", typeof(Object));
            section.AuthorizationProviders.Add(authorizationProvider2Data);

            MockRegistryKey machineAuthorizationProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(SecuritySettingsManageabilityProvider.AuthorizationProvidersKeyName, machineAuthorizationProvidersKey);
            MockRegistryKey machineAuthorizationProvider2Key = new MockRegistryKey(false);
            machineAuthorizationProvidersKey.AddSubKey("authorizationProvider2", machineAuthorizationProvider2Key);
            machineAuthorizationProvider2Key.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPolicies(section, false, machineKey, userKey);

            Assert.AreEqual(2, section.AuthorizationProviders.Count);
            Assert.IsNotNull(section.AuthorizationProviders.Get("authorizationProvider1"));
            Assert.IsNotNull(section.AuthorizationProviders.Get("authorizationProvider2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineAuthorizationProvidersKey, machineAuthorizationProvider2Key));
        }

        [TestMethod]
        public void SecurityCacheProviderWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(SecurityCacheProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            SecurityCacheProviderData securityCacheProvider1Data = new SecurityCacheProviderData("securityCacheProvider1", typeof(Object));
            section.SecurityCacheProviders.Add(securityCacheProvider1Data);
            SecurityCacheProviderData securityCacheProvider2Data = new SecurityCacheProviderData("securityCacheProvider2", typeof(Object));
            section.SecurityCacheProviders.Add(securityCacheProvider2Data);

            MockRegistryKey machineSecurityCacheProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(SecuritySettingsManageabilityProvider.SecurityCacheProvidersKeyName, machineSecurityCacheProvidersKey);
            MockRegistryKey machineSecurityCacheProvider2Key = new MockRegistryKey(false);
            machineSecurityCacheProvidersKey.AddSubKey("securityCacheProvider2", machineSecurityCacheProvider2Key);
            machineSecurityCacheProvider2Key.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual(1, section.SecurityCacheProviders.Count);
            Assert.IsNotNull(section.SecurityCacheProviders.Get("securityCacheProvider1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineSecurityCacheProvidersKey, machineSecurityCacheProvider2Key));
        }

        [TestMethod]
        public void SecurityCacheProviderWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(SecurityCacheProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            SecurityCacheProviderData securityCacheProvider1Data = new SecurityCacheProviderData("securityCacheProvider1", typeof(Object));
            section.SecurityCacheProviders.Add(securityCacheProvider1Data);
            SecurityCacheProviderData securityCacheProvider2Data = new SecurityCacheProviderData("securityCacheProvider2", typeof(Object));
            section.SecurityCacheProviders.Add(securityCacheProvider2Data);

            MockRegistryKey machineSecurityCacheProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(SecuritySettingsManageabilityProvider.SecurityCacheProvidersKeyName, machineSecurityCacheProvidersKey);
            MockRegistryKey machineSecurityCacheProvider2Key = new MockRegistryKey(false);
            machineSecurityCacheProvidersKey.AddSubKey("securityCacheProvider2", machineSecurityCacheProvider2Key);
            machineSecurityCacheProvider2Key.AddBooleanValue(SecuritySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPolicies(section, false, machineKey, userKey);

            Assert.AreEqual(2, section.SecurityCacheProviders.Count);
            Assert.IsNotNull(section.SecurityCacheProviders.Get("securityCacheProvider1"));
            Assert.IsNotNull(section.SecurityCacheProviders.Get("securityCacheProvider2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineSecurityCacheProvidersKey, machineSecurityCacheProvider2Key));
        }

        [TestMethod]
        public void RegisteredAuthorizationProviderDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(AuthorizationRuleProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            AuthorizationRuleProviderData authorizationProviderData = new AuthorizationRuleProviderData("authorizationProvider1");
            section.AuthorizationProviders.Add(authorizationProviderData);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(authorizationProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredAuthorizationProviderDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(AuthorizationRuleProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            AuthorizationRuleProviderData authorizationProviderData = new AuthorizationRuleProviderData("authorizationProvider1");
            section.AuthorizationProviders.Add(authorizationProviderData);

            MockRegistryKey machineauthorizationProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(SecuritySettingsManageabilityProvider.AuthorizationProvidersKeyName, machineauthorizationProvidersKey);
            MockRegistryKey machineauthorizationProviderKey = new MockRegistryKey(false);
            machineauthorizationProvidersKey.AddSubKey("authorizationProvider1", machineauthorizationProviderKey);
            MockRegistryKey machineOtherauthorizationProviderKey = new MockRegistryKey(false);
            machineauthorizationProvidersKey.AddSubKey("authorizationProvider2", machineOtherauthorizationProviderKey);

            MockRegistryKey userauthorizationProvidersKey = new MockRegistryKey(false);
            userKey.AddSubKey(SecuritySettingsManageabilityProvider.AuthorizationProvidersKeyName, userauthorizationProvidersKey);
            MockRegistryKey userauthorizationProviderKey = new MockRegistryKey(false);
            userauthorizationProvidersKey.AddSubKey("authorizationProvider1", userauthorizationProviderKey);
            MockRegistryKey userOtherauthorizationProviderKey = new MockRegistryKey(false);
            userauthorizationProvidersKey.AddSubKey("authorizationProvider2", userOtherauthorizationProviderKey);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(authorizationProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machineauthorizationProviderKey, registeredProvider.machineKey);
            Assert.AreSame(userauthorizationProviderKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineauthorizationProvidersKey, machineauthorizationProviderKey, machineOtherauthorizationProviderKey,
                                               userauthorizationProvidersKey, userauthorizationProviderKey, userOtherauthorizationProviderKey));
        }

        [TestMethod]
        public void RegisteredSecurityCacheProviderDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(CustomSecurityCacheProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            CustomSecurityCacheProviderData securitycacheProviderData = new CustomSecurityCacheProviderData("securitycacheProvider1", typeof(Object));
            section.SecurityCacheProviders.Add(securitycacheProviderData);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(securitycacheProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredSecurityCacheProviderDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(CustomSecurityCacheProviderData), registeredProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            CustomSecurityCacheProviderData securitycacheProviderData = new CustomSecurityCacheProviderData("securitycacheProvider1", typeof(Object));
            section.SecurityCacheProviders.Add(securitycacheProviderData);

            MockRegistryKey machinesecuritycacheProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(SecuritySettingsManageabilityProvider.SecurityCacheProvidersKeyName, machinesecuritycacheProvidersKey);
            MockRegistryKey machinesecuritycacheProviderKey = new MockRegistryKey(false);
            machinesecuritycacheProvidersKey.AddSubKey("securitycacheProvider1", machinesecuritycacheProviderKey);
            MockRegistryKey machineOthersecuritycacheProviderKey = new MockRegistryKey(false);
            machinesecuritycacheProvidersKey.AddSubKey("securitycacheProvider2", machineOthersecuritycacheProviderKey);

            MockRegistryKey usersecuritycacheProvidersKey = new MockRegistryKey(false);
            userKey.AddSubKey(SecuritySettingsManageabilityProvider.SecurityCacheProvidersKeyName, usersecuritycacheProvidersKey);
            MockRegistryKey usersecuritycacheProviderKey = new MockRegistryKey(false);
            usersecuritycacheProvidersKey.AddSubKey("securitycacheProvider1", usersecuritycacheProviderKey);
            MockRegistryKey userOthersecuritycacheProviderKey = new MockRegistryKey(false);
            usersecuritycacheProvidersKey.AddSubKey("securitycacheProvider2", userOthersecuritycacheProviderKey);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(securitycacheProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machinesecuritycacheProviderKey, registeredProvider.machineKey);
            Assert.AreSame(usersecuritycacheProviderKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machinesecuritycacheProvidersKey, machinesecuritycacheProviderKey, machineOthersecuritycacheProviderKey,
                                               usersecuritycacheProvidersKey, usersecuritycacheProviderKey, userOthersecuritycacheProviderKey));
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(SecuritySettings.SectionName, section);

            AuthorizationProviderData authorization = new AuthorizationProviderData("authorization", typeof(object));
            SecurityCacheProviderData security = new SecurityCacheProviderData("security", typeof(object));
            section.AuthorizationProviders.Add(authorization);
            section.SecurityCacheProviders.Add(security);

            MockConfigurationElementManageabilityProvider subProvider = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(AuthorizationProviderData), subProvider);
            subProviders.Add(typeof(SecurityCacheProviderData), subProvider);
            provider = new SecuritySettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            Assert.AreEqual(2, subProvider.configurationObjects.Count);
            Assert.AreSame(authorization, subProvider.configurationObjects[0]);
            Assert.AreSame(security, subProvider.configurationObjects[1]);
            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> sectionPartsEnumerator = sectionPoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(SecuritySettingsManageabilityProvider.DefaultAuthorizationProviderPropertyName,
                            ((AdmDropDownListPart)sectionPartsEnumerator.Current).ValueName);
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(SecuritySettingsManageabilityProvider.DefaultSecurityCacheProviderPropertyName,
                            ((AdmDropDownListPart)sectionPartsEnumerator.Current).ValueName);
            Assert.IsFalse(sectionPartsEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }
    }
}
