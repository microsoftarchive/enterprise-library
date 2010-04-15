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

using System.Linq;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    [TestClass]
    public class when_exception_step_created : NewConfigurationSourceModelContext
    {
        private PickExceptionStep step;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }
        protected override void Act()
        {
            step = Container.Resolve<PickExceptionStep>();
        }

        [TestMethod]
        public void then_exception_type_property_has_no_suggested_values()
        {
            Assert.IsFalse(step.ExceptionType.SuggestedValues.Any());
        }

        [TestMethod]
        public void then_exception_type_bindable_is_popup_editor()
        {
            Assert.IsInstanceOfType(step.ExceptionType.BindableProperty, typeof(PopupEditorBindableProperty));
        }

        [TestMethod]
        public void then_step_is_not_valid()
        {
            Assert.IsFalse(step.IsValid);
        }
    }
}
