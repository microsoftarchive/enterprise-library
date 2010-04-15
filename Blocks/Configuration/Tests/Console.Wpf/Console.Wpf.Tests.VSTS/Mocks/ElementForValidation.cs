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
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    [ElementValidationAttribute(typeof(ElementErrorProducingValidator))]
    [NamePropertyAttribute("Name")]
    public class ElementForValidation : ConfigurationSection
    {
        private const string typeValidatedCollection = "typeValidatedCollection ";
        private const string typeValidatedProperty = "typeValidatedProperty";
        private const string requiredReferencingElement = "requiredReferencingElement";
        private const string referencedItemsCollection = "referencedItemsCollection";
        private const string defaultTestElement = "defaultTestElement";
        private const string defaultTestElementRequired = "defaultTestElementRequired";
        private const string testElements = "testElements";
        private const string propertyWithMultipleValidationTypes = "propertyWithMultipleValidationTypes";
        private const string intProperty = "intProperty";
        private const string propertyWithConfigurationValidators = "propertyWithConfigurationValidators";
        private const string propertyWithConfigurationRequirements = "propertyWithConfigurationRequirements";
        private const string propertyWithValidationError = "propertyWithValidationError";
        private const string propertyWithNoValidators = "propertyWithNoValidators";
        private const string propertyName = "propertyName";

        [ConfigurationProperty(propertyName)]
        public string Name
        {
            get { return (string)this[propertyName]; }
            set { this[propertyName] = value; }
        }

        [ConfigurationProperty(propertyWithNoValidators)]
        public string PropertyWithNoValidators
        {
            get { return (string)this[propertyWithNoValidators]; }
            set { this[propertyWithNoValidators] = value; }
        }

        [ConfigurationProperty(propertyWithValidationError)]
        [Validation(typeof(ErrorProducingValidator))]
        public string PropertyWithValidationError
        {
            get { return (string)this[propertyWithValidationError]; }
            set { this[propertyWithValidationError] = value; }
        }

        [ConfigurationProperty(propertyWithConfigurationRequirements, IsRequired = true)]
        public string PropertyWithConfigurationRequirements
        {
            get { return (string)this[propertyWithConfigurationRequirements]; }
            set { this[propertyWithConfigurationRequirements] = value; }
        }

        [ConfigurationProperty(propertyWithConfigurationValidators, DefaultValue = "AValidValue")]
        [StringValidator(MinLength = 5)]
        public string PropertyWithConfigurationValidators
        {
            get { return (string)this[propertyWithConfigurationValidators]; }
            set { this[propertyWithConfigurationValidators] = value; }
        }

        [ConfigurationProperty(propertyWithMultipleValidationTypes, DefaultValue = "AValidValue")]
        [StringValidator(MinLength = 5)]
        [Validation(typeof(ErrorProducingValidator))]
        public string PropertyWithMultipleValidationTypes
        {
            get { return (string)this[propertyWithMultipleValidationTypes]; }
            set { this[propertyWithMultipleValidationTypes] = value; }
        }

        [ConfigurationProperty(intProperty)]
        public int IntProperty
        {
            get { return (int)this[intProperty]; }
            set { this[intProperty] = value; }
        }

        [ConfigurationProperty(testElements)]
        public TestElementConfigurationCollection TestElements
        {
            get { return (TestElementConfigurationCollection)this[testElements]; }
            set { this[testElements] = value; }
        }

        [ConfigurationProperty(referencedItemsCollection)]
        [ConfigurationCollection(typeof(TestNamedElement))]
        public NamedElementCollection<TestNamedElement> ReferencedItems
        {
            get { return (NamedElementCollection<TestNamedElement>)this[referencedItemsCollection];}   
            set { this[referencedItemsCollection] = value;}   
        }

        [ConfigurationProperty(defaultTestElement)] 
        [Reference(typeof(ElementForValidation), typeof(TestNamedElement))]
        public string DefaultTestElement
        {
            get { return (string) this[defaultTestElement];}
            set { this[defaultTestElement] = value;}
        }

        [ConfigurationProperty(defaultTestElementRequired, IsRequired = true)]
        /*review: this was needed becuase test 'when_validating_property_with_element_validation.then_element_validation_results_do_not_appear_on_property' needs it*/
        [Reference(typeof(ElementForValidation), typeof(TestNamedElement))]
        public string DefaultTestElementRequired
        {
            get { return (string)this[defaultTestElementRequired]; }
            set { this[defaultTestElementRequired] = value; }
        }

        [ConfigurationProperty(requiredReferencingElement, IsRequired = true)]
        [Reference(typeof(ElementForValidation), typeof(TestNamedElement))]
        public string RequiredReferencingElement
        {
            get { return (string)this[requiredReferencingElement]; }
            set { this[requiredReferencingElement] = value; }
        }

        [ConfigurationProperty(typeValidatedProperty)]
        [Validation(typeof(TypeValidator))]
        public string ValidatedTypeName
        {
            get { return (string) this[typeValidatedProperty]; }
            set { this[typeValidatedProperty] = value; }
        }

        [ConfigurationProperty(typeValidatedCollection)]
        [ConfigurationCollection(typeof(TestNamedElement))]
        [Editor(typeof(FrameworkElement), typeof(FrameworkElement))]
        [ElementValidation(typeof(CollectionCountOneValidator))]
        [Validation(typeof(ErrorProducingValidator))]
        public NamedElementCollection<TestNamedElement> ValidatedCollection
        {
            get { return (NamedElementCollection<TestNamedElement>)this[referencedItemsCollection]; }
            set { this[referencedItemsCollection] = value; }
        }
    }

    [ConfigurationCollection(typeof(TestElement))]
    public class TestElementConfigurationCollection : ConfigurationElementCollection, IMergeableConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TestElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            TestElement testElement = element as TestElement;
            return testElement.KeyValue;
        }

        public void ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            base.BaseClear();
            foreach (var element in configurationElements)
            {
                base.BaseAdd(element);
            }
        }

        public ConfigurationElement CreateNewElement(Type configurationType)
        {
            return CreateNewElement();
        }
    }

    public class TestElement : ConfigurationElement
    {
        private const string keyValueProperty = "keyValueProperty";
        private const string otherKeyValueProperty = "otherKeyValueProperty";

        [ConfigurationProperty(keyValueProperty, IsKey = true, IsRequired = true, DefaultValue = 1)]
        public int KeyValue
        {
            get { return (int)this[keyValueProperty]; }
            set { this[keyValueProperty] = value; }
        }

        [ConfigurationProperty(otherKeyValueProperty, IsKey = true, IsRequired = true, DefaultValue = "blah")]
        public string OtherKeyValue
        {
            get { return (string)this[otherKeyValueProperty]; }
            set { this[otherKeyValueProperty] = value; }
        }
    }

    public class TestNamedElement : NamedConfigurationElement
    {
        private const string requiredValueProperty = "requiredValueProperty";

        [ConfigurationProperty(requiredValueProperty, IsRequired = true, DefaultValue = "InitialValue")]
        public string RequiredValue
        {
            get { return (string)this[requiredValueProperty]; }
            set { this[requiredValueProperty] = value;}
        }
    }

}
