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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;

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
            viewModel = SectionViewModel.CreateSection(ServiceProvider, Section);
            loggingModel = SectionViewModel.CreateSection(ServiceProvider, LogginSettings);

            numberOfLogCategories = loggingModel.DescendentElements(x => x.ConfigurationType == typeof(TraceSourceData)).Count();
            numberOfLogCategories -= 3; //3 special sources
        }

        [TestMethod]
        public void then_property_value_is_element_view_model()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.AreEqual(typeof(ElementViewModel), logCategoryProperty.Type);
            Assert.IsNotNull(logCategoryProperty.Value);
            Assert.IsInstanceOfType(logCategoryProperty.Value, typeof(ElementViewModel));
        }

        [TestMethod]
        public void then_type_converter_suggests_possible_instances()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.IsTrue(logCategoryProperty.HasSuggestedValues);

            var suggestedValues = logCategoryProperty.SuggestedValues;
            Assert.AreEqual(numberOfLogCategories, suggestedValues.Count());
            Assert.IsTrue(suggestedValues.Cast<ElementViewModel>().Any(x => x.Name == "General"));
        }

        [TestMethod]
        public void then_property_changed_if_refferred_element_name_changed_configuration_element_is_updated()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            var logCategoryElement = (ElementViewModel)logCategoryProperty.Value;
            var logCategoryNameProperty = logCategoryElement.Property("Name");

            logCategoryNameProperty.Value = "new name";

            Assert.AreEqual("new name", ((LoggingExceptionHandlerData)logExceptionHandler.ConfigurationElement).LogCategory);          
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

            var viewModel = SectionViewModel.CreateSection(ServiceProvider, Section);
            var loggingModel = SectionViewModel.CreateSection(ServiceProvider, LogginSettings);

            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            logCategoryProperty = logExceptionHandler.Property("LogCategory");
            logCategoryPropertyChangeListener = new PropertyChangedListener(logCategoryProperty);
        }

        protected override void Act()
        {
            base.Act();

            var referredElement = logCategoryProperty.Value as CollectionElementViewModel;
            referredElement.DeleteCommand.Execute(null);
        }

        [TestMethod]
        public void then_reference_property_returns_null()
        {
            Assert.IsNull(logCategoryProperty.Value);
        }

        [TestMethod]
        public void then_reference_property_signals_change()
        {
            Assert.IsTrue(logCategoryPropertyChangeListener.ChangedProperties.Contains("Value"));
        }


    }

    [TestClass]
    public class when_editing_broken_reference_property : ExceptionHandlingSettingsContext
    {
        SectionViewModel viewModel;
        protected override void Act()
        {
            //not loading logging breaks reference
            viewModel = SectionViewModel.CreateSection(ServiceProvider, Section);

        }

        [TestMethod]
        public void then_property_returns_null()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.IsNull(logCategoryProperty.Value);
        }
    }
}
