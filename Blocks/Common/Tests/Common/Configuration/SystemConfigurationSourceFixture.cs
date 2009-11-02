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

using System.Collections;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class SystemConfigurationSourceFixture
    {
        const string localSection = "dummy.local";
        const string addedSectionName = "dummy.local.newSection";
        const string removeSectionName = "dummy.toBeRemoved";
        const string localSectionSource = "";

        [TestMethod]
        public void SystemConfigurationSourceReturnsReadOnlySections()
        {
            SystemConfigurationSource source = new SystemConfigurationSource(false);
            ConfigurationSection dummySection = source.GetSection(localSection);

            Assert.IsTrue(dummySection.IsReadOnly());
        }

        [TestMethod]
        public void RemovingAndAddingSection()
        {
            SystemConfigurationSource sysSource = new SystemConfigurationSource(false);

            DummySection dummySection = sysSource.GetSection(localSection) as DummySection;
            Assert.IsTrue(dummySection != null);

            System.Configuration.Configuration rwConfiguration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string fileName = rwConfiguration.FilePath;
            int numSections = rwConfiguration.Sections.Count;
            sysSource.Remove(localSection);

            rwConfiguration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Assert.AreEqual(rwConfiguration.Sections.Count, numSections - 1);
            sysSource.Add(localSection, new DummySection()); // can't be the same instance

            rwConfiguration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Assert.AreEqual(rwConfiguration.Sections.Count, numSections);
        }

        [TestMethod]
        public void AddingASectionIsReflectedInMemoryAndOnDisk_Bug2931()
        {
            SystemConfigurationSource sysSource = new SystemConfigurationSource(false);
            var originalSection = (DummySection)(sysSource.GetSection(addedSectionName));
            Assert.IsNull(originalSection);

            var newSection = new DummySection();
            sysSource.Add(addedSectionName, newSection);

            var returnedSection = (DummySection)(sysSource.GetSection(addedSectionName));
            Assert.IsNotNull(returnedSection);
            sysSource.Remove(addedSectionName);
        }

        [TestMethod]
        public void RemovingSectionIsReflectedInMemoryAndOnDisk_Bug2931()
        {
            SystemConfigurationSource sysSource = new SystemConfigurationSource(false);
            var originalSection = (DummySection)(sysSource.GetSection(removeSectionName));
            Assert.IsNotNull(originalSection);

            sysSource.Remove(removeSectionName);

            var returnedSection = (DummySection)(sysSource.GetSection(removeSectionName));
            Assert.IsNull(returnedSection);
        }
    }
}
