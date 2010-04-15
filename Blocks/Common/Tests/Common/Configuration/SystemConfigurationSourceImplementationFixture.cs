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

using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    /// <summary>
    /// Summary description for SystemConfigurationSourceFixture
    /// </summary>
    [TestClass]
    public partial class SystemConfigurationSourceFixture2
    {
        const string nonExistingSection = "dummy.nonexisting";
        const string localSection = "dummy.local";
        const string localSection2 = "dummy.local2";
        const string externalSection = "dummy.external";
        const string externalSection2 = "dummy.external2";
        const string localSectionSource = "";
        const string externalSectionSource = "dummy.external.config";
        const string externalSectionSourceAlt = "dummy.external.alt.config";
        const string externalSection2Source = "dummy.external2.config";

        IDictionary<string, int> updatedSectionsTally;
        ICollection<string> updatedSections;

        [TestInitialize]
        public void Setup()
        {
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection;

            rwConfiguration.Sections.Remove(localSection);
            rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
            rwSection.Name = localSection;
            rwSection.Value = 10;
            rwSection.SectionInformation.ConfigSource = localSectionSource;

            rwConfiguration.Sections.Remove(externalSection);
            rwConfiguration.Sections.Add(externalSection, rwSection = new DummySection());
            rwSection.Name = externalSection;
            rwSection.Value = 20;
            rwSection.SectionInformation.ConfigSource = externalSectionSource;

            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Sections.Add(localSection2, rwSection = new DummySection());
            rwSection.Name = localSection2;
            rwSection.Value = 30;
            rwSection.SectionInformation.ConfigSource = localSectionSource;

            rwConfiguration.Save();

            ConfigurationManager.RefreshSection(localSection);
            ConfigurationManager.RefreshSection(localSection2);
            ConfigurationManager.RefreshSection(externalSection);

            updatedSectionsTally = new Dictionary<string, int>(0);
            updatedSections = null;
        }

        [TestMethod]
        public void CanGetExistingSectionInAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            object section = source.GetSection(localSection);

            Assert.IsNotNull(section);
        }

        [TestMethod]
        public void CanGetExistingSectionInExternalFile()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            object section = source.GetSection(externalSection);

            Assert.IsNotNull(section);
        }

        [TestMethod]
        public void GetsNullIfSectionDoesNotExist()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            object section = source.GetSection(nonExistingSection);

            Assert.IsNull(section);
        }

        [TestMethod]
        public void NewInstanceHasNoWatchers()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            Assert.AreEqual(0, source.WatchedConfigSources.Count);
            Assert.AreEqual(0, source.WatchedSections.Count);
        }

        [TestMethod]
        public void RequestForNonexistentSectionCreatesNoWatcher()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section = source.GetSection(nonExistingSection);

            Assert.IsNull(section);
            Assert.AreEqual(1, source.WatchedConfigSources.Count); //watches only ConfigurationSourceSection.
            Assert.AreEqual(1, source.WatchedSections.Count);
        }

        [TestMethod]
        public void FirstRequestForSectionInAppConfigCreatesWatcherForAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(true);

            object section = source.GetSection(localSection);

            Assert.IsNotNull(section);
            Assert.AreEqual(1, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(2, source.WatchedSections.Count); //watches  ConfigurationSourceSection + localSection
            Assert.IsTrue(source.WatchedSections.Contains(localSection));

            Assert.IsNotNull(source.ConfigSourceWatcherMappings[localSectionSource].Watcher);
            Assert.AreEqual(source.ConfigSourceWatcherMappings[localSectionSource].Watcher.GetType(), typeof(ConfigurationChangeFileWatcher));

            ((IDisposable)source).Dispose();
        }

        [TestMethod]
        public void SecondRequestForSameSectionInAppConfigDoesNotCreateSecondWatcherForAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(localSection);
            object section2 = source.GetSection(localSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(1, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(2, source.WatchedSections.Count); //watches  ConfigurationSourceSection + localSection
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
        }

        [TestMethod]
        public void SecondRequestForDifferentSectionInAppConfigDoesNotCreateSecondWatcherForAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(localSection);
            object section2 = source.GetSection(localSection2);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(1, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(3, source.WatchedSections.Count); //watches  ConfigurationSourceSection. + localSection + localSection2
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
            Assert.IsTrue(source.WatchedSections.Contains(localSection2));
        }

        [TestMethod]
        public void FirstRequestForSectionInExternalFileCreatesWatchersForExternalFileAndAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section = source.GetSection(externalSection);

            Assert.IsNotNull(section);
            Assert.AreEqual(2, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(source.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(2, source.WatchedSections.Count); //watches  ConfigurationSourceSection + externalSection
            Assert.IsTrue(source.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void SecondRequestForSameSectionInExternalFileDoesNotCreateWatcherForExternalFile()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(externalSection);
            object section2 = source.GetSection(externalSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(2, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(source.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(2, source.WatchedSections.Count);  //watches  ConfigurationSourceSection + externalSection
            Assert.IsTrue(source.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void RequestsForAppConfigAndExternalFileCreatesWatchersForBoth()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(localSection);
            object section2 = source.GetSection(externalSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(2, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(source.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(3, source.WatchedSections.Count); //watches  ConfigurationSourceSection + externalSection + localSection
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
            Assert.IsTrue(source.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void WatchedSectionInAppConfigValuesAreUpdatedIfAppConfigChangesAndNotificationIsFired()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(localSection);
            Assert.IsNotNull(section1);
            DummySection dummySection1 = section1 as DummySection;
            Assert.AreEqual(localSection, dummySection1.Name);
            Assert.AreEqual(10, dummySection1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection = rwConfiguration.GetSection(localSection) as DummySection;
            rwSection.Value = 15;
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);

            section1 = source.GetSection(localSection);
            Assert.IsNotNull(section1);
            dummySection1 = section1 as DummySection;
            Assert.AreEqual(localSection, dummySection1.Name);
            Assert.AreEqual(15, dummySection1.Value);
        }

        [TestMethod]
        public void WatchedSectionInExternalFileValuesAreUpdatedIfExternalFileChangesAndNotificationIsFired()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(externalSection);
            Assert.IsNotNull(section1);
            DummySection dummySection1 = section1 as DummySection;
            Assert.AreEqual(externalSection, dummySection1.Name);
            Assert.AreEqual(20, dummySection1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection = rwConfiguration.GetSection(externalSection) as DummySection;
            rwSection.Value = 25;
            rwConfiguration.Save();

            source.ExternalConfigSourceChanged(externalSectionSource);

            section1 = source.GetSection(externalSection);
            Assert.IsNotNull(section1);
            dummySection1 = section1 as DummySection;
            Assert.AreEqual(externalSection, dummySection1.Name);
            Assert.AreEqual(25, dummySection1.Value);
        }

        [TestMethod]
        public void WatchedExistingSectionIsNoLongerWatchedIfRemovedFromConfiguration()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            DummySection dummySection1 = source.GetSection(localSection) as DummySection;
            DummySection dummySection2 = source.GetSection(localSection2) as DummySection;
            Assert.IsNotNull(dummySection1);
            Assert.IsNotNull(dummySection2);
            Assert.AreEqual(1, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(3, source.WatchedSections.Count); //watches  ConfigurationSourceSection + localSection + localSection2
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
            Assert.IsTrue(source.WatchedSections.Contains(localSection2));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(2, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(source.WatchedConfigSources.Contains(SystemConfigurationSource.NullConfigSource));
            Assert.AreEqual(3, source.WatchedSections.Count);
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
            Assert.IsTrue(source.WatchedSections.Contains(localSection2));
            Assert.AreEqual(2, source.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Count);
            Assert.IsTrue(source.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Contains(localSection));
            Assert.AreEqual(1, source.ConfigSourceWatcherMappings[SystemConfigurationSource.NullConfigSource].WatchedSections.Count);
            Assert.IsTrue(source.ConfigSourceWatcherMappings[SystemConfigurationSource.NullConfigSource].WatchedSections.Contains(localSection2));
        }

        [TestMethod]
        public void WatchedExistingSectionInExternalFileIsNoLongerWatchedIfRemovedFromConfigurationAndExternalFileWatcherIsRemoved()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            DummySection dummySection1 = source.GetSection(localSection) as DummySection;
            DummySection dummySection2 = source.GetSection(externalSection) as DummySection;
            Assert.IsNotNull(dummySection1);
            Assert.IsNotNull(dummySection2);
            Assert.AreEqual(2, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(source.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(3, source.WatchedSections.Count); //watches  ConfigurationSourceSection + externalSection + localSection
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
            Assert.IsTrue(source.WatchedSections.Contains(externalSection));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(externalSection);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(2, source.WatchedConfigSources.Count);
            Assert.IsTrue(source.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(source.WatchedConfigSources.Contains(SystemConfigurationSource.NullConfigSource));
            Assert.AreEqual(3, source.WatchedSections.Count); 
            Assert.IsTrue(source.WatchedSections.Contains(localSection));
            Assert.IsTrue(source.WatchedSections.Contains(externalSection));
            Assert.AreEqual(2, source.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Count);
            Assert.IsTrue(source.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Contains(localSection));
            Assert.AreEqual(1, source.ConfigSourceWatcherMappings[SystemConfigurationSource.NullConfigSource].WatchedSections.Count);
            Assert.IsTrue(source.ConfigSourceWatcherMappings[SystemConfigurationSource.NullConfigSource].WatchedSections.Contains(externalSection));
        }

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionInAppConfigValuesAreUpdatedIfAppConfigChangesAutomatically()
        //{
        //    ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);

        //    IConfigurationSourceTest source = new SystemConfigurationSource(true);

        //    object section1 = source.GetSection(localSection);
        //    Assert.IsNotNull(section1);
        //    DummySection dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(localSection, dummySection1.Name);
        //    Assert.AreEqual(10, dummySection1.Value);

        //    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    DummySection rwSection = rwConfiguration.GetSection(localSection) as DummySection;
        //    rwSection.Value = 15;
        //    rwConfiguration.Save();

        //    Thread.Sleep(150);

        //    section1 = source.GetSection(localSection);
        //    Assert.IsNotNull(section1);
        //    dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(localSection, dummySection1.Name);
        //    Assert.AreEqual(15, dummySection1.Value);
        //}

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionInExternalFileValuesAreUpdatedIfExternalFileChangesAutomatically()
        //{
        //    ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);

        //    IConfigurationSourceTest source = new SystemConfigurationSource(true);

        //    object section1 = source.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    DummySection dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(20, dummySection1.Value);

        //    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    DummySection rwSection = rwConfiguration.GetSection(externalSection) as DummySection;
        //    rwSection.Value = 25;
        //    rwConfiguration.Save();

        //    Thread.Sleep(150);

        //    section1 = source.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(25, dummySection1.Value);
        //}

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionChangingFromAppConfigToExternalFileIsAppropriatelyWatched()
        //{
        //}

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionChangingFromExternalFileToAppConfigIsAppropriatelyWatched()
        //{
        //    IConfigurationSourceTest source = new SystemConfigurationSource(false);

        //    object section1 = source.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    DummySection dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(20, dummySection1.Value);
        //    Assert.AreEqual(externalSectionSource, dummySection1.SectionInformation.ConfigSource);

        //    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    DummySection rwSection = rwConfiguration.GetSection(externalSection) as DummySection;
        //    rwSection.Value = 25;
        //    rwSection.SectionInformation.ConfigSource = localSectionSource;
        //    rwConfiguration.Save();

        //    // what changed is the app.config/web.config file
        //    source.ConfigSourceChanged(localSectionSource);

        //    section1 = source.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(25, dummySection1.Value);
        //    Assert.AreEqual(localSectionSource, dummySection1.SectionInformation.ConfigSource);
        //}

        [TestMethod]
        public void RegisteredObjectIsNotifiedOfSectionChangesForAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(localSection);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[localSection]);
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfSectionChangesForAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(localSection);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(3, updatedSectionsTally[localSection]);
        }

        [TestMethod]
        public void RegisteredObjectForNonRequestedSectionIsNotNotified()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ConfigSourceChanged(localSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(localSection));
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfDifferentSectionsChangesForAppConfig()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(localSection);
            source.GetSection(localSection2);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void RegisteredObjectIsNotifiedOfSectionChangesForExternalFile()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(externalSection);
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ExternalConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfSectionChangesForExternalFile()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(externalSection);
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ExternalConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(3, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void RegisteredObjectForExternalFileIsNotNotifiedOfSectionChangesForAppConfigIfConfigSourceForExternalSectionHasNotChanged()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(externalSection);
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ConfigSourceChanged(localSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(externalSection));
        }

        [TestMethod]
        public void RegisteredObjectForExternalFileIsNotifiedOfSectionChangesForAppConfigIfConfigSourceForExternalSectionNotChanged()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(externalSection);
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            source.ConfigSourceChanged(localSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(externalSection));
        }

        [TestMethod]
        public void CanAddAndRemoveHandlers()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            object section = source.GetSection(externalSection);
            Assert.IsNotNull(section);

            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[externalSection]);

            source.RemoveSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[externalSection]);

            source.RemoveSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[externalSection]);
        }

        // Watchers are removed if section changed source and none left (except for "").
        // watchers aren't removed if section changed source and other left.
        [TestMethod]
        public void FirstRequestForSectionGetsFreshInformation()
        {
            DummySection section1, section2;

            section1 = ConfigurationManager.GetSection(localSection) as DummySection;
            Assert.AreEqual(10, section1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection1 = rwConfiguration.GetSection(localSection) as DummySection;
            rwSection1.Value = 15;
            DummySection rwSection2 = rwConfiguration.GetSection(localSection2) as DummySection;
            rwSection2.Value = 25;
            rwConfiguration.Save();

            section1 = ConfigurationManager.GetSection(localSection) as DummySection;
            section2 = ConfigurationManager.GetSection(localSection2) as DummySection;
            Assert.AreEqual(10, section1.Value); // gets old value for cached section
            Assert.AreEqual(25, section2.Value); // gets new value for fresh section
        }

        [TestMethod]
        public void RemovedSectionGetsNotificationOnRemovalAndDoesNotGetFurtherNotifications()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(localSection);
            Assert.IsNotNull(section1);
            object section2 = source.GetSection(localSection2);
            Assert.IsNotNull(section2);

            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            // a change in system config notifies both sections
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);

            // removal of the section notifies both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // further updates only notify the remaining section
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void RestoredSectionGetsNotificationOnRestoreAndGetsFurtherNotifications()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);

            object section1 = source.GetSection(localSection);
            Assert.IsNotNull(section1);
            object section2 = source.GetSection(localSection2);
            Assert.IsNotNull(section2);

            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            source.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            // a change in system config notifies both sections
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);

            // removal of the section notifies both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // further updates only notify the remaining section
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // restore of section gets notified
            DummySection rwSection = new DummySection();
            rwSection.Name = localSection2;
            rwSection.Value = 30;
            rwSection.SectionInformation.ConfigSource = localSectionSource;
            rwConfiguration.Sections.Add(localSection2, rwSection);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(4, updatedSectionsTally[localSection]);
            Assert.AreEqual(3, updatedSectionsTally[localSection2]);

            // further updates notify both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            source.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(5, updatedSectionsTally[localSection]);
            Assert.AreEqual(4, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void GetsNotifiedOfSingleRetrievedSection()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(localSection);
            source.SourceChanged += this.OnConfigurationSourceChanged;

            source.ConfigSourceChanged(localSectionSource);

            CollectionAssert.AreEquivalent(new[] { ConfigurationSourceSection.SectionName, localSection }, new List<string>(updatedSections));
        }

        [TestMethod]
        public void GetsNotifiedOfMultipleRetrievedSection()
        {
            IConfigurationSourceTest source = new SystemConfigurationSource(false);
            source.GetSection(localSection);
            source.GetSection(localSection2);
            source.SourceChanged += this.OnConfigurationSourceChanged;

            source.ConfigSourceChanged(localSectionSource);

            CollectionAssert.AreEquivalent(new[] { ConfigurationSourceSection.SectionName, localSection, localSection2 }, new List<string>(updatedSections));
        }

        void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
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
            this.updatedSections = args.ChangedSectionNames;
        }
    }
}
