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
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class SystemConfigurationSourceFixture
    {
        const string localSection = "dummy.local";
        const string localSectionSource = "";

        IDictionary<string, int> updatedSectionsTally;
        ICollection updatedSectionNames;

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

        void OnConfigurationChanged(object sender,
                                    ConfigurationChangedEventArgs args)
        {
            if (updatedSectionsTally.ContainsKey(args.SectionName))
            {
                updatedSectionsTally[args.SectionName] = updatedSectionsTally[args.SectionName] + 1;
            }
            else
            {
                updatedSectionsTally[args.SectionName] = 1;
            }
        }

        void OnConfigurationSourceChanged(object sender, ConfigurationSourceChangedEventArgs args)
        {
            this.updatedSectionNames = args.ChangedSectionNames;
        }
    }
}
