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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Tests
{
    [TestClass]
    public class CacheManagerSettingsManageabilityProviderFixture
    {
        CacheManagerSettingsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        CacheManagerSettings section;
        DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            provider = new CacheManagerSettingsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new CacheManagerSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, section);
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

            Assembly assembly = typeof(CacheManagerSettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(CacheManagerSettings.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CacheManagerSettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
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
            section.DefaultCacheManager = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("default", section.DefaultCacheManager);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfMachineKeyIsNull()
        {
            section.DefaultCacheManager = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfUserKeyIsNull()
        {
            section.DefaultCacheManager = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, true, wmiSettings);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromMachineKey()
        {
            section.DefaultCacheManager = "default";

            machineKey.AddBooleanValue(CacheManagerSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(CacheManagerSettingsManageabilityProvider.DefaultCacheManagerPropertyName, "machineOverridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("machineOverridenDefault", section.DefaultCacheManager);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromUserKey()
        {
            section.DefaultCacheManager = "default";

            userKey.AddBooleanValue(CacheManagerSettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(CacheManagerSettingsManageabilityProvider.DefaultCacheManagerPropertyName, "userOverridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("userOverridenDefault", section.DefaultCacheManager);
        }

        [TestMethod]
        public void SectionPropertiesAreNotOverridenIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            section.DefaultCacheManager = "default";

            machineKey.AddBooleanValue(CacheManagerSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(CacheManagerSettingsManageabilityProvider.DefaultCacheManagerPropertyName, "machineOverridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("default", section.DefaultCacheManager);
        }

        [TestMethod]
        public void CacheManagerIsNotOverridenIfThereAreNoPolicyOverrides()
        {
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            data1.CacheStorage = "cache storage";
            data1.ExpirationPollFrequencyInSeconds = 100;
            data1.MaximumElementsInCacheBeforeScavenging = 200;
            data1.NumberToRemoveWhenScavenging = 300;
            section.CacheManagers.Add(data1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("cache storage", data1.CacheStorage);
            Assert.AreEqual(100, data1.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(200, data1.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(300, data1.NumberToRemoveWhenScavenging);
        }

        [TestMethod]
        public void CustomCacheManagerIsNotOverridenIfThereAreNoPolicyOverrides()
        {
            CustomCacheManagerData data1 = new CustomCacheManagerData();
            data1.Name = "cache manager 1";
            data1.Type = typeof(object);
            data1.Attributes.Add("key1", "value1");
            data1.Attributes.Add("key2", "value2");

            section.CacheManagers.Add(data1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("value1", data1.Attributes["key1"]);
            Assert.AreEqual("value2", data1.Attributes["key2"]);
        }

        [TestMethod]
        public void CacheManagerIsOverridenIfThereAreMachinePolicyOverrides()
        {
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            data1.CacheStorage = "cache storage";
            data1.ExpirationPollFrequencyInSeconds = 100;
            data1.MaximumElementsInCacheBeforeScavenging = 200;
            data1.NumberToRemoveWhenScavenging = 300;
            section.CacheManagers.Add(data1);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey(data1.Name, machineCacheManager1Key);
            machineCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerExpirationPollFrequencyInSecondsPropertyName, 150);
            machineCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerMaximumElementsInCacheBeforeScavengingPropertyName, 250);
            machineCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerNumberToRemoveWhenScavengingPropertyName, 350);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("cache storage", data1.CacheStorage);
            Assert.AreEqual(150, data1.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(250, data1.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(350, data1.NumberToRemoveWhenScavenging);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void CacheManagerIsOverridenIfThereAreUserPolicyOverrides()
        {
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            data1.CacheStorage = "cache storage";
            data1.ExpirationPollFrequencyInSeconds = 100;
            data1.MaximumElementsInCacheBeforeScavenging = 200;
            data1.NumberToRemoveWhenScavenging = 300;
            section.CacheManagers.Add(data1);

            MockRegistryKey userCacheManagersKey = new MockRegistryKey(false);
            userKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, userCacheManagersKey);
            MockRegistryKey userCacheManager1Key = new MockRegistryKey(false);
            userCacheManagersKey.AddSubKey(data1.Name, userCacheManager1Key);
            userCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerExpirationPollFrequencyInSecondsPropertyName, 160);
            userCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerMaximumElementsInCacheBeforeScavengingPropertyName, 260);
            userCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerNumberToRemoveWhenScavengingPropertyName, 360);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("cache storage", data1.CacheStorage);
            Assert.AreEqual(160, data1.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(260, data1.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(360, data1.NumberToRemoveWhenScavenging);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(userCacheManagersKey, userCacheManager1Key));
        }

        [TestMethod]
        public void CacheManagerIsNotOverridenIfThereArePolicyOverridesForDifferentName()
        {
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            data1.CacheStorage = "cache storage";
            data1.ExpirationPollFrequencyInSeconds = 100;
            data1.MaximumElementsInCacheBeforeScavenging = 200;
            data1.NumberToRemoveWhenScavenging = 300;
            section.CacheManagers.Add(data1);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey("cache manager 2", machineCacheManager1Key);
            machineCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerExpirationPollFrequencyInSecondsPropertyName, 150);
            machineCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerMaximumElementsInCacheBeforeScavengingPropertyName, 250);
            machineCacheManager1Key.AddIntValue(CacheManagerSettingsManageabilityProvider.CacheManagerNumberToRemoveWhenScavengingPropertyName, 350);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("cache storage", data1.CacheStorage);
            Assert.AreEqual(100, data1.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(200, data1.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(300, data1.NumberToRemoveWhenScavenging);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void CacheManagerWithDisabledPolicyIsRemoved()
        {
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            section.CacheManagers.Add(data1);
            CacheManagerData data2 = new CacheManagerData();
            data2.Name = "cache manager 2";
            section.CacheManagers.Add(data2);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey("cache manager 1", machineCacheManager1Key);
            machineCacheManager1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.CacheManagers.Count);
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void UnknowCacheManagerWithDisabledPolicyIsRemoved()
        {
            MockCacheManagerData data1 = new MockCacheManagerData();
            data1.Name = "cache manager 1";
            data1.Type = typeof(object);
            section.CacheManagers.Add(data1);
            MockCacheManagerData data2 = new MockCacheManagerData();
            data2.Name = "cache manager 2";
            data2.Type = typeof(object);
            section.CacheManagers.Add(data2);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey("cache manager 1", machineCacheManager1Key);
            machineCacheManager1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.CacheManagers.Count);
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void CustomCacheManagerWithDisabledPolicyIsRemoved()
        {
            CustomCacheManagerData data1 = new CustomCacheManagerData();
            data1.Name = "cache manager 1";
            data1.Type = typeof(object);
            data1.Attributes.Add("key1", "value1");

            section.CacheManagers.Add(data1);

            CustomCacheManagerData data2 = new CustomCacheManagerData();
            data2.Name = "cache manager 2";
            data2.Type = typeof(object);
            data2.Attributes.Add("key11", "value11");

            section.CacheManagers.Add(data2);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey("cache manager 1", machineCacheManager1Key);
            machineCacheManager1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.CacheManagers.Count);
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void CacheManagerWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            section.CacheManagers.Add(data1);
            CacheManagerData data2 = new CacheManagerData();
            data2.Name = "cache manager 2";
            section.CacheManagers.Add(data2);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey("cache manager 1", machineCacheManager1Key);
            machineCacheManager1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.CacheManagers.Count);
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 1"));
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void UnknownCacheManagerWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockCacheManagerData data1 = new MockCacheManagerData();
            data1.Name = "cache manager 1";
            data1.Type = typeof(object);
            section.CacheManagers.Add(data1);
            MockCacheManagerData data2 = new MockCacheManagerData();
            data2.Name = "cache manager 2";
            data2.Type = typeof(object);
            section.CacheManagers.Add(data2);

            MockRegistryKey machineCacheManagersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.CacheManagersKeyName, machineCacheManagersKey);
            MockRegistryKey machineCacheManager1Key = new MockRegistryKey(false);
            machineCacheManagersKey.AddSubKey("cache manager 1", machineCacheManager1Key);
            machineCacheManager1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.CacheManagers.Count);
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 1"));
            Assert.IsNotNull(section.CacheManagers.Get("cache manager 2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCacheManagersKey, machineCacheManager1Key));
        }

        [TestMethod]
        public void RegisteredCacheStorageDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(CacheStorageData), registeredProvider);
            provider = new CacheManagerSettingsManageabilityProvider(subProviders);

            CacheStorageData data = new CacheStorageData("store1", typeof(NullBackingStore));
            section.BackingStores.Add(data);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(data, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredCacheStorageDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(CacheStorageData), registeredProvider);
            provider = new CacheManagerSettingsManageabilityProvider(subProviders);

            CacheStorageData data = new CacheStorageData("store1", typeof(NullBackingStore));
            section.BackingStores.Add(data);

            MockRegistryKey machineStoresKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.BackingStoresKeyName, machineStoresKey);
            MockRegistryKey machineStoreKey = new MockRegistryKey(false);
            machineStoresKey.AddSubKey("store1", machineStoreKey);
            MockRegistryKey machineOtherStoreKey = new MockRegistryKey(false);
            machineStoresKey.AddSubKey("store2", machineOtherStoreKey);

            MockRegistryKey userStoresKey = new MockRegistryKey(false);
            userKey.AddSubKey(CacheManagerSettingsManageabilityProvider.BackingStoresKeyName, userStoresKey);
            MockRegistryKey userStoreKey = new MockRegistryKey(false);
            userStoresKey.AddSubKey("store1", userStoreKey);
            MockRegistryKey userOtherStoreKey = new MockRegistryKey(false);
            userStoresKey.AddSubKey("store2", userOtherStoreKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(data, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(machineStoreKey, registeredProvider.machineKey);
            Assert.AreEqual(userStoreKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineStoresKey, machineStoreKey, machineOtherStoreKey,
                                               userStoresKey, userStoreKey, userOtherStoreKey));
        }

        [TestMethod]
        public void RegisteredEncryptionProviderDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(StorageEncryptionProviderData), registeredProvider);
            provider = new CacheManagerSettingsManageabilityProvider(subProviders);

            StorageEncryptionProviderData data = new StorageEncryptionProviderData("encryptionprovider1", typeof(Object));
            section.EncryptionProviders.Add(data);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(data, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredEncryptionProviderDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(StorageEncryptionProviderData), registeredProvider);
            provider = new CacheManagerSettingsManageabilityProvider(subProviders);

            StorageEncryptionProviderData data = new StorageEncryptionProviderData("encryptionprovider1", typeof(Object));
            section.EncryptionProviders.Add(data);

            MockRegistryKey machineEncryptionProvidersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CacheManagerSettingsManageabilityProvider.EncryptionProvidersKeyName, machineEncryptionProvidersKey);
            MockRegistryKey machineEncryptionProviderKey = new MockRegistryKey(false);
            machineEncryptionProvidersKey.AddSubKey("encryptionprovider1", machineEncryptionProviderKey);
            MockRegistryKey machineOtherEncryptionProviderKey = new MockRegistryKey(false);
            machineEncryptionProvidersKey.AddSubKey("encryptionprovider2", machineOtherEncryptionProviderKey);

            MockRegistryKey userEncryptionProvidersKey = new MockRegistryKey(false);
            userKey.AddSubKey(CacheManagerSettingsManageabilityProvider.EncryptionProvidersKeyName, userEncryptionProvidersKey);
            MockRegistryKey userEncryptionProviderKey = new MockRegistryKey(false);
            userEncryptionProvidersKey.AddSubKey("encryptionprovider1", userEncryptionProviderKey);
            MockRegistryKey userOtherEncryptionProviderKey = new MockRegistryKey(false);
            userEncryptionProvidersKey.AddSubKey("encryptionprovider2", userOtherEncryptionProviderKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(data, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(machineEncryptionProviderKey, registeredProvider.machineKey);
            Assert.AreEqual(userEncryptionProviderKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineEncryptionProvidersKey, machineEncryptionProviderKey, machineOtherEncryptionProviderKey,
                                               userEncryptionProvidersKey, userEncryptionProviderKey, userOtherEncryptionProviderKey));
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            section.DefaultCacheManager = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            section.DefaultCacheManager = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CachingBlockSetting), wmiSettings[0].GetType());
            Assert.AreEqual("default", ((CachingBlockSetting)wmiSettings[0]).DefaultCacheManager);
        }

        [TestMethod]
        public void WmiSettingsForCacheManagerAreGeneratedIfWmiIsEnabled()
        {
            section.DefaultCacheManager = "default";
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "cache manager 1";
            data1.CacheStorage = "cache storage";
            data1.ExpirationPollFrequencyInSeconds = 100;
            data1.MaximumElementsInCacheBeforeScavenging = 200;
            data1.NumberToRemoveWhenScavenging = 300;
            section.CacheManagers.Add(data1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, wmiSettings.Count);
            Assert.AreSame(typeof(CachingBlockSetting), wmiSettings[0].GetType());
            Assert.AreEqual("default", ((CachingBlockSetting)wmiSettings[0]).DefaultCacheManager);
            Assert.AreSame(typeof(CacheManagerSetting), wmiSettings[1].GetType());
            Assert.AreEqual("cache manager 1", ((CacheManagerSetting)wmiSettings[1]).Name);
            Assert.AreEqual("cache storage", ((CacheManagerSetting)wmiSettings[1]).CacheStorage);
            Assert.AreEqual(100, ((CacheManagerSetting)wmiSettings[1]).ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(200, ((CacheManagerSetting)wmiSettings[1]).MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(300, ((CacheManagerSetting)wmiSettings[1]).NumberToRemoveWhenScavenging);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            section.DefaultCacheManager = "default";

            machineKey.AddBooleanValue(CacheManagerSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(CacheManagerSettingsManageabilityProvider.DefaultCacheManagerPropertyName, "machineOverridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CachingBlockSetting), wmiSettings[0].GetType());
            Assert.AreEqual("machineOverridenDefault", ((CachingBlockSetting)wmiSettings[0]).DefaultCacheManager);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, section);

            IsolatedStorageCacheStorageData storage1, storage2;
            CacheManagerData manager1, manager2;
            StorageEncryptionProviderData encryption1;
            section.BackingStores.Add(storage1 = new IsolatedStorageCacheStorageData("storage1", "encryption1", "partition1"));
            section.CacheManagers.Add(manager1 = new CacheManagerData("manager1", 100, 200, 300, "storage1"));
            section.EncryptionProviders.Add(encryption1 = new StorageEncryptionProviderData("encryption1", typeof(object)));
            section.BackingStores.Add(storage2 = new IsolatedStorageCacheStorageData("storage2", "", "partition2"));
            section.CacheManagers.Add(manager2 = new CacheManagerData("manager2", 100, 200, 300, "storage2"));

            MockConfigurationElementManageabilityProvider subProvider = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(IsolatedStorageCacheStorageData), subProvider);
            subProviders.Add(typeof(StorageEncryptionProviderData), subProvider);
            provider = new CacheManagerSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            Assert.AreEqual(3, subProvider.configurationObjects.Count);
            Assert.AreSame(storage1, subProvider.configurationObjects[0]);
            Assert.AreSame(encryption1, subProvider.configurationObjects[1]);
            Assert.AreSame(storage2, subProvider.configurationObjects[2]);
            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> cacheManagerPoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsTrue(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsFalse(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContentWithCustomCacheManager()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, section);

            CustomCacheManagerData manager1, manager2;
            section.CacheManagers.Add(manager1 = new CustomCacheManagerData("manager1", typeof(object)));
            section.CacheManagers.Add(manager2 = new CustomCacheManagerData("manager2", typeof(object)));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> cacheManagerPoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsTrue(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsFalse(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContentWithUnknownCacheManager()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, section);

            MockCacheManagerData manager1, manager2;
            section.CacheManagers.Add(manager1 = new MockCacheManagerData("manager1", typeof(object)));
            section.CacheManagers.Add(manager2 = new MockCacheManagerData("manager2", typeof(object)));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> cacheManagerPoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsTrue(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsFalse(cacheManagerPoliciesEnumerator.MoveNext());
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }
    }
}