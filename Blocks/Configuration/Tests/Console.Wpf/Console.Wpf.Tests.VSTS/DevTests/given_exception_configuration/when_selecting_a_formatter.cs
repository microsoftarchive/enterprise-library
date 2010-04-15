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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_exception_configuration
{
    [TestClass]
    public class when_selecting_a_formatter : ContainerContext
    {
        private LogFormatterValidator validator;
        private IEnumerable<Property> properties;
        private Property typeNameProperty;
        private List<ValidationResult> errors;

        protected override void Arrange()
        {
            base.Arrange();

            validator = new LogFormatterValidator();

            errors = new List<ValidationResult>();

            IConfigurationSource source = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder sourceBuiler = new ConfigurationSourceBuilder();

            sourceBuiler.ConfigureExceptionHandling()
                            .GivenPolicyWithName("policy")
                                .ForExceptionType<Exception>()
                                    .LogToCategory("category")
                                        .UsingExceptionFormatter<BadImageFormatException>();

            sourceBuiler.UpdateConfigurationWithReplace(source);

            ExceptionHandlingSettings EhabSettings = (ExceptionHandlingSettings)source.GetSection(ExceptionHandlingSettings.SectionName);

            var sectionModel = SectionViewModel.CreateSection(Container, "Ehab Section", EhabSettings);

            properties = sectionModel.GetDescendentsOfType<LoggingExceptionHandlerData>().First().Properties;
        }

        protected override void Act()
        {
            base.Act();

            typeNameProperty = properties.Where(p => p.PropertyName == "FormatterTypeName").FirstOrDefault();

            Assert.IsNotNull(typeNameProperty);

            validator.Validate(typeNameProperty, (string)typeNameProperty.Value, errors);
        }

        [TestMethod]
        public void then_invalid_formatter_type_is_validated()
        {
            Assert.AreEqual(1, errors.Count);
        }
    }
}
