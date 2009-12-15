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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_configuration
{
    public abstract class given_logging_configuration : Contexts.ContainerContext
    {
        protected LoggingSettings LoggingSection;

        protected override void Arrange()
        {
            base.Arrange();

            IConfigurationSource source = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder sourceBuiler = new ConfigurationSourceBuilder();

            sourceBuiler.ConfigureLogging()
                .WithOptions.DisableTracing()
                            .DoNotRevertImpersonation()
                .FilterOnPriority("prio filter").StartingWithPriority(10)
                .FilterOnCategory("categoryFiler").AllowAllCategoriesExcept("cat1")
                .LogToCategoryNamed("General")
                        .SendTo.EventLog("Event Log Listener")
                                    .FormatWith(new FormatterBuilder().TextFormatterNamed("Default"))
                .LogToCategoryNamed("Critial")
                        .SendTo.SharedListenerNamed("Event Log Listener")
                        .SendTo.Custom<MyCustomListener>("Custom Listener")
                        .SendTo.Email("Email Listener")
                        .SendTo.SystemDiagnosticsListener("system diagnostics")
                .LogToCategoryNamed("msmq")
                        .SendTo.Msmq("msmq");

            sourceBuiler.UpdateConfigurationWithReplace(source);
            LoggingSection = (LoggingSettings)source.GetSection(LoggingSettings.SectionName);
        }

        private class MyCustomListener : CustomTraceListener
        {
            public override void Write(string message)
            {
                throw new NotImplementedException();
            }

            public override void WriteLine(string message)
            {
                throw new NotImplementedException();
            }
        }
    }

    [TestClass]
    public class when_creating_logging_view_model : given_logging_configuration
    {
        SectionViewModel loggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender(base.Container.Resolve<IServiceProvider>()));
        }

        protected override void Act()
        {
            loggingViewModel = SectionViewModel.CreateSection(base.Container, LoggingSettings.SectionName, LoggingSection);
        }

        [TestMethod]
        public void then_trace_listener_collection_has_custom_trace_listener_types()
        {
            var tracelistenerCollection =  (ElementCollectionViewModel) loggingViewModel.DescendentElements().Where(x => typeof(TraceListenerDataCollection) == x.ConfigurationType).First();
            Assert.IsTrue(tracelistenerCollection.PolymorphicCollectionElementTypes.Contains(typeof(SystemDiagnosticsTraceListenerData)));
            Assert.IsTrue(tracelistenerCollection.PolymorphicCollectionElementTypes.Contains(typeof(CustomTraceListenerData)));
        }

        [TestMethod]
        public void then_view_model_has_category_with_name_AllEvents()
        {
            Assert.IsTrue(loggingViewModel.DescendentElements().Where(x => x.Name == "All Events").Any());
        }

        [TestMethod]
        public void then_view_model_has_category_with_name_ErrorsAndWarnings()
        {
            Assert.IsTrue(loggingViewModel.DescendentElements().Where(x => x.Name == "Logging Errors & Warnings").Any());
        }

        [TestMethod]
        public void then_view_model_has_category_with_name_UnProcessed()
        {
            Assert.IsTrue(loggingViewModel.DescendentElements().Where(x => x.Name == "Unprocessed Category").Any());
        }

        [TestMethod]
        public void then_custom_trace_listener_has_attributes_property()
        {
            var customListener = loggingViewModel.DescendentElements(x => x.Name == "Custom Listener").First();
            Assert.IsNotNull(customListener.Property("Attributes"));
        }

        [TestMethod]
        public void then_trace_sources_have_listeners_as_related_elements()
        {
            var traceSources = loggingViewModel.GetDescendentsOfType<TraceSourceData>();
            foreach (var source in traceSources)
            {
                foreach (var listener in source.GetDescendentsOfType<TraceListenerReferenceData>())
                {
                    var thisListener = listener;

                    Assert.IsTrue(loggingViewModel.GetRelatedElements(source)
                                                    .Where(x=>x.Name == listener.Name && typeof(TraceListenerData).IsAssignableFrom( x.ConfigurationType))
                                                    .Any());
                }
            }
        }

        [TestMethod]
        public void then_trace_source_data_trace_listener_references_property_has_collection_editor()
        {
            var traceSources = loggingViewModel.GetDescendentsOfType<TraceSourceData>().First();

            Assert.IsInstanceOfType(traceSources.Property("TraceListeners").CreateCustomVisual(), typeof(ElementCollectionEditor));
        }
    }

    [TestClass]
    public class when_adding_new_trace_listeners : given_logging_configuration
    {
        SectionViewModel loggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();
            LoggingSection.TraceListeners.Add(
                new TraceListenerData(typeof(FlatFileTraceListener)){Name = "UnreferencedListener"});

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender(base.Container.Resolve<IServiceProvider>()));
        }

        protected override void Act()
        {
            loggingViewModel = SectionViewModel.CreateSection(base.Container, LoggingSettings.SectionName, LoggingSection);
        }

        [TestMethod]
        public void then_trace_listener_reference_suggested_values_include_name()
        {
            var traceListenerReference = loggingViewModel.GetDescendentsOfType<TraceListenerReferenceData>().First();

            var nameProperty = traceListenerReference.Property("Name");
            Assert.IsTrue(nameProperty.SuggestedValues.Contains(nameProperty.Value));
        }

        [TestMethod]
        public void then_trace_listener_reference_excludes_unreferenced_listener()
        {
            var allTraceListeners =
                loggingViewModel.GetDescendentsOfType<TraceListenerData>().Select(x => x.Name);
            var category = loggingViewModel.GetDescendentsOfType<TraceSourceData>().First();
            var categoryListeners = category.GetDescendentsOfType<TraceListenerReferenceData>();
            var categoryTraceListener = categoryListeners.First();

            var expectedSuggestions = allTraceListeners
                    .Except(categoryListeners.Select(x => x.Name))
                    .Union(new string[] {categoryTraceListener.Name});

            var suggestedValues = categoryTraceListener.Property("Name").SuggestedValues.Cast<string>();
            CollectionAssert.AreEquivalent(expectedSuggestions.ToArray(), suggestedValues.ToArray());
        }
    }

    [TestClass]
    public class when_changing_listener_reference_from_category : given_logging_configuration
    {
        SectionViewModel loggingViewModel;
        private ElementViewModel category;
        private bool propertyChangedFired = false;
        private ElementViewModel unchangingReference;

        protected override void Arrange()
        {
            base.Arrange();

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender(base.Container.Resolve<IServiceProvider>()));
            loggingViewModel = SectionViewModel.CreateSection(base.Container, LoggingSettings.SectionName, LoggingSection);

            category = loggingViewModel.GetDescendentsOfType<TraceSourceData>().Where(
                x => x.GetDescendentsOfType<TraceListenerReferenceData>().Count() > 1).First();

            unchangingReference = category.GetDescendentsOfType<TraceListenerReferenceData>().First();

            /*
                General
                       Listener1 (1,4,5) -> (1,2,4,5)
                       UnusedTestValue (2,4,5) -> (2,4,5)

               Listener 1
               Listener 2
               Listener 4
               Listener 5
           */

            unchangingReference.Property("Name").PropertyChanged += (o, s) => { propertyChangedFired = (s.PropertyName == "SuggestedValues"); };
        }

        protected override void Act()
        {
            propertyChangedFired = false;
            category.GetDescendentsOfType<TraceListenerReferenceData>().Last()
                .Property("Name").Value = "UnusedTestValue";
        }

        [TestMethod]
        public void then_property_change_fires_for_other_reference()
        {
            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void then_trace_listener_reference_excludes_unreferenced_listener()
        {
            var allTraceListeners =
                loggingViewModel.GetDescendentsOfType<TraceListenerData>().Select(x => x.Name);

            var categoryListeners = category.GetDescendentsOfType<TraceListenerReferenceData>()
                .Except(new[] {unchangingReference});

            var expectedSuggestions = allTraceListeners
                .Except(categoryListeners.Select(x => x.Name));

            var suggestedValues = unchangingReference.Property("Name").SuggestedValues.Cast<string>();
            CollectionAssert.AreEquivalent(expectedSuggestions.ToArray(), suggestedValues.ToArray());
        }
    }

}
