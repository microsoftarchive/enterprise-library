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
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability.Tests
{
    [TestClass]
    public class LoggingExceptionHandlerDataManageabilityProviderFixture
    {
        LoggingExceptionHandlerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        LoggingExceptionHandlerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new LoggingExceptionHandlerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            configurationObject = new LoggingExceptionHandlerData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(LoggingExceptionHandlerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(LoggingExceptionHandlerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(LoggingExceptionHandlerData), selectedAttribute.TargetType);
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
            configurationObject.EventId = 100;
            configurationObject.FormatterType = typeof(ExceptionFormatter);
            configurationObject.LogCategory = "category";
            configurationObject.Priority = 50;
            configurationObject.Severity = TraceEventType.Error;
            configurationObject.Title = "title";

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual(100, configurationObject.EventId);
            Assert.AreSame(typeof(ExceptionFormatter), configurationObject.FormatterType);
            Assert.AreEqual("category", configurationObject.LogCategory);
            Assert.AreEqual(50, configurationObject.Priority);
            Assert.AreEqual(TraceEventType.Error, configurationObject.Severity);
            Assert.AreEqual("title", configurationObject.Title);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.EventId = 100;
            configurationObject.FormatterType = typeof(ExceptionFormatter);
            configurationObject.LogCategory = "category";
            configurationObject.Priority = 50;
            configurationObject.Severity = TraceEventType.Error;
            configurationObject.Title = "title";

            machineKey.AddIntValue(LoggingExceptionHandlerDataManageabilityProvider.EventIdPropertyName, 200);
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.FormatterTypePropertyName, typeof(Object).AssemblyQualifiedName);
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.LogCategoryPropertyName, "overriden category");
            machineKey.AddIntValue(LoggingExceptionHandlerDataManageabilityProvider.PriorityPropertyName, 150);
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.SeverityPropertyName, TraceEventType.Critical.ToString());
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.TitlePropertyName, "overriden title");

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreEqual(200, configurationObject.EventId);
            Assert.AreSame(typeof(Object), configurationObject.FormatterType);
            Assert.AreEqual("overriden category", configurationObject.LogCategory);
            Assert.AreEqual(150, configurationObject.Priority);
            Assert.AreEqual(TraceEventType.Critical, configurationObject.Severity);
            Assert.AreEqual("overriden title", configurationObject.Title);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.EventId = 100;
            configurationObject.FormatterType = typeof(ExceptionFormatter);
            configurationObject.LogCategory = "category";
            configurationObject.Priority = 50;
            configurationObject.Severity = TraceEventType.Error;
            configurationObject.Title = "title";

            userKey.AddIntValue(LoggingExceptionHandlerDataManageabilityProvider.EventIdPropertyName, 200);
            userKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.FormatterTypePropertyName, typeof(Object).AssemblyQualifiedName);
            userKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.LogCategoryPropertyName, "overriden category");
            userKey.AddIntValue(LoggingExceptionHandlerDataManageabilityProvider.PriorityPropertyName, 150);
            userKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.SeverityPropertyName, TraceEventType.Critical.ToString());
            userKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.TitlePropertyName, "overriden title");

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual(200, configurationObject.EventId);
            Assert.AreSame(typeof(Object), configurationObject.FormatterType);
            Assert.AreEqual("overriden category", configurationObject.LogCategory);
            Assert.AreEqual(150, configurationObject.Priority);
            Assert.AreEqual(TraceEventType.Critical, configurationObject.Severity);
            Assert.AreEqual("overriden title", configurationObject.Title);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.EventId = 100;
            configurationObject.FormatterType = typeof(ExceptionFormatter);
            configurationObject.LogCategory = "category";
            configurationObject.Priority = 50;
            configurationObject.Severity = TraceEventType.Error;
            configurationObject.Title = "title";

            machineKey.AddIntValue(LoggingExceptionHandlerDataManageabilityProvider.EventIdPropertyName, 200);
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.FormatterTypePropertyName, typeof(Object).AssemblyQualifiedName);
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.LogCategoryPropertyName, "overriden category");
            machineKey.AddIntValue(LoggingExceptionHandlerDataManageabilityProvider.PriorityPropertyName, 150);
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.SeverityPropertyName, TraceEventType.Critical.ToString());
            machineKey.AddStringValue(LoggingExceptionHandlerDataManageabilityProvider.TitlePropertyName, "overriden title");

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, null);

            Assert.AreEqual(100, configurationObject.EventId);
            Assert.AreSame(typeof(ExceptionFormatter), configurationObject.FormatterType);
            Assert.AreEqual("category", configurationObject.LogCategory);
            Assert.AreEqual(50, configurationObject.Priority);
            Assert.AreEqual(TraceEventType.Error, configurationObject.Severity);
            Assert.AreEqual("title", configurationObject.Title);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LoggingSettings loggingSection = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, loggingSection);
            loggingSection.TraceSources.Add(new TraceSourceData("trace source", SourceLevels.All));

            configurationObject.FormatterType = typeof(object);

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
            Assert.AreEqual(LoggingExceptionHandlerDataManageabilityProvider.TitlePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(LoggingExceptionHandlerDataManageabilityProvider.EventIdPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(LoggingExceptionHandlerDataManageabilityProvider.SeverityPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(LoggingExceptionHandlerDataManageabilityProvider.PriorityPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(LoggingExceptionHandlerDataManageabilityProvider.LogCategoryPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmComboBoxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(LoggingExceptionHandlerDataManageabilityProvider.FormatterTypePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
