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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class InstrumentationConfigurationSectionManageabilityProviderFixture
    {
        InstrumentationConfigurationSectionManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        InstrumentationConfigurationSection section;

        [TestInitialize]
        public void SetUp()
        {
            provider = new InstrumentationConfigurationSectionManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            section = new InstrumentationConfigurationSection();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPolicies(new TestsConfigurationSection(), true, machineKey, userKey);
        }

        [TestMethod]
        public void SectionWithoutPolicyOverridesIsNotModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            provider.OverrideWithGroupPolicies(section, true, null, null);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void SectionWithNullRegistryKeysOverridesIsNotModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            provider.OverrideWithGroupPolicies(section, true, null, null);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void SectionWithMachineOverrideForEventLoggingIsModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);

            provider.OverrideWithGroupPolicies(section, true, machineKey, null);

            Assert.AreEqual(false, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void SectionWithMachineOverrideForPerformanceCountersIsModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, false);

            provider.OverrideWithGroupPolicies(section, true, machineKey, null);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(false, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void SectionWithMachineOverrideForWmiIsModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);

            provider.OverrideWithGroupPolicies(section, true, machineKey, null);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void MachinePolicyOverrideTakesPrecedenceOverUserPolicyOverride()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, false);

            provider.OverrideWithGroupPolicies(section, true, machineKey, userKey);

            Assert.AreEqual(false, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void OverridesAreIgnoredIfGroupPoliciesAreDisabled()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, false);

            provider.OverrideWithGroupPolicies(section, false, machineKey, userKey);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(InstrumentationConfigurationSectionManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(InstrumentationConfigurationSection.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(InstrumentationConfigurationSectionManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            IConfigurationSource configurationSource = new DictionaryConfigurationSource();

            section.EventLoggingEnabled = false;
            section.PerformanceCountersEnabled = true;

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");
        }
    }
}
