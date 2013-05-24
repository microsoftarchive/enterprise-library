//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class PropertyValidationFactoryFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void RequestForValidatorBasedOnAttributesReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidatorFromAttributes(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                               typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"), "",
                                                                               new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message1", resultsList[0].Message);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnAttributesReturnsNullForNonExistingRuleName()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidatorFromAttributes(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                               typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"), "InvalidRule",
                                                                               new MemberAccessValidatorBuilderFactory());

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnAttributesWithRulesetReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidatorFromAttributes(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                               typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"), "RuleA",
                                                                               new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message1-RuleA", resultsList[0].Message);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnAttributesWithRulesetAndValidationSpecificationSourceReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidator(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                 typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"),
                                                                 "RuleA",
                                                                 ValidationSpecificationSource.Attributes,
                                                                 new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message1-RuleA", resultsList[0].Message);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnConfigurationReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidatorFromConfiguration(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                                  typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"), "",
                                                                                  new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message-from-config1", resultsList[0].Message);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnConfigurationReturnsNullForNonExistingRuleName()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidatorFromAttributes(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                               typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"), "InvalidRule",
                                                                               new MemberAccessValidatorBuilderFactory());

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnConfigurationWithRulesetReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidatorFromConfiguration(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                                  typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"), "RuleA",
                                                                                  new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message-from-config1-RuleA", resultsList[0].Message);
            Assert.AreEqual("message-from-config2-RuleA", resultsList[1].Message);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnConfigurationWithRulesetAndValidationSpecificationSourceReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidator(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                 typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"),
                                                                 "RuleA",
                                                                 ValidationSpecificationSource.Configuration,
                                                                 new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message-from-config1-RuleA", resultsList[0].Message);
            Assert.AreEqual("message-from-config2-RuleA", resultsList[1].Message);
        }

        [TestMethod]
        public void RequestForValidatorBasedOnAttributesAndConfigurationWithRulesetThroughStaticFacadeReturnsAppropriateValidator()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidator(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                 typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("FailingProperty1"),
                                                                 "RuleA",
                                                                 ValidationSpecificationSource.Both,
                                                                 new MemberAccessValidatorBuilderFactory());

            TestObjectWithFailingAttributesOnProperties objectToTest = new TestObjectWithFailingAttributesOnProperties();
            objectToTest.FailingProperty1 = "property value";
            ValidationResults validationResults = validator.Validate(objectToTest);

            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void RequestForValidatorBasedOnAttributesAndConfigurationWithRulesetThroughStaticFacadeReturnsNullIfNoValidationInfomationExistsForProperty()
        {
            Validator validator
                = PropertyValidationFactory.GetPropertyValidator(typeof(TestObjectWithFailingAttributesOnProperties),
                                                                 typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("PropertyWithoutAttributes"),
                                                                 "RuleA",
                                                                 ValidationSpecificationSource.Both,
                                                                 new MemberAccessValidatorBuilderFactory());

            Assert.IsNull(validator);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InstanceConfiguredWithNullTypeThrows()
        {
            PropertyValidationFactory.GetPropertyValidator(null,
                                                           typeof(TestObjectWithFailingAttributesOnProperties).GetProperty("PropertyWithoutAttributes"),
                                                           "",
                                                           ValidationSpecificationSource.Attributes,
                                                           new MemberAccessValidatorBuilderFactory());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InstanceConfiguredWithNullPropertyNameThrows()
        {
            PropertyValidationFactory.GetPropertyValidator(typeof(TestObjectWithFailingAttributesOnProperties),
                                                           null,
                                                           "",
                                                           ValidationSpecificationSource.Attributes,
                                                           new MemberAccessValidatorBuilderFactory());
        }

        [TestMethod]
        public void CanCreateValidatorForValidationAttributes()
        {
            var validator =
                PropertyValidationFactory.GetPropertyValidator(
                    typeof(TestObjectWithMultipleSourceValidationAttributesOnProperties),
                    StaticReflection.GetPropertyInfo((TestObjectWithMultipleSourceValidationAttributesOnProperties t)
                        => t.PropertyWithMixedAttributes),
                    "",
                    ValidationSpecificationSource.DataAnnotations,
                    new MemberAccessValidatorBuilderFactory());

            var validResults =
                validator.Validate(
                    new TestObjectWithMultipleSourceValidationAttributesOnProperties
                    {
                        PropertyWithMixedAttributes = "v"
                    });
            var invalidResults =
                    validator.Validate(
                    new TestObjectWithMultipleSourceValidationAttributesOnProperties
                    {
                        PropertyWithMixedAttributes = "invalid"
                    });

            Assert.IsTrue(validResults.IsValid);
            Assert.IsFalse(invalidResults.IsValid);
            Assert.AreEqual(1, invalidResults.Count);
            Assert.IsTrue(
                invalidResults.Any(vr =>
                    vr.Key == "PropertyWithMixedAttributes" && vr.Message == "data annotations-mixed"));
        }

        [TestMethod]
        public void CanCreateValidatorForValidatorAndValidationAttributes()
        {
            var validator =
                PropertyValidationFactory.GetPropertyValidator(
                    typeof(TestObjectWithMultipleSourceValidationAttributesOnProperties),
                    StaticReflection.GetPropertyInfo((TestObjectWithMultipleSourceValidationAttributesOnProperties t)
                        => t.PropertyWithMixedAttributes),
                    "",
                    ValidationSpecificationSource.DataAnnotations | ValidationSpecificationSource.Attributes,
                    new MemberAccessValidatorBuilderFactory());

            var invalidResults =
                    validator.Validate(
                    new TestObjectWithMultipleSourceValidationAttributesOnProperties
                    {
                        PropertyWithMixedAttributes = "invalid"
                    });

            Assert.IsFalse(invalidResults.IsValid);
            Assert.AreEqual(2, invalidResults.Count);
            Assert.IsTrue(
                invalidResults.Any(vr =>
                    vr.Key == "PropertyWithMixedAttributes" && vr.Message == "data annotations-mixed"));
            Assert.IsTrue(
                invalidResults.Any(vr =>
                    vr.Key == "PropertyWithMixedAttributes" && vr.Message == "vab-mixed"));
        }
    }
}
