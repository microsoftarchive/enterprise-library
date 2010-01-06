//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability.Tests
{
    [TestClass]
    public class FaultContractExceptionHandlerDataManageabilityProviderFixture
    {
        FaultContractExceptionHandlerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        FaultContractExceptionHandlerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new FaultContractExceptionHandlerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new FaultContractExceptionHandlerData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(FaultContractExceptionHandlerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(FaultContractExceptionHandlerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(FaultContractExceptionHandlerData), selectedAttribute.TargetType);
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
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name1", "value1"));
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name2", "value2"));
            configurationObject.ExceptionMessage = "message";
            configurationObject.FaultContractType = "fault contract";

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual(2, configurationObject.Attributes.Count);
            Assert.AreEqual("value1", configurationObject.Attributes["name1"]);
            Assert.AreEqual("value2", configurationObject.Attributes["name2"]);
            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreEqual("fault contract", configurationObject.FaultContractType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name1", "value1"));
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name2", "value2"));
            configurationObject.ExceptionMessage = "message";
            configurationObject.FaultContractType = "fault contract";

            machineKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.AttributesPropertyName,
                                      "name3=value3;name4=value4;name5=value 5");
            machineKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            machineKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.FaultContractTypePropertyName, "overriden fault contract");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreEqual(3, configurationObject.Attributes.Count);
            Assert.AreEqual("value3", configurationObject.Attributes.Get("name3"));
            Assert.AreEqual("value4", configurationObject.Attributes.Get("name4"));
            Assert.AreEqual("value 5", configurationObject.Attributes.Get("name5"));
            Assert.AreEqual("overriden message", configurationObject.ExceptionMessage);
            Assert.AreEqual("overriden fault contract", configurationObject.FaultContractType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name1", "value1"));
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name2", "value2"));
            configurationObject.ExceptionMessage = "message";
            configurationObject.FaultContractType = "fault contract";

            userKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.AttributesPropertyName,
                                   "name3=value3;name4=value4;name5=value 5");
            userKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            userKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.FaultContractTypePropertyName, "overriden fault contract");

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual(3, configurationObject.Attributes.Count);
            Assert.AreEqual("value3", configurationObject.Attributes.Get("name3"));
            Assert.AreEqual("value4", configurationObject.Attributes.Get("name4"));
            Assert.AreEqual("value 5", configurationObject.Attributes.Get("name5"));
            Assert.AreEqual("overriden message", configurationObject.ExceptionMessage);
            Assert.AreEqual("overriden fault contract", configurationObject.FaultContractType);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name1", "value1"));
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name2", "value2"));
            configurationObject.ExceptionMessage = "message";
            configurationObject.FaultContractType = "fault contract";

            machineKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.AttributesPropertyName,
                                      "name3=value3;name4=value4;name5=value 5");
            machineKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            machineKey.AddStringValue(FaultContractExceptionHandlerDataManageabilityProvider.FaultContractTypePropertyName, "overriden fault contract");

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, null);

            Assert.AreEqual(2, configurationObject.Attributes.Count);
            Assert.AreEqual("value1", configurationObject.Attributes["name1"]);
            Assert.AreEqual("value2", configurationObject.Attributes["name2"]);
            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreEqual("fault contract", configurationObject.FaultContractType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name1", "value1"));
            configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("name2", "value2"));
            configurationObject.ExceptionMessage = "message";
            configurationObject.FaultContractType = "fault contract";

            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

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
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FaultContractExceptionHandlerDataManageabilityProvider.ExceptionMessagePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FaultContractExceptionHandlerDataManageabilityProvider.FaultContractTypePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(FaultContractExceptionHandlerDataManageabilityProvider.AttributesPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            IDictionary<String, String> attributes = new Dictionary<String, String>();
            KeyValuePairParser.ExtractKeyValueEntries(((AdmEditTextPart)partsEnumerator.Current).DefaultValue, attributes);
            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("value1", attributes["name1"]);
            Assert.AreEqual("value2", attributes["name2"]);

            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
