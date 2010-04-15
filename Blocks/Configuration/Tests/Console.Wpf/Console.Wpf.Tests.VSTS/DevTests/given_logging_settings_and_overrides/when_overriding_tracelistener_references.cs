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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    public abstract class given_logging_settings_and_overrides : ContainerContext
    {
        protected SectionViewModel LoggingSectionViewModel;
        protected EnvironmentSourceViewModel Environment;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder sourceBuilder = new ConfigurationSourceBuilder();
            sourceBuilder.ConfigureLogging().LogToCategoryNamed("General").SendTo.EventLog("Listener").SendTo.Msmq("msmqListener");

            DesignDictionaryConfigurationSource source = new DesignDictionaryConfigurationSource();
            sourceBuilder.UpdateConfigurationWithReplace(source);

            var applicationModel = Container.Resolve<ApplicationViewModel>();
            var sourceModel = applicationModel.CurrentConfigurationSource;

            sourceModel.Load(source);
            foreach (var element in sourceModel.Sections.SelectMany(x => x.DescendentElements())) element.InheritedFromParentConfiguration = false;

            applicationModel.NewEnvironment();
            Environment = applicationModel.Environments.First();

            LoggingSectionViewModel = sourceModel.Sections.Where(x => x.SectionName == LoggingSettings.SectionName).First();
        }

        protected static Property GetOverridesProperty(ElementViewModel declaringElement)
        {
            return declaringElement.Properties.Where(x => x.PropertyName.Contains("Override")).FirstOrDefault();
        }
    }

    [TestClass]
    public class when_category_overrides_is_set_to_override : given_logging_settings_and_overrides
    {
        Property overridesPropertyForCategoryElement;
        ElementViewModel categoryElement;
        protected override void Arrange()
        {
            base.Arrange();

            categoryElement = LoggingSectionViewModel.GetDescendentsOfType<TraceSourceData>().First();
            overridesPropertyForCategoryElement = GetOverridesProperty(categoryElement);

        }

        protected override void Act()
        {
            overridesPropertyForCategoryElement.Value = true;
        }

        [TestMethod]
        public void then_overridden_listeners_have_special_editor()
        {
            var overriddenTraceListeners = overridesPropertyForCategoryElement.ChildProperties.Where(x => x.PropertyName == "TraceListeners").First();
            Assert.IsInstanceOfType(((FrameworkEditorBindableProperty) overriddenTraceListeners.BindableProperty).CreateEditorInstance(), typeof(OverriddenTraceListenerCollectionEditor));
        }


        [TestMethod]
        public void then_referenced_tracelisteners_allow_to_be_overwritten()
        {
            var traceListenerReferences = categoryElement.GetDescendentsOfType<TraceListenerReferenceData>();
            Assert.IsTrue(traceListenerReferences.Any());

            foreach (var traceListenerReferenceData in traceListenerReferences)
            {
                var overridesProprtyForReferenceData = GetOverridesProperty(traceListenerReferenceData);
                Assert.IsTrue((bool)overridesProprtyForReferenceData.Value);
            }
        }

        [TestMethod]
        public void then_adding_new_tracelistener_reference_allows_to_be_overwritten()
        {
            ElementCollectionViewModel tracelistenerReferenceCollection = (ElementCollectionViewModel)categoryElement.ChildElement("TraceListeners");
            
            var addedTracelistenerReference = tracelistenerReferenceCollection.AddNewCollectionElement(typeof(TraceListenerReferenceData));
            addedTracelistenerReference.Property("Name").Value = "unreferenced element";


            var overridesProprtyForReferenceData = GetOverridesProperty(addedTracelistenerReference);
            Assert.IsTrue((bool)overridesProprtyForReferenceData.Value);
        }

        
    }

    [TestClass]
    public class when_category_overrides_is_set_to_no_override : given_logging_settings_and_overrides
    {
        Property overridesPropertyForCategoryElement;
        ElementViewModel categoryElement;

        protected override void Arrange()
        {
            base.Arrange();

            categoryElement = LoggingSectionViewModel.GetDescendentsOfType<TraceSourceData>().First();
            overridesPropertyForCategoryElement = GetOverridesProperty(categoryElement);
            overridesPropertyForCategoryElement.Value = true;

        }

        protected override void Act()
        {
            overridesPropertyForCategoryElement.Value = false;
        }

        [TestMethod]
        public void then_referenced_tracelisteners_disallow_to_be_overwritten()
        {
            var traceListenerReferences = categoryElement.GetDescendentsOfType<TraceListenerReferenceData>();
            Assert.IsTrue(traceListenerReferences.Any());

            foreach (var traceListenerReferenceData in traceListenerReferences)
            {
                var overridesProprtyForReferenceData = GetOverridesProperty(traceListenerReferenceData);
                Assert.IsFalse((bool)overridesProprtyForReferenceData.Value);
            }
        }
    }

    [TestClass]
    public class when_overridden_trace_listener_reference_is_deleted : given_logging_settings_and_overrides
    {
        private ElementViewModel referenceData;
        private Property overrideProperty;

        protected override void Arrange()
        {
            base.Arrange();

            referenceData = LoggingSectionViewModel.DescendentConfigurationsOfType<TraceListenerReferenceData>().First();
            overrideProperty = GetOverridesProperty(referenceData);
            overrideProperty.Value = true;
        }

        protected override void Act()
        {
            referenceData.Delete();
        }

        [TestMethod]
        public void then_referenceddata_removed_from_source ()
        {
            Assert.IsFalse(LoggingSectionViewModel.DescendentConfigurationsOfType<TraceListenerReferenceData>().Any(d => d.ElementId == referenceData.ElementId));
        }

        [TestMethod]
        public void then_environment_contains_no_reference_to_deleted_element()
        {
            // For environments, we don't have a ViewModel representation for this
            Assert.IsFalse(((EnvironmentalOverridesSection) Environment.ConfigurationElement)
                               .MergeElements.Cast<EnvironmentalOverridesElement>()
                               .Any(e => e.LogicalParentElementPath == referenceData.Path));

        }
    }
}
