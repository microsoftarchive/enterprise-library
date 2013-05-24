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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_invalid_model
{
    [TestClass]
    public class when_navigating_validation_error : LoggingElementModelContext
    {
        private ValidationModel validationModel;
        private ValidationResult result;
        private ElementViewModel traceListener;

        protected override void Arrange()
        {
            base.Arrange();
            validationModel = Container.Resolve<ValidationModel>();
            traceListener = LoggingSettingsViewModel.DescendentElements(x => x.ConfigurationType == typeof(FormattedEventLogTraceListenerData)).First();

            var bindableProperty = traceListener.Property("TraceOutputOptions").BindableProperty;
            bindableProperty.BindableValue = "Invalid";

            result = bindableProperty.Property.ValidationResults.First();
        }

        protected override void Act()
        {
            validationModel.Navigate(result);
        }

        [TestMethod]
        public void then_element_is_selected()
        {
            Assert.IsTrue(traceListener.IsSelected);
        }

        [TestMethod]
        public void then_section_is_exanded()
        {
            Assert.IsTrue(traceListener.ContainingSection.IsExpanded);
        }

        [TestMethod]
        public void then_element_properties_are_shown()
        {
            Assert.IsTrue(traceListener.PropertiesShown);
        }
    }
}
