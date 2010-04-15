//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.TraceListeners
{
    [TestClass]
    public class RollingFlatFileTraceListenerDataManageabilityProviderFixture
    {
        RollingFlatFileTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey machineOptionsKey;
        MockRegistryKey userKey;
        MockRegistryKey userOptionsKey;
        RollingFlatFileTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new RollingFlatFileTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            machineOptionsKey = new MockRegistryKey(false);
            userKey = new MockRegistryKey(true);
            userOptionsKey = new MockRegistryKey(false);
            configurationObject = new RollingFlatFileTraceListenerData();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(RollingFlatFileTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(RollingFlatFileTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(RollingFlatFileTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
            configurationObject.Filter = SourceLevels.Error;
            configurationObject.Header = "header";
            configurationObject.Footer = "footer";
            configurationObject.MaxArchivedFiles = 10;

            provider.OverrideWithGroupPolicies(configurationObject, true, null, null);

            Assert.AreEqual("file name", configurationObject.FileName);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Increment, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Month, configurationObject.RollInterval);
            Assert.AreEqual(100, configurationObject.RollSizeKB);
            Assert.AreEqual("pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
            Assert.AreEqual("header", configurationObject.Header);
            Assert.AreEqual("footer", configurationObject.Footer);
            Assert.AreEqual(10, configurationObject.MaxArchivedFiles);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
            configurationObject.Filter = SourceLevels.Error;
            configurationObject.Header = "header";
            configurationObject.Footer = "footer";
            configurationObject.MaxArchivedFiles = 10;

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.MaxArchivedFilesPropertyName, 20);
            machineKey.AddSubKey(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, machineOptionsKey);
            machineOptionsKey.AddIntValue(TraceOptions.ProcessId.ToString(), 1);
            machineOptionsKey.AddIntValue(TraceOptions.ThreadId.ToString(), 1);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, null);

            Assert.AreEqual("overriden file name", configurationObject.FileName);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Overwrite, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Day, configurationObject.RollInterval);
            Assert.AreEqual(200, configurationObject.RollSizeKB);
            Assert.AreEqual("overriden pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
            Assert.AreEqual("overriden header", configurationObject.Header);
            Assert.AreEqual("overriden footer", configurationObject.Footer);
            Assert.AreEqual(20, configurationObject.MaxArchivedFiles);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
            configurationObject.Filter = SourceLevels.Error;
            configurationObject.Header = "header";
            configurationObject.Footer = "footer";
            configurationObject.MaxArchivedFiles = 10;

            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            userKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            userKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            userKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            userKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.MaxArchivedFilesPropertyName, 20);
            userKey.AddSubKey(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, userOptionsKey);
            userOptionsKey.AddIntValue(TraceOptions.ProcessId.ToString(), 1);
            userOptionsKey.AddIntValue(TraceOptions.ThreadId.ToString(), 1);

            provider.OverrideWithGroupPolicies(configurationObject, true, null, userKey);

            Assert.AreEqual("overriden file name", configurationObject.FileName);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Overwrite, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Day, configurationObject.RollInterval);
            Assert.AreEqual(200, configurationObject.RollSizeKB);
            Assert.AreEqual("overriden pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
            Assert.AreEqual("overriden header", configurationObject.Header);
            Assert.AreEqual("overriden footer", configurationObject.Footer);
            Assert.AreEqual(20, configurationObject.MaxArchivedFiles);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
            configurationObject.Filter = SourceLevels.Error;
            configurationObject.Header = "header";
            configurationObject.Footer = "footer";
            configurationObject.MaxArchivedFiles = 10;

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.MaxArchivedFilesPropertyName, 20);
            machineKey.AddSubKey(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, machineOptionsKey);
            machineOptionsKey.AddIntValue(TraceOptions.ProcessId.ToString(), 1);
            machineOptionsKey.AddIntValue(TraceOptions.ThreadId.ToString(), 1);

            provider.OverrideWithGroupPolicies(configurationObject, false, machineKey, null);

            Assert.AreEqual("file name", configurationObject.FileName);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Increment, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Month, configurationObject.RollInterval);
            Assert.AreEqual(100, configurationObject.RollSizeKB);
            Assert.AreEqual("pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
            Assert.AreEqual("header", configurationObject.Header);
            Assert.AreEqual("footer", configurationObject.Footer);
            Assert.AreEqual(10, configurationObject.MaxArchivedFiles);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.MaxArchivedFilesPropertyName, 20);
            machineKey.AddSubKey(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, machineOptionsKey);
            machineOptionsKey.AddIntValue(TraceOptions.ProcessId.ToString(), 1);
            machineOptionsKey.AddIntValue(TraceOptions.ThreadId.ToString(), 1);

            provider.OverrideWithGroupPolicies(configurationObject, true, machineKey, userKey);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LoggingSettings section = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, section);

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
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.MaxArchivedFilesPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.IsNull(partsEnumerator.Current.ValueName);

            // trace output options checkboxes
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual("LogicalOperationStack", partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual("DateTime", partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual("Timestamp", partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual("ProcessId", partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual("ThreadId", partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual("Callstack", partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
