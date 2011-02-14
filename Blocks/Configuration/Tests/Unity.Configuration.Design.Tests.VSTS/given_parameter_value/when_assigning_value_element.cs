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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_parameter_value
{
    [TestClass]
    public class when_assigning_value_element : given_parameter_element
    {
        ElementViewModel valueParameterViewModel;

        protected override void Act()
        {
            var parameterValueProperty = base.ParameterElement.Property("Value");
            parameterValueProperty.Value = typeof(ValueElement);

            var valueElementProperty = (ValueElementProperty)ParameterElement.Property("Value");
            valueParameterViewModel = valueElementProperty.ValueElementViewModel;
        }

        [TestMethod]
        public void then_type_converter_property_has_editor()
        {
            var typeNameProperty = valueParameterViewModel.Property("TypeConverterTypeName");
            Assert.IsInstanceOfType(typeNameProperty.BindableProperty, typeof(PopupEditorBindableProperty));
        }
    }
}
