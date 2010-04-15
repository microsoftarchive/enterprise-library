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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_CompositeConfigurationSource : ArrangeActAssert
    {
        protected TestConfigurationSource MainSource;
        protected TestConfigurationSource ChildSource1;
        protected TestConfigurationSource ChildSource2;
        protected string SectionInChildSource1 = "sectionInChild1";

        protected TestCompositeConfigurationSourceHandler CompositionHandler;
        protected ConfigurationSourceEventListener MainSourceEventListener;

        protected override void Arrange()
        {
            ConfigurationSourceSection configurationSourcesSection = CreateConfigurationSourceSection();

            MainSource = new TestConfigurationSource();
            MainSource.Add(ConfigurationSourceSection.SectionName, configurationSourcesSection);

            CompositionHandler = new TestCompositeConfigurationSourceHandler(MainSource);
            MainSource.SetCompositeHandler(CompositionHandler);

            ChildSource1 = (TestConfigurationSource)CompositionHandler["Source1"];
            ChildSource2 = (TestConfigurationSource)CompositionHandler["Source2"];

            MainSourceEventListener = new ConfigurationSourceEventListener(MainSource);

            ChildSource1.Add(SectionInChildSource1, new DummySection { Name = SectionInChildSource1, Value = 15 });
        }

        protected ConfigurationSourceSection CreateConfigurationSourceSection()
        {
            ConfigurationSourceSection configurationSourcesSection = new ConfigurationSourceSection();
            configurationSourcesSection.Sources.Add(new TestConfigurationSourceElement("Source1"));
            configurationSourcesSection.Sources.Add(new TestConfigurationSourceElement("Source2"));
            configurationSourcesSection.RedirectedSections.Add(new RedirectedSectionElement
            {
                Name = "sectionInChild1",
                SourceName = "Source1"
            });
            configurationSourcesSection.RedirectedSections.Add(new RedirectedSectionElement
            {
                Name = "SectionInSource2",
                SourceName = "Source2"
            });

            return configurationSourcesSection;
        }

        public class TestCompositeConfigurationSourceHandler : CompositeConfigurationSourceHandler
        {
            public TestCompositeConfigurationSourceHandler(IConfigurationSource mainConfigurationSource)
                : base(mainConfigurationSource)
            {

            }

            public int RefreshCallCount = 0;

            protected override void DoRefresh()
            {
                ++RefreshCallCount;
                base.DoRefresh();
            }

            public IConfigurationSource this[string configurationSourceName]
            {
                get
                {
                    return base.GetSubordinateSource(configurationSourceName);
                }
            }
        }

        protected class ConfigurationSourceEventListener
        {
            public int ConfigurationSourceChangedRaiseCount;
            public ConfigurationSourceChangedEventArgs LastConfigurationSourceChangedEventArgs;

            IConfigurationSource configurationSourcetoListenTo;

            public ConfigurationSourceEventListener(IConfigurationSource configurationSourcetoListenTo)
            {
                this.configurationSourcetoListenTo = configurationSourcetoListenTo;
                configurationSourcetoListenTo.SourceChanged += new EventHandler<ConfigurationSourceChangedEventArgs>(configurationSourcetoListenTo_SourceChanged);
            }

            void configurationSourcetoListenTo_SourceChanged(object sender, ConfigurationSourceChangedEventArgs e)
            {
                ConfigurationSourceChangedRaiseCount++;
                LastConfigurationSourceChangedEventArgs = e;
            }
        }
    }

    [TestClass]
    public class When_ChildSourceRaisesConfigurationSourceChangedEvent : Given_CompositeConfigurationSource
    {
        protected override void Act()
        {
            DummySection dummySectionBeforeChange = (DummySection) MainSource.GetSection(SectionInChildSource1);

            ChildSource1.Remove(SectionInChildSource1);
            ChildSource1.Add(SectionInChildSource1, new DummySection { Name = "new name", Value = 16 });
            ChildSource1.DoSourceChanged( new[]{SectionInChildSource1});
        }

        [TestMethod]
        public void Then_CompositeSourceRaisesConfigurationChangedEvent()
        {
            Assert.AreEqual(1, MainSourceEventListener.ConfigurationSourceChangedRaiseCount);
            Assert.IsTrue(MainSourceEventListener.LastConfigurationSourceChangedEventArgs.ChangedSectionNames.Any(x => x == SectionInChildSource1));
        }
        
        [TestMethod]
        public void Then_CompositeSourceReturnsUpdatedConfigurationSection()
        {
            DummySection dummySectionAfterChange = (DummySection)MainSource.GetSection(SectionInChildSource1);
            Assert.AreEqual("new name", dummySectionAfterChange.Name);
        }
    }

    [TestClass]
    public class When_SectionInChildSourceIsMovedToOtherSource : Given_CompositeConfigurationSource
    {
        protected override void Act()
        {
            DummySection dummySectionBeforeChange = (DummySection)MainSource.GetSection(SectionInChildSource1);

            var sourcesSection =  CreateConfigurationSourceSection();
            var redirect = sourcesSection.RedirectedSections.Where(x => x.Name == SectionInChildSource1).First();
            redirect.SourceName = "Source2";

            MainSource.Remove(ConfigurationSourceSection.SectionName);
            MainSource.Add(ConfigurationSourceSection.SectionName, sourcesSection);
            MainSource.DoSourceChanged( new []{ConfigurationSourceSection.SectionName});


            try
            {
                MainSource.GetSection(SectionInChildSource1);
                Assert.Fail(); // Getting here indicates the section is still in original source
            }
            catch(ConfigurationSourceErrorsException)
            {
                // not caught intentionally
            }

            ChildSource2 = (TestConfigurationSource)CompositionHandler["Source2"];
            ChildSource2.Add(SectionInChildSource1, new DummySection { Name = "SectionInSource2" });
        }


        [TestMethod]
        public void Then_CompositeSourceReturnsUpdatedSectionFromOtherSource()
        {
            DummySection dummySectionAfterChange = (DummySection)MainSource.GetSection(SectionInChildSource1);
            Assert.AreEqual("SectionInSource2", dummySectionAfterChange.Name);
        }
    }

    [TestClass]
    public class When_CompositeConfigurationSourceIsDisposed : Given_CompositeConfigurationSource
    {
        protected override void Act()
        {
            //setup
            var section = MainSource.GetSection(SectionInChildSource1);

            CompositionHandler.Dispose();
        }

        [TestMethod]
        public void Then_SubordinateConfigurationSourcesAreDisposed()
        {
            Assert.AreEqual(1, ChildSource1.DisposeCallCount);
            Assert.AreEqual(1, ChildSource2.DisposeCallCount);
        }

        [TestMethod]
        public void Then_CompositeSourceStopsListeningToConfigurationSourcesSection()
        {
            var eventHandlerOnPreviousSource = MainSource.SectionChangedHandlers[ConfigurationSourceSection.SectionName];
            Assert.IsNull(eventHandlerOnPreviousSource);
        }
    }

    [TestClass]
    public class When_ConfigurationCompositeSourcesSectionChanges : Given_CompositeConfigurationSource
    {
        protected override void Act()
        {
            //setup
            var section = MainSource.GetSection(SectionInChildSource1);
            
            MainSource.DoSourceChanged( new[]{ConfigurationSourceSection.SectionName});
        }

        [TestMethod]
        public void Then_CompositeConfigurationSourceIsRefreshed()
        {
            Assert.AreEqual(1, CompositionHandler.RefreshCallCount);
        }

        [TestMethod]
        public void Then_SubordinateConfigurationSourcesAreDisposed()
        {
            Assert.AreEqual(1, ChildSource1.DisposeCallCount);
            Assert.AreEqual(1, ChildSource2.DisposeCallCount);
        }

        [TestMethod]
        public void Then_CompositeConfigurationSourceStillListensToSectionChangedOnSubordinateSource()
        {
            bool called = false;
            MainSource.AddSectionChangeHandler(SectionInChildSource1, (sender, args) => called = true);


            ChildSource1 = (TestConfigurationSource)CompositionHandler["Source1"];
            ChildSource1.DoSourceSectionChanged(SectionInChildSource1);

            Assert.IsTrue(called);
        }
    }

    [TestClass]
    public class When_RemovingAndAddingSectionWithSourceToCompositeConfigurationSource : Given_CompositeConfigurationSource
    {
        DummySection section;

        protected override void Arrange()
        {
            base.Arrange();

            section = new DummySection
            {
                Name = "NewSection",
                Value = 21
            };
        }

        protected override void Act()
        {
            MainSource.Remove(SectionInChildSource1);
            MainSource.Add(SectionInChildSource1, section);
        }

        [TestMethod]
        public void Then_SectionEndsUpInAppropriateSource()
        {
            DummySection sectionFromSource1 = (DummySection)ChildSource1.GetSection(SectionInChildSource1);
            Assert.IsNotNull( sectionFromSource1 );
            Assert.AreEqual("NewSection", sectionFromSource1.Name);
        }

        [TestMethod]
        public void Then_SectionCanBeAccessedFromMainSource()
        {
            DummySection sectionFromMainSource = (DummySection)MainSource.GetSection(SectionInChildSource1);
        }
    }

    [TestClass]
    public class When_SubordinateConfigurationSourceChanges : Given_CompositeConfigurationSource
    {
        protected override void Act()
        {
            ChildSource1.DoSourceChanged(new []{"section1"});
        }

        protected override void Teardown()
        {
            TestConfigurationSource.ConfigurationSourceContents.Remove("section1");
            base.Teardown();
        }

        [TestMethod]
        public void Then_MainSourceRaisedChange()
        {
            Assert.AreEqual(1, this.MainSourceEventListener.ConfigurationSourceChangedRaiseCount);
        }

        [TestMethod]
        public void Then_MainSourceKeepsRaisingEventsAfterReset()
        {
            TestConfigurationSource.ConfigurationSourceContents["section1"] = new DummySection();

            //force reset
            MainSource.DoSourceChanged( new []{ConfigurationSourceSection.SectionName});
            Assert.AreEqual(1, CompositionHandler.RefreshCallCount);
            this.MainSourceEventListener.ConfigurationSourceChangedRaiseCount = 0; //reset counter

            var childSource = (TestConfigurationSource) CompositionHandler["Source1"];
            childSource.DoSourceChanged(new[] { "section1" });

            Assert.IsTrue(MainSourceEventListener.ConfigurationSourceChangedRaiseCount > 0);
        }
    }

    [TestClass]
    public class When_RequestingMissingSections : Given_CompositeConfigurationSource
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationSourceErrorsException))]
        public void then_throws_on_registered_but_missing_sections()
        {
            ConfigurationSection section = MainSource.GetSection("SectionInSource2");
        }

        [TestMethod]
        public void then_does_not_throw_on_unregistered_missing_sections()
        {
            ConfigurationSection section = MainSource.GetSection("UnregisteredSection");
            Assert.IsNull(section);
        }
    }
}
