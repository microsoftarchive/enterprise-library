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
    public class ReplaceHandlerDataManageabilityProviderFixture
    {
        ReplaceHandlerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        ReplaceHandlerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new ReplaceHandlerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new ReplaceHandlerData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(ReplaceHandlerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(ReplaceHandlerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(ReplaceHandlerData), selectedAttribute.TargetType);
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
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(ArgumentException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overridden message");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreEqual("overridden message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(int).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("overridden resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(NullReferenceException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            userKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overridden message");
            userKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            userKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            userKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual("overridden message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(int).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("overridden resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(NullReferenceException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ExceptionMessageResourceType = typeof(object).AssemblyQualifiedName;
            configurationObject.ExceptionMessageResourceName = "resource";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overridden message");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, null);

            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, configurationObject.ExceptionMessageResourceType);
            Assert.AreEqual("resource", configurationObject.ExceptionMessageResourceName);
            Assert.AreSame(typeof(ArgumentException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ExceptionTypeIsoverriddenIfValueIsValid()
        {
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "msg");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(NullReferenceException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void TypeIsNotoverriddenIfValueIsInvalid()
        {
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "msg");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName, typeof(int).AssemblyQualifiedName);
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName, "overridden resource");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, "An invalid type name");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreSame(typeof(ArgumentException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationObject.ReplaceExceptionType = typeof(Exception);

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
            Assert.AreEqual(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(ReplaceHandlerDataManageabilityProvider.ExceptionMessageResourceNamePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(((AdmEditTextPart)partsEnumerator.Current).Required);
            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
