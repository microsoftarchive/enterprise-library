//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Tests
{
    /// <summary>
    /// Summary description for SqlConfigurationSourceImplementationFixture
    /// </summary>
    [TestClass]
    public class SqlConfigurationSourceImplementationFixture
    {
        const string nonExistingSection = "dummy.nonexisting";
        const string localSection = "dummy.local";
        const string localSection2 = "dummy.local2";
        const string externalSection = "dummy.external";
        const string externalSection2 = "dummy.external2";
        const string externalSectionSource = @"server=(local)\SQLExpress;database=Northwind;Integrated Security=true";
        const string localSectionSource = @"";

        SqlConfigurationData data = null;

        IDictionary<string, int> updatedSectionsTally;

        void UpdateSection(string sectionName,
                           string sectionType,
                           object sectionValue)
        {
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("EntLib_SetConfig", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@section_name", sectionName);
                myCommand.Parameters.AddWithValue("@section_type", sectionType);
                if (sectionValue == null)
                    sectionValue = String.Empty;
                myCommand.Parameters.AddWithValue("@section_value", sectionValue);

                // Execute the command
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
        }

        [TestInitialize]
        public void Setup()
        {
            string connectString = @"server=(local)\SQLExpress;database=Northwind;Integrated Security=true";
            string getStoredProc = @"EntLib_GetConfig";
            string setStoredProc = @"EntLib_SetConfig";
            string refreshStoredProc = @"UpdateSectionDate";
            string removeStoredProc = @"EntLib_RemoveSection";

            data = new SqlConfigurationData(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);

            SqlConfigurationSource.ResetImplementation(data, false);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection;

            rwConfiguration.Sections.Remove(localSection);
            rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
            rwSection.Name = localSection;
            rwSection.Value = 10;
            SqlConfigurationSourceImplementation configSourceImpl = new SqlConfigurationSourceImplementation(data, false);
            configSourceImpl.SaveSection(rwSection.Name, rwSection);

            rwConfiguration.Sections.Remove(externalSection);
            rwConfiguration.Sections.Add(externalSection, rwSection = new DummySection());
            rwSection.Name = externalSection;
            rwSection.Value = 20;
            configSourceImpl.SaveSection(rwSection.Name, rwSection);

            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Sections.Add(localSection2, rwSection = new DummySection());
            rwSection.Name = localSection2;
            rwSection.Value = 30;
            configSourceImpl.SaveSection(rwSection.Name, rwSection);

            rwConfiguration.Save();

            SqlConfigurationManager.RefreshSection(localSection, data);
            SqlConfigurationManager.RefreshSection(localSection2, data);
            SqlConfigurationManager.RefreshSection(externalSection, data);

            ConfigurationChangeSqlWatcher.ResetDefaultPollDelay();

            updatedSectionsTally = new Dictionary<string, int>(0);
        }

        [TestMethod]
        public void CanGetExistingSection()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            object section = implementation.GetSection(localSection);

            Assert.IsNotNull(section);
        }

        [TestMethod]
        public void GetsNullIfSectionDoesNotExist()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            object section = implementation.GetSection(nonExistingSection);

            Assert.IsNull(section);
        }

        [TestMethod]
        public void GetsNullForSectionWithEmptyValue()
        {
            UpdateSection(localSection, typeof(DummySection).FullName, null);
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            object section = implementation.GetSection(localSection);

            Assert.IsNull(section);
        }

        [TestMethod]
        public void NewInstanceHasNoWatchers()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            Assert.AreEqual(0, implementation.WatchedConfigSources.Count);
            Assert.AreEqual(0, implementation.WatchedSections.Count);
        }

        [TestMethod]
        public void RequestForNonexistentSectionCreatesNoWatcher()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section = implementation.GetSection(nonExistingSection);

            Assert.IsNull(section);
            Assert.AreEqual(0, implementation.WatchedConfigSources.Count);
            Assert.AreEqual(0, implementation.WatchedSections.Count);
        }

        [TestMethod]
        public void FirstRequestForSectionCreatesWatcher()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, true);

            object section = implementation.GetSection(localSection);

            Assert.IsNotNull(section);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));

            Assert.IsNotNull(implementation.ConfigSourceWatcherMappings[externalSectionSource].Watcher);
            Assert.AreEqual(implementation.ConfigSourceWatcherMappings[externalSectionSource].Watcher.GetType(), typeof(ConfigurationChangeSqlWatcher));

            implementation.Dispose();
        }

        [TestMethod]
        public void SecondRequestForSameSectionDoesNotCreateSecondWatcher()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section1 = implementation.GetSection(localSection);
            object section2 = implementation.GetSection(localSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
        }

        [TestMethod]
        public void SecondRequestForDifferentSectionDoesNotCreateSecondConfigSourceWatcher()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section1 = implementation.GetSection(localSection);
            object section2 = implementation.GetSection(localSection2);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
        }

        [TestMethod]
        public void RequestsForTwoSectionsCreatesSectionWatchersForBoth()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section1 = implementation.GetSection(localSection);
            object section2 = implementation.GetSection(externalSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void WatchedSectionIsUpdatedIfNotificationIsFired()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            DummySection dummySection1 = section1 as DummySection;
            Assert.AreEqual(localSection, dummySection1.Name);
            Assert.AreEqual(10, dummySection1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection = rwConfiguration.GetSection(localSection) as DummySection;
            rwSection.Value = 15;
            implementation.SaveSection(localSection, rwSection);

            implementation.ConfigSourceChanged(externalSectionSource);

            section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            dummySection1 = section1 as DummySection;
            Assert.AreEqual(localSection, dummySection1.Name);
            Assert.AreEqual(15, dummySection1.Value);
        }

        [TestMethod]
        public void WatchedExistingSectionIsNoLongerWatchedIfRemovedFromConfiguration()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            DummySection dummySection1 = implementation.GetSection(localSection) as DummySection;
            DummySection dummySection2 = implementation.GetSection(localSection2) as DummySection;
            Assert.IsNotNull(dummySection1);
            Assert.IsNotNull(dummySection2);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection2));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            implementation.RemoveSection(localSection2);

            implementation.ConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(SqlConfigurationSourceImplementation.NullConfigSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.AreEqual(1, implementation.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Count);
            Assert.IsTrue(implementation.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Contains(localSection));
            Assert.AreEqual(1, implementation.ConfigSourceWatcherMappings[SqlConfigurationSourceImplementation.NullConfigSource].WatchedSections.Count);
        }

        [TestMethod]
        public void RegisteredObjectForNonRequestedSectionIsNotNotified()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(externalSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(localSection));
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfDifferentSectionsChanges()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            implementation.GetSection(localSection);
            implementation.GetSection(localSection2);
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void RegisteredObjectIsNotifiedOfSectionChanges()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ExternalConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfSectionChanges()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ExternalConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(3, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void RegisteredObjectIsNotifiedOfSectionChangesIfConfigSourceHasChanged()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(externalSectionSource);

            Assert.IsTrue(updatedSectionsTally.ContainsKey(externalSection));
        }

        [TestMethod]
        public void CanAddAndRemoveHandlers()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);
            object section = implementation.GetSection(externalSection);
            Assert.IsNotNull(section);

            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[externalSection]);

            implementation.RemoveSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[externalSection]);

            implementation.RemoveSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void RemovedSectionGetsNotificationOnRemovalAndDoesNotGetFurtherNotifications()
        {
            ConfigurationChangeSqlWatcher.SetDefaultPollDelayInMilliseconds(100);

            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            object section2 = implementation.GetSection(localSection2);
            Assert.IsNotNull(section2);

            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save(ConfigurationSaveMode.Minimal, true);

            // config source changed notifies both sections
            implementation.ConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);

            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            implementation.RemoveSection(localSection2);

            //since localsection2 is removed, localsection2 only gets initial notification
            Assert.AreEqual(2, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            implementation.ConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void RestoredSectionGetsNotificationOnRestoreAndGetsFurtherNotifications()
        {
            SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data, false);

            object section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            object section2 = implementation.GetSection(localSection2);
            Assert.IsNotNull(section2);

            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            implementation.ConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);

            // removal of the section notifies both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            implementation.RemoveSection(localSection2);

            Assert.AreEqual(2, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            implementation.ConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // restore of section gets notified
            DummySection rwSection = new DummySection();
            rwSection.Name = localSection2;
            rwSection.Value = 30;
            rwSection.SectionInformation.ConfigSource = externalSectionSource;
            implementation.SaveSection(rwSection.Name, rwSection);

            implementation.ConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(4, updatedSectionsTally[localSection]);
            Assert.AreEqual(3, updatedSectionsTally[localSection2]);
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
    }
}
