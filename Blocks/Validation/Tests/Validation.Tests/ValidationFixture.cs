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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValidationFixture
    {
        [TestMethod]
        public void CanValidateObjectWithoutRuleSet()
        {
            ValidationResults validationResults
                = Validation.ValidateFromAttributes(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message1", resultsList[0].Message);
            Assert.AreEqual("message2", resultsList[1].Message);
        }

        [TestMethod]
        public void CanValidateObjectWithRuleSet()
        {
            ValidationResults validationResults
                = Validation.ValidateFromAttributes(new TestObjectWithFailingAttributesOnProperties(), "RuleA");

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("message1-RuleA", resultsList[0].Message);
        }

        [TestMethod]
        public void CanValidateObjectFromAttributesWithMultipleRuleSets()
        {
            ValidationResults validationResults
                = Validation.ValidateFromAttributes(new TestObjectWithFailingAttributesOnProperties(), "RuleA", "RuleB");

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);
            Assert.AreEqual("message1-RuleA", resultsList[0].Message);
            Assert.AreEqual("message1-RuleB", resultsList[1].Message);
        }

        [TestMethod]
        public void CanValidateObjectFromConfigWithMultipleRuleSets()
        {
            ValidationResults validationResults
                = Validation.ValidateFromConfiguration(new TestObjectWithFailingAttributesOnProperties(), "RuleA", "RuleB");

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
        }

        [TestMethod]
        public void CanValidateObjectWithDefaultRulesetFromConfiguration()
        {
            ValidationResults validationResults
                = Validation.ValidateFromConfiguration(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
        }

        [TestMethod]
        public void CanValidateObjectWithRulesetFromConfiguration()
        {
            ValidationResults validationResults
                = Validation.ValidateFromConfiguration(new TestObjectWithFailingAttributesOnProperties(), "RuleA");

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
        }

        [TestMethod]
        public void CanValidateObjectWithDefaultRulesetFromAttributesAndConfiguration()
        {
            ValidationResults validationResults
                = Validation.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message2"));
        }

        [TestMethod]
        public void CanValidateObjectWithRulesetFromAttributesAndConfiguration()
        {
            ValidationResults validationResults
                = Validation.Validate(new TestObjectWithFailingAttributesOnProperties(), "RuleA");

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void CanValidateObjectFromAttributesAndConfigurationWithMultipleRulesets()
        {
            ValidationResults validationResults
                = Validation.Validate(new TestObjectWithFailingAttributesOnProperties(), "RuleA", "RuleB");

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(6, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleB"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationRequestForNullRuleThrows()
        {
            Validation.Validate("", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationFromConfigurationRequestForNullRuleThrows()
        {
            Validation.ValidateFromConfiguration("", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationFromAttributesRequestForNullRuleThrows()
        {
            Validation.ValidateFromAttributes("", null);
        }


        [TestMethod]
        public void CanBuildValidationInstnceFactoryFromGivenConfiguration()
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

            CompositeValidatorFactory factory
                =EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource)
                        .GetInstance<CompositeValidatorFactory>();

            var validator = factory.CreateValidator<BaseTestDomainObject>("RuleA");
            var results = validator.Validate(new BaseTestDomainObject());
            Assert.IsNotNull(factory);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(validator11.MessageTemplate, results.ElementAt(0).Message);
        }
    }
}
