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
    public class GenericValidatorFixture : Validator<string>
    {
        public GenericValidatorFixture()
            : base(null, null) {}

        [TestMethod]
        public void ValidatorWillCreateValidationResults()
        {
            Validator<object> validator = new MockValidator(true, "template");

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("template"));
        }

        [TestMethod]
        public void ValidatorWillUseProvidedValidationResults()
        {
            Validator<object> validator = new MockValidator(true, "template");
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
            Validator<object> validator = new MockValidator(true, "template");
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(this, null);
        }

        [TestMethod]
        public void GenericValidatorValidatingTargetOfWrongTypeReturnsFailure()
        {
            Assert.IsTrue(((Validator)this).Validate("").IsValid);

            ValidationResults validationResults = ((Validator)this).Validate(10);
            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void GenericValidatorValidatingTargetOfWrongTypeAddFailureToProvidedValidationResults()
        {
            ValidationResults validationResults = new ValidationResults();

            ((Validator)this).Validate("", validationResults);
            Assert.IsTrue(validationResults.IsValid);

            ((Validator)this).Validate(10, validationResults);
            Assert.IsFalse(validationResults.IsValid);
        }

        protected override void DoValidate(string objectToValidate,
                                           object currentTarget,
                                           string key,
                                           ValidationResults validationResults)
        {
            // do nothing
        }

        protected override string DefaultMessageTemplate
        {
            get { return ""; }
        }
    }
}
