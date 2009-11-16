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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_creating_model_that_contains_properties : ExceptionHandlingSettingsContext
    {
        SectionViewModel viewModel;

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);
        }

        [TestMethod]
        public void then_properties_declared_on_elements_are_element_property()
        {
            var anyPolicyElement = viewModel.DescendentElements(x => x.ConfigurationType == typeof(ExceptionPolicyData)).FirstOrDefault();
            var nameProperty = anyPolicyElement.Property("Name");

            Assert.IsNotNull(nameProperty);
            Assert.IsInstanceOfType(nameProperty, typeof(ElementProperty));
        }

        [TestMethod]
        public void then_properties_that_refer_to_elements_are_element_reference_properties()
        {
            var logExceptionHandler = viewModel.DescendentElements(x => x.ConfigurationType == typeof(LoggingExceptionHandlerData)).First();
            var logCategoryProperty = logExceptionHandler.Property("LogCategory");

            Assert.IsNotNull(logCategoryProperty);
            Assert.IsInstanceOfType(logCategoryProperty, typeof(ElementReferenceProperty));
        }
    }
}
