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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class ValueAccessComparisonValidatorFixture
    {
        [TestMethod]
        public void CreatingWithValueAccessAndOperatorSetsValuesForValidators()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            ValueAccessComparisonValidator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            Assert.AreSame(valueAccess, validator.ValueAccess);
            Assert.AreEqual(ComparisonOperator.LessThan, validator.ComparisonOperator);
            Assert.AreEqual(null, validator.Tag);
            Assert.AreEqual(false, validator.Negated);
            // Assert.AreEqual(null, validator.MessageTemplate);
        }

        [TestMethod]
        public void CreatingWithValueAccessAndOperatorAndTagSetsValuesForValidators()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            ValueAccessComparisonValidator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan, null, "tag");

            Assert.AreSame(valueAccess, validator.ValueAccess);
            Assert.AreEqual(ComparisonOperator.LessThan, validator.ComparisonOperator);
            Assert.AreEqual("tag", validator.Tag);
            Assert.AreEqual(false, validator.Negated);
            // Assert.AreEqual(null, validator.MessageTemplate);
        }

        [TestMethod]
        public void CreatingWithValueAccessAndOperatorAndNegatedSetsValuesForValidators()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            ValueAccessComparisonValidator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan, null, true);

            Assert.AreSame(valueAccess, validator.ValueAccess);
            Assert.AreEqual(ComparisonOperator.LessThan, validator.ComparisonOperator);
            Assert.AreEqual(null, validator.Tag);
            Assert.AreEqual(true, validator.Negated);
            // Assert.AreEqual(null, validator.MessageTemplate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingWithNullValueAccessThrows()
        {
            new ValueAccessComparisonValidator(null, ComparisonOperator.LessThan);
        }

        [TestMethod]
        public void ComparingWithValueAccessReturningAccessFailureReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(null, null, true);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);

            ValidationResults validationResults = validator.Validate("non null");

            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, ValidationTestHelper.GetResultsList(validationResults).Count);
        }

        #region equal

        [TestMethod]
        public void ComparingNullForEqualWithNullReturnsSuccess()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNullForEqualityWithNonNullReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess("non null");
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonNullForEqualityWithNullReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);

            ValidationResults validationResults = validator.Validate("non null");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonNullForEqualityWithNonEqualReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(5);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);

            ValidationResults validationResults = validator.Validate("non null");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonNullForEqualityWithEqualReturnsSuccess()
        {
            ValueAccess valueAccess = new MockValueAccess(5);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);

            ValidationResults validationResults = validator.Validate(5);

            Assert.IsTrue(validationResults.IsValid);
        }

        #endregion

        #region not equal

        [TestMethod]
        public void ComparingNullForNonEqualWithNullReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.NotEqual);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNullForNonEqualityWithNonNullReturnsSuccess()
        {
            ValueAccess valueAccess = new MockValueAccess("non null");
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.NotEqual);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonNullForNonEqualityWithNullReturnsSuccess()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.NotEqual);

            ValidationResults validationResults = validator.Validate("non null");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonNullForNonEqualityWithNonEqualReturnsSuccess()
        {
            ValueAccess valueAccess = new MockValueAccess(5);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.NotEqual);

            ValidationResults validationResults = validator.Validate("non null");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonNullForNonEqualityWithEqualReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(5);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.NotEqual);

            ValidationResults validationResults = validator.Validate(5);

            Assert.IsFalse(validationResults.IsValid);
        }

        #endregion

        #region less than

        [TestMethod]
        public void ComparingNullForLessThanReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingNonComparableForLessThanReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate(new object());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingComparableForLessThanWithNullReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate("a string");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingComparableForLessThanWithDifferentTypeReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate("a string");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingComparableForLessThanWithEqualOfSameTypeReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate(7);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingComparableForLessThanWithSmallerOfSameTypeReturnsFailure()
        {
            ValueAccess valueAccess = new MockValueAccess(6);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate(7);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ComparingComparableForLessThanWithLargerOfSameTypeReturnsSuccess()
        {
            ValueAccess valueAccess = new MockValueAccess(8);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThan);

            ValidationResults validationResults = validator.Validate(7);

            Assert.IsTrue(validationResults.IsValid);
        }

        #endregion

        #region other comparisons

        [TestMethod]
        public void ComparingComparableForLessThanEqualReturnsExpectedResults()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.LessThanEqual);

            Assert.IsTrue(validator.Validate(5).IsValid);
            Assert.IsTrue(validator.Validate(6).IsValid);
            Assert.IsTrue(validator.Validate(7).IsValid);
            Assert.IsFalse(validator.Validate(8).IsValid);
            Assert.IsFalse(validator.Validate(9).IsValid);
        }

        [TestMethod]
        public void ComparingComparableForGreaterThanReturnsExpectedResults()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.GreaterThan);

            Assert.IsFalse(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(6).IsValid);
            Assert.IsFalse(validator.Validate(7).IsValid);
            Assert.IsTrue(validator.Validate(8).IsValid);
            Assert.IsTrue(validator.Validate(9).IsValid);
        }

        [TestMethod]
        public void ComparingComparableForGreaterThanEqualsReturnsExpectedResults()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.GreaterThanEqual);

            Assert.IsFalse(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(6).IsValid);
            Assert.IsTrue(validator.Validate(7).IsValid);
            Assert.IsTrue(validator.Validate(8).IsValid);
            Assert.IsTrue(validator.Validate(9).IsValid);
        }

        [TestMethod]
        public void ComparingWithUnknownOperatorFails()
        {
            ValueAccess valueAccess = new MockValueAccess(7);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, (ComparisonOperator)100);

            Assert.IsFalse(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(6).IsValid);
            Assert.IsFalse(validator.Validate(7).IsValid);
            Assert.IsFalse(validator.Validate(8).IsValid);
            Assert.IsFalse(validator.Validate(9).IsValid);
        }

        #endregion

        #region negated

        [TestMethod]
        public void NegatedEqualityReturnsExpectedResults()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal, null, true);

            Assert.IsTrue(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(null).IsValid);

            valueAccess = new MockValueAccess(6);
            validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal, null, true);

            Assert.IsTrue(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(6).IsValid);
            Assert.IsTrue(validator.Validate("string").IsValid);
            Assert.IsTrue(validator.Validate(null).IsValid);
        }

        [TestMethod]
        public void NegatedComparisonReturnsExpectedResults()
        {
            ValueAccess valueAccess = new MockValueAccess(null);
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.GreaterThanEqual, null, true);

            Assert.IsFalse(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(null).IsValid);

            valueAccess = new MockValueAccess(6);
            validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.GreaterThanEqual, null, true);

            Assert.IsTrue(validator.Validate(5).IsValid);
            Assert.IsFalse(validator.Validate(6).IsValid);
            Assert.IsFalse(validator.Validate(7).IsValid);
            Assert.IsFalse(validator.Validate("string").IsValid);
            Assert.IsFalse(validator.Validate(null).IsValid);
        }

        #endregion

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            ValueAccess valueAccess = new MockValueAccess(5, "referenced key");
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);
            validator.MessageTemplate = "{0}|{1}|{2}|{3}|{4}|{5}";
            validator.Tag = "tag";
            object target = 6;
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsTrue(match.Groups["param0"].Success);
            Assert.AreEqual(target.ToString(), match.Groups["param0"].Value);
            Assert.IsTrue(match.Groups["param1"].Success);
            Assert.AreEqual(key, match.Groups["param1"].Value);
            Assert.IsTrue(match.Groups["param2"].Success);
            Assert.AreEqual(validator.Tag, match.Groups["param2"].Value);
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual("5", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("referenced key", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("Equal", match.Groups["param5"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            ValueAccess valueAccess = new MockValueAccess(5, "referenced key");
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.Equal);
            validator.Tag = "tag";
            object target = 6;
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsFalse(match.Groups["param0"].Success);
            Assert.IsFalse(match.Groups["param1"].Success);
            Assert.IsFalse(match.Groups["param2"].Success);
            Assert.IsFalse(match.Groups["param3"].Success);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("referenced key", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("Equal", match.Groups["param5"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            ValueAccess valueAccess = new MockValueAccess(5, "referenced key");
            Validator validator = new ValueAccessComparisonValidator(valueAccess, ComparisonOperator.NotEqual, null, true);
            validator.Tag = "tag";
            object target = 6;
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsFalse(match.Groups["param0"].Success);
            Assert.IsFalse(match.Groups["param1"].Success);
            Assert.IsFalse(match.Groups["param2"].Success);
            Assert.IsFalse(match.Groups["param3"].Success);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("referenced key", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("NotEqual", match.Groups["param5"].Value);
        }
    }
}
