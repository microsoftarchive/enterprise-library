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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    public abstract class given_exception_handling_settings_with_logging_reference : ExceptionHandlingSettingsContext
    {
        protected LoggingSettings LogginSettings { get; set; }

        protected override void Arrange()
        {
            base.Arrange();

            var sourceBuilder = new ConfigurationSourceBuilder();

            sourceBuilder.ConfigureLogging()
                    .LogToCategoryNamed("General")
                        .SendTo.EventLog("eventlog listener")
                                    .ToLog("Application")
                    .LogToCategoryNamed("ArithmicExceptions")
                        .SendTo.SharedListenerNamed("eventlog listener");

            sourceBuilder.UpdateConfigurationWithReplace(Source);
            
            LogginSettings = (LoggingSettings)Source.GetSection(LoggingSettings.SectionName);
        }
    }

    [TestClass]
    public class when_editing_reference_property : given_exception_handling_settings_with_logging_reference
    {
        SectionViewModel viewModel;
        SectionViewModel loggingModel;
        int numberOfLogCategories;

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            viewModel = configurationSourceModel.AddSection(ExceptionHandlingSettings.SectionName, Section);
            loggingModel = configurationSourceModel.AddSection(LoggingSettings.SectionName, LogginSettings);

            numberOfLogCategories = loggingModel.DescendentElements(x => x.ConfigurationType == typeof(TraceSourceData)).Count();
            numberOfLogCategories -= 3; //3 special sources
        }

        [TestMethod]
        public void then_property_value_is_element_reference_property()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.IsInstanceOfType(logCategoryProperty, typeof(ElementReferenceProperty));
            Assert.IsNotNull(((ElementReferenceProperty)logCategoryProperty).ReferencedElement);
        }

        [TestMethod]
        public void then_type_converter_suggests_possible_instances()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.IsTrue(logCategoryProperty.HasSuggestedValues);

            var suggestedValues = logCategoryProperty.SuggestedValues;
            Assert.AreEqual(numberOfLogCategories, suggestedValues.Count());
            Assert.IsTrue(suggestedValues.Any(x => ((string)x) == "General"));
        }

        [TestMethod]
        public void then_property_changed_if_referred_element_name_changed_configuration_element_is_updated()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            var logCategoryElement = ((ElementReferenceProperty)logCategoryProperty).ReferencedElement;
            var logCategoryNameProperty = logCategoryElement.Property("Name");

            logCategoryNameProperty.Value = "new name";

            Assert.AreEqual("new name", ((LoggingExceptionHandlerData)logExceptionHandler.ConfigurationElement).LogCategory);          
        }

        [TestMethod]
        public void then_property_changed_notifies_bindable_changed()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            bool notifiedBindableChanged = false;
            logCategoryProperty.BindableProperty.PropertyChanged += (s, e) =>
                                                       {
                                                           if (e.PropertyName == "BindableValue")
                                                               notifiedBindableChanged = true;
                                                       };

            var logCategoryElement = ((ElementReferenceProperty)logCategoryProperty).ReferencedElement;
            var logCategoryNameProperty = logCategoryElement.Property("Name");

            logCategoryNameProperty.Value = "new name";

            Assert.IsTrue(notifiedBindableChanged);            
        }

    }


    [TestClass]
    public class when_removing_referred_to_element : given_exception_handling_settings_with_logging_reference
    {
        Property logCategoryProperty;
        PropertyChangedListener logCategoryPropertyChangeListener;

        protected override void Arrange()
        {
            base.Arrange();

            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            var viewModel = configurationSourceModel.AddSection(ExceptionHandlingSettings.SectionName, Section);
            var loggingModel = configurationSourceModel.AddSection(LoggingSettings.SectionName, LogginSettings);

            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            logCategoryProperty = logExceptionHandler.Property("LogCategory");
            logCategoryPropertyChangeListener = new PropertyChangedListener(logCategoryProperty);
        }

        protected override void Act()
        {
            base.Act();

            var referredElement = ((ElementReferenceProperty)logCategoryProperty).ReferencedElement as ElementViewModel;
            referredElement.Delete();
        }
        
        [TestMethod]
        public void then_reference_property_returns_null_element_reference()
        {
            Assert.IsNull(((ElementReferenceProperty)logCategoryProperty).ReferencedElement);
        }

        [TestMethod]
        public void then_internal_value_is_empty()
        {
            string internalValue = ((ElementReferenceProperty)logCategoryProperty).Value as string;
            Assert.IsTrue(string.IsNullOrEmpty(internalValue));
        }

    }

    [TestClass]
    public class when_editing_broken_reference_property : ExceptionHandlingSettingsContext
    {
        SectionViewModel viewModel;
        protected override void Act()
        {
            //not loading logging breaks reference
            viewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);

        }

        [TestMethod]
        public void then_property_returns_null_element_reference()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.IsNull(((ElementReferenceProperty)logCategoryProperty).ReferencedElement);
        }

        [TestMethod]
        public void then_property_returns_value()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.AreEqual("ArithmicExceptions", logCategoryProperty.Value);
        }

    }
}
