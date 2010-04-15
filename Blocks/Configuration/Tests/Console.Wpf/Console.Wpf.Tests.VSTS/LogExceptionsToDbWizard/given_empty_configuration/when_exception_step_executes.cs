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
using System.Configuration;
using System.Linq;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    [TestClass]
    public class when_exception_step_executes : NewConfigurationSourceModelContext
    {
        private PickExceptionStep step;

        protected override void Arrange()
        {
            base.Arrange();

            step = Container.Resolve<PickExceptionStep>();
            step.ExceptionType.Value = typeof(Exception).AssemblyQualifiedName;
            step.Policy.Value = "TestPolicy";
            step.ReferencedDatabaseName = "SomeDatabaseName";

        }

        protected override void Act()
        {
            step.Execute();
        }

        private SectionViewModel AddedSection
        {
            get
            {
                return ConfigurationSourceModel
                    .Sections.Where(s => s.ConfigurationType == typeof(ExceptionHandlingSettings))
                    .Single();
            }
        }

        [TestMethod]
        public void then_exception_policy_section_added()
        {
            Assert.IsTrue(
                ConfigurationSourceModel.Sections.Where(s => s.ConfigurationType == typeof(ExceptionHandlingSettings)).
                    Any());
        }

        [TestMethod]
        public void then_policy_type_added()
        {
            Assert.IsTrue(AddedSection.GetDescendentsOfType<ExceptionPolicyData>().Where(e => e.Name == "TestPolicy").Any());
        }

        [TestMethod]
        public void then_added_policy_has_exception_type()
        {
            Assert.IsTrue(AddedSection.GetDescendentsOfType<ExceptionTypeData>()
                              .Where(e => e.Property("TypeName").Value == step.ExceptionType.Value).Any());
        }

        [TestMethod]
        public void then_exception_type_has_logging_handler()
        {
            Assert.IsTrue(AddedSection.GetDescendentsOfType<LoggingExceptionHandlerData>().Any());
        }

        [TestMethod]
        public void then_configuration_contains_logging_settings()
        {
            Assert.IsTrue(
                ConfigurationSourceModel.Sections.Where(x => x.ConfigurationType == typeof (LoggingSettings)).Any());
        }
        
        [TestMethod]
        public void then_configuration_contains_trace_source_configuration()
        {
            ConfigurationSourceModel.Logging()
                .HasCategory("Category")
                .WithListenerOfType<FormattedDatabaseTraceListenerData>("Database Trace Listener");

            ConfigurationSourceModel.Logging()
                .HasListener("Database Trace Listener")
                .OfConfigurationType<FormattedDatabaseTraceListenerData>()
                .WithProperty(x => x.DatabaseInstanceName == step.ReferencedDatabaseName)
                .WithProperty(x => x.Formatter == "Text Formatter");
        }

        [TestMethod]
        public void then_configuration_contains_text_formatter()
        {
            ConfigurationSourceModel.Logging()
                .HasFormatter("Text Formatter");
        }
    }
}
