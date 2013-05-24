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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class GivenAttributeValidationInstanceFactory
    {
        private AttributeValidatorFactory validationFactory;

        [TestInitialize]
        public void Given()
        {
            validationFactory = new AttributeValidatorFactory();
        }

        [TestMethod]
        public void WhenCreatingAValidatorFromAttributes_ReturnsAValidatorInstance()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void WhenRetrievingATypeValidatorTwiceUsingGenericMethods_ThenTheSameValidatorIsReturned()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> firstValidator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Validator<TestObjectWithFailingAttributesOnProperties> secondValidator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreSame(firstValidator,secondValidator);
        }

        [TestMethod]
        public void WhenRetrievingATypeValidatorTwiceUsingNonGenericMethods_ThenTheSameValidatorIsReturned()
        {
            Validator firstValidator
                = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            Validator secondValidator
                = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            Assert.AreSame(firstValidator, secondValidator);
            
        }

        [TestMethod]
        public void WhenCacheIsReset_ThenReturnsNewGenericAttributeValidators()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> firstValidator
               = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            validationFactory.ResetCache();

            Validator<TestObjectWithFailingAttributesOnProperties> secondValidator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreNotSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void WhenCacheIsReset_ThenReturnsNewNonGenericAttributeValidators()
        {
            Validator firstValidator
               = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            validationFactory.ResetCache();

            Validator secondValidator
                = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            Assert.AreNotSame(firstValidator, secondValidator);
        }


        [TestMethod]
        public void WhenValidatingAttributeRulesThatFail_ThenReceiveCorrectFailedValidationResults()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
              = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message1", resultsList[0].Message);
            Assert.AreEqual("message2", resultsList[1].Message);
        }

        [TestMethod]
        public void WhenValidatingRulesFromRuleSet_ThenValidationResultsOnlyContainExpectedRuleFailures()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>("RuleA");

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message1-RuleA", resultsList[0].Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenValidatingFromNullRulesetName_ThenThrowsArgumentNullException()
        {
            validationFactory.CreateValidator<object>((string)null);
        }
    }


    [TestClass]
    public class GivenAValidatorBuiltFromAnInstanceFactoryWithAnEmptyConfigurationSource
    {
        private Validator<BaseTestDomainObject> validator;

        [TestInitialize]
        public void Given()
        {
            var validatorFactory = ConfigurationValidatorFactory.FromConfigurationSource(new DictionaryConfigurationSource());
            validator = validatorFactory.CreateValidator<BaseTestDomainObject>();
        }

        [TestMethod]
        public void ThenValidatorIsNotNull()
        {
            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void WhenValidatingAnObject_ShouldDetermineObjectIsValid()
        {
            ValidationResults results = validator.Validate(new BaseTestDomainObject());
            Assert.IsTrue(results.IsValid);
        }

    }

    [TestClass]
    public class GivenAValidationInstanceFactoryBasedOnANonEmptyConfigurationSource
    {
        private ConfigurationValidatorFactory validationFactory;

        [TestInitialize]
        public void Given()
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

            ValidationRulesetData ruleDataB = new ValidationRulesetData("RuleB");
            typeReference.Rulesets.Add(ruleDataB);
            ValidatedPropertyReference propertyReferenceB1 = new ValidatedPropertyReference("Property1");
            ruleDataB.Properties.Add(propertyReferenceB1);
            MockValidatorData validator21 = new MockValidatorData("validator21", true);
            propertyReferenceB1.Validators.Add(validator21);
            validator21.MessageTemplate = "message-from-config1-RuleB";

            validationFactory = ConfigurationValidatorFactory.FromConfigurationSource(configurationSource);
        }

        [TestMethod]
        public void WhenValidatingUsingDefaultRuleset_ThenShouldValidateWithDefaultRuleset()
        {
            Validator<BaseTestDomainObject> validator
                           = validationFactory.CreateValidator<BaseTestDomainObject>();

            ValidationResults validationResults = validator.Validate(new BaseTestDomainObject());

            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(2, validationResults.Count);
            Assert.AreEqual("message-from-config1-RuleA", validationResults.ElementAt(0).Message);
            Assert.AreEqual("message-from-config2-RuleA", validationResults.ElementAt(1).Message);
        }

        [TestMethod]
        public void WhenValidatingUsingNonDefaultRuleset_ThenShouldProduceResultsOnlyFromSpecifiedRuleset()
        {
            Validator<BaseTestDomainObject> validator
                = validationFactory.CreateValidator<BaseTestDomainObject>("RuleB");

            ValidationResults validationResults = validator.Validate(new BaseTestDomainObject());

            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("message-from-config1-RuleB", validationResults.ElementAt(0).Message);
        }

        [TestMethod]
        public void WhenRetrievingAValidatorTwiceUsingGenericMethods_ThenTheValidatorsAreTheSame()
        {
            Validator<BaseTestDomainObject> firstValidator
                 = validationFactory.CreateValidator<BaseTestDomainObject>("RuleB");

            Validator<BaseTestDomainObject> secondValidator
                 = validationFactory.CreateValidator<BaseTestDomainObject>("RuleB");  

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void WhenRetrievingAValidatorTwiceUsingNonGenericMethods_ThenTheValidatorsAreTheSame()
        {
            var firstValidator
                 = validationFactory.CreateValidator(typeof(BaseTestDomainObject), "RuleB");

            var secondValidator
                 = validationFactory.CreateValidator(typeof(BaseTestDomainObject), "RuleB");

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void WhenCacheIsReset_ThenValidatorsFromConfiguredGenericMethodsAreNew()
        {
            Validator<BaseTestDomainObject> firstValidator
                = validationFactory.CreateValidator<BaseTestDomainObject>("RuleB");

            validationFactory.ResetCache();

            Validator<BaseTestDomainObject> secondValidator
                 = validationFactory.CreateValidator<BaseTestDomainObject>("RuleB");

            Assert.AreNotSame(firstValidator, secondValidator);
        }


        [TestMethod]
        public void WhenCacheIsReset_ThenValidatorsFromConfiguredNonGenericMethodsAreNew()
        {
            var firstValidator
               = validationFactory.CreateValidator(typeof(BaseTestDomainObject), "RuleB");

            validationFactory.ResetCache();

            var secondValidator
                 = validationFactory.CreateValidator(typeof(BaseTestDomainObject), "RuleB");

            Assert.AreNotSame(firstValidator, secondValidator);
        }
    }

    [TestClass]
    public class GivenAValidationConfiguratinAndValidationAttributes
    {
        private CompositeValidatorFactory validationFactory;

        [TestInitialize]
        public void Given()
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

            validationFactory = new CompositeValidatorFactory(
                new ValidatorFactory[]
                    {
                        new AttributeValidatorFactory(),
                        new ConfigurationValidatorFactory(configurationSource)
                    });
        }

        [TestMethod]
        public void WhenValidatingWithDefaultRuleset_ThenReturnsProperValidationResults()
        {

            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

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
        public void WhenValidatingFromNamedRulest_ThenReturnsProperValidationResults()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>("RuleA");

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void WhenRetrievingAValidatorTwiceUsingGenericMethods_ThenTheValidatorsAreTheSame()
        {
            var firstValidator
                 = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            var secondValidator
                 = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void WhenRetrievingAValidatorTwiceUsingNonGenericMethods_ThenTheValidatorsAreTheSame()
        {
            var firstValidator
                = validationFactory.CreateValidator(typeof (TestObjectWithFailingAttributesOnProperties));

            var secondValidator
                = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            Assert.AreSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void WhenCacheIsReset_ThenSameNonGenericValidatorsAreNotReturned()
        {
            var firstValidator
                = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            validationFactory.ResetCache();

            var secondValidator
               = validationFactory.CreateValidator(typeof(TestObjectWithFailingAttributesOnProperties));

            Assert.AreNotSame(firstValidator, secondValidator);
        }

        [TestMethod]
        public void WhenCacheIsReset_ThenSameGenericValidatorsAreNotReturned()
        {
            var firstValidator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            validationFactory.ResetCache();

            var secondValidator
               = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            Assert.AreNotSame(firstValidator, secondValidator);
        }
    }

    [TestClass]
    public class GivenACompositeValidationInstanceFactoryUsingConfigurationFile
    {
        private CompositeValidatorFactory validationFactory;

        [TestInitialize]
        public void Given()
        {
            validationFactory = new CompositeValidatorFactory(
                new ValidatorFactory[]
                    {
                        new AttributeValidatorFactory(),
                        new ConfigurationValidatorFactory(ConfigurationSourceFactory.Create())
                    });
        }

        [TestMethod]
        public void WhenAValidatorIsCreatedUsingANamedRuleset_ThenWillFailOnRulesFromConfigurationFileAndFromAttributes()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>("RuleA");

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void WhenAValidatorIsCreatedUsingANamedRuleset_ThenWillApplyDefaultRulesetAndAttributeRules()
        {
            Validator<TestObjectWithFailingAttributesOnProperties> validator
                = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>();

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message2"));
        }
    }
}
