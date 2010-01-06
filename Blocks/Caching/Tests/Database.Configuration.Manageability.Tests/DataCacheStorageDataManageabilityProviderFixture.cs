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
        DataCacheStorageData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new DataCacheStorageDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new DataCacheStorageData();
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
            provider.OverrideWithGroupPolicies(new TestsConfigurationSection(), true, machineKey, userKey);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            configurationObject.DatabaseInstanceName = "instance";
            configurationObject.PartitionName = "partition";
            configurationObject.StorageEncryption = "encryption";

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

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

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

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

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

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

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, null);

            Assert.AreEqual("instance", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("partition", configurationObject.PartitionName);
            Assert.AreEqual("encryption", configurationObject.StorageEncryption);
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
