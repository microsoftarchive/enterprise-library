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
using System.Configuration;
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class FileConfigurationSourceFixture
    {
        const string localSection = "dummy.local";
        const string localSectionSource = "";
        const string externalSection = "dummy.external";
        const string protectedSection = "dummy.protected";
        const string externalSectionSource = "dummy.external.config";

        [TestMethod]
        public void SectionsCanBeAccessedThroughFileConfigurationSource()
        {
            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string otherConfigurationFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(fullConfigurationFilepath, otherConfigurationFilepath);

            try
            {
                using (FileConfigurationSource otherConfiguration =
                    new FileConfigurationSource(otherConfigurationFilepath, false))
                {
                    DummySection dummySection = otherConfiguration.GetSection(localSection) as DummySection;

                    Assert.IsNotNull(dummySection);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFilepath))
                {
                    File.Delete(otherConfigurationFilepath);
                }
            }
        }

        [TestMethod]
        public void CanUseRelativePathToBaseDirectory()
        {
            var currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.GetTempPath();

            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string otherConfigurationFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(fullConfigurationFilepath, otherConfigurationFilepath);

            try
            {
                using (FileConfigurationSource otherConfiguration =
                    new FileConfigurationSource("Other.config", false))
                {
                    DummySection dummySection = otherConfiguration.GetSection(localSection) as DummySection;

                    Assert.IsNotNull(dummySection);
                }
            }
            finally
            {
                Environment.CurrentDirectory = currentDirectory;
                if (File.Exists(otherConfigurationFilepath))
                {
                    File.Delete(otherConfigurationFilepath);
                }
            }
        }

        [TestMethod]
        public void NonExistentSectionReturnsNullThroughFileConfigurationSource()
        {
            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string otherConfigurationFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(fullConfigurationFilepath, otherConfigurationFilepath);

            try
            {
                using (FileConfigurationSource otherConfiguration =
                    new FileConfigurationSource(otherConfigurationFilepath, false))
                {
                    object wrongSection = otherConfiguration.GetSection("wrong section");

                    Assert.IsNull(wrongSection);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFilepath))
                {
                    File.Delete(otherConfigurationFilepath);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatingFileConfigurationSourceWithNullArgumentThrows()
        {
            new FileConfigurationSource(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreatingFileConfigurationSourceForNonExistingRelativeFileThrows()
        {
            new FileConfigurationSource("this.config.file.doesnt.exist.config");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreatingFileConfigurationSourceForNonExistingRootedFileThrows()
        {
            new FileConfigurationSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "this.config.file.doesnt.exist.config"));
        }

        [TestMethod]
        public void DifferentFileConfigurationSourcesDoNotShareEvents()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

            try
            {
                bool sysSourceChanged = false;
                bool otherSourceChanged = false;

                using (SystemConfigurationSource systemSource = new SystemConfigurationSource(true, 50))
                using (FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile, true, 50))
                {
                    DummySection sysDummySection = systemSource.GetSection(localSection) as DummySection;
                    DummySection otherDummySection = otherSource.GetSection(localSection) as DummySection;
                    Assert.IsTrue(sysDummySection != null);
                    Assert.IsTrue(otherDummySection != null);

                    systemSource.AddSectionChangeHandler(
                        localSection,
                        delegate(object o, ConfigurationChangedEventArgs args)
                        {
                            sysSourceChanged = true;
                        });

                    otherSource.AddSectionChangeHandler(
                        localSection,
                        delegate(object o, ConfigurationChangedEventArgs args)
                        {
                            Assert.AreEqual(12, ((DummySection)otherSource.GetSection(localSection)).Value);
                            otherSourceChanged = true;
                        });

                    DummySection rwSection = new DummySection();
                    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(otherConfigurationFile);
                    rwConfiguration.Sections.Remove(localSection);
                    rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
                    rwSection.Name = localSection;
                    rwSection.Value = 12;
                    rwSection.SectionInformation.ConfigSource = localSectionSource;

                    rwConfiguration.SaveAs(otherConfigurationFile);

                    Thread.Sleep(500);

                    Assert.AreEqual(false, sysSourceChanged);
                    Assert.AreEqual(true, otherSourceChanged);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void RemovingSectionCausesChangeNotification()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

            try
            {
                bool otherSourceChanged = false;

                using (FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile, true, 50))
                {
                    DummySection otherDummySection = otherSource.GetSection(localSection) as DummySection;
                    Assert.IsTrue(otherDummySection != null);

                    otherSource.AddSectionChangeHandler(
                        localSection,
                        delegate(object o, ConfigurationChangedEventArgs args)
                        {
                            Assert.IsNull(otherSource.GetSection(localSection));
                            otherSourceChanged = true;
                        });

                    otherSource.Remove(localSection);

                    Thread.Sleep(300);

                    Assert.AreEqual(true, otherSourceChanged);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        [Ignore]
        // this test relied on lazily initialization of the Configuration object, and would only work if changes
        // happened before any section was requested
        public void ReadsLatestVersionOnFirstRequest()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");

            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

                using (FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile, false))
                {
                    DummySection rwSection = null;
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = otherConfigurationFile;
                    System.Configuration.Configuration rwConfiguration =
                        ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    rwConfiguration.Sections.Remove(localSection);
                    rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
                    rwSection.Name = localSection;
                    rwSection.Value = 12;
                    rwSection.SectionInformation.ConfigSource = localSectionSource;

                    rwConfiguration.Save();

                    DummySection otherSection = otherSource.GetSection(localSection) as DummySection;
                    Assert.IsNotNull(otherSection);
                    Assert.AreEqual(12, otherSection.Value);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void AddIsReflectedInNextRequestWithoutRefresh()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");

            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

                using (FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile, false))
                {
                    DummySection otherSection = otherSource.GetSection(localSection) as DummySection;

                    // update twice, just to make sure
                    DummySection newSection = new DummySection();
                    newSection.Value = 13;
                    otherSource.Add(localSection, newSection);

                    newSection = new DummySection();
                    newSection.Value = 12;
                    otherSource.Add(localSection, newSection);

                    otherSection = otherSource.GetSection(localSection) as DummySection;
                    Assert.IsNotNull(otherSection);
                    Assert.AreEqual(12, otherSection.Value);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void RemoveIsReflectedInNextRequestWithoutRefresh()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            
            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

                using (FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile, false))
                {
                    DummySection otherSection = otherSource.GetSection(localSection) as DummySection;

                    DummySection newSection = new DummySection();
                    newSection.Value = 13;
                    otherSource.Add(localSection, newSection);

                    otherSource.Remove(localSection);

                    otherSection = otherSource.GetSection(localSection) as DummySection;
                    Assert.IsNull(otherSection);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void ChangeInExternalConfigSourceIsDetected()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");

            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

                using (FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile, true, 50))
                {
                    DummySection rwSection;
                    System.Configuration.Configuration rwConfiguration =
                        ConfigurationManager.OpenExeConfiguration(otherConfigurationFile);
                    rwConfiguration.Sections.Remove(externalSection);
                    rwConfiguration.Sections.Add(externalSection, rwSection = new DummySection());
                    rwSection.Name = externalSection;
                    rwSection.Value = 12;
                    rwSection.SectionInformation.ConfigSource = externalSectionSource;
                    rwConfiguration.Save(ConfigurationSaveMode.Full);

                    DummySection otherSection = otherSource.GetSection(externalSection) as DummySection;
                    Assert.AreEqual(12, otherSection.Value);

                    rwSection.Value = 13;
                    rwConfiguration.Save(ConfigurationSaveMode.Modified);

                    Thread.Sleep(300);

                    otherSection = otherSource.GetSection(externalSection) as DummySection;
                    Assert.AreEqual(13, otherSection.Value);
                }
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SavingWithEmptyProtectionProviderThrowsArgumentException()
        {
            string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            FileConfigurationSource fileConfigSource = new FileConfigurationSource(configurationFile, false);

            DummySection newSection = new DummySection();

            fileConfigSource.Add(localSection, newSection, string.Empty);
        }

        [TestMethod]
        public void AddingUnprotectedSectionsWithProtectionProviderWillProtectThem()
        {
            DummySection dummySection = new DummySection();

            string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            FileConfigurationSource fileConfigSource = new FileConfigurationSource(configurationFile, false);

            fileConfigSource.Add(protectedSection, dummySection, ProtectedConfiguration.DefaultProvider);

            ConfigurationSection section = fileConfigSource.GetSection(protectedSection);

            Assert.IsTrue(section.SectionInformation.IsProtected);
            Assert.IsNotNull(section.SectionInformation);
            Assert.AreEqual(ProtectedConfiguration.DefaultProvider, section.SectionInformation.ProtectionProvider.Name);
        }
    }
}
