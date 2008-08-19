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

using System.Collections.Generic;
using System.Management;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Tests.WMI
{
    [TestClass]
    public class KeyedHashAlgorithmProviderSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(KeyedHashAlgorithmProviderSetting));
            NamedConfigurationSetting.ClearPublishedInstances();
        }

        [TestCleanup]
        public void TearDown()
        {
            ManagementEntityTypesRegistrar.UnregisterAll();
            NamedConfigurationSetting.ClearPublishedInstances();
        }

        [TestMethod]
        public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
        {
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM KeyedHashAlgorithmProviderSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            KeyedHashAlgorithmProviderSetting setting
                = new KeyedHashAlgorithmProviderSetting(null, "name", "AlgorithmType", "ProtectedKeyFilename", "ProtectedKeyProtectionScope", false);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();

            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM KeyedHashAlgorithmProviderSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("AlgorithmType", resultEnumerator.Current.Properties["AlgorithmType"].Value);
                Assert.AreEqual("ProtectedKeyFilename", resultEnumerator.Current.Properties["ProtectedKeyFilename"].Value);
                Assert.AreEqual("ProtectedKeyProtectionScope", resultEnumerator.Current.Properties["ProtectedKeyProtectionScope"].Value);
                Assert.AreEqual(false, resultEnumerator.Current.Properties["SaltEnabled"].Value);
                Assert.AreEqual("KeyedHashAlgorithmProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CanBindObject()
        {
            KeyedHashAlgorithmProviderSetting setting
                = new KeyedHashAlgorithmProviderSetting(null, "name", "AlgorithmType", "ProtectedKeyFilename", "ProtectedKeyProtectionScope", false);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            //setting.Changed += this.Changed;

            setting.Publish();

            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM KeyedHashAlgorithmProviderSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("KeyedHashAlgorithmProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);

                //should throw 
                managementObject.Put();
            }
        }

        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            KeyedHashAlgorithmProviderData sourceElement
                = new KeyedHashAlgorithmProviderData("name",
                                                     typeof(bool),
                                                     true,
                                                     "file name",
                                                     DataProtectionScope.CurrentUser);

            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            KeyedHashAlgorithmProviderDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);

            KeyedHashAlgorithmProviderSetting setting = settings[0] as KeyedHashAlgorithmProviderSetting;
            Assert.IsNotNull(setting);

            setting.AlgorithmType = typeof(int).AssemblyQualifiedName;
            setting.SaltEnabled = false;
            setting.ProtectedKeyFilename = "overriden file name";
            setting.ProtectedKeyProtectionScope = DataProtectionScope.LocalMachine.ToString();

            setting.Commit();

            Assert.AreEqual(typeof(int), sourceElement.AlgorithmType);
            Assert.AreEqual(false, sourceElement.SaltEnabled);
            Assert.AreEqual("overriden file name", sourceElement.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.LocalMachine, sourceElement.ProtectedKeyProtectionScope);
        }
    }
}