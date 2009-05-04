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
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Tests
{
    [TestClass]
    public class CryptographySettingsManageabilityProviderFixture
    {
        CryptographySettingsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        CryptographySettings section;
        DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            provider = new CryptographySettingsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new CryptographySettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add("securityCryptographyConfiguration", section);
        }

        [TestCleanup]
        public void TearDown()
        {
            // preventive unregister to work around WMI.NET 2.0 issues with appdomain unloading
            ManagementEntityTypesRegistrar.UnregisterAll();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(CryptographySettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals("securityCryptographyConfiguration"))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CryptographySettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void SectionIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("default hash", section.DefaultHashProviderName);
            Assert.AreEqual("default symmetric", section.DefaultSymmetricCryptoProviderName);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfMachineKeyIsNull()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfUserKeyIsNull()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, true, wmiSettings);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromMachineKey()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            machineKey.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultHashProviderPropertyName, "machine hash");
            machineKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultSymmetricCryptoProviderPropertyName, "machine symmetric");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("machine hash", section.DefaultHashProviderName);
            Assert.AreEqual("machine symmetric", section.DefaultSymmetricCryptoProviderName);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromUserKey()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            userKey.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultHashProviderPropertyName, "user hash");
            userKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultSymmetricCryptoProviderPropertyName, "user symmetric");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, userKey, userKey, true, wmiSettings);

            Assert.AreEqual("user hash", section.DefaultHashProviderName);
            Assert.AreEqual("user symmetric", section.DefaultSymmetricCryptoProviderName);
        }

        [TestMethod]
        public void DefaultHashProviderIsOverridenWithListItemNone()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            machineKey.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultHashProviderPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultSymmetricCryptoProviderPropertyName, "machine symmetric");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", section.DefaultHashProviderName);
            Assert.AreEqual("machine symmetric", section.DefaultSymmetricCryptoProviderName);
        }

        [TestMethod]
        public void DefaultSymmetricCryptoProviderIsOverridenWithListItemNone()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            machineKey.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultHashProviderPropertyName, "machine hash");
            machineKey.AddStringValue(CryptographySettingsManageabilityProvider.DefaultSymmetricCryptoProviderPropertyName, AdmContentBuilder.NoneListItem);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("machine hash", section.DefaultHashProviderName);
            Assert.AreEqual("", section.DefaultSymmetricCryptoProviderName);
        }

        [TestMethod]
        public void HashProviderWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(HashAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            HashAlgorithmProviderData hashProvider1Data = new HashAlgorithmProviderData("hashProvider1", typeof(Object), false);
            section.HashProviders.Add(hashProvider1Data);
            HashAlgorithmProviderData hashProvider2Data = new HashAlgorithmProviderData("hashProvider2", typeof(Object), false);
            section.HashProviders.Add(hashProvider2Data);

            MockRegistryKey machineHashProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CryptographySettingsManageabilityProvider.HashProvidersKeyName, machineHashProvidersKey);
            MockRegistryKey machineHashProvider2Key = new MockRegistryKey(false);
            machineHashProvidersKey.AddSubKey("hashProvider2", machineHashProvider2Key);
            machineHashProvider2Key.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.HashProviders.Count);
            Assert.IsNotNull(section.HashProviders.Get("hashProvider1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineHashProvidersKey, machineHashProvider2Key));
        }

        [TestMethod]
        public void HashProviderWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(HashAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            HashAlgorithmProviderData hashProvider1Data = new HashAlgorithmProviderData("hashProvider1", typeof(Object), false);
            section.HashProviders.Add(hashProvider1Data);
            HashAlgorithmProviderData hashProvider2Data = new HashAlgorithmProviderData("hashProvider2", typeof(Object), false);
            section.HashProviders.Add(hashProvider2Data);

            MockRegistryKey machineHashProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CryptographySettingsManageabilityProvider.HashProvidersKeyName, machineHashProvidersKey);
            MockRegistryKey machineHashProvider2Key = new MockRegistryKey(false);
            machineHashProvidersKey.AddSubKey("hashProvider2", machineHashProvider2Key);
            machineHashProvider2Key.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.HashProviders.Count);
            Assert.IsNotNull(section.HashProviders.Get("hashProvider1"));
            Assert.IsNotNull(section.HashProviders.Get("hashProvider2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineHashProvidersKey, machineHashProvider2Key));
        }

        [TestMethod]
        public void SymmetricCryptoProviderWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(SymmetricAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            SymmetricAlgorithmProviderData symmetricCryptoProvider1Data
                = new SymmetricAlgorithmProviderData("symmetricCryptoProvider1", typeof(Object), "key", DataProtectionScope.CurrentUser);
            section.SymmetricCryptoProviders.Add(symmetricCryptoProvider1Data);
            SymmetricAlgorithmProviderData symmetricCryptoProvider2Data
                = new SymmetricAlgorithmProviderData("symmetricCryptoProvider2", typeof(Object), "key", DataProtectionScope.CurrentUser);
            section.SymmetricCryptoProviders.Add(symmetricCryptoProvider2Data);

            MockRegistryKey machineSymmetricCryptoProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CryptographySettingsManageabilityProvider.SymmetricCryptoProvidersKeyName, machineSymmetricCryptoProvidersKey);
            MockRegistryKey machineSymmetricCryptoProvider2Key = new MockRegistryKey(false);
            machineSymmetricCryptoProvidersKey.AddSubKey("symmetricCryptoProvider2", machineSymmetricCryptoProvider2Key);
            machineSymmetricCryptoProvider2Key.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.SymmetricCryptoProviders.Count);
            Assert.IsNotNull(section.SymmetricCryptoProviders.Get("symmetricCryptoProvider1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineSymmetricCryptoProvidersKey, machineSymmetricCryptoProvider2Key));
        }

        [TestMethod]
        public void SymmetricCryptoProviderWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(SymmetricAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            SymmetricAlgorithmProviderData symmetricCryptoProvider1Data
                = new SymmetricAlgorithmProviderData("symmetricCryptoProvider1", typeof(Object), "key", DataProtectionScope.CurrentUser);
            section.SymmetricCryptoProviders.Add(symmetricCryptoProvider1Data);
            SymmetricAlgorithmProviderData symmetricCryptoProvider2Data
                = new SymmetricAlgorithmProviderData("symmetricCryptoProvider2", typeof(Object), "key", DataProtectionScope.CurrentUser);
            section.SymmetricCryptoProviders.Add(symmetricCryptoProvider2Data);

            MockRegistryKey machineSymmetricCryptoProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CryptographySettingsManageabilityProvider.SymmetricCryptoProvidersKeyName, machineSymmetricCryptoProvidersKey);
            MockRegistryKey machineSymmetricCryptoProvider2Key = new MockRegistryKey(false);
            machineSymmetricCryptoProvidersKey.AddSubKey("symmetricCryptoProvider2", machineSymmetricCryptoProvider2Key);
            machineSymmetricCryptoProvider2Key.AddBooleanValue(CryptographySettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.SymmetricCryptoProviders.Count);
            Assert.IsNotNull(section.SymmetricCryptoProviders.Get("symmetricCryptoProvider1"));
            Assert.IsNotNull(section.SymmetricCryptoProviders.Get("symmetricCryptoProvider2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineSymmetricCryptoProvidersKey, machineSymmetricCryptoProvider2Key));
        }

        [TestMethod]
        public void RegisteredHashProviderDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(HashAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            HashAlgorithmProviderData hashProviderData = new HashAlgorithmProviderData("hashProvider1", typeof(Object), false);
            section.HashProviders.Add(hashProviderData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(hashProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredHashProviderDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(HashAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            HashAlgorithmProviderData hashProviderData = new HashAlgorithmProviderData("hashProvider1", typeof(Object), false);
            section.HashProviders.Add(hashProviderData);

            MockRegistryKey machineHashProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CryptographySettingsManageabilityProvider.HashProvidersKeyName, machineHashProvidersKey);
            MockRegistryKey machineHashProviderKey = new MockRegistryKey(false);
            machineHashProvidersKey.AddSubKey("hashProvider1", machineHashProviderKey);
            MockRegistryKey machineOtherhashProviderKey = new MockRegistryKey(false);
            machineHashProvidersKey.AddSubKey("hashProvider2", machineOtherhashProviderKey);

            MockRegistryKey userhashProvidersKey = new MockRegistryKey(false);
            userKey.AddSubKey(CryptographySettingsManageabilityProvider.HashProvidersKeyName, userhashProvidersKey);
            MockRegistryKey userhashProviderKey = new MockRegistryKey(false);
            userhashProvidersKey.AddSubKey("hashProvider1", userhashProviderKey);
            MockRegistryKey userOtherhashProviderKey = new MockRegistryKey(false);
            userhashProvidersKey.AddSubKey("hashProvider2", userOtherhashProviderKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(hashProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machineHashProviderKey, registeredProvider.machineKey);
            Assert.AreSame(userhashProviderKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineHashProvidersKey, machineHashProviderKey, machineOtherhashProviderKey,
                                               userhashProvidersKey, userhashProviderKey, userOtherhashProviderKey));
        }

        [TestMethod]
        public void RegisteredSymmetricCryptoProviderDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(SymmetricAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            SymmetricAlgorithmProviderData symmetricCryptoProviderData = new SymmetricAlgorithmProviderData("symmetricCryptoProvider1", typeof(Object), "key", DataProtectionScope.CurrentUser);
            section.SymmetricCryptoProviders.Add(symmetricCryptoProviderData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(symmetricCryptoProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredSymmetricCryptoProviderDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(SymmetricAlgorithmProviderData), registeredProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            SymmetricAlgorithmProviderData symmetricCryptoProviderData = new SymmetricAlgorithmProviderData("symmetricCryptoProvider1", typeof(Object), "key", DataProtectionScope.CurrentUser);
            section.SymmetricCryptoProviders.Add(symmetricCryptoProviderData);

            MockRegistryKey machinesymmetricCryptoProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CryptographySettingsManageabilityProvider.SymmetricCryptoProvidersKeyName, machinesymmetricCryptoProvidersKey);
            MockRegistryKey machinesymmetricCryptoProviderKey = new MockRegistryKey(false);
            machinesymmetricCryptoProvidersKey.AddSubKey("symmetricCryptoProvider1", machinesymmetricCryptoProviderKey);
            MockRegistryKey machineOthersymmetricCryptoProviderKey = new MockRegistryKey(false);
            machinesymmetricCryptoProvidersKey.AddSubKey("symmetricCryptoProvider2", machineOthersymmetricCryptoProviderKey);

            MockRegistryKey usersymmetricCryptoProvidersKey = new MockRegistryKey(false);
            userKey.AddSubKey(CryptographySettingsManageabilityProvider.SymmetricCryptoProvidersKeyName, usersymmetricCryptoProvidersKey);
            MockRegistryKey usersymmetricCryptoProviderKey = new MockRegistryKey(false);
            usersymmetricCryptoProvidersKey.AddSubKey("symmetricCryptoProvider1", usersymmetricCryptoProviderKey);
            MockRegistryKey userOthersymmetricCryptoProviderKey = new MockRegistryKey(false);
            usersymmetricCryptoProvidersKey.AddSubKey("symmetricCryptoProvider2", userOthersymmetricCryptoProviderKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(symmetricCryptoProviderData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machinesymmetricCryptoProviderKey, registeredProvider.machineKey);
            Assert.AreSame(usersymmetricCryptoProviderKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machinesymmetricCryptoProvidersKey, machinesymmetricCryptoProviderKey, machineOthersymmetricCryptoProviderKey,
                                               usersymmetricCryptoProvidersKey, usersymmetricCryptoProviderKey, userOthersymmetricCryptoProviderKey));
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            section.DefaultHashProviderName = "default hash";
            section.DefaultSymmetricCryptoProviderName = "default symmetric";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CryptographyBlockSetting), wmiSettings[0].GetType());
            Assert.AreEqual("default hash", ((CryptographyBlockSetting)wmiSettings[0]).DefaultHashProvider);
            Assert.AreEqual("default symmetric", ((CryptographyBlockSetting)wmiSettings[0]).DefaultSymmetricCryptoProvider);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            HashProviderData hash = new HashProviderData("hash", typeof(object));
            section.HashProviders.Add(hash);
            SymmetricProviderData symmetric = new SymmetricProviderData("symmetric", typeof(object));
            section.SymmetricCryptoProviders.Add(symmetric);

            MockConfigurationElementManageabilityProvider subProvider = new MockConfigurationElementManageabilityProvider(true, false);
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(HashProviderData), subProvider);
            subProviders.Add(typeof(SymmetricProviderData), subProvider);
            provider = new CryptographySettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            Assert.AreEqual(2, subProvider.configurationObjects.Count);
            Assert.AreSame(hash, subProvider.configurationObjects[0]);
            Assert.AreSame(symmetric, subProvider.configurationObjects[1]);

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();

            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.AreEqual(MockConfigurationElementManageabilityProvider.Policy, policiesEnumerator.Current.Name);
            Assert.IsFalse(policiesEnumerator.MoveNext());

            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.AreEqual(MockConfigurationElementManageabilityProvider.Policy, policiesEnumerator.Current.Name);
            Assert.IsFalse(policiesEnumerator.MoveNext());

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();

            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> sectionPartsEnumerator = sectionPoliciesEnumerator.Current.Parts.GetEnumerator();

            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(CryptographySettingsManageabilityProvider.DefaultHashProviderPropertyName,
                            sectionPartsEnumerator.Current.ValueName);

            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(CryptographySettingsManageabilityProvider.DefaultSymmetricCryptoProviderPropertyName,
                            sectionPartsEnumerator.Current.ValueName);

            Assert.IsFalse(sectionPartsEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }
    }
}
