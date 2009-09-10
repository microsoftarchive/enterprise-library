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
using System.Configuration;
using Console.Wpf.ViewModels.ConfigBased;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_validation
{

    //public abstract class given_configuration_element_with_validated_properties : given_configuration_element_with_properties.given_configuration_element_with_properties
    //{
    //    IEnumerable<ConfigElementPropertyModel> properties;
    //    protected ConfigElementPropertyModel StringValidatedProperty;

    //    protected override void Arrange()
    //    {
    //        base.Arrange();

    //        properties = DiscoverPropertyModel(new ConfigurationElementWithValidatedProperties());
    //        StringValidatedProperty = properties.Where(x => x.PropertyName == "StringValidated").First();
    //    }

    //    protected class ConfigurationElementWithValidatedProperties : ConfigurationElement
    //    {
    //        public const string StringValidatedName = "stringValidated";

    //        [ConfigurationProperty(StringValidatedName)]
    //        [StringValidator(InvalidCharacters="@#")]
    //        public string StringValidated
    //        {
    //            get { return (string)base[StringValidatedName]; }
    //            set { base[StringValidatedName] = value; }
    //        }
    //    }
    //}


    //[TestClass]
    //public class when_writing_to_property_with_validation : given_configuration_element_with_validated_properties
    //{

    //    [TestMethod]
    //    public void then_writing_invalid_value_sets_has_errors()
    //    {
    //        using (var StringValidatedPropertyChangedListener = new PropertyChangedListener(StringValidatedProperty))
    //        {
    //            StringValidatedProperty.Value = "@invalid";

    //            Assert.IsTrue(StringValidatedProperty.HasErrors);
    //            Assert.IsTrue(StringValidatedPropertyChangedListener.ChangedProperties.Contains("HasErrors"));
    //            Assert.IsTrue(StringValidatedPropertyChangedListener.ChangedProperties.Contains("Value"));
    //        }
    //    }

    //    [TestMethod]
    //    public void then_writing_invalid_value_sets_has_error_messages()
    //    {
    //        using (var StringValidatedPropertyChangedListener = new PropertyChangedListener(StringValidatedProperty))
    //        {
    //            StringValidatedProperty.Value = "@invalid";

    //            Assert.AreNotEqual(0, StringValidatedProperty.Errors.Count());
    //            Assert.IsTrue(StringValidatedPropertyChangedListener.ChangedProperties.Contains("Errors"));
    //        }
    //    }

    //    [TestMethod]
    //    public void then_reading_after_writing_invalid_value_returns_invalid_value()
    //    {
    //        using (var StringValidatedPropertyChangedListener = new PropertyChangedListener(StringValidatedProperty))
    //        {
    //            StringValidatedProperty.Value = "@invalid";
                
    //            Assert.AreEqual("@invalid", StringValidatedProperty.Value);
    //        }
    //    }

    //    [TestMethod]
    //    public void then_fixing_error_after_writing_invalid_value_returns_valid_value()
    //    {
    //        using (var StringValidatedPropertyChangedListener = new PropertyChangedListener(StringValidatedProperty))
    //        {
    //            StringValidatedProperty.Value = "fixed";

    //            Assert.AreEqual("fixed", StringValidatedProperty.Value);
    //            Assert.IsTrue(StringValidatedPropertyChangedListener.ChangedProperties.Contains("HasErrors"));
    //            Assert.IsTrue(StringValidatedPropertyChangedListener.ChangedProperties.Contains("Value"));
    //            Assert.IsTrue(StringValidatedPropertyChangedListener.ChangedProperties.Contains("Errors"));
    //        }
    //    }

        
        
    //}
}
