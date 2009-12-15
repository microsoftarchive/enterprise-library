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
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValueAccessValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfValidatorWithNullValueAccessThrows()
        {
            new ValueAccessValidator(null, new MockValidator<int>(false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfValidatorWithNullValueValidatorThrows()
        {
            PropertyInfo propertyInfo = typeof(string).GetProperty("Length");
            Assert.IsNotNull(propertyInfo);

            new ValueAccessValidator(new PropertyValueAccess(propertyInfo), null);
        }

        [TestMethod]
        public void ValueValidatorIsInvokedWithProperlySetObjectToValidateAndCurrentTargetAndKey()
        {
            object valueToValidate = new object();
            string key = new string(new char[] { 'a', 'b', 'c' });
            MockValueAccess valueAccess = new MockValueAccess(valueToValidate, key);
            MockValidator<object> valueValidator = new MockValidator<object>(true, "message");
            Validator validator = new ValueAccessValidator(valueAccess, valueValidator);

            ValidationResults validationResults = validator.Validate(this);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.AreSame(this, resultsMapping["message"].Target);
            Assert.AreSame(key, resultsMapping["message"].Key);
            Assert.AreEqual(1, valueValidator.ValidatedTargets.Count);
            Assert.AreSame(valueToValidate, valueValidator.ValidatedTargets[0]);
        }

        [TestMethod]
        public void ReturnsSuccessIfValueValidatorReturnsSuccessAndValueValidatorIsInvoked()
        {
            object valueToValidate = new object();
            MockValueAccess valueAccess = new MockValueAccess(valueToValidate);
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            Validator validator = new ValueAccessValidator(valueAccess, valueValidator);

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsTrue(validationResults.IsValid);
            Assert.AreEqual(1, valueValidator.ValidatedTargets.Count);
            Assert.AreSame(valueToValidate, valueValidator.ValidatedTargets[0]);
        }

        [TestMethod]
        public void ReturnsFailureIfValueValidatorReturnsFailureAndValueValidatorIsInvoked()
        {
            object valueToValidate = new object();
            MockValueAccess valueAccess = new MockValueAccess(valueToValidate);
            MockValidator<object> valueValidator = new MockValidator<object>(true);
            Validator validator = new ValueAccessValidator(valueAccess, valueValidator);

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, valueValidator.ValidatedTargets.Count);
            Assert.AreSame(valueToValidate, valueValidator.ValidatedTargets[0]);
        }

        [TestMethod]
        public void FailureFromValueAccessHasCorrectValidationKey()
        {
            string key = new string(new char[0]);
            object valueToValidate = new object();
            MockValueAccess valueAccess = new MockValueAccess(valueToValidate, key);
            MockValidator<object> valueValidator = new MockValidator<object>(true);
            Validator validator = new ValueAccessValidator(valueAccess, valueValidator);

            ValidationResults validationResults = validator.Validate(this);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual(key, resultsList[0].Key);
        }

        [TestMethod]
        public void CanValidateThroughPropertyAccess()
        {
            DerivedTestDomainObject targetToValidate = new DerivedTestDomainObject();
            PropertyValueAccess valueAccess = new PropertyValueAccess(typeof(DerivedTestDomainObject).GetProperty("Property2"));
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            Validator validator = new ValueAccessValidator(valueAccess, valueValidator);

            ValidationResults validationResults = validator.Validate(targetToValidate);

            Assert.IsTrue(validationResults.IsValid);
            Assert.AreEqual(1, valueValidator.ValidatedTargets.Count);
            Assert.AreSame(targetToValidate.Property2, valueValidator.ValidatedTargets[0]);
        }
    }
}
