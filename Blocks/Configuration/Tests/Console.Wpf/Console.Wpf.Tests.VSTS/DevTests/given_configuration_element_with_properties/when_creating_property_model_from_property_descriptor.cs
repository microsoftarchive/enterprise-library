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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Drawing.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    public abstract class given_configuration_properties_decorated_with_component_model_attributes : given_configuration_element_with_properties
	{
        protected class ConfigurationElementWithComponentModelAttributes : ConfigurationElementWithSimpleProperties
        {
            public const string Prop1DisplayName =  "Property With Component Model Meta Information";
            public const string Prop1Name = "prop1";
            public const string Prop2Name = "prop2";
            public const string Prop4Name = "prop4";
            public const string Prop1Description = "Lorem Impsum, etc. ";

            [ConfigurationProperty(Prop1Name)]
            [System.ComponentModel.DisplayName(Prop1DisplayName)]
            [System.ComponentModel.Description(Prop1Description)]
            [System.ComponentModel.TypeConverter(typeof(CustomStringConverter))]
            //implicitly readonly, typeconverter has stdValuesExclusive = true
            public string Prop1
            {
                get { return (string)base[Prop1Name]; }
                set { base[Prop1Name] = value; }
            }


            [ConfigurationProperty(Prop2Name)]
            [System.ComponentModel.ReadOnly(true)]
            public string Prop2
            {
                get { return (string)base[Prop2Name]; }
                set { base[Prop2Name] = value; }
            }
            [ConfigurationProperty(Prop4Name)]
            [System.ComponentModel.Browsable(false)]
            public string Prop4
            {
                get { return (string)base[Prop4Name]; }
                set { base[Prop4Name] = value; }
            }

        }

        protected class CustomStringConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                ArrayList suggestedValues = new ArrayList();
                suggestedValues.Add("one");
                suggestedValues.Add("two");
                suggestedValues.Add("three");

                return new StandardValuesCollection(suggestedValues);
            }
        }

	}

    [TestClass]
    public class when_discovering_properties_with_component_model_info_on_configuration_element : given_configuration_properties_decorated_with_component_model_attributes
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            var sectionModel = SectionViewModel.CreateSection(Container, "mock section", new ConfigurationElementWithComponentModelAttributes());
            properties = sectionModel.Properties;
        }

        [TestMethod]
        public void then_display_name_from_component_model_is_used_on_property_model()
        {
            var Prop1 = properties.Where(x => x.PropertyName == "Prop1").FirstOrDefault();
            
            Assert.IsNotNull(Prop1);
            Assert.AreEqual(ConfigurationElementWithComponentModelAttributes.Prop1DisplayName, Prop1.DisplayName);
        }

        [TestMethod]
        public void then_description_from_componenet_model_is_used_on_property_model()
        {
            var Prop1 = properties.Where(x => x.PropertyName == "Prop1").FirstOrDefault();

            Assert.IsNotNull(Prop1);
            Assert.AreEqual(ConfigurationElementWithComponentModelAttributes.Prop1Description, Prop1.Description);
        }

        [TestMethod]
        public void then_type_converter_from_component_model_overrides_configuration_converter()
        {
            var Prop1 = properties.Where(x => x.PropertyName == "Prop1").FirstOrDefault();

            Assert.IsNotNull(Prop1);
            Assert.IsInstanceOfType(Prop1.Converter, typeof(CustomStringConverter));
        }

        [TestMethod]
        public void then_property_model_has_suggested_values_from_component_model_type_descriptor()
        {
            var Prop1 = properties.Where(x => x.PropertyName == "Prop1").FirstOrDefault();

            Assert.IsTrue(Prop1.HasSuggestedValues);
            Assert.AreEqual(3, Prop1.SuggestedValues.Count());
            Assert.IsTrue(Prop1.SuggestedValues.Where(x => ((string)x) == "one").Any());
            Assert.IsTrue(Prop1.SuggestedValues.Where(x => ((string)x) == "two").Any());
            Assert.IsTrue(Prop1.SuggestedValues.Where(x => ((string)x) == "three").Any());
        }

        [TestMethod]
        public void then_property_model_has_suggested_values_are_exlcusive()
        {
            var Prop1 = properties.Where(x => x.PropertyName == "Prop1").FirstOrDefault();

            Assert.IsNotNull(Prop1);
            Assert.IsFalse(((SuggestedValuesBindableProperty)Prop1.BindableProperty).SuggestedValuesEditable);
        }

        [TestMethod]
        public void then_readonly_attribute_from_component_model_makes_property_readonly()
        {
            var Prop2 = properties.Where(x => x.PropertyName == "Prop2").FirstOrDefault();

            Assert.IsNotNull(Prop2);
            Assert.IsTrue(Prop2.ReadOnly);
        }

        [TestMethod]
        public void then_non_browsable_properties_are_hidden()
        {
            var Prop4 = properties.Where(x => x.PropertyName == "Prop4").FirstOrDefault();

            Assert.IsNotNull(Prop4);
            Assert.IsTrue(Prop4.Hidden);
        }
    }
}
