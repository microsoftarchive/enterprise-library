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

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValidationFactoryFixture
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
        public void ShouldCacheGenericValidatorWhenUsingUnspecifiedConfigSource()
        {
            var firstValidator = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();
            var secondValidator = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void ShouldCacheNonGenericValidatorWhenUsingUnspecifiedConfigSource()
        {
            var firstValidator = ValidationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));
            var secondValidator = ValidationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void ShouldCacheGenericValidatorFromAttributes()
        {
            var firstValidator = ValidationFactory.CreateValidatorFromAttributes<TestObjectWithFailingAttributesOnProperties>();
            var secondValidator = ValidationFactory.CreateValidatorFromAttributes<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void ShouldCacheNonGenericValidatorFromAttributes()
        {
            var firstValidator = ValidationFactory.CreateValidatorFromAttributes(typeof(TestObjectWithFailingAttributesOnProperties), string.Empty);
            var secondValidator = ValidationFactory.CreateValidatorFromAttributes(typeof(TestObjectWithFailingAttributesOnProperties), string.Empty);

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void ShouldCacheGenericValidatorFromConfig()
        {
            var firstValidator = ValidationFactory.CreateValidatorFromConfiguration<TestObjectWithFailingAttributesOnProperties>();
            var secondValidator = ValidationFactory.CreateValidatorFromConfiguration<TestObjectWithFailingAttributesOnProperties>();

            Assert.IsNotNull(firstValidator);
            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void ShouldCacheNonGenericValidatorFromConfig()
        {
            var firstValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(TestObjectWithFailingAttributesOnProperties), string.Empty);
            var secondValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(TestObjectWithFailingAttributesOnProperties), string.Empty);

            Assert.IsNotNull(firstValidator);
            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void ResetCachesShouldReturnNewValidators()
        {
            var firstValidator = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();
            ValidationFactory.ResetCaches();
            var secondValidator = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreNotSame(firstValidator, secondValidator);
        }

        #region Test Fix "Remove unnecessary AndCompositeValidator from hierarchy"

        [TestMethod]
        public void CreatedAndCompositeValidatorFromAttributesWithPropertiesAppropiately()
        {
            Validator validator = ValidationFactory.CreateValidator(typeof(MockClassWithValidatorAttributesOnProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(GenericValidatorWrapper<MockClassWithValidatorAttributesOnProperties>), validator.GetType());

            var validatorWrapper = validator as GenericValidatorWrapper<MockClassWithValidatorAttributesOnProperties>;

            AndCompositeValidator compositeValidator = validatorWrapper.WrappedValidator as AndCompositeValidator;

            Assert.IsNotNull(compositeValidator);

            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(2, allValidators.Count);

            Assert.AreEqual(typeof(ValueAccessValidator), allValidators[0].GetType());
            Assert.AreEqual(typeof(ValueAccessValidator), allValidators[1].GetType());
        }

        [TestMethod]
        public void CreatedAndCompositeValidatorFromConfigAppropiately()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(BaseTestDomainObject));
            settings.Types.Add(typeReference);
            ValidationRulesetData ruleData = new ValidationRulesetData("RuleA");
            typeReference.Rulesets.Add(ruleData);
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("Property1");
            ruleData.Properties.Add(propertyReference1);
            MockValidatorData validator11 = new MockValidatorData("validator1", true);
            propertyReference1.Validators.Add(validator11);
            validator11.MessageTemplate = "message-from-config1-RuleA";

            ValidatedPropertyReference propertyReference2 = new ValidatedPropertyReference("Property2");
            ruleData.Properties.Add(propertyReference2);
            propertyReference2.Validators.Add(validator11);

            Validator validator
                = ValidationFactory.CreateValidatorFromConfiguration(typeof(BaseTestDomainObject), "RuleA", (IConfigurationSource)configurationSource);

            var validatorWrapper = validator as GenericValidatorWrapper<BaseTestDomainObject>;

            AndCompositeValidator compositeValidator = validatorWrapper.WrappedValidator as AndCompositeValidator;

            Assert.IsNotNull(compositeValidator);

            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(2, allValidators.Count);

            Assert.AreEqual(typeof(ValueAccessValidator), allValidators[0].GetType());
            Assert.AreEqual(typeof(ValueAccessValidator), allValidators[1].GetType());
        }

        #endregion

        #region Test adding validators with DataAnnotations validation attributes

        [TestMethod]
        public void CanGetValidatorFromVABAttributesOnly()
        {
            var validator =
                ValidationFactory.CreateValidator<TestObjectWithMultipleSourceValidationAttributesOnProperties>(
                    ValidationSpecificationSource.Attributes);

            var instance =
                new TestObjectWithMultipleSourceValidationAttributesOnProperties
                {
                    PropertyWithDataAnnotationsAttributes = "invalid",
                    PropertyWithMixedAttributes = "invalid",
                    PropertyWithVABOnlyAttributes = "invalid"
                };

            var results = validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithMixedAttributes" && vr.Message == "vab-mixed"));
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithVABOnlyAttributes" && vr.Message == "vab-only"));
        }

        [TestMethod]
        public void CanGetValidatorFromDataAnnotationsAttributesOnly()
        {
            var validator =
                ValidationFactory.CreateValidator<TestObjectWithMultipleSourceValidationAttributesOnProperties>(
                    ValidationSpecificationSource.DataAnnotations);

            var instance =
                new TestObjectWithMultipleSourceValidationAttributesOnProperties
                {
                    PropertyWithDataAnnotationsAttributes = "invalid",
                    PropertyWithMixedAttributes = "invalid",
                    PropertyWithVABOnlyAttributes = "invalid"
                };

            var results = validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithMixedAttributes" && vr.Message == "data annotations-mixed"));
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithDataAnnotationsAttributes" && vr.Message == "data annotations-only"));
        }

        [TestMethod]
        public void CanGetValidatorFromDataAnnotationsAndVABAttributes()
        {
            var validator =
                ValidationFactory.CreateValidator<TestObjectWithMultipleSourceValidationAttributesOnProperties>(
                    ValidationSpecificationSource.Attributes | ValidationSpecificationSource.DataAnnotations);

            var instance =
                new TestObjectWithMultipleSourceValidationAttributesOnProperties
                {
                    PropertyWithDataAnnotationsAttributes = "invalid",
                    PropertyWithMixedAttributes = "invalid",
                    PropertyWithVABOnlyAttributes = "invalid"
                };

            var results = validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithMixedAttributes" && vr.Message == "vab-mixed"));
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithMixedAttributes" && vr.Message == "data annotations-mixed"));
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithVABOnlyAttributes" && vr.Message == "vab-only"));
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithDataAnnotationsAttributes" && vr.Message == "data annotations-only"));
        }

        [TestMethod]
        public void CanGetValidatorFromConfigurationOly()
        {
            var validator =
                ValidationFactory.CreateValidator<TestObjectWithMultipleSourceValidationAttributesOnProperties>(
                    ValidationSpecificationSource.Configuration);

            var instance =
                new TestObjectWithMultipleSourceValidationAttributesOnProperties
                {
                    PropertyWithDataAnnotationsAttributes = "invalid",
                    PropertyWithMixedAttributes = "invalid",
                    PropertyWithVABOnlyAttributes = "invalid"
                };

            var results = validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithDataAnnotationsAttributes" && vr.Message == "configuration1"));
            Assert.IsTrue(results.Any(vr => vr.Key == "PropertyWithVABOnlyAttributes" && vr.Message == "configuration2"));
        }

        #endregion
    }
}
