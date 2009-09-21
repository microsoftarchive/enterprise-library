﻿//===============================================================================
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.ViewModel;
using System.ComponentModel.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    public abstract class given_configuration_element_with_properties : ArrangeActAssert
    {
        protected ServiceContainer ServiceProvider = new ServiceContainer();
        protected ConfigurationElementWithSimpleProperties SectionWithSimpleProperties = new ConfigurationElementWithSimpleProperties();

        protected override void Arrange()
        {
            SectionWithSimpleProperties = new ConfigurationElementWithSimpleProperties();
        }

        protected class ConfigurationElementWithSimpleProperties : ConfigurationSection
        {
            private const string designTimeReadOnlyTrueProperty = "designTimeReadOnlyTrue";
            private const string designTimeReadOnlyFalseProperty = "designTimeReadOnlyFalse";
            private const string readOnlyProperty = "readOnly";
            private const string numberProperty = "number";
            private const string requiredProperty = "required";
            private const string guidProperty = "guid";
            private const string validatedProperty = "validated";
            private const string nameProperty = "name";

            [ConfigurationProperty(nameProperty, IsRequired=true, IsKey=true)]
            public string Name
            {
                get { return (string)base[nameProperty]; }
                set { base[nameProperty] = value; }
            }

            [ConfigurationProperty(numberProperty)]
            public int Number
            {
                get { return (int)base[numberProperty]; }
                set { base[numberProperty] = value; }
            }

            [ConfigurationProperty(guidProperty)]
            [DesignTimeType(typeof(string), typeof(GuidConverter))]
            public Guid GuidAsString
            {
                get { return (Guid)base[guidProperty]; }
                set { base[guidProperty] = value; }
            }

            [ConfigurationProperty(requiredProperty, IsRequired=true)]
            public string RequiredProperty
            {
                get { return (String)base[requiredProperty]; }
                set { base[requiredProperty] = value; }
            }

            [ConfigurationProperty(validatedProperty, DefaultValue="asd")]
            [StringValidator(MinLength=2, MaxLength=10, InvalidCharacters="(*&^%$#@")]
            public string PropertyWithConfigurationValidator
            {
                get { return (string) base[validatedProperty];}
                set { base[validatedProperty] = value; }
            }

            [ConfigurationProperty(designTimeReadOnlyTrueProperty)]
            [DesignTimeReadOnly(true)]
            public string DesignTimeReadOnly
            {
                get { return (string) base[designTimeReadOnlyTrueProperty]; }
                set { base[designTimeReadOnlyTrueProperty] = value;}
            }

            [ConfigurationProperty(designTimeReadOnlyFalseProperty)]
            [DesignTimeReadOnly(false)]
            public string DesignTimeWritable
            {
                get { return (string) base[designTimeReadOnlyFalseProperty]; }
                set { base[designTimeReadOnlyFalseProperty] = value;}
            }

            [ConfigurationProperty(readOnlyProperty)]
            [ReadOnly(true)]
            public string ReadOnlyProperty
            {
                get { return (string) base[readOnlyProperty]; }
                set { base[readOnlyProperty] = value;}
            }
            
        }
    }


    [TestClass]
    public class when_disovering_simple_properties_on_configuration_element : given_configuration_element_with_properties
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            var sectionModel = SectionViewModel.CreateSection(ServiceProvider, SectionWithSimpleProperties);
            properties = sectionModel.Properties;
        }


        [TestMethod]
        public void can_create_property_wihtout_pd()
        {
            var property = new Property(ServiceProvider, null, null);
            Assert.IsNotNull(property);
        }

        [TestMethod]
        public void then_property_model_was_created_for_all_properties()
        {
            Assert.IsTrue(properties.Where(x => x.PropertyName == "Number").Any());
            Assert.IsTrue(properties.Where(x => x.PropertyName == "Name").Any());
        }

        [TestMethod]
        public void then_calling_get_child_properties_doesnt_throw()
        {
            var a = properties.SelectMany(x=>x.ChildProperties).ToArray();
        }

        [TestMethod]
        public void then_property_contains_type_from_property_info()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.AreEqual(typeof(int), numberProperty.PropertyType);
        }

        [TestMethod]
        public void then_property_has_name_from_configuration_property_information()
        {
            ElementProperty numberProperty = properties.OfType<ElementProperty>().Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.AreEqual("number", numberProperty.ConfigurationName);
        }

        [TestMethod]
        public void then_property_converter_defaults_to_converter_from_configuration_property_information()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.IsInstanceOfType(numberProperty.Converter, typeof(Int32Converter));

            Property nameProperty = properties.Where(x => x.PropertyName == "Name").FirstOrDefault();
            Assert.IsInstanceOfType(nameProperty.Converter, typeof(StringConverter));
        }

        [TestMethod]
        public void then_property_display_name_defaults_to_reflected_property_name()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.AreEqual("Number", numberProperty.DisplayName);
        }

        [TestMethod]
        public void then_property_description_defaults_to_empty_string()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.AreEqual(string.Empty, numberProperty.Description);
        }

        [TestMethod]
        public void then_property_has_no_suggested_values()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.IsFalse(numberProperty.HasSuggestedValues);
            Assert.AreEqual(0, numberProperty.SuggestedValues.Count());
        }

        [TestMethod]
        public void then_property_is_not_read_only()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.IsFalse(numberProperty.ReadOnly);
        }

        [TestMethod]
        public void then_property_is_not_hidden()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.IsFalse(numberProperty.Hidden);
        }

        [TestMethod]
        public void then_property_has_no_editor()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.IsFalse(numberProperty.HasEditor);
        }

        [TestMethod]
        public void then_property_has_no_child_properties()
        {
            Property numberProperty = properties.Where(x => x.PropertyName == "Number").FirstOrDefault();
            Assert.IsFalse(numberProperty.HasChildProperties);
        }
    }

    [TestClass]
    public class when_disovering_property_with_designtime_attributes : given_configuration_element_with_properties
    {
        IEnumerable<Property> properties;
        ConfigurationElementWithSimpleProperties configurationElement;

        protected override void Act()
        {
            var sectionModel = SectionViewModel.CreateSection(ServiceProvider, SectionWithSimpleProperties);

            configurationElement = (ConfigurationElementWithSimpleProperties)sectionModel.ConfigurationElement;
            properties = sectionModel.Properties;
        }

        [TestMethod]
        public void then_property_type_matches_designtime_type()
        {
            Property guidProperty = properties.Where(x => x.PropertyName == "GuidAsString").FirstOrDefault();
            Assert.AreEqual(typeof(string), guidProperty.Type);
        }

        [TestMethod]
        public void then_property_value_has_designtime_type()
        {
            Property guidProperty = properties.Where(x => x.PropertyName == "GuidAsString").FirstOrDefault();
            Assert.IsInstanceOfType(guidProperty.Value, typeof(string));
        }

        [TestMethod]
        public void then_property_value_can_be_assigned_to_designtime_type()
        {
            Guid g = Guid.NewGuid();
            Property guidProperty = properties.Where(x => x.PropertyName == "GuidAsString").FirstOrDefault();
            guidProperty.Value = g.ToString();

            Assert.AreEqual(g, configurationElement.GuidAsString);
        }
    }

    [TestClass]
    public class when_disovering_property_with_required_value : given_configuration_element_with_properties
    {
        IEnumerable<ElementProperty> properties;
        ConfigurationElementWithSimpleProperties configurationElement;

        protected override void Act()
        {
            var sectionModel = SectionViewModel.CreateSection(ServiceProvider, SectionWithSimpleProperties);

            configurationElement = (ConfigurationElementWithSimpleProperties)sectionModel.ConfigurationElement;
            properties = sectionModel.Properties.OfType<ElementProperty>();
        }


        [TestMethod]
        public void then_is_required_is_true()
        {
            var requiredProperty = properties.Where(x=>x.PropertyName == "RequiredProperty").First();
            Assert.IsTrue(requiredProperty.IsRequired);
        }
    }


    [TestClass]
    public class when_disovering_property_with_designtime_readonly : given_configuration_element_with_properties
    {
        IEnumerable<ElementProperty> properties;
        ConfigurationElementWithSimpleProperties configurationElement;

        protected override void Act()
        {
            var sectionModel = SectionViewModel.CreateSection(ServiceProvider, SectionWithSimpleProperties);

            configurationElement = (ConfigurationElementWithSimpleProperties)sectionModel.ConfigurationElement;
            properties = sectionModel.Properties.OfType<ElementProperty>();
        }

        [TestMethod]
        public void then_should_return_true_if_designtimereadonly()
        {
            var property = properties.Where(p => p.PropertyName == "DesignTimeReadOnly").First();
            Assert.IsTrue(property.DesignTimeReadOnly);
        }

        [TestMethod]
        public void then_should_return_true_if_not_designtimereadonly()
        {
            var property = properties.Where(p => p.PropertyName == "DesignTimeWritable").First();
            Assert.IsFalse(property.DesignTimeReadOnly);
        }

        [TestMethod]
        public void then_should_default_to_readonly_value_if_not_set()
        {
            var property = properties.Where(p => p.PropertyName == "ReadOnlyProperty").First();
            Assert.AreEqual(property.ReadOnly, property.DesignTimeReadOnly);
        }
    }
    //[TestClass]
    //public class when_disovering_property_with_validation_attribute : given_configuration_element_with_properties
    //{
    //    IEnumerable<ConfigElementPropertyModel> properties;
    //    ConfigurationElementWithSimpleProperties simpleElement;

    //    protected override void Act()
    //    {
    //        simpleElement = new ConfigurationElementWithSimpleProperties();
    //        properties = DiscoverPropertyModel(simpleElement);
    //    }

    //    [TestMethod]
    //    public void then_property_model_contains_configuration_validation_rule()
    //    {
    //        var validatedProperty = properties.Where(x => x.PropertyName == "PropertyWithConfigurationValidator").First();
    //        Assert.AreEqual(1, validatedProperty.ValidationRules.Count());
    //        Assert.IsInstanceOfType(validatedProperty.ValidationRules.First(), typeof(ConfigurationValidatorRule));
    //        Assert.IsInstanceOfType(validatedProperty.ValidationRules.OfType<ConfigurationValidatorRule>().First().ConfigurationValidator, typeof(StringValidator));
    //    }
    //}

    //[TestClass]
    //public class when_changing_monitored_property_values : given_configuration_element_with_properties
    //{
    //    private string lastPropertyChanged;
    //    private ConfigElementModel elementModel;


    //    protected override void Arrange()
    //    {
    //        base.Arrange();
    //        var simpleElement = new ConfigurationElementWithSimpleProperties();
    //        elementModel = ElementFactory.Create(null, null, simpleElement);
    //        elementModel.PropertyChanged += (s, e) => { lastPropertyChanged = e.PropertyName;  };
    //        lastPropertyChanged = String.Empty;
    //    }

    //    protected override void Act()
    //    {
    //        elementModel.Properties.First(p => p.PropertyName == "Name").Value = "SomeNewValue";
    //    }

    //    [TestMethod]
    //    public void then_property_change_notification_is_raised_for_name_change()
    //    {
    //        Assert.AreEqual("Name", lastPropertyChanged);
    //    }
    //}
}
