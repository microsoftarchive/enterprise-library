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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_prior_policy_configuration
{
    [TestClass]
    public class when_exception_step_executes_with_clashing_policy : ExceptionHandlingConfiguredSourceModelContext
    {
        private PickExceptionStep step;
        private ElementViewModel existingPolicy;

        protected override void Arrange()
        {
            base.Arrange();

            step = Container.Resolve<PickExceptionStep>();

            existingPolicy = GetDescendentsOfType<ExceptionPolicyData>().First();

            step.Policy.Value = existingPolicy.Name;
            step.ExceptionType.Value = typeof(BadImageFormatException).AssemblyQualifiedName;
        }

        protected override void Act()
        {
            step.Execute();
        }

        private ElementViewModel AddedException
        {
            get
            {
                return existingPolicy.GetDescendentsOfType<ExceptionTypeData>()
                    .Where(e => e.Property("TypeName").Value == step.ExceptionType.Value).FirstOrDefault();
            }
        }

        [TestMethod]
        public void then_existing_policy_gets_new_exception_type()
        {
            Assert.IsNotNull(AddedException);
        }

        [TestMethod]
        public void then_new_exception_type_has_logging_handler()
        {
            Assert.IsTrue(AddedException.GetDescendentsOfType<LoggingExceptionHandlerData>().Any());
        }
    }

    [TestClass]
    public class when_exception_step_executes_with_clashing_exception : ExceptionHandlingConfiguredSourceModelContext
    {
        private PickExceptionStep step;
        private ElementViewModel existingPolicy;
        private ElementViewModel existingException;
        private string existingHandlerName;

        protected override void Arrange()
        {
            base.Arrange();

            step = Container.Resolve<PickExceptionStep>();

            existingPolicy = GetDescendentsOfType<ExceptionPolicyData>().First();
            existingException = existingPolicy.GetDescendentsOfType<ExceptionTypeData>().Last();

            var handlerCollection = existingException.GetDescendentsOfType<NamedElementCollection<ExceptionHandlerData>>()
                .OfType<ElementCollectionViewModel>().First();
            existingHandlerName = handlerCollection.AddNewCollectionElement(typeof (LoggingExceptionHandlerData)).Name;


            step.Policy.Value = existingPolicy.Name;
            step.ExceptionType.Value = existingException.Property("TypeName").Value;
        }

        protected override void Act()
        {
            step.Execute();
        }

        [TestMethod]
        public void then_preexisting_exception_used()
        {
            Assert.AreEqual(1, existingPolicy.GetDescendentsOfType<ExceptionTypeData>().Count(x => x.Property("TypeName").Value == step.ExceptionType.Value));
        }

        [TestMethod]
        public void then_existing_exception_gets_new_logging_handler()
        {
            Assert.IsTrue(existingException.GetDescendentsOfType<LoggingExceptionHandlerData>().
                Where(x => x.Name != existingHandlerName).Any());
        }
    }
}
