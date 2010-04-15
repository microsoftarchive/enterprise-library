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
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Console.Wpf.Tests.VSTS.DevTests.given_property_to_validate
{
    [TestClass]
    public class when_validation_results_change : SectionWithValidatablePropertiesContext
    {
        PropertyChangedListener propertyPropertyChangedListener;

        protected override void Arrange()
        {
            base.Arrange();
            propertyPropertyChangedListener = new PropertyChangedListener(base.property.BindableProperty);
        }

        protected override void Act()
        {
            ((ObservableCollection<ValidationResult>)property.ValidationResults).Add(new PropertyValidationResult(property, "error"));
        }

        protected override string ArrangePropertyName()
        {
            return "PropertyWithNoValidators";
        }

        [TestMethod]
        public void then_bindable_value_raised_property_changed_event()
        {
            Assert.IsTrue(propertyPropertyChangedListener.ChangedProperties.Contains("BindableValue"));
        }
    }
}
