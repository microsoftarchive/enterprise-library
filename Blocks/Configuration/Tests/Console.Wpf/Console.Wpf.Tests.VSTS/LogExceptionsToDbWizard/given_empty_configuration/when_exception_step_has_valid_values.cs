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
using System.ComponentModel;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    [TestClass]
    public class when_exception_step_has_valid_values : NewConfigurationSourceModelContext
    {
        private PickExceptionStep step;
        private PropertyChangedListener propertyChangedListener;

        protected override void Arrange()
        {
            base.Arrange();

            step = Container.Resolve<PickExceptionStep>();

            propertyChangedListener = new PropertyChangedListener(step);
        }

        protected override void Act()
        {
            step.ExceptionType.Value = typeof(Exception).AssemblyQualifiedName;
            step.Policy.Value = "TestPolicy";
        }

        [TestMethod]
        public void then_step_is_valid()
        {
            Assert.IsTrue(step.IsValid);
        }

        [TestMethod]
        public void then_is_valid_change_notification()
        {
            Assert.IsTrue(propertyChangedListener.ChangedProperties.Contains("IsValid"));
        }

        [TestMethod]
        public void then_the_type_exception_is_not_editable()
        {
            var bindableForExceptionType = step.ExceptionType.BindableProperty as PopupEditorBindableProperty;
            Assert.IsNotNull(bindableForExceptionType);
            Assert.IsTrue(bindableForExceptionType.TextReadOnly);
        }
    }
}
