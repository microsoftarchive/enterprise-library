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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationProperty = System.Configuration.ConfigurationProperty;
using System.Collections.Specialized;

namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    [TestClass]
    public class when_validating_a_property_with_no_validators : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "PropertyWithNoValidators";
        }

        protected override void Act()
        {
            property.Validate(property.BindableProperty.BindableValue);
        }

        [TestMethod]
        public void then_property_is_valid()
        {
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_setting_value_on_property_with_validation_errors : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "PropertyWithValidationError";
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "SomeValue";
        }

        [TestMethod]
        public void then_property_is_invalid()
        {
            Assert.IsFalse(property.IsValid);
        }

        [TestMethod]
        public void then_all_validation_result_returns_correct_error_message()
        {
            Assert.AreEqual("Test Validation Error", property.ValidationResults.Single().Message);
        }

        [TestMethod]
        public void then_property_name_returned_in_results()
        {
            Assert.AreEqual(property.DisplayName, property.ValidationResults.Single().PropertyName);
        }

        [TestMethod]
        public void then_result_level_defaults_to_error()
        {
            Assert.IsFalse(property.ValidationResults.First().IsWarning);
        }

        [TestMethod]
        public void then_element_path_matches()
        {
            Assert.AreEqual(property.DeclaringElement.ElementId, property.ValidationResults.First().ElementId);
        }
    }

    [TestClass]
    public class when_validating_property_marked_as_required : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "PropertyWithConfigurationRequirements";
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = null;
        }

        [TestMethod]
        public void then_required_field_fires()
        {
            Assert.IsTrue(
                property.ValidationResults.Where(
                    r => r.Message == string.Format("The field {0} is missing a required value.", property.DisplayName))
                    .Any());
        }
    }

    [TestClass]
    public class when_setting_invalid_configuration_bindable_property_values : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "PropertyWithConfigurationValidators";
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "Bad";
        }

        [TestMethod]
        public void then_revalidating_produces_same_results()
        {
            Assert.IsTrue(
                property.ValidationResults.Where(r => r.Message == string.Format("The string must be at least 5 characters long.")).Any());
        }
    }

    [TestClass]
    public class when_valid_property_with_rules_to_execute : SectionWithValidatablePropertiesContext
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
                property.BindableProperty.BindableValue = "GoodValue";
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
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_converter_fails_will_appear_in_validation_results : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "IntProperty";
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "NonInteger";
        }

        [TestMethod]
        public void then_conversion_failure_appears_in_error_result()
        {
            Assert.IsTrue(property.ValidationResults.Where(r => r.Message == "NonInteger is not a valid value for Int32.").Any());
        }

    }

    [TestClass]
    public class when_composite_key_validation_fails : SectionWithValidatablePropertiesContext
    {
        private ElementCollectionViewModel collection;
        private ElementViewModel addedElement;

        protected override void Arrange()
        {
            base.Arrange();

            collection = Section.GetDescendentsOfType<TestElementConfigurationCollection>().OfType<ElementCollectionViewModel>().Single();
            var newElement = collection.AddNewCollectionElement(typeof(TestElement));
            newElement.Property("KeyValue").BindableProperty.BindableValue = "5";
            newElement.Property("OtherKeyValue").BindableProperty.BindableValue = "StringKey";
        }

        protected override string ArrangePropertyName()
        {
            return "TestElements";
        }

        protected override void Act()
        {
            addedElement = collection.AddNewCollectionElement(typeof(TestElement));
            addedElement.Property("KeyValue").BindableProperty.BindableValue = "5";
            addedElement.Property("OtherKeyValue").BindableProperty.BindableValue = "StringKey";
        }

        [TestMethod]
        public void then_results_indicate_key_clash()
        {
            Assert.IsTrue(addedElement.Property("OtherKeyValue").ValidationResults.Any(r => r.PropertyName == "OtherKeyValue" && r.Message == "Duplicate key value."));
        }

        [TestMethod]
        public void then_value_not_commited()
        {
            var addedProperty = addedElement.Property("OtherKeyValue");
            Assert.IsFalse(addedProperty.Value.ToString() == addedProperty.BindableProperty.BindableValue);
        }

        [TestMethod]
        public void then_validating_other_key_participant_doesnt_result_in_validation_error()
        {
            var anotherProperty = addedElement.Property("KeyValue");
            anotherProperty.Validate();
            Assert.IsFalse(addedElement.Property("KeyValue").ValidationResults.Any(r => r.PropertyName == "KeyValue" && r.Message == "Duplicate key value."));
        }
    }

    [TestClass]
    public class when_resolving_validation_error : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "PropertyWithConfigurationValidators";
        }

        protected override void Arrange()
        {
            base.Arrange();
            property.BindableProperty.BindableValue = "Bad";

        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "ItIsNowLongEnough";    
        }

        [TestMethod]
        public void then_property_error_collection_cleared()
        {
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_validating_nonrequired_unresolved_reference_property : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "DefaultTestElement";
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "UnreferenceTestElement";
        }

        [TestMethod]
        public void then_produces_validation_error()
        {
            Assert.IsTrue(property.ValidationResults.Any());
        }

        [TestMethod]
        public void then_error_message_matches()
        {
           var message = property.ValidationResults.Single(m => m.PropertyName == property.PropertyName).Message;
           Assert.IsTrue(message.StartsWith("Referenced item is not available"));
        }

        [TestMethod]
        public void then_severity_is_warning()
        {
            Assert.IsTrue(property.ValidationResults.Single(m => m.PropertyName == property.PropertyName).IsWarning);
        }
    }

    [TestClass]
    public class when_validating_nonrequired_reference_set_to_none : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "DefaultTestElement";
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "<none>";
        }

        [TestMethod]
        public void then_should_not_produce_validation_error()
        {
            Assert.IsFalse(property.ValidationResults.Any()); 
        }
    }

    [TestClass]
    public class when_reference_property_resolved : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "DefaultTestElement";
        }

        protected override void Arrange()
        {
            base.Arrange();

            var testElementCollection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().Single();
            var testElement = testElementCollection.AddNewCollectionElement(typeof(TestNamedElement));
            testElement.Property("Name").BindableProperty.BindableValue = "ValidTestElement";

            property.BindableProperty.BindableValue = "Invalid Test Element Nmae";
            Assert.IsTrue(property.ValidationResults.Any());
        }

        protected override void Act()
        {
            property.BindableProperty.BindableValue = "ValidTestElement";
        }

        [TestMethod]
        public void then_validation_errors_removed()
        {
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_reference_property_resolved_by_reference_name_changing : SectionWithValidatablePropertiesContext
    {
        private ElementViewModel referencedElement;

        protected override string ArrangePropertyName()
        {
            return "DefaultTestElement";
        }

        protected override void Arrange()
        {
            base.Arrange();

            var testElementCollection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().Single();
            referencedElement = testElementCollection.AddNewCollectionElement(typeof(TestNamedElement));
            referencedElement.Property("Name").BindableProperty.BindableValue = "ValidTestElement";

            property.BindableProperty.BindableValue = "Soon To Be Valid Element";

            Assert.IsTrue(property.ValidationResults.Any());
        }

        protected override void Act()
        {
            referencedElement.Property("Name").BindableProperty.BindableValue = "Soon To Be Valid Element";
        }

        [TestMethod]
        public void then_validation_errors_removed()
        {
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_referenced_element_is_deleted : SectionWithValidatablePropertiesContext
    {
        private ElementViewModel referencedElement;

        protected override string ArrangePropertyName()
        {
            return "DefaultTestElementRequired";
        }

        protected override void Arrange()
        {
            base.Arrange();

            var testElementCollection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().Single();
            referencedElement = testElementCollection.AddNewCollectionElement(typeof(TestNamedElement));
            referencedElement.Property("Name").BindableProperty.BindableValue = "ValidTestElement";

            property.BindableProperty.BindableValue = "ValidTestElement";
            Assert.IsFalse(property.ValidationResults.Any());
        }

        protected override void Act()
        {
            referencedElement.Delete();
        }

        [TestMethod]
        public void then_reference_validation_errors_appear()
        {
            Assert.IsTrue(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_referenced_element_has_validation_error : SectionWithValidatablePropertiesContext
    {
        private ElementViewModel referencedElement;

        protected override string ArrangePropertyName()
        {
            return "DefaultTestElement";
        }

        protected override void Arrange()
        {
            base.Arrange();

            property.Value = "TestElement";

            var testElementCollection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().Single();
            referencedElement = testElementCollection.AddNewCollectionElement(typeof(TestNamedElement));
            referencedElement.Property("Name").Value = property.Value;
        }

        protected override void Act()
        {
            referencedElement.Property("RequiredValue").BindableProperty.BindableValue = string.Empty;
        }

        [TestMethod]
        public void then_referenced_element_does_not_contain_child_errors()
        {
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_required_reference_set_to_empty : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "RequiredReferencingElement";
        }

        protected override void Act()
        {
            property.Value = "";
        }

        [TestMethod]
        public void then_required_reference_validation_error_provided()
        {
            Assert.IsTrue(property.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_validating_property_with_element_validation : SectionWithValidatablePropertiesContext
    {
        private ElementCollectionViewModel collection;

        protected override string ArrangePropertyName()
        {
            return "ValidatedCollection";
        }

        protected override void Act()
        {
            collection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == property.DisplayName).OfType<ElementCollectionViewModel>().First();

            collection.AddNewCollectionElement(typeof(TestNamedElement));
        }

        [TestMethod]
        public void then_element_validation_results_do_not_appear_on_property()
        {
            Assert.IsFalse(property.ValidationResults.Any(e => e.Message == CollectionCountOneValidator.Message));
        }
    }

    [TestClass]
    public class when_validating_element_with_property_validation : SectionWithValidatablePropertiesContext
    {
        private ElementCollectionViewModel collection;

        protected override string ArrangePropertyName()
        {
            return "ValidatedCollection";
        }

        protected override void Act()
        {
            collection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == property.DisplayName).OfType<ElementCollectionViewModel>().First();
        }

        [TestMethod]
        public void then_property_validation_error_shows_in_results()
        {
            Assert.IsTrue(property.ValidationResults.Any(e => e.Message == ErrorProducingValidator.ErrorMessage));
        }
    }

    [TestClass]
    public class when_collection_element_removed_and_revalidated : SectionWithValidatablePropertiesContext
    {
        private ElementCollectionViewModel collection;

        protected override string ArrangePropertyName()
        {
            return "ValidatedCollection";
        }

        protected override void Act()
        {
            collection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                .Where(e => e.Name == property.DisplayName).OfType<ElementCollectionViewModel>().First();

            var newItem = collection.AddNewCollectionElement(typeof(TestNamedElement));
            Assert.IsTrue(collection.ValidationResults.Any(e => e.Message == CollectionCountOneValidator.Message));
            newItem.Delete();
            collection.Validate();
        }

        [TestMethod]
        public void then_collection_property_validation_fired()
        {
            Assert.IsFalse(collection.ValidationResults.Any(e => e.Message == CollectionCountOneValidator.Message));
        }
    }
}
