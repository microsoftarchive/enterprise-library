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
    public abstract class given_configuration_properties_decorated_with_component_model_attributes_and_child_properties : given_configuration_properties_decorated_with_component_model_attributes
	{
        protected class ConfigurationElementWithComponentModelAttributesAndChildProperties : ConfigurationElementWithComponentModelAttributes
        {
            public const string Prop5Name = "prop5";

            [ConfigurationProperty(Prop5Name)]
            [System.ComponentModel.TypeConverter(typeof(StringConverterWithChildProperties))]
            public ChildObject Prop5
            {
                get { return (ChildObject)base[Prop5Name]; }
                set { base[Prop5Name] = value; }
            }
        }

        protected class StringConverterWithChildProperties : StringConverter
        {
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(value);
            }
        }

        public class ChildObject
        {
            public string Str
            {
                get;
                set;
            }

            public int Int
            {
                get;
                set;
            }
        }
	}

    [TestClass]
    public class when_discovering_properties_with_child_properties_on_configuration_element : given_configuration_properties_decorated_with_component_model_attributes_and_child_properties
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            var sectionModel = SectionViewModel.CreateSection(Container, "mockSection", new ConfigurationElementWithComponentModelAttributesAndChildProperties());
            properties = sectionModel.Properties;
        }

        [TestMethod]
        public void then_child_properties_end_up_in_model()
        {
            var Prop5 = properties.Where(x => x.PropertyName == "Prop5").FirstOrDefault();
            Prop5.Value = new ChildObject();

            Assert.IsTrue(Prop5.HasChildProperties);
            Assert.AreEqual(2, Prop5.ChildProperties.Count());
        }

        [TestMethod]
        public void then_child_properties_can_be_read()
        {
            var Prop5 = properties.Where(x => x.PropertyName == "Prop5").FirstOrDefault();
            Prop5.Value = new ChildObject() { Int = 24, Str = "abc" };

            Assert.AreEqual(24, Prop5.ChildProperties.Where(x => x.PropertyName == "Int").First().Value);
            Assert.AreEqual("abc", Prop5.ChildProperties.Where(x => x.PropertyName == "Str").First().Value);

        }

        [TestMethod]
        public void then_child_properties_can_be_written()
        {
            var Prop5 = properties.Where(x => x.PropertyName == "Prop5").FirstOrDefault();
            Prop5.Value = new ChildObject() { Int = 24, Str = "abc" };

            var Prop5Int = Prop5.ChildProperties.Where(x => x.PropertyName == "Int").First();
            var Prop5Str = Prop5.ChildProperties.Where(x => x.PropertyName == "Str").First();

            Prop5Int.Value = 44;
            Prop5Str.Value = "def";

            Assert.AreEqual(44, ((ChildObject)Prop5.Value).Int);
            Assert.AreEqual("def", ((ChildObject)Prop5.Value).Str);
        }
    }
}
