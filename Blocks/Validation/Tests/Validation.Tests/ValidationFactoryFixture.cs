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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValidationFactoryFixture
    {
        [TestMethod]
        public void CanCreateValidatorForType()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidatorFromAttributes<TestObjectWithFailingAttributesOnProperties>();

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void CreatedValidatorIsBasedOnValidationAttributes()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidatorFromAttributes<TestObjectWithFailingAttributesOnProperties>();

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message1", resultsList[0].Message);
            Assert.AreEqual("message2", resultsList[1].Message);
        }

        [TestMethod]
        public void CreatedValidatorIsBasedOnValidationAttributesAndProvidedRuleset()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidatorFromAttributes<TestObjectWithFailingAttributesOnProperties>("RuleA");

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message1-RuleA", resultsList[0].Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatorCreationWithNullRulesetNameThrows()
        {
            ValidationFactory.CreateValidatorFromAttributes<object>((string)null);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeFromConfiguration()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            Validator<BaseTestDomainObject> validator
                = ValidationFactory.CreateValidatorFromConfiguration<BaseTestDomainObject>(configurationSource);

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeFromConfigurationWithGivenRulesetName()
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
            MockValidatorData validator12 = new MockValidatorData("validator2", true);
            propertyReference1.Validators.Add(validator12);
            validator12.MessageTemplate = "message-from-config2-RuleA";
            MockValidatorData validator13 = new MockValidatorData("validator3", false);
            propertyReference1.Validators.Add(validator13);
            validator13.MessageTemplate = "message-from-config3-RuleA";

            Validator<BaseTestDomainObject> validator
                = ValidationFactory.CreateValidatorFromConfiguration<BaseTestDomainObject>("RuleA", (IConfigurationSource)configurationSource);

            ValidationResults validationResults = validator.Validate(new BaseTestDomainObject());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message-from-config1-RuleA", resultsList[0].Message);
            Assert.AreEqual("message-from-config2-RuleA", resultsList[1].Message);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeFromConfigurationWithDefaultRulesetName()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(BaseTestDomainObject));
            settings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "RuleA";
            ValidationRulesetData ruleData = new ValidationRulesetData("RuleA");
            typeReference.Rulesets.Add(ruleData);
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("Property1");
            ruleData.Properties.Add(propertyReference1);
            MockValidatorData validator11 = new MockValidatorData("validator1", true);
            propertyReference1.Validators.Add(validator11);
            validator11.MessageTemplate = "message-from-config1-RuleA";
            MockValidatorData validator12 = new MockValidatorData("validator2", true);
            propertyReference1.Validators.Add(validator12);
            validator12.MessageTemplate = "message-from-config2-RuleA";
            MockValidatorData validator13 = new MockValidatorData("validator3", false);
            propertyReference1.Validators.Add(validator13);
            validator13.MessageTemplate = "message-from-config3-RuleA";

            Validator<BaseTestDomainObject> validator
                = ValidationFactory.CreateValidatorFromConfiguration<BaseTestDomainObject>((IConfigurationSource)configurationSource);

            ValidationResults validationResults = validator.Validate(new BaseTestDomainObject());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message-from-config1-RuleA", resultsList[0].Message);
            Assert.AreEqual("message-from-config2-RuleA", resultsList[1].Message);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeFromAttributesAndConfigurationWithDefaultRulesetName()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestObjectWithFailingAttributesOnProperties));
            settings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "RuleA";
            ValidationRulesetData ruleData = new ValidationRulesetData("RuleA");
            typeReference.Rulesets.Add(ruleData);
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("FailingProperty1");
            ruleData.Properties.Add(propertyReference1);
            MockValidatorData validator11 = new MockValidatorData("validator1", true);
            propertyReference1.Validators.Add(validator11);
            validator11.MessageTemplate = "message-from-config1-RuleA";
            MockValidatorData validator12 = new MockValidatorData("validator2", true);
            propertyReference1.Validators.Add(validator12);
            validator12.MessageTemplate = "message-from-config2-RuleA";
            MockValidatorData validator13 = new MockValidatorData("validator3", false);
            propertyReference1.Validators.Add(validator13);
            validator13.MessageTemplate = "message-from-config3-RuleA";

            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>((IConfigurationSource)configurationSource);

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeFromAttributesAndConfigurationWithRulesetName()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestObjectWithFailingAttributesOnProperties));
            settings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "RuleA";
            ValidationRulesetData ruleData = new ValidationRulesetData("RuleA");
            typeReference.Rulesets.Add(ruleData);
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("FailingProperty1");
            ruleData.Properties.Add(propertyReference1);
            MockValidatorData validator11 = new MockValidatorData("validator1", true);
            propertyReference1.Validators.Add(validator11);
            validator11.MessageTemplate = "message-from-config1-RuleA";
            MockValidatorData validator12 = new MockValidatorData("validator2", true);
            propertyReference1.Validators.Add(validator12);
            validator12.MessageTemplate = "message-from-config2-RuleA";
            MockValidatorData validator13 = new MockValidatorData("validator3", false);
            propertyReference1.Validators.Add(validator13);
            validator13.MessageTemplate = "message-from-config3-RuleA";

            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>("RuleA", (IConfigurationSource)configurationSource);

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationFile()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidatorFromConfiguration<TestObjectWithFailingAttributesOnProperties>("RuleA");

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationFileAndAttributes()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>("RuleA");

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationFileUsingDefaultRule()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidatorFromConfiguration<TestObjectWithFailingAttributesOnProperties>();

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationFileAndAttributesUsingDefaultRule()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = ValidationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message2"));
        }

        #region Test Fix "Remove unnecessary AndCompositeValidator from hierarchy"

        [TestMethod]
        public void CreatedAndCompositeValidatorFromAttributesWithPropertiesAppropiately()
        {
            Validator validator = ValidationFactory.CreateValidator(typeof(MockClassWithValidatorAttributesOnProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValidatorWrapper), validator.GetType());

            ValidatorWrapper validatorWrapper = validator as ValidatorWrapper;

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

            ValidatorWrapper validatorWrapper = validator as ValidatorWrapper;

            AndCompositeValidator compositeValidator = validatorWrapper.WrappedValidator as AndCompositeValidator;

            Assert.IsNotNull(compositeValidator);

            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(2, allValidators.Count);

            Assert.AreEqual(typeof(ValueAccessValidator), allValidators[0].GetType());
            Assert.AreEqual(typeof(ValueAccessValidator), allValidators[1].GetType());
        }

        #endregion
    }
}
