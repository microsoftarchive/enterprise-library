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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class SelfValidationValidatorFixture
    {
        ValidationResults suppliedValidationResults;

        [TestInitialize]
        public void SetUp()
        {
            suppliedValidationResults = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingAValidatorWithANullMethodReferenceThrows()
        {
            new SelfValidationValidator(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatingAValidatorWithMethodReturningAValueThrows()
        {
            new SelfValidationValidator(GetType().GetMethod("SelfValidationMethodReturningAResult", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatingAValidatorWithMethodWithWrongSignatureThrows()
        {
            new SelfValidationValidator(GetType().GetMethod("SelfValidationMethodWithWrongParameters", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [TestMethod]
        public void ProvidesValidationResultsToValidationMethod()
        {
            Validator validator = new SelfValidationValidator(GetType().GetMethod("SelfValidationMethod", BindingFlags.NonPublic | BindingFlags.Instance));
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(this, validationResults);

            Assert.AreSame(validationResults, suppliedValidationResults);
        }

        [TestMethod]
        public void LogsOwnValidationFailureIfValidatedObjectIsANullReference()
        {
            Validator validator = new SelfValidationValidator(GetType().GetMethod("SelfValidationMethod", BindingFlags.NonPublic | BindingFlags.Instance));
            validator.Tag = "tag";
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(null, validationResults);

            Assert.IsNull(suppliedValidationResults); // the self validation method should not have been called when validating null
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual(Resources.SelfValidationValidatorMessage, resultsList[0].Message);
            Assert.AreSame(null, resultsList[0].Key);
            Assert.AreSame("tag", resultsList[0].Tag);
            Assert.AreSame(null, resultsList[0].Target);
            Assert.AreSame(validator, resultsList[0].Validator);
        }

        [TestMethod]
        public void LogsOwnValidationFailureIfValidatedObjectIsOfIncompatibleType()
        {
            Validator validator = new SelfValidationValidator(GetType().GetMethod("SelfValidationMethod", BindingFlags.NonPublic | BindingFlags.Instance));
            ValidationResults validationResults = new ValidationResults();

            validator.Validate("a string", validationResults);

            Assert.IsNull(suppliedValidationResults); // the self validation method should not have been called when incompatible type
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual(Resources.SelfValidationValidatorMessage, resultsList[0].Message);
            Assert.AreSame(null, resultsList[0].Key);
            Assert.IsNull(resultsList[0].Tag);
            Assert.AreSame("a string", resultsList[0].Target);
            Assert.AreSame(validator, resultsList[0].Validator);
        }

        [TestMethod]
        public void LogsOwnValidationFailureIfValidationMethodThrows()
        {
            Validator validator = new SelfValidationValidator(GetType().GetMethod("ThrowingSelfValidationMethod", BindingFlags.NonPublic | BindingFlags.Instance));
            ValidationResults validationResults = new ValidationResults();

            validator.Validate(this, validationResults);

            Assert.IsNull(suppliedValidationResults);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual(Resources.SelfValidationMethodThrownMessage, resultsList[0].Message);
        }

        void SelfValidationMethod(ValidationResults suppliedValidationResults)
        {
            this.suppliedValidationResults = suppliedValidationResults;
        }

        object SelfValidationMethodReturningAResult(ValidationResults suppliedValidationResults)
        {
            return null;
        }

        void SelfValidationMethodWithWrongParameters(ValidationResults suppliedValidationResults,
                                                     bool shouldSucceed) {}

        void ThrowingSelfValidationMethod(ValidationResults suppliedValidationResults)
        {
            throw new Exception();
        }
    }
}
