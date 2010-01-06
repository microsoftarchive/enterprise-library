//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConnectionStringsManageabilityProviderFixture
    {
        ConnectionStringsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        ConnectionStringsSection section;

        [TestInitialize]
        public void SetUp()
        {
            provider = new ConnectionStringsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            section = new ConnectionStringsSection();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPolicies(new TestsConfigurationSection(), true, machineKey, userKey);
        }

        [TestMethod]
        public void ConnectionStringWithoutPolicyOverridesIsNotModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);
        }

        [TestMethod]
        public void ConnectionStringWithNullRegistryKeysOverridesIsNotModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPolicies(section, true, null, null);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);
        }

        [TestMethod]
        public void ConnectionStringWithPolicyOverridesForOtherNameIsNotModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            machineKey.AddSubKey("cs2", overrideKey);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        // machine key overrides
        [TestMethod]
        public void ConnectionStringWithMachinePolicyOverrideIsModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            machineKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("overridenConnectionString", connectionString.ConnectionString);
            Assert.AreEqual("overridenProviderName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        [TestMethod]
        public void ConnectionStringWithMachinePolicyOverrideIsNotModifiedIfGroupPoliciesAreDisabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            machineKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPolicies(section, false, machineKey, userKey);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        // user key overrides
        [TestMethod]
        public void ConnectionStringWithUserPolicyOverrideIsModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            userKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("overridenConnectionString", connectionString.ConnectionString);
            Assert.AreEqual("overridenProviderName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        [TestMethod]
        public void ConnectionStringWithUserPolicyOverrideIsNotModifiedIfGroupPoliciesAreDisabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            userKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPolicies(section, false, machineKey, userKey);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        // mixed key overrides
        [TestMethod]
        public void ConnectionStringMachinePolicyOverrideTakesPrecedenceOverUserPolicyOverride()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey machineOverrideKey = new MockRegistryKey(false);
            machineOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "machineOverridenConnectionString");
            machineOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "machineOverridenProviderName");
            machineKey.AddSubKey("cs1", machineOverrideKey);
            MockRegistryKey userOverrideKey = new MockRegistryKey(false);
            userOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "userOverridenConnectionString");
            userOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "userOverridenProviderName");
            userKey.AddSubKey("cs1", userOverrideKey);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual("machineOverridenConnectionString", connectionString.ConnectionString);
            Assert.AreEqual("machineOverridenProviderName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineOverrideKey, userOverrideKey));
        }

        [TestMethod]
        public void ConnectionStringWithDisabledPolicyIsRemoved()
        {
            ConnectionStringSettings connectionString1 = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString1);
            ConnectionStringSettings connectionString2 = new ConnectionStringSettings("cs2", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString2);

            MockRegistryKey machineConnectionString1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs1", machineConnectionString1Key);
            machineConnectionString1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey machineConnectionString2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs2", machineConnectionString2Key);
            machineConnectionString2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual(1, section.ConnectionStrings.Count);
            Assert.IsNotNull(section.ConnectionStrings["cs2"]);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineConnectionString1Key, machineConnectionString2Key));
        }

        [TestMethod]
        public void LogFormatterWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            ConnectionStringSettings connectionString1 = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString1);
            ConnectionStringSettings connectionString2 = new ConnectionStringSettings("cs2", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString2);

            MockRegistryKey machineConnectionString1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs1", machineConnectionString1Key);
            machineConnectionString1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey machineConnectionString2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs2", machineConnectionString2Key);
            machineConnectionString2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);

            provider.OverrideWithGroupPolicies(section, false, machineKey, userKey);

            Assert.AreEqual(2, section.ConnectionStrings.Count);
            Assert.IsNotNull(section.ConnectionStrings["cs1"]);
            Assert.IsNotNull(section.ConnectionStrings["cs2"]);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineConnectionString1Key, machineConnectionString2Key));
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(ConnectionStringsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals("connectionStrings"))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ConnectionStringsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add("connectionStrings", section);

            section.ConnectionStrings.Add(new ConnectionStringSettings("cs1", "cs1"));
            section.ConnectionStrings.Add(new ConnectionStringSettings("cs2", "cs2"));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();

            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> connectionStringPartsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();

            Assert.IsTrue(connectionStringPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), connectionStringPartsEnumerator.Current.GetType());
            Assert.IsNull(connectionStringPartsEnumerator.Current.KeyName);
            Assert.AreEqual(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName,
                            connectionStringPartsEnumerator.Current.ValueName);

            Assert.IsTrue(connectionStringPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmComboBoxPart), connectionStringPartsEnumerator.Current.GetType());
            Assert.IsNull(connectionStringPartsEnumerator.Current.KeyName);
            Assert.AreEqual(ConnectionStringsManageabilityProvider.ProviderNamePropertyName,
                            connectionStringPartsEnumerator.Current.ValueName);

            Assert.IsFalse(connectionStringPartsEnumerator.MoveNext());
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext()); // 2 connection strings -> 2 policies

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }
    }
}
