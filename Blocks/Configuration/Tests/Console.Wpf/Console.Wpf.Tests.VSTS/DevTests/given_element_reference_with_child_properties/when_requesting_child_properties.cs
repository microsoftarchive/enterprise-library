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
using System.ComponentModel;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference_with_child_properties
{
    public abstract class exception_referencing_log_category_context : Contexts.ContainerContext
    {
        protected SectionViewModel ExceptionHandlingSection { get; set; }
        protected SectionViewModel LoggingSection { get; set; }

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new ConfigurationSourceBuilder();

            builder.ConfigureLogging()
                .LogToCategoryNamed("General")
                    .SendTo.EventLog("Event Log Listener")
                    .FormatWith(new FormatterBuilder().TextFormatterNamed("Text Formatter"))
                    .ToLog("Application");

            builder.ConfigureExceptionHandling()
                    .GivenPolicyWithName("AllExceptions")
                        .ForExceptionType<Exception>()
                            .LogToCategory("General")
                            .ThenDoNothing()
                    .GivenPolicyWithName("OtherExceptions")
                        .ForExceptionType<ArgumentNullException>()
                            .LogToCategory("InvalidCategoryName")
                            .ThenDoNothing();


            var configuration = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(configuration);

            LoggingSection = GetSection(LoggingSettings.SectionName, configuration);
            ExceptionHandlingSection = GetSection(ExceptionHandlingSettings.SectionName, configuration);
        }

        private SectionViewModel GetSection(string sectionName, IConfigurationSource source)
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            return configurationSourceModel.AddSection(sectionName, source.GetSection(sectionName));
        }
    }

    [TestClass]
    public class when_requesting_child_properties : exception_referencing_log_category_context
    {
        private ElementViewModel unresolvedCategoryReference;
        private Property resolvedCategoryProperty;

        protected override void Act()
        {
            var resolvedCategoryReference = ExceptionHandlingSection
                .GetDescendentsOfType<LoggingExceptionHandlerData>()
                    .Single(x => x.Name=="General");

            unresolvedCategoryReference = ExceptionHandlingSection.GetDescendentsOfType<LoggingExceptionHandlerData>()
                                   .Single(x => x.Name == "InvalidCategoryName");

            resolvedCategoryProperty = resolvedCategoryReference.Property("LogCategory");
        }

        [TestMethod]
        public void then_child_properties_exist_for_resolved_element_references()
        {
            Assert.IsTrue(resolvedCategoryProperty.HasChildProperties);
        }

        [TestMethod]
        public void then_child_properties_are_those_from_resolved_reference()
        {
            var generalCategory = LoggingSection
                                        .DescendentElements(x => x.ConfigurationType == typeof (TraceSourceData))
                                        .Where(x => x.Name == "General").Single();

            CollectionAssert.AreEquivalent(generalCategory.Properties.ToArray(), resolvedCategoryProperty.ChildProperties.ToArray());
        }

        [TestMethod]
        public void then_has_no_child_properties_for_unresolved_element_reference()
        {
            Assert.IsFalse(unresolvedCategoryReference.Property("LogCategory").HasChildProperties);
        }

        [TestMethod]
        public void then_unresolved_reference_child_property_count_is_none()
        {
            Assert.IsFalse(unresolvedCategoryReference.Property("LogCategory").HasChildProperties);  
        }
    }

    [TestClass]
    public class when_referencing_property_value_changes : exception_referencing_log_category_context
    {
        private Stack<string> propertiesChanged;
        private Property property;

        protected override void Arrange()
        {
            base.Arrange();

            var categoryReference = ExceptionHandlingSection
             .GetDescendentsOfType<LoggingExceptionHandlerData>().First();

            property = categoryReference.Property("LogCategory");
            propertiesChanged = new Stack<string>();

            property.PropertyChanged += (o, e) => propertiesChanged.Push(e.PropertyName);
        }

        protected override void Act()
        {
            property.Value = "SomeNewValue";
        }

        [TestMethod]
        public void then_notifies_child_properties_changed()
        {
            Assert.IsTrue(propertiesChanged.Contains("ChildProperties"));
        }

        
        [TestMethod]
        public void then_notifies_has_child_properties_changed()
        {
            Assert.IsTrue(propertiesChanged.Contains("HasChildProperties"));
        }
    }


}
