//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class CustomProviderDataManageabilityProviderFixture
    {
        MockCustomProviderDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        MockCustomProviderData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new MockCustomProviderDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new MockCustomProviderData();
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
            configurationObject.Type = typeof(Object);
            configurationObject.Attributes.Add("name1", "value1");
            configurationObject.Attributes.Add("name2", "value2");

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreSame(typeof(Object), configurationObject.Type);
            Assert.AreEqual(2, configurationObject.Attributes.Count);
            Assert.IsNotNull(configurationObject.Attributes.Get("name1"));
            Assert.IsNotNull(configurationObject.Attributes.Get("name2"));
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereIsMachinePolicyOverrideForType()
        {
            configurationObject.Type = typeof(Object);

            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName, typeof(String).AssemblyQualifiedName);
            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.AttributesPropertyName, "");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(String), configurationObject.Type);
            Assert.AreEqual(0, configurationObject.Attributes.Count);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereIsMachinePolicyOverrideForAttributes()
        {
            configurationObject.Type = typeof(Object);

            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName, typeof(String).AssemblyQualifiedName);
            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.AttributesPropertyName,
                                      "name3=value3;name4=value4;name5=value 5");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(String), configurationObject.Type);
            Assert.AreEqual(3, configurationObject.Attributes.Count);
            Assert.AreEqual("value3", configurationObject.Attributes.Get("name3"));
            Assert.AreEqual("value4", configurationObject.Attributes.Get("name4"));
            Assert.AreEqual("value 5", configurationObject.Attributes.Get("name5"));
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereIsUserPolicyOverrideForType()
        {
            configurationObject.Type = typeof(Object);

            userKey.AddStringValue(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName, typeof(String).AssemblyQualifiedName);
            userKey.AddStringValue(MockCustomProviderDataManageabilityProvider.AttributesPropertyName, "");

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreSame(typeof(String), configurationObject.Type);
            Assert.AreEqual(0, configurationObject.Attributes.Count);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereIsUserPolicyOverrideForAttributes()
        {
            configurationObject.Type = typeof(Object);

            userKey.AddStringValue(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName, typeof(String).AssemblyQualifiedName);
            userKey.AddStringValue(MockCustomProviderDataManageabilityProvider.AttributesPropertyName,
                                   "name3=value3;name4=value4;name5=value 5");

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreSame(typeof(String), configurationObject.Type);
            Assert.AreEqual(3, configurationObject.Attributes.Count);
            Assert.AreEqual("value3", configurationObject.Attributes.Get("name3"));
            Assert.AreEqual("value4", configurationObject.Attributes.Get("name4"));
            Assert.AreEqual("value 5", configurationObject.Attributes.Get("name5"));
        }

        [TestMethod]
        public void TypeIsOverridenIfValueIsValid()
        {
            configurationObject.Type = typeof(Object);

            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName, typeof(String).AssemblyQualifiedName);
            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.AttributesPropertyName, "");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(String), configurationObject.Type);
        }

        [TestMethod]
        public void TypeIsNotOverridenIfValueIsInvalid()
        {
            configurationObject.Type = typeof(Object);

            machineKey.AddStringValue(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName, "An invalid type name");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(Object), configurationObject.Type);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            configurationObject.Type = typeof(object);
            configurationObject.Attributes.Add("name1", "valu;e1");
            configurationObject.Attributes.Add("name2", "value2");

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            contentBuilder.StartCategory("category");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndCategory();

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.AreEqual(configurationObject.Name,
                            policiesEnumerator.Current.Name);
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(MockCustomProviderDataManageabilityProvider.ProviderTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, ((AdmEditTextPart)partsEnumerator.Current).DefaultValue);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(MockCustomProviderDataManageabilityProvider.AttributesPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNull(partsEnumerator.Current.KeyName);
            IDictionary<String, String> attributes = new Dictionary<String, String>();
            KeyValuePairParser.ExtractKeyValueEntries(((AdmEditTextPart)partsEnumerator.Current).DefaultValue, attributes);
            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("valu;e1", attributes["name1"]);
            Assert.AreEqual("value2", attributes["name2"]);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
