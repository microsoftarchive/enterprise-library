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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Tests
{
    [TestClass]
    public class SymmetricAlgorithmProviderDataManageabilityProviderFixture
    {
        SymmetricAlgorithmProviderDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        SymmetricAlgorithmProviderData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new SymmetricAlgorithmProviderDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new SymmetricAlgorithmProviderData();
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
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

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

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

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

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

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

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("file name", configurationObject.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, configurationObject.ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(SymmetricAlgorithmProviderSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).Name);
            Assert.AreEqual(typeof(DESCryptoServiceProvider).AssemblyQualifiedName, ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).AlgorithmType);
            Assert.AreEqual("file name", ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser.ToString(), ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).ProtectedKeyProtectionScope);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Name = "provider name";
            configurationObject.AlgorithmType = typeof(DESCryptoServiceProvider);
            configurationObject.ProtectedKeyFilename = "file name";
            configurationObject.ProtectedKeyProtectionScope = DataProtectionScope.CurrentUser;

            machineKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyFilenamePropertyName, "machine file name");
            machineKey.AddStringValue(SymmetricAlgorithmProviderDataManageabilityProvider.ProtectedKeyProtectionScopePropertyName, DataProtectionScope.LocalMachine.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(SymmetricAlgorithmProviderSetting), wmiSettings[0].GetType());
            Assert.AreEqual("provider name", ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).Name);
            Assert.AreEqual(typeof(DESCryptoServiceProvider).AssemblyQualifiedName, ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).AlgorithmType);
            Assert.AreEqual("machine file name", ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.LocalMachine.ToString(), ((SymmetricAlgorithmProviderSetting)wmiSettings[0]).ProtectedKeyProtectionScope);
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