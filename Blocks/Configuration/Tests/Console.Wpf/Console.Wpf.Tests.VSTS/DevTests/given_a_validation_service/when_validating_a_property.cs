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
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.Unity;
using ConfigurationProperty = System.Configuration.ConfigurationProperty;

namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    public abstract class SectionWithValidatableProperties : ContainerContext
    {
        protected ValidationService Validator { get; private set; }
        protected SectionViewModel Section { get; private set; }
        protected ElementProperty property { get; private set; }

        private IEnumerable<ValidationError> resultsForValidProperty;
        private IEnumerable<ValidationError> resultsForInvalidProperty;

        protected override void Arrange()
        {
            base.Arrange();

            Validator = Container.Resolve<ValidationService>();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new ElementForValidation();

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            Section = sourceModel.Sections.Where(s => s.SectionName == "testSection").Single();

            property = (ElementProperty)Section.Property(ArrangePropertyName());
        }

        protected abstract string ArrangePropertyName();
    }

    [TestClass]
    public class when_validating_a_property_with_no_validators : SectionWithValidatableProperties
    {
        private IEnumerable<ValidationError> results;

        protected override string ArrangePropertyName()
        {
            return "PropertyWithNoValidators";
        }

        protected override void Act()
        {
            results = Validator.Validate(property);
        }

        [TestMethod]
        public void then_property_is_valid()
        {
            Assert.IsFalse(results.Any());
        }
    }

    [TestClass]
    public class when_validating_an_invalid_property : SectionWithValidatableProperties
    {
        private IEnumerable<ValidationError> results;

        protected override string ArrangePropertyName()
        {
            return "PropertyWithValidationError";
        }

        protected override void Act()
        {
            results = Validator.Validate(property);
        }

        [TestMethod]
        public void then_property_is_invalid()
        {
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void then_all_validation_result_returns_correct_error_message()
        {
            Assert.AreEqual("Test Validation Error", results.First().Message);
        }

        [TestMethod]
        public void then_property_name_returned_in_results()
        {
            Assert.AreEqual(property.DisplayName, results.First().PropertyName);
        }

        [TestMethod]
        public void then_result_level_defaults_to_error()
        {
            Assert.IsFalse(results.First().IsWarning);
        }

        [TestMethod]
        public void then_element_path_matches()
        {
            Assert.AreEqual(property.DeclaringElement.Path,results.First().ElementPath );
        }
    }

    [TestClass]
    public class when_validating_property_marked_as_required : SectionWithValidatableProperties
    {
        private IEnumerable<ValidationError> results;

        protected override string ArrangePropertyName()
        {
            return "PropertyWithConfigurationRequirements";
        }

        protected override void Act()
        {
            results = Validator.Validate(property);
        }

        [TestMethod]
        public void then_required_field_fires()
        {
            Assert.IsTrue(
                results.Where(
                    r => r.Message == string.Format("The field {0} is missing a required value.", property.DisplayName))
                    .Any());
        }
    }

    [TestClass]
    public class when_setting_invalid_configuration_bindable_property_values : SectionWithValidatableProperties
    {
        private Exception capturedException;

        protected override string ArrangePropertyName()
        {
            return "PropertyWithConfigurationValidators";
        }

        protected override void Act()
        {
            try
            {
                property.BindableValue = "Bad";
            }
            catch (Exception ex)
            {
                capturedException = ex;
            }

            Assert.IsNotNull(capturedException);
        }

        [TestMethod]
        public void then_revalidating_produces_same_results()
        {
            var results = Validator.Validate(property);
            Assert.IsTrue(
                results.Where(r => r.Message == string.Format("The string must be at least 5 characters long.")).Any());
        }
    }

    [TestClass]
    public class when_valid_property_with_rules_to_execute : SectionWithValidatableProperties
    {
        private Exception capturedException;

        protected override string ArrangePropertyName()
        {
            return "PropertyWithConfigurationValidators";
        }

        protected override void Act()
        {
            try
            {
                property.BindableValue = "GoodValue";
            }
            catch (Exception ex)
            {
                capturedException = ex;
            }

            Assert.IsNull(capturedException);
        }

        [TestMethod]
        public void then_should_return_no_errors()
        {
            var results = Validator.Validate(property);
            Assert.IsFalse(results.Any());
        }
    }

    [TestClass]
    public class when_converter_fails_will_appear_in_validation_results : SectionWithValidatableProperties
    {
        private Exception capturedException;

        protected override string ArrangePropertyName()
        {
            return "IntProperty";
        }

        protected override void Act()
        {
            try
            {
                property.BindableValue = "NonInteger";
            }
            catch (Exception ex)
            {
                capturedException = ex;
            }

            Assert.IsNotNull(capturedException);
        }

        [TestMethod]
        public void then_conversion_failure_appears_in_error_result()
        {
            var results = Validator.Validate(property);
            Assert.IsTrue(results.Where(r => r.Message == "NonInteger is not a valid value for Int32.").Any());
        }

    }

    [TestClass]
    public class when_composite_key_validation_fails : SectionWithValidatableProperties
    {
        private ElementCollectionViewModel collection;
        private IEnumerable<ValidationError> results;

        protected override void Arrange()
        {
            base.Arrange();

            collection = Section.GetDescendentsOfType<TestElementConfigurationCollection>().OfType<ElementCollectionViewModel>().Single();
            var newElement = collection.AddNewCollectionElement(typeof(TestElement));
            newElement.Property("KeyValue").Value = 5;
            newElement.Property("OtherKeyValue").Value = "StringKey";
        }

        protected override string ArrangePropertyName()
        {
            return "TestElements";
        }

        protected override void Act()
        {
            var addedElement = collection.AddNewCollectionElement(typeof(TestElement));
            addedElement.Property("KeyValue").BindableValue = 5.ToString();
            addedElement.Property("OtherKeyValue").BindableValue = "StringKey";

            results = Validator.Validate(addedElement.Property("KeyValue"));
        }

        [TestMethod]
        public void then_results_indicate_key_clash()
        {
            Assert.IsTrue(results.Any(r => r.PropertyName == "KeyValue" && r.Message == "Duplicate key value."));
        }
    }

    public class ElementForValidation : ConfigurationSection
    {
        private const string testElements = "testElements";
        private const string propertyWithMultipleValidationTypes = "propertyWithMultipleValidationTypes";
        private const string intProperty = "intProperty";
        private const string propertyWithConfigurationValidators = "propertyWithConfigurationValidators";
        private const string propertyWithConfigurationRequirements = "propertyWithConfigurationRequirements";
        private const string propertyWithValidationError = "propertyWithValidationError";
        private const string propertyWithNoValidators = "propertyWithNoValidators";

        [ConfigurationProperty(propertyWithNoValidators)]
        public string PropertyWithNoValidators
        {
            get { return (string)this[propertyWithNoValidators]; }
            set { this[propertyWithNoValidators] = value; }
        }

        [ConfigurationProperty(propertyWithValidationError)]
        [ErrorProducingValidator]
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
        [ErrorProducingValidator]
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

    public class ErrorProducingValidator : ValidationAttribute
    {
        protected override void ValidateCore(object instance, IList<ValidationError> errors)
        {
            var property = instance as ElementProperty;
            if (property == null)
            {
                errors.Add(new ValidationError("Unknown", "instance was not an instance", string.Empty));
            }

            errors.Add(new ValidationError(property.DisplayName, "Test Validation Error", property.DeclaringElement.Path));
        }
    }
}
