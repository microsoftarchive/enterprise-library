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
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_parameter_value
{
    [TestClass]
    public class when_assinging_optional_value : given_parameter_element
    {
        ElementViewModel optionalParameterViewModel;

        protected override void Act()
        {
            var parameterValueProperty = base.ParameterElement.Property("Value");
            parameterValueProperty.Value = typeof(OptionalElement);

            var valueElementProperty = (ValueElementProperty)ParameterElement.Property("Value");
            optionalParameterViewModel = valueElementProperty.ValueElementViewModel;
        }

        [TestMethod]
        public void then_type_name_property_has_editor()
        {
            var typeNameProperty = optionalParameterViewModel.Property("TypeName");
            Assert.IsInstanceOfType(typeNameProperty.BindableProperty, typeof(PopupEditorBindableProperty));
        }
    }
}
