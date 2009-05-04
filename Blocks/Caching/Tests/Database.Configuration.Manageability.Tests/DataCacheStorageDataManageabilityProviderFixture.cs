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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Manageability.Tests
{
    [TestClass]
    public class DataCacheStorageDataManageabilityProviderFixture
    {
        DataCacheStorageDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        DataCacheStorageData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new DataCacheStorageDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new DataCacheStorageData();
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

            Assembly assembly = typeof(DataCacheStorageDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(DataCacheStorageDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CacheManagerSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(DataCacheStorageData), selectedAttribute.TargetType);
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
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("instance", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            machineKey.AddStringValue(DataCacheStorageDataManageabilityProvider.DatabaseInstanceNamePropertyName, "machine instance");
            machineKey.AddStringValue(DataCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("machine instance", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("machine partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            userKey.AddStringValue(DataCacheStorageDataManageabilityProvider.DatabaseInstanceNamePropertyName, "machine instance");
            userKey.AddStringValue(DataCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("machine instance", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("machine partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            machineKey.AddStringValue(DataCacheStorageDataManageabilityProvider.DatabaseInstanceNamePropertyName, "machine instance");
            machineKey.AddStringValue(DataCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("instance", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(DataCacheStorageSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((DataCacheStorageSetting)wmiSettings[0]).Name);
            Assert.AreEqual("instance", ((DataCacheStorageSetting)wmiSettings[0]).DatabaseInstanceName);
            Assert.AreEqual("partition", ((DataCacheStorageSetting)wmiSettings[0]).PartitionName);
            Assert.AreEqual("encryption", ((DataCacheStorageSetting)wmiSettings[0]).StorageEncryption);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            machineKey.AddStringValue(DataCacheStorageDataManageabilityProvider.DatabaseInstanceNamePropertyName, "machine instance");
            machineKey.AddStringValue(DataCacheStorageDataManageabilityProvider.PartitionNamePropertyName, "machine partition");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(DataCacheStorageSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((DataCacheStorageSetting)wmiSettings[0]).Name);
            Assert.AreEqual("machine instance", ((DataCacheStorageSetting)wmiSettings[0]).DatabaseInstanceName);
            Assert.AreEqual("machine partition", ((DataCacheStorageSetting)wmiSettings[0]).PartitionName);
            Assert.AreEqual("encryption", ((DataCacheStorageSetting)wmiSettings[0]).StorageEncryption);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ConnectionStringsSection connectionStringsSection = new ConnectionStringsSection();
            configurationSource.Add("connectionStrings", connectionStringsSection);
            connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings("cs1", "cs1"));

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
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(DataCacheStorageDataManageabilityProvider.DatabaseInstanceNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(DataCacheStorageDataManageabilityProvider.PartitionNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
