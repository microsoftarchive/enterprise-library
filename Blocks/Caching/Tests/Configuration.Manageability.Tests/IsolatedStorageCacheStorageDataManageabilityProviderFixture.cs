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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Tests
{
    [TestClass]
    public class IsolatedStorageCacheStorageDataManageabilityProviderFixture
    {
        IsolatedStorageCacheStorageDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        IsolatedStorageCacheStorageData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new IsolatedStorageCacheStorageDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new IsolatedStorageCacheStorageData();
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
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(IsolatedStorageCacheStorageDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(IsolatedStorageCacheStorageDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CacheManagerSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(IsolatedStorageCacheStorageData), selectedAttribute.TargetType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            machineKey.AddStringValue(IsolatedStorageCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("machine partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            userKey.AddStringValue(IsolatedStorageCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "user partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("user partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            machineKey.AddStringValue(IsolatedStorageCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(IsolatedStorageCacheStorageSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((IsolatedStorageCacheStorageSetting)wmiSettings[0]).Name);
            Assert.AreEqual("partition", ((IsolatedStorageCacheStorageSetting)wmiSettings[0]).PartitionName);
            Assert.AreEqual("encryption", ((IsolatedStorageCacheStorageSetting)wmiSettings[0]).StorageEncryption);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            machineKey.AddStringValue(IsolatedStorageCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(IsolatedStorageCacheStorageSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((IsolatedStorageCacheStorageSetting)wmiSettings[0]).Name);
            Assert.AreEqual("machine partition", ((IsolatedStorageCacheStorageSetting)wmiSettings[0]).PartitionName);
            Assert.AreEqual("encryption", ((IsolatedStorageCacheStorageSetting)wmiSettings[0]).StorageEncryption);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            configurationObject.Type = typeof(Object);

            contentBuilder.StartCategory("category");
            contentBuilder.StartPolicy("policy", "policy key");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndPolicy();
            contentBuilder.EndCategory();

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(IsolatedStorageCacheStorageDataManageabilityProvider.PartitionNamePropertyName,
                            ((AdmEditTextPart)partsEnumerator.Current).ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}