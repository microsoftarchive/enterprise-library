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
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_ConfigurationFileWithSectionsInOtherConfigurationSources : ArrangeActAssert
    {
        protected string LocaldummySectionName = "localdummy";
        protected string FileSourceDummySectionName = "externaldummy.filesource";
        protected string CustomSourceDummySectionName = "externaldummy.customsource";
        protected string PointsToSelfSourceDummySectionName = "localdummy.pointsToSelf";
        protected string NonExistingSourceDummy = "localdummy.pointsToNothing";
        protected IConfigurationSource CompositeSource;

        protected override void Arrange()
        {
            CompositeSource = new FileConfigurationSource(@"ComposedConfigurationFile.config");
        }

        protected override void Teardown()
        {
            using (CompositeSource) { }
        }
    }

    [TestClass]
    public class When_ReadingLocalSectionFromConfigurationSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        DummySection section;

        protected override void Act()
        {
            section = (DummySection)CompositeSource.GetSection(LocaldummySectionName);
        }

        [TestMethod]
        public void Then_ConfigurationSourceReturnsSection()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual(10, section.Value);
            Assert.AreEqual(LocaldummySectionName, section.Name);
        }
    }

    [TestClass]
    public class When_ReadingExternalSectionThroughFileConfigurationSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        DummySection section;

        protected override void Act()
        {
            section = (DummySection)CompositeSource.GetSection(FileSourceDummySectionName);
        }

        [TestMethod]
        public void Then_ConfigurationReturnsSectionFromExternalFileConfigurationSource()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual(11, section.Value);
            Assert.AreEqual(FileSourceDummySectionName, section.Name);
        }
    }

    [TestClass]
    public class When_ReadingSectionFromCustomConfigurationSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        DummySection sectionInCustomSource;
        DummySection section;

        protected override void Arrange()
        {
            base.Arrange();

            var customSourceContents = new Dictionary<string, System.Configuration.ConfigurationSection>();
            customSourceContents.Add(CustomSourceDummySectionName,
                                     sectionInCustomSource = new DummySection
                                        {
                                            Value = 12,
                                            Name = CustomSourceDummySectionName
                                        });

            TestConfigurationSource.ConfigurationSourceContents = customSourceContents;
        }

        protected override void Act()
        {
            section = (DummySection)CompositeSource.GetSection(CustomSourceDummySectionName);
        }

        [TestMethod]
        public void Then_ConfigurationReturnsSectionFromCustomSection()
        {
            Assert.AreEqual(sectionInCustomSource, section);
        }
    }

    [TestClass]
    public class When_WritngExternalSectionThroughFileConfigurationSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        string originalConfigurationFileContents;

        protected override void Arrange()
        {
            originalConfigurationFileContents = File.ReadAllText("ExternalFileSource.config");
            base.Arrange();
        }

        protected override void Act()
        {
            var section = (DummySection)CompositeSource.GetSection(FileSourceDummySectionName);
            CompositeSource.Remove(FileSourceDummySectionName);
            CompositeSource.Add(FileSourceDummySectionName, new DummySection
            {
                Value = 24,
                Name = "new name"
            });
        }

        [TestMethod]
        public void Then_ConfigurationSectionEndsUpInExternalSource()
        {
            var text = File.ReadAllText("ExternalFileSource.config");
            Assert.IsTrue(text.Contains("new name"));
        }


        [TestMethod]
        public void Then_CompositeReturnsNewSection()
        {
            var section = (DummySection)CompositeSource.GetSection(FileSourceDummySectionName);
            Assert.AreEqual("new name", section.Name);
            Assert.AreEqual(24, section.Value);
        }

        protected override void Teardown()
        {
            base.Teardown();

            File.WriteAllText(@"ExternalFileSource.config", originalConfigurationFileContents);
        }
    }

    [TestClass]
    public class When_ReadingConfigurationSectionThatPointsToSelfSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        [TestMethod]
        public void Then_MethodReturnsConfiguationSection()
        {
            DummySection section = (DummySection)CompositeSource.GetSection(PointsToSelfSourceDummySectionName);
            Assert.IsNotNull(section);
        }
    }

    [TestClass]
    public class When_ReadingSectionThatPointsToNonExistingSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationSourceErrorsException))]
        public void Then_GetSectionThrows_ConfigurationErrorsException()
        {
            CompositeSource.GetSection(NonExistingSourceDummy);
        }
    }

    [TestClass]
    public class When_ReadingSectionFromRecursiveSectionSource : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        DummySection section;
        string originalConfigurationFileContents;
        protected override void Arrange()
        {
            originalConfigurationFileContents = File.ReadAllText(@"ExternalFileSource.config");

            using (var externalConfigurationFileSource = new FileConfigurationSource(@"ExternalFileSource.config"))
            {
                externalConfigurationFileSource.Save(FileSourceDummySectionName, new DummySection { });
            }

            base.Arrange();
        }

        protected override void Act()
        {
            section = CompositeSource.GetSection(FileSourceDummySectionName) as DummySection;
        }

        [TestMethod]
        public void Then_ReturnsSectionFromFirstSource()
        {
            Assert.IsNotNull(section);
        }

        protected override void Teardown()
        {
            base.Teardown();

            File.WriteAllText(@"ExternalFileSource.config", originalConfigurationFileContents);
        }
    }

    [TestClass]
    public class When_ComposedConfigurationSourceChanges : Given_ConfigurationFileWithSectionsInOtherConfigurationSources
    {
        DummySection section;
        string originalConfigurationFileContents;
        int sourceChangedEvents = 0;
        int sectionChangedEvents = 0;
        private CountdownEvent waitForChangedEvents;

        protected override void Arrange()
        {
            waitForChangedEvents = new CountdownEvent(2);
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(1000);

            originalConfigurationFileContents = File.ReadAllText(@"ExternalFileSource.config");

            using (var externalConfigurationFileSource = new FileConfigurationSource(@"ExternalFileSource.config", false))
            {
                externalConfigurationFileSource.Save(FileSourceDummySectionName, new DummySection { });
            }

            base.Arrange();

            this.CompositeSource.SourceChanged +=
                (sender, e) =>
                {
                    sourceChangedEvents++;
                    waitForChangedEvents.Signal();
                };
            this.CompositeSource.AddSectionChangeHandler(
                FileSourceDummySectionName,
                (sender, e) =>
                {
                    sectionChangedEvents++;
                    waitForChangedEvents.Signal();
                });
        }

        protected override void Act()
        {
            section = CompositeSource.GetSection(FileSourceDummySectionName) as DummySection;

            File.SetLastWriteTime(@"ExternalFileSource.config", DateTime.Now);

            // Wait for at least two events
            Assert.IsTrue(waitForChangedEvents.Wait(30000), "timed out");

            // And give it a little more time in case more come in
            Thread.Sleep(3000);
        }

        [TestMethod]
        public void Then_ThereIsSingleSourceChangedEvent()
        {
            Assert.AreEqual(1, sourceChangedEvents);
        }

        [TestMethod]
        public void Then_ThereIsSingleSectionChangedEvent()
        {
            Assert.AreEqual(1, sectionChangedEvents);
        }

        protected override void Teardown()
        {
            base.Teardown();

            ConfigurationChangeWatcher.ResetDefaultPollDelay();

            File.WriteAllText(@"ExternalFileSource.config", originalConfigurationFileContents);
            waitForChangedEvents.Dispose();
        }
    }
}
