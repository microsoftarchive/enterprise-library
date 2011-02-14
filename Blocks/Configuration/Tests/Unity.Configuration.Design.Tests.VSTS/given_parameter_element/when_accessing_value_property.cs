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
    public class when_accessing_value_property : given_parameter_element
    {
        Property valueProperty;
 
        protected override void Act()
        {
            valueProperty = base.ParameterElement.Property("Value");
        }

        [TestMethod]
        public void then_value_property_has_exclusive_suggested_values()
        {
            Assert.IsTrue(valueProperty.HasSuggestedValues);
            Assert.IsFalse(valueProperty.SuggestedValuesEditable);
        }

        [TestMethod]
        public void then_setting_value_property_changes_value()
        {
            valueProperty.Value = typeof(OptionalElement);
            Assert.AreEqual(typeof(OptionalElement), valueProperty.Value);
        }

        [TestMethod]
        public void then_value_property_has_child_properties()
        {
            Assert.IsTrue(valueProperty.HasChildProperties);
        }

        [TestMethod]
        public void then_value_property_is_last()
        {
            Assert.AreEqual(valueProperty, base.ParameterElement.Properties.Last());
        }
    }
}
