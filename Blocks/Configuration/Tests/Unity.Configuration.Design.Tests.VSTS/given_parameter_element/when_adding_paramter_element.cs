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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_adding_paramter_element : given_parameter_element
    {

        [TestMethod]
        public void then_parameter_configuration_element_has_dependency_value_element()
        {
            ParameterElement parameter = (ParameterElement) base.ParameterElement.ConfigurationElement;
            Assert.IsInstanceOfType(parameter.Value, typeof(DependencyElement));
        }

        [TestMethod]
        public void then_parameter_element_has_value_set_to_dependency()
        {
            Property valueProperty = (Property)base.ParameterElement.Property("Value");
            Assert.AreEqual(typeof(DependencyElement), valueProperty.Value);
        }
        
        [TestMethod]
        public void then_type_name_property_has_editor()
        {
            Property typeName = (Property)base.ParameterElement.Property("TypeName");
            Assert.IsInstanceOfType(typeName.BindableProperty, typeof(PopupEditorBindableProperty));
        }

    }
}
