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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class OrCompositeValidatorFixture
    {
        [TestMethod]
        public void ReturnsSuccessIfAllValidatorsReturnSuccess()
        {
            MockValidator<string> subValidator1 = new MockValidator<string>(false, "validator1");
            MockValidator<string> subValidator2 = new MockValidator<string>(false, "validator2");
            MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
            Validator validator = new OrCompositeValidator(subValidator1, subValidator2, subValidator3);
            string target = new string('a', 10); // just any string, but a new one

            ValidationResults validationResults = validator.Validate(target);

            Assert.IsTrue(validationResults.IsValid);
            Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
            Assert.AreEqual(0, subValidator2.ValidatedTargets.Count);
            Assert.AreEqual(0, subValidator3.ValidatedTargets.Count);
        }

        [TestMethod]
        public void ReturnsSuccessAfterQueryingPreviousValidatorsIfOneValidatorReturnFailure()
        {
            MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
            MockValidator<string> subValidator2 = new MockValidator<string>(false, "validator2");
            MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
            Validator validator = new OrCompositeValidator(subValidator1, subValidator2, subValidator3);
            string target = new string('a', 10); // just any string, but a new one

            ValidationResults validationResults = validator.Validate(target);

            Assert.IsTrue(validationResults.IsValid);
            Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
            Assert.AreEqual(0, subValidator3.ValidatedTargets.Count);
        }

        [TestMethod]
        public void ReturnsSuccessAfterQueryingPreviousValidatorsIfManyValidatorsReturnFailure()
        {
            MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
            MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
            MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
            Validator validator = new OrCompositeValidator(subValidator1, subValidator2, subValidator3);
            string target = new string('a', 10); // just any string, but a new one

            ValidationResults validationResults = validator.Validate(target);

            Assert.IsTrue(validationResults.IsValid);
            Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
        }

        [TestMethod]
        public void ReturnsSuccessWhenValidatingNullAfterQueryingPreviousValidatorsIfManyValidatorsReturnFailure()
        {
            MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
            MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
            MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
            Validator validator = new OrCompositeValidator(subValidator1, subValidator2, subValidator3);
            string target = null;

            ValidationResults validationResults = validator.Validate(target);

            Assert.IsTrue(validationResults.IsValid);
            Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
        }

        [TestMethod]
        public void ReturnsCompositeFailureAfterQueryingAllValidatorsIfAllValidatorsReturnFailure()
        {
            MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
            MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
            MockValidator<string> subValidator3 = new MockValidator<string>(true, "validator3");
            Validator validator = new OrCompositeValidator(subValidator1, subValidator2, subValidator3);
            validator.MessageTemplate = "composite validator";
            string target = new string('a', 10); // just any string, but a new one

            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("composite validator", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Tag);
            IList<ValidationResult> nestedResultsList = ValidationTestHelper.GetResultsList(resultsList[0].NestedValidationResults);
            Assert.AreEqual(3, nestedResultsList.Count);
            Assert.AreEqual("validator1", nestedResultsList[0].Message);
            Assert.AreEqual("validator2", nestedResultsList[1].Message);
            Assert.AreEqual("validator3", nestedResultsList[2].Message);
            Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
        }

        [TestMethod]
        public void CanPerformValidationWithSuppliedValidationResults()
        {
            MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
            MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
            MockValidator<string> subValidator3 = new MockValidator<string>(true, "validator3");
            Validator validator = new OrCompositeValidator(subValidator1, subValidator2, subValidator3);
            validator.Tag = "tag";
            string target = new string('a', 10); // just any string, but a new one
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(target, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.OrCompositeValidatorDefaultMessageTemplate, resultsList[0].Message));
            Assert.AreEqual("tag", resultsList[0].Tag);
            IList<ValidationResult> nestedResultsList = ValidationTestHelper.GetResultsList(resultsList[0].NestedValidationResults);
            Assert.AreEqual(3, nestedResultsList.Count);
            Assert.AreEqual("validator1", nestedResultsList[0].Message);
            Assert.AreEqual("validator2", nestedResultsList[1].Message);
            Assert.AreEqual("validator3", nestedResultsList[2].Message);
            Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
            Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationRequestWithNullSuppliedValidationResultsThrows()
        {
            Validator validator = new OrCompositeValidator();
            string target = new string('a', 10); // just any string, but a new one

            validator.Validate(target, null);
        }
    }
}
