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
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_assigning_value_element_to_value_property : given_parameter_element
    {
        Property valueProperty;
        PropertyChangedListener valuePropertyPropertyChangedListener;

        protected override void Act()
        {
            valueProperty = base.ParameterElement.Property("Value");
            valuePropertyPropertyChangedListener = new PropertyChangedListener(valueProperty);
            valueProperty.Value = typeof(ValueElement);
        }

        [TestMethod]
        public void then_underlying_element_is_value_element()
        {
            ParameterElement paramElement = base.ParameterElement.ConfigurationElement as ParameterElement;
            Assert.IsInstanceOfType(paramElement.Value, typeof(ValueElement));
        }

        [TestMethod]
        public void then_value_property_contains_value_child_property()
        {
            Assert.IsTrue(valueProperty.HasChildProperties);
            Assert.IsTrue(valueProperty.ChildProperties.Any(x=>x.PropertyName == "Value"));
        }

        [TestMethod]
        public void then_value_property_has_type_converter_type_name_child_property()
        {
            Assert.IsTrue(valueProperty.HasChildProperties);
            Assert.IsTrue(valueProperty.ChildProperties.Any(x => x.PropertyName == "TypeConverterTypeName"));
        }

        [TestMethod]
        public void then_value_property_signals_change_on_child_properties()
        {
            Assert.IsTrue(valuePropertyPropertyChangedListener.ChangedProperties.Contains("ChildProperties"));
        }
    }
}
