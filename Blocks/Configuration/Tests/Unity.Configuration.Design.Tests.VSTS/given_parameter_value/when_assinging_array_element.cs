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
using System.Configuration;
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_parameter_value
{
    [TestClass]
    public class when_assigning_array_element : given_parameter_element
    {
        ElementViewModel arrayParameterViewModel;

        protected override void Act()
        {
            var parameterValueProperty = base.ParameterElement.Property("Value");
            parameterValueProperty.Value = typeof(ArrayElement);

            var valueElementProperty = (ValueElementProperty)ParameterElement.Property("Value");
            arrayParameterViewModel = valueElementProperty.ValueElementViewModel;
        }

        [TestMethod]
        public void then_type_name_property_has_editor()
        {
            var typeNameProperty = arrayParameterViewModel.Property("TypeName");
            Assert.IsInstanceOfType(typeNameProperty.BindableProperty, typeof(PopupEditorBindableProperty));
        }

        [TestMethod]
        public void then_has_values_property()
        {
            var valuesProperty = arrayParameterViewModel.Property("Values");
            Assert.IsNotNull(valuesProperty);
        }

        [TestMethod]
        public void then_values_property_is_editable_collection()
        {
            var valuesProperty = arrayParameterViewModel.Property("Values");
            Assert.IsInstanceOfType(valuesProperty.BindableProperty,typeof(PopupEditorBindableProperty));
            Assert.IsInstanceOfType(valuesProperty.Value, typeof(ConfigurationElementCollection));
        }

        [TestMethod]
        public void then_values_property_has_readonly_text()
        {
            var valuesProperty = arrayParameterViewModel.Property("Values");
            PopupEditorBindableProperty bindableForValue = valuesProperty.BindableProperty as PopupEditorBindableProperty;
            Assert.IsTrue(bindableForValue.TextReadOnly);
        }

        [TestMethod]
        public void then_values_property_is_collection_is_polymorphic()
        {
            var valuesChild = (ElementCollectionViewModel)arrayParameterViewModel.ChildElement("Values");
            Assert.IsNotNull(valuesChild);
            Assert.IsTrue(valuesChild.IsPolymorphicCollection);
        }

        [TestMethod]
        public void then_underlying_configuration_property_has_array_element()
        {
            ParameterElement parameter = arrayParameterViewModel.ParentElement.ConfigurationElement as ParameterElement;
            Assert.IsInstanceOfType(parameter.Value, typeof(ArrayElement));
        }

    }
}
