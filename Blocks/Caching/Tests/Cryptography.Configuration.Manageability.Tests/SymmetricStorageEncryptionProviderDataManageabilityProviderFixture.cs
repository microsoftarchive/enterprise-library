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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Manageability.Tests
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderDataManageabilityProviderFixture
    {
        SymmetricStorageEncryptionProviderDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        SymmetricStorageEncryptionProviderData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new SymmetricStorageEncryptionProviderDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new SymmetricStorageEncryptionProviderData();
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

            Assembly assembly = typeof(SymmetricStorageEncryptionProviderDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(SymmetricStorageEncryptionProviderDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CacheManagerSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(SymmetricStorageEncryptionProviderData), selectedAttribute.TargetType);
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
            configurationObject.SymmetricInstance = "symmetric instance";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("symmetric instance", configurationObject.SymmetricInstance);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.SymmetricInstance = "symmetric instance";

            machineKey.AddStringValue(SymmetricStorageEncryptionProviderDataManageabilityProvider.SymmetricInstancePropertyName, "machine symmetric instance");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("machine symmetric instance", configurationObject.SymmetricInstance);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.SymmetricInstance = "symmetric instance";

            userKey.AddStringValue(SymmetricStorageEncryptionProviderDataManageabilityProvider.SymmetricInstancePropertyName, "user symmetric instance");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("user symmetric instance", configurationObject.SymmetricInstance);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.SymmetricInstance = "symmetric instance";

            machineKey.AddStringValue(SymmetricStorageEncryptionProviderDataManageabilityProvider.SymmetricInstancePropertyName, "machine symmetric instance");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("symmetric instance", configurationObject.SymmetricInstance);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.SymmetricInstance = "symmetric instance";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.SymmetricInstance = "symmetric instance";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(SymmetricStorageEncryptionProviderSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((SymmetricStorageEncryptionProviderSetting)wmiSettings[0]).Name);
            Assert.AreEqual("symmetric instance", ((SymmetricStorageEncryptionProviderSetting)wmiSettings[0]).SymmetricInstance);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.SymmetricInstance = "symmetric instance";

            machineKey.AddStringValue(SymmetricStorageEncryptionProviderDataManageabilityProvider.SymmetricInstancePropertyName, "machine symmetric instance");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(SymmetricStorageEncryptionProviderSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((SymmetricStorageEncryptionProviderSetting)wmiSettings[0]).Name);
            Assert.AreEqual("machine symmetric instance", ((SymmetricStorageEncryptionProviderSetting)wmiSettings[0]).SymmetricInstance);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            CryptographySettings cryptographySettings = new CryptographySettings();
            configurationSource.Add("securityCryptographyConfiguration", cryptographySettings);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

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
            Assert.AreEqual(SymmetricStorageEncryptionProviderDataManageabilityProvider.SymmetricInstancePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
