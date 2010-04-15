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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Tests
{
    [TestClass]
    public class WrapHandlerDataManageabilityProviderFixture
    {
        WrapHandlerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        WrapHandlerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new WrapHandlerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new WrapHandlerData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(WrapHandlerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(WrapHandlerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(WrapHandlerData), selectedAttribute.TargetType);
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
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.WrapExceptionType = typeof(ArgumentException);

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(ArgumentException), configurationObject.WrapExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.WrapExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overridden message");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.WrapExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreEqual("overridden message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(int).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("overridden resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(NullReferenceException), configurationObject.WrapExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.WrapExceptionType = typeof(ArgumentException);

            userKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overridden message");
            userKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            userKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            userKey.AddStringValue(WrapHandlerDataManageabilityProvider.WrapExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual("overridden message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(int).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("overridden resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(NullReferenceException), configurationObject.WrapExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.WrapExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overridden message");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.WrapExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, null);

            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(ArgumentException), configurationObject.WrapExceptionType);
        }

        [TestMethod]
        public void ExceptionTypeIsoverriddenIfValueIsValid()
        {
            configurationObject.WrapExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "msg");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.WrapExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(NullReferenceException), configurationObject.WrapExceptionType);
        }

        [TestMethod]
        public void TypeIsNotoverriddenIfValueIsInvalid()
        {
            configurationObject.WrapExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "msg");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(WrapHandlerDataManageabilityProvider.WrapExceptionTypePropertyName, "An invalid type name");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(ArgumentException), configurationObject.WrapExceptionType);
        }


        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationObject.WrapExceptionType = typeof(Exception);

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
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(WrapHandlerDataManageabilityProvider.ExceptionMessagePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(WrapHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(WrapHandlerDataManageabilityProvider.WrapExceptionTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
