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
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_ConfigurationSourceHandler :  ArrangeActAssert
    {
        protected TestConfigurationSource Source;
        protected TestableConfigurationSourceHandler ConfigurationSourceHandler;
        protected TestConfigurationSource SubordinateSource1;
        protected string SubordinateSource1Name = "SubordinateSource1";
        protected override void Arrange()
        {
            Source = new TestConfigurationSource();
            ConfigurationSourceHandler = new TestableConfigurationSourceHandler(Source);

            SubordinateSource1 = new TestConfigurationSource();

            ConfigurationSourceHandler.DoAddCustomSubordinateSource(SubordinateSource1Name, SubordinateSource1);
        }

        protected class TestableConfigurationSourceHandler : ConfigurationSourceHandler
        {
            public TestableConfigurationSourceHandler(IConfigurationSource source)
                :base(source)
            {

            }

            public void DoAddCustomSubordinateSource(string sourceName, IConfigurationSource configurationSource)
            {
                base.AddCustomSubordinateSource(sourceName, configurationSource);
            }

            public void DoEnsureInitialize()
            {
                var a = base.CheckGetSection("force init", null);
            }

            public void DoStopPropagatingSectionChangeEvents(string sectionName)
            {
                base.StopPropagatingSectionChangeEvents(sectionName);
            }

            public void DoEnsurePropagatingSectionChangeEvents(string sourceName, string sectionName)
            {
                base.EnsurePropagatingSectionChangeEvents(sourceName, sectionName);
            }

            public void DoPublicRefresh()
            {
                base.Refresh();
            }

            public IConfigurationSource DoGetConfigurationSource(string sourceName)
            {
                return base.GetSubordinateSource(sourceName);
            }


            public int RefreshCount;
            protected override void DoRefresh()
            {
                base.DoRefresh();
                ++RefreshCount;
            }
        }
    }

    public abstract class Given_ConfigurationSourceHandlerWithConfigurationSourcesSection : Given_ConfigurationSourceHandler
    {
        protected override void Arrange()
        {
            Source = new TestConfigurationSource();
            Source.Add(ConfigurationSourceSection.SectionName, CreateConfigurationSourceSection());

            ConfigurationSourceHandler = new TestableConfigurationSourceHandler(Source);

            SubordinateSource1 = ConfigurationSourceHandler.DoGetConfigurationSource(SubordinateSource1Name) as TestConfigurationSource;

        }

        protected virtual ConfigurationSourceSection CreateConfigurationSourceSection()
        {
            ConfigurationSourceSection section = new ConfigurationSourceSection();
            section.Sources.Add(new TestConfigurationSourceElement(SubordinateSource1Name));

            return section;
        }
    }

    [TestClass]
    public class When_ConfigurationSourceIsInitialized : Given_ConfigurationSourceHandler
    {
        protected override void Act()
        {
            ConfigurationSourceHandler.DoEnsureInitialize();
        }

        [TestMethod]
        public void Then_HandlerRegisteresSourceChangeHandler()
        {
            var handler = Source.SourceChangedHandler;
            Assert.AreEqual(1, handler.GetInvocationList().Count());
        }

        [TestMethod]
        public void Then_InitializingAgainDoesntAddAnotherHandler()
        {
            ConfigurationSourceHandler.DoEnsureInitialize();
            var handler = Source.SourceChangedHandler;
            Assert.AreEqual(1, handler.GetInvocationList().Count());
        }

    }

    [TestClass]
    public class When_ConfigurationSourcesSectionChangesForUninitializedHandler : Given_ConfigurationSourceHandler
    {
        protected override void Act()
        {
            base.Source.DoSourceChanged(new[] { ConfigurationSourceSection.SectionName });
        }

        [TestMethod]
        public void Then_ConfigurationSourceHandlerIsNotRefreshed()
        {
            Assert.AreEqual(0, ConfigurationSourceHandler.RefreshCount);
        }
    }

    [TestClass]
    public class When_ConfigurationSourcesSectionChanges : Given_ConfigurationSourceHandler
    {
        protected override void Act()
        {
            ConfigurationSourceHandler.DoEnsureInitialize();

            base.Source.DoSourceChanged(new[] { ConfigurationSourceSection.SectionName });
        }

        [TestMethod]
        public void Then_ConfigurationSourceHandlerIsRefreshed()
        {
            Assert.AreEqual(1, ConfigurationSourceHandler.RefreshCount);
        }
    }

    [TestClass]
    public class When_ConfigurationChangeHandlerIsRefreshed : Given_ConfigurationSourceHandler
    {
        ConfigurationSourceChangedEventArgs sourceChangedEvent;

        protected override void Act()
        {
            base.ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section1");
            base.ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section2");

            ConfigurationSourceHandler.ConfigurationSourceChanged += (sender, args) =>
            {
                sourceChangedEvent = args;
            };

            base.ConfigurationSourceHandler.DoPublicRefresh();

        }

        [TestMethod]
        public void Then_SourceChangedEventIsFiredForWatchedSections()
        {
            Assert.IsNotNull(sourceChangedEvent);
            Assert.IsTrue(sourceChangedEvent.ChangedSectionNames.Any(x => x == "Section1"));
            Assert.IsTrue(sourceChangedEvent.ChangedSectionNames.Any(x => x == "Section2"));
        }

        [TestMethod]
        public void Then_SubordinateSourceIsNotDisposd()
        {
            Assert.AreEqual(0, SubordinateSource1.DisposeCallCount);
        }
    }

    [TestClass]
    public class When_ConfigurationChangeHandlerIsRefreshedThroughConfiguration : Given_ConfigurationSourceHandlerWithConfigurationSourcesSection
    {
        ConfigurationSourceChangedEventArgs sourceChangedEvent;

        protected override void Act()
        {
            base.ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section1");
            base.ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section2");

            ConfigurationSourceHandler.ConfigurationSourceChanged += (sender, args) =>
            {
                sourceChangedEvent = args;
            };

            base.ConfigurationSourceHandler.DoPublicRefresh();

        }

        [TestMethod]
        public void Then_SourceChangedEventIsFiredForWatchedSections()
        {
            Assert.IsNotNull(sourceChangedEvent);
            Assert.IsTrue(sourceChangedEvent.ChangedSectionNames.Any(x => x == "Section1"));
            Assert.IsTrue(sourceChangedEvent.ChangedSectionNames.Any(x => x == "Section2"));
        }

        [TestMethod]
        public void Then_SubordinateSourceIsDisposd()
        {
            Assert.AreEqual(1, SubordinateSource1.DisposeCallCount);
        }

        [TestMethod]
        public void Then_NewSubordinateSourceIsCreated()
        {
            var newSubordinateSource = (TestConfigurationSource)base.ConfigurationSourceHandler.DoGetConfigurationSource(SubordinateSource1Name);
            Assert.AreEqual(0, newSubordinateSource.DisposeCallCount);
        }

        [TestMethod]
        public void Then_SourceChangedEventsAreStillPropagated()
        {
            bool sourceChangedWasCalled = false;
            ConfigurationSourceHandler.ConfigurationSourceChanged += (sender, args) =>
                {
                    sourceChangedWasCalled = true;
                };

            var newSubordinateSource = (TestConfigurationSource)base.ConfigurationSourceHandler.DoGetConfigurationSource(SubordinateSource1Name);
            newSubordinateSource.DoSourceChanged(new[] { "abc" });

            Assert.IsTrue(sourceChangedWasCalled);
        }

        [TestMethod]
        public void Then_SectionChangeEventsAreStillPropagated()
        {
            bool sectionChangeWasFired = false;
            ConfigurationSourceHandler.ConfigurationSectionChanged += (sender, args) => sectionChangeWasFired = true;

            var newSubordinateSource = (TestConfigurationSource)base.ConfigurationSourceHandler.DoGetConfigurationSource(SubordinateSource1Name);
            newSubordinateSource.DoSourceSectionChanged("Section2");

            Assert.IsTrue(sectionChangeWasFired);

        }

    }

    [TestClass]
    public class When_ConfigurationChangeHandlerStopsWatchingSection : Given_ConfigurationSourceHandler
    {
        private bool sectionChangeWasFired;

        protected override void Act()
        {

            base.ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section1");
            base.ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section2");

            base.ConfigurationSourceHandler.DoStopPropagatingSectionChangeEvents("Section2");
            base.ConfigurationSourceHandler.ConfigurationSectionChanged += (sender, args) => sectionChangeWasFired = true;


            SubordinateSource1.DoSourceSectionChanged("Section2");
        }

        [TestMethod]
        public void Then_ChangeEventsAreNorPropagatedAnyMore()
        {
            Assert.IsFalse(sectionChangeWasFired);
        }
    }

    [TestClass]
    public class When_ConfigurationIsRemovedInConfigurationConfiguration : Given_ConfigurationSourceHandlerWithConfigurationSourcesSection
    {

        protected override void Act()
        {
            ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section1");
            ConfigurationSourceHandler.DoEnsurePropagatingSectionChangeEvents(SubordinateSource1Name, "Section2");

            Source.Remove(ConfigurationSourceSection.SectionName);
            Source.Add(ConfigurationSourceSection.SectionName, new ConfigurationSourceSection { });
            ConfigurationSourceHandler.DoPublicRefresh();
        }

        [TestMethod]
        public void DoesntThrow()
        {
        }
    }

}
