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
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class ValidatorFixture
    {
        [TestMethod]
        public void ValidatorWillCreateValidationResults()
        {
            Validator validator = new MockValidator(true, "template");

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("template"));
        }

        [TestMethod]
        public void ValidatorWillUseProvidedValidationResults()
        {
            Validator validator = new MockValidator(true, "template");
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(this, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("template"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatorWillThrowIfProvidedNulValidationResults()
        {
            Validator validator = new MockValidator(true, "template");
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(this, null);
        }

        [TestMethod]
        public void ValidatorWillNotUseTagIfUnavailable()
        {
            Validator validator = new MockValidator(true, "template");

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.IsNull(resultsMapping["template"].Tag);
        }

        [TestMethod]
        public void ValidatorWillUseTagIfAvailable()
        {
            Validator validator = new MockValidator(true, "template");
            validator.Tag = "tag";

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual("tag", resultsMapping["template"].Tag);
        }

        [TestMethod]
        public void ValidatorWillSetTheValidatedObjectAsTargetWhenCalledDirectly()
        {
            Validator validator = new MockValidator(true, "template");

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreSame(this, resultsMapping["template"].Target);
        }

        [TestMethod]
        public void ValidatorWillNotSetKeyWhenCalledDirectly()
        {
            Validator validator = new MockValidator(true, "template");

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.IsNull(resultsMapping["template"].Key);
        }

        [TestMethod]
        public void ValidatorWillSetKeyIfProvided()
        {
            Validator validator = new MockValidator(true, "template");

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(this, this, "key", validationResults);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual("key", resultsMapping["template"].Key);
        }

        [TestMethod]
        public void ValidatorWillSetValidatorToThis()
        {
            Validator validator = new MockValidator(true, "template");

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(this, this, "key", validationResults);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreSame(validator, resultsMapping["template"].Validator);
        }

        [TestMethod]
        public void ValidatorWillSetMessageToDefaultMessageIfMessageTemplateIsNotSet()
        {
            Validator validator = new MockValidator(true);

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreSame(validator, resultsMapping[MockValidator.DefaultMockValidatorMessageTemplate].Validator);
        }

        [TestMethod]
        public void ValidatorWillSetMessageToSpecifiedMessageIfMessageTemplateIsSet()
        {
            Validator validator = new MockValidator(true);
            validator.MessageTemplate = "overriden message";

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreSame(validator, resultsMapping["overriden message"].Validator);
        }
    }
}
