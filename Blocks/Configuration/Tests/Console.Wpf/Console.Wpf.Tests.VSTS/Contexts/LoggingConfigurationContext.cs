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
using System.Linq;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Contexts
{
    public abstract class LoggingConfigurationContext : DevTests.Contexts.ContainerContext
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
                .LogToCategoryNamed("Critical")
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
    public class when_creating_logging_view_model : LoggingConfigurationContext
    {
        SectionViewModel loggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender());
        }

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            loggingViewModel = configurationSourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);
        }

        [TestMethod]
        public void then_trace_listener_collection_has_custom_trace_listener_types()
        {
            var tracelistenerCollection = (ElementCollectionViewModel)loggingViewModel.DescendentElements().Where(x => typeof(TraceListenerDataCollection) == x.ConfigurationType).First();
            Assert.IsTrue(tracelistenerCollection.PolymorphicCollectionElementTypes.Contains(typeof(SystemDiagnosticsTraceListenerData)));
            Assert.IsTrue(tracelistenerCollection.PolymorphicCollectionElementTypes.Contains(typeof(CustomTraceListenerData)));
        }

        [TestMethod]
        public void then_view_model_has_category_with_name_AllEvents()
        {
            Assert.IsTrue(loggingViewModel.DescendentElements().Where(x => x.Name == "All Events").Any());
        }


        [TestMethod]
        public void then_view_model_delete_command_on_all_events_is_disabled()
        {
            var allEventsViewModel = loggingViewModel.DescendentElements().Where(x => x.Name == "All Events").First();
            var allEventsDeleteCommand = allEventsViewModel.Commands.OfType<DefaultDeleteCommandModel>().First();

            Assert.IsFalse(allEventsDeleteCommand.CanExecute(null));
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
                                      .Where(x => x.Name == listener.Name && typeof(TraceListenerData).IsAssignableFrom(x.ConfigurationType))
                                      .Any());
                }
            }
        }

        [TestMethod]
        public void then_trace_source_data_trace_listener_references_property_has_collection_editor()
        {
            var traceSources = loggingViewModel.GetDescendentsOfType<TraceSourceData>().First();

            var bindable = (FrameworkEditorBindableProperty)traceSources.Property("TraceListeners").BindableProperty;
            Assert.IsInstanceOfType(bindable.CreateEditorInstance(), typeof(ElementCollectionEditor));
        }
    }

    [TestClass]
    public class when_adding_new_trace_listeners : LoggingConfigurationContext
    {
        SectionViewModel loggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();
            LoggingSection.TraceListeners.Add(
                new TraceListenerData(typeof(FlatFileTraceListener)) { Name = "UnreferencedListener" });

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender());
        }

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            loggingViewModel = configurationSourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);
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
                .Union(new string[] { categoryTraceListener.Name });

            var suggestedValues = categoryTraceListener.Property("Name").SuggestedValues.Cast<string>();
            CollectionAssert.AreEquivalent(expectedSuggestions.ToArray(), suggestedValues.ToArray());
        }
    }

    [TestClass]
    public class when_creating_logging_view_model_with_filtering_profile : LoggingConfigurationContext
    {
        SectionViewModel loggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();
            var profile = new Profile
            {
                MatchFilters =
                    new MatchFilter[] { new TypeMatchFilter { Name = typeof(CustomTraceListenerData).AssemblyQualifiedName } }
            };

            Container.RegisterInstance(profile);
        }

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            loggingViewModel = configurationSourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);
        }

        [TestMethod]
        public void then_trace_listener_collection_has_custom_trace_listener_type_filtered()
        {
            var tracelistenerCollection = (ElementCollectionViewModel)loggingViewModel.DescendentElements().Where(x => typeof(TraceListenerDataCollection) == x.ConfigurationType).First();
            Assert.IsFalse(tracelistenerCollection.PolymorphicCollectionElementTypes.Contains(typeof(CustomTraceListenerData)));
        }
    }
}
