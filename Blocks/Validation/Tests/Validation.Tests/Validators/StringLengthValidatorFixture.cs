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
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class StringLengthValidatorFixture
    {
        [TestMethod]
        public void ReturnFailureForNullString()
        {
            Validator<string> validator = new StringLengthValidator(1, 10);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForNullStringThroughNonGenericInterface()
        {
            Validator<string> validator = new StringLengthValidator(1, 10);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringShorterThanMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringShorterThanMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringOneCharShorterThanMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 9).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringAsLongAsMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 10).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringAsLongAsMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 10).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringLongerThanMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 11).ToString());

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void NegatedReturnsSuccessForStringLongerThanMaxLengthForMaxLengthOnlyValidator()
        {
            Validator<string> validator = new StringLengthValidator(10, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 11).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringAsLongAsMinLengthForMinAndMaxLengthValidator()
        {
            Validator<string> validator = new StringLengthValidator(5, 10);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringAsLongAsMinLengthForMinAndMaxLengthValidator()
        {
            Validator<string> validator = new StringLengthValidator(5, 10, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ConstructorWithUpperBoundOnlyInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(10);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperBoundOnlyAndNegatedInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(10, true);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerAndUpperBoundInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(5, 10);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerAndUpperBoundAndNegatedInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(5, 10, true);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithBoundsAndBoundTypesInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Exclusive);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithBoundsAndBoundTypesAndNegatedInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Exclusive, true);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithBoundsAndBoundTypesAndMessageOverrideInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Exclusive, "message template override");

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithBoundsAndBoundTypesAndMessageOverrideAndNegatedInitializesAppropriateInstance()
        {
            StringLengthValidator validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Exclusive, "message template override", true);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithBoundsAndBoundTypesAndNullMessageOverrideInitializesAppropriateInstanceWithDefaultMessage()
        {
            StringLengthValidator validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Exclusive, null);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithBoundsAndBoundTypesAndNullMessageOverrideInitializesAndNegatedAppropriateInstanceWithDefaultMessage()
        {
            StringLengthValidator validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Exclusive, null, true);

            Assert.AreEqual(5, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(10, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ReturnsSuccessForStringShorterThanLowerBoundIfLowerBoundIsIgnored()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 2).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringShorterThanLowerBoundIfLowerBoundIsIgnored()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 2).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringLongerThanLowerBound()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 7).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringLongerThanLowerBound()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 7).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringShorterThanLowerBound()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 3).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringShorterThanLowerBound()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 3).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringWithLengthEqualToLowerBoundIfLowerBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForStringWithLengthEqualToLowerBoundIfLowerBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringWithLengthEqualToLowerBoundIfLowerBoundIsExclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 5).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringWithLengthOneShoterThanLowerBoundIfLowerBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 4).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsSuccessForStringWithLengthOneShoterThanLowerBoundIfLowerBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, true);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 4).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringWithLengthOneShorterThanLowerBoundIfLowerBoundIsExclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 4).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringWithLengthOneLongerThanLowerBoundIfLowerBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 6).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringWithLengthOneLongerThanLowerBoundIfLowerBoundIsExclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 6).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringLongerThanUpperBoundIfUpperBoundIsIgnored()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Ignore);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 12).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringShorterThanUpperBound()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 7).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringLongerThanUpperBound()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 12).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringWithLengthEqualToUpperBoundIfUpperBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 10).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringWithLengthEqualToUpperBoundIfUpperBoundIsExclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 10).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringWithLengthOneShoterThanLowerBoundIfLUpperBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 9).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForStringWithLengthOneShorterThanUpperBoundIfUpperBoundIsExclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 9).ToString());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringWithLengthOneLongerThanUpperBoundIfUpperBoundIsInclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 11).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForStringWithLengthOneLongerThanUpperBoundIfUpperBoundIsExclusive()
        {
            Validator<string> validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);

            ValidationResults validationResults = validator.Validate(new StringBuilder().Append('a', 11).ToString());

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithUpperBoundIsLowerThanLowerBoundAndNeitherBoundIsIgnoredThrows()
        {
            new StringLengthValidator(15, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);
        }

        [TestMethod]
        public void CreationWithUpperBoundIsLowerThanLowerBoundDoesNotThrowIfAnyBoundIsIgnored()
        {
            new StringLengthValidator(15, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Exclusive);
            new StringLengthValidator(15, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Ignore);
            new StringLengthValidator(15, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Ignore);
            new StringLengthValidator(15, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Exclusive, true);
            new StringLengthValidator(15, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Ignore, true);
            new StringLengthValidator(15, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Ignore, true);
        }

        [TestMethod]
        public void ReturnsFailureForValidationForNonStringTargetThroughNonGenericInterface()
        {
            Validator validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(validator.Validate(3).IsValid);
        }

        [TestMethod]
        public void NegatedReturnsSuccessForValidationForNonStringTargetThroughNonGenericInterface()
        {
            Validator validator = new StringLengthValidator(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, true);

            Assert.IsFalse(validator.Validate(3).IsValid);
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            StringLengthValidator validator = new StringLengthValidator(10, RangeBoundaryType.Exclusive, 20, RangeBoundaryType.Inclusive);
            validator.MessageTemplate = "{0}-{1}-{2}-{3}-{4}-{5}-{6}";
            validator.Tag = "tag";
            object target = "short";
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
            Assert.AreEqual("10", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("Exclusive", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("20", match.Groups["param5"].Value);
            Assert.IsTrue(match.Groups["param6"].Success);
            Assert.AreEqual("Inclusive", match.Groups["param6"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            StringLengthValidator validator = new StringLengthValidator(10, RangeBoundaryType.Exclusive, 20, RangeBoundaryType.Inclusive);
            validator.Tag = "tag";
            object target = "short";
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
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual("10", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("Exclusive", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("20", match.Groups["param5"].Value);
            Assert.IsTrue(match.Groups["param6"].Success);
            Assert.AreEqual("Inclusive", match.Groups["param6"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            StringLengthValidator validator = new StringLengthValidator(10, RangeBoundaryType.Exclusive, 20, RangeBoundaryType.Inclusive, true);
            validator.Tag = "tag";
            object target = "not so short";
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
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual("10", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("Exclusive", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("20", match.Groups["param5"].Value);
            Assert.IsTrue(match.Groups["param6"].Success);
            Assert.AreEqual("Inclusive", match.Groups["param6"].Value);
        }
    }
}
