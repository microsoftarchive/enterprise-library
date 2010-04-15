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

using System.Collections.Specialized;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Linq;
using System.Windows.Documents;
using System.Collections.Generic;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_lookup
{
    public abstract class DatabaseContext : ContainerContext
    {
        protected internal ElementLookup ElementLookup { get; private set; }
        protected internal DatabaseSettings DatabaseSection { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new ConfigurationSourceBuilder();
            builder.ConfigureData()
                    .ForDatabaseNamed("someDatabase")
                    .AsDefault();

            var source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            ElementLookup = Container.Resolve<ElementLookup>();
            DatabaseSection = (DatabaseSettings)source.GetSection(DatabaseSettings.SectionName);
        }
    }

    public abstract class LoggingContext : ContainerContext
    {
        protected internal ElementLookup ElementLookup { get; private set; }
        protected internal LoggingSettings LoggingSection { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new ConfigurationSourceBuilder();
            builder.ConfigureLogging()
                        .LogToCategoryNamed("category")
                            .SendTo
                                .EventLog("listener")
                        .SpecialSources
                            .AllEventsCategory
                                .SendTo
                                    .EventLog("listener")
                        .SpecialSources
                            .LoggingErrorsAndWarningsCategory
                                .SendTo
                                    .EventLog("listener")
                        .SpecialSources
                            .UnprocessedCategory
                                .SendTo
                                    .EventLog("listener");


            var source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            ElementLookup = Container.Resolve<ElementLookup>();
            LoggingSection = (LoggingSettings)source.GetSection(LoggingSettings.SectionName);
        }
    }

    [TestClass]
    public class when_adding_a_new_element_with_child_elements : LoggingContext
    {
        private int addedElementCount = 0;
        private int interestedChildCreatedCount = 0;
        private SectionViewModel section;

        protected override void Arrange()
        {
            base.Arrange();

            ElementLookup.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ElementLookup_CollectionChanged);
        }

        void ElementLookup_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    ElementViewModel newElement = item as ElementViewModel;

                    if (newElement != null)
                    {
                        addedElementCount++;

                        if (newElement.ConfigurationType == typeof(TraceListenerReferenceData))
                        {
                            interestedChildCreatedCount++;
                        }
                    }
                }
            }
        }

        protected override void Act()
        {
            section = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, LoggingSection);
            ElementLookup.AddSection(section);
        }

        [TestMethod]
        public void then_events_are_correctly_raised()
        {
            Assert.AreEqual(4, interestedChildCreatedCount);
        }
    }

    [TestClass]
    public class when_adding_a_new_element_without_children : DatabaseContext
    {
        private int addedElementCount = 0;
        protected override void Arrange()
        {
            base.Arrange();

            ElementLookup.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ElementLookup_CollectionChanged);
        }

        void ElementLookup_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    ElementViewModel newElement = item as ElementViewModel;

                    if (newElement != null)
                    {
                        addedElementCount++;
                    }
                }
            }
        }

        protected override void Act()
        {
            var section = SectionViewModel.CreateSection(Container, DatabaseSettings.SectionName, DatabaseSection);
            ElementLookup.AddSection(section);
        }

        [TestMethod]
        public void then_events_are_correctly_raised()
        {
            Assert.AreEqual(2, addedElementCount);
        }
    }
}
