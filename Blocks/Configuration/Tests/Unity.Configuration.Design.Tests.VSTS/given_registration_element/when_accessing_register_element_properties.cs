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
using System.Reflection;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_accessing_register_element_properties : given_registration_element
    {
        IEnumerable<Property> registrationProperties;

        protected override void Act()
        {
            registrationProperties = base.RegistrationElement.Properties;
        }

        [TestMethod]
        public void then_properties_contain_properties_for_lifetime()
        {
            foreach (PropertyInfo property in typeof(LifetimeElement).GetProperties().Where(x=>Attribute.GetCustomAttributes(x).OfType<ConfigurationProperty>().Any()))
            {
                Assert.IsNotNull(FindCorrespondingProperty(registrationProperties, property));
            }
        }

        [TestMethod]
        public void then_map_to_name_property_has_type_picker()
        {
            var reflectionProperty = typeof(RegisterElement).GetProperty("MapToName");
            Property mapToNameProperty = FindCorrespondingProperty(registrationProperties, reflectionProperty);

            Assert.IsNotNull(mapToNameProperty.BindableProperty as PopupEditorBindableProperty);
        }

        [TestMethod]
        [Ignore]
        public void then_lifetime_properties_have_lifetime_category()
        {
            foreach (PropertyInfo property in typeof(LifetimeElement).GetProperties().Where(x => Attribute.GetCustomAttributes(x).OfType<ConfigurationProperty>().Any()))
            {
                var propertyModal = FindCorrespondingProperty(registrationProperties, property);
                Assert.AreNotEqual(-1, propertyModal.Category.IndexOf("Lifetime", StringComparison.OrdinalIgnoreCase));
            }
        }
        


        [TestMethod]
        [Ignore]
        public void then_lifetime_lifetime_manager_property_has_default_value()
        {
            var reflectionProperty = typeof(LifetimeElement).GetProperty("TypeName");
            Property lifetimeManagerProperty = FindCorrespondingProperty(registrationProperties, reflectionProperty);

            Assert.IsNotNull(lifetimeManagerProperty.Value);
        }

        [TestMethod]
        [Ignore]
        public void then_lifetime_lifetime_manager_property_has_editor()
        {
            var reflectionProperty = typeof(LifetimeElement).GetProperty("TypeName");
            Property lifetimeManagerProperty = FindCorrespondingProperty(registrationProperties, reflectionProperty);

            Assert.IsInstanceOfType(lifetimeManagerProperty.BindableProperty, typeof(PopupEditorBindableProperty));
        }

        [TestMethod]
        [Ignore]
        public void then_lifetime_lifetime_manager_type_converter_property_has_editor()
        {
            var reflectionProperty = typeof(LifetimeElement).GetProperty("TypeConverterTypeName");
            Property lifetimeManagerTypeConverterProperty = FindCorrespondingProperty(registrationProperties, reflectionProperty);

            Assert.IsInstanceOfType(lifetimeManagerTypeConverterProperty.BindableProperty, typeof(PopupEditorBindableProperty));
        }


        private static Property FindCorrespondingProperty(IEnumerable<Property> properties, PropertyInfo originalProperty)
        {
            return properties.FirstOrDefault(x =>
                        x.DeclaringProperty.ComponentType == originalProperty.DeclaringType
                        && x.DeclaringProperty.Name == originalProperty.Name);
                        
        }
    }
}
