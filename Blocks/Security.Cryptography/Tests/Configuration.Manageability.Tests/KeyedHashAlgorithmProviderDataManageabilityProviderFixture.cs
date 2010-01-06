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
    public class KeyedHashAlgorithmProviderDataManageabilityProviderFixture
    {
        KeyedHashAlgorithmProviderDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        KeyedHashAlgorithmProviderData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new KeyedHashAlgorithmProviderDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new KeyedHashAlgorithmProviderData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(KeyedHashAlgorithmProviderDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(KeyedHashAlgorithmProviderDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CryptographySettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(KeyedHashAlgorithmProviderData), selectedAttribute.TargetType);
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
            configurationObject.SaltEnabled = true;
            configurationObject.AlgorithmType = typeof(HMACSHA256);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, userKey);

            Assert.AreEqual(true, configurationObject.SaltEnabled);
            Assert.AreEqual("file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.SaltEnabled = true;
            configurationObject.AlgorithmType = typeof(HMACSHA256);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            machineKey.AddBooleanValue(KeyedHashAlgorithmProviderDataManageabilityProvider.SaltEnabledPropertyName, false);
            machineKey.AddStringValue(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "machine file name");
            machineKey.AddStringValue(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, userKey);

            Assert.AreEqual(false, configurationObject.SaltEnabled);
            Assert.AreEqual("machine file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.LocalMachine, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.SaltEnabled = true;
            configurationObject.AlgorithmType = typeof(HMACSHA256);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            userKey.AddBooleanValue(KeyedHashAlgorithmProviderDataManageabilityProvider.SaltEnabledPropertyName, false);
            userKey.AddStringValue(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "user file name");
            userKey.AddStringValue(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual(false, configurationObject.SaltEnabled);
            Assert.AreEqual("user file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.LocalMachine, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.SaltEnabled = true;
            configurationObject.AlgorithmType = typeof(HMACSHA256);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            machineKey.AddBooleanValue(KeyedHashAlgorithmProviderDataManageabilityProvider.SaltEnabledPropertyName, false);
            machineKey.AddStringValue(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "machine file name");
            machineKey.AddStringValue(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, userKey);

            Assert.AreEqual(true, configurationObject.SaltEnabled);
            Assert.AreEqual("file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

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
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(KeyedHashAlgorithmProviderDataManageabilityProvider.SaltEnabledPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(KeyedHashAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
