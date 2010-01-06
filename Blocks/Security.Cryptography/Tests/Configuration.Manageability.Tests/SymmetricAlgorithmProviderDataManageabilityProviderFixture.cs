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
    public class SymmetricAlgorithmProviderDataManageabilityProviderFixture
    {
        SymmetricAlgorithmProviderDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        SymmetricAlgorithmProviderData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new SymmetricAlgorithmProviderDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new SymmetricAlgorithmProviderData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(SymmetricAlgorithmProviderDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(SymmetricAlgorithmProviderDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(CryptographySettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(SymmetricAlgorithmProviderData), selectedAttribute.TargetType);
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
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual("file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            machineKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "machine file name");
            machineKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreEqual("machine file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.LocalMachine, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            userKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "user file name");
            userKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual("user file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.LocalMachine, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            machineKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "machine file name");
            machineKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, userKey);

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
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
