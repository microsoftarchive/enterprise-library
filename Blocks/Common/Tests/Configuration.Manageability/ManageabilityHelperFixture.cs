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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass()]
    public class ManageabilityHelperFixture
    {
        const string ApplicationName = "TestApp";
        const string SectionName = "TestSection";
        const string AltSectionName = "AltTestSection";
        const String originalValue = "original value";

        DictionaryConfigurationSource configurationSource;
        DictionaryConfigurationSourceConfigurationAccessor configurationAccessor;
        MockRegistryAccessor registryAccessor;
        MockRegistryKey currentUser;
        MockRegistryKey localMachine;
        IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders;
        IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders;

        [TestInitialize()]
        public void SetUp()
        {
            configurationSource = new DictionaryConfigurationSource();
            configurationAccessor = new DictionaryConfigurationSourceConfigurationAccessor(configurationSource);
            currentUser = new MockRegistryKey(true);
            localMachine = new MockRegistryKey(true);
            registryAccessor = new MockRegistryAccessor(currentUser, localMachine);

            manageabilityProviders = new Dictionary<string, ConfigurationSectionManageabilityProvider>();
            subProviders = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
        }

        [TestMethod()]
        public void HelperWithEmtpyManageabilityProvidersDoesNothing()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWithManageabilityProviderForMissingSectionDoesNothing()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(AltSectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsFalse(manageabilityProvider.called);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWithManageabilityProviderForExistingSectionDoesInvokeWithApproriateParameters()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.AreEqual(1, currentUser.GetRequests().Count);
            Assert.AreSame(userKey, manageabilityProvider.userKey);
            Assert.AreEqual(1, localMachine.GetRequests().Count);
            Assert.AreSame(machineKey, manageabilityProvider.machineKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWillNotSendRegistryKeysIfNotReadingGroupPolicies()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, false, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.IsFalse(manageabilityProvider.readGroupPolicies);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.IsNull(manageabilityProvider.userKey);
            Assert.AreEqual(0, localMachine.GetRequests().Count);
            Assert.IsNull(manageabilityProvider.machineKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWillNotSendRegistryKeysIfNotAvailable()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.IsTrue(manageabilityProvider.readGroupPolicies);
            Assert.AreEqual(1, currentUser.GetRequests().Count);
            Assert.IsNull(manageabilityProvider.userKey);
            Assert.AreEqual(1, localMachine.GetRequests().Count);
            Assert.AreSame(machineKey, manageabilityProvider.machineKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineKey));
        }

        [TestMethod]
        public void HelperIgnoresUpdateEmptySectionsList()
        {
            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);
        }

        [TestMethod]
        public void HelperIgnoresUpdateSectionsListWithoutMappedProvider()
        {
            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);
        }

        [TestMethod]
        public void HelperIgnoresUpdateSectionsListWithMappedProviderForMissingSection()
        {
            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsFalse(manageabilityProvider.called);
        }

        [TestMethod]
        public void HelperPerformsUpdateForSectionNotPreviouslyProcessed()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
        }

        [TestMethod]
        public void WillRemoveRegisteredSectionWithDisabledPolicyIfPolicyOverridesAreEnabled()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);
            machineKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);
            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, ApplicationName);

            Assert.IsTrue(configurationSource.Contains(SectionName));

            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsFalse(configurationSource.Contains(SectionName));
        }

        [TestMethod]
        public void WillNotRemoveRegisteredSectionWithDisabledPolicyIfPolicyOverridesAreDisabled()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);
            machineKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);
            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, false, registryAccessor, ApplicationName);

            Assert.IsTrue(configurationSource.Contains(SectionName));

            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(configurationSource.Contains(SectionName));
        }
    }
}
