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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class RangeValidatorFixture
    {
        [TestMethod]
        public void NegatedRejectsNull()
        {
            RangeValidator validator = new RangeValidator(2);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNull()
        {
            RangeValidator validator = new RangeValidator(2);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullLowerBoundThrows()
        {
            new RangeValidator(null, RangeBoundaryType.Inclusive, "value", RangeBoundaryType.Inclusive, null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullUpperBoundThrows()
        {
            new RangeValidator("value", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Inclusive, null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfValidatorWithNullUpperAndLowerBoundsAndIgnoreThrows()
        {
            new RangeValidator(null, RangeBoundaryType.Ignore, null, RangeBoundaryType.Ignore, null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfValidatorWithLowerHigerThanUpperBoundThrows()
        {
            new RangeValidator("cccc", RangeBoundaryType.Inclusive, "aaaa", RangeBoundaryType.Inclusive, null, false);
        }

        [TestMethod]
        public void ConstructionOfValidatorWithNullLowerBoundsAndIgnoreCreatesValidator()
        {
            Validator validator = new RangeValidator(null, RangeBoundaryType.Ignore, "value", RangeBoundaryType.Inclusive, null, false);

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void ConstructionOfValidatorWithNullUpperBoundsAndIgnoreCreatesValidator()
        {
            Validator validator = new RangeValidator("value", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Ignore, null, false);

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void OneStringArgumentConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator validator = new RangeValidator("zzzz");

            RangeChecker<IComparable> rangeChecker = validator.RangeChecker;
            Assert.AreEqual(null, rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeChecker.LowerBoundType);
            Assert.AreEqual("zzzz", rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void OneIntArgumentConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator validator = new RangeValidator(12);

            RangeChecker<IComparable> rangeChecker = validator.RangeChecker;
            Assert.AreEqual(null, rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeChecker.LowerBoundType);
            Assert.AreEqual(12, rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void TwoArgumentsConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator validator = new RangeValidator("aaaa", "zzzz");

            RangeChecker<IComparable> rangeChecker = validator.RangeChecker;
            Assert.AreEqual("aaaa", rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.LowerBoundType);
            Assert.AreEqual("zzzz", rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void AllArgumentsConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator validator = new RangeValidator(12.22, RangeBoundaryType.Exclusive, 99.99, RangeBoundaryType.Ignore);

            RangeChecker<IComparable> rangeChecker = validator.RangeChecker;
            Assert.AreEqual(12.22, rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeChecker.LowerBoundType);
            Assert.AreEqual(99.99, rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void AllArgumentsWithNegatedConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator validator = new RangeValidator(12.22, RangeBoundaryType.Exclusive, 99.99, RangeBoundaryType.Ignore, true);

            RangeChecker<IComparable> rangeChecker = validator.RangeChecker;
            Assert.AreEqual(12.22, rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeChecker.LowerBoundType);
            Assert.AreEqual(99.99, rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatedInstanceWithMessageTemplateHasProvidedMessageTemplate()
        {
            RangeValidator validator = new RangeValidator("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Ignore, "message template override");

            Assert.AreEqual("message template override", validator.MessageTemplate);
        }

        [TestMethod]
        public void CreatedInstanceWithNullMessageTemplateHasDefaultMessageTemplate()
        {
            RangeValidator validator = new RangeValidator("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Ignore, null);

            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
        }

        [TestMethod]
        public void NonNegatedSuccessWithStringBeetweenLowerAndUpper()
        {
            RangeValidator inclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, "rrrr", RangeBoundaryType.Inclusive, null);

            Assert.IsTrue(inclusiveValidator.Validate("cccc").IsValid);
            Assert.IsTrue(inclusiveValidator.Validate("cccca").IsValid);
            Assert.IsTrue(inclusiveValidator.Validate("gggg").IsValid);
            Assert.IsTrue(inclusiveValidator.Validate("rrrr").IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureWithStringBeetweenLowerAndUpper()
        {
            RangeValidator inclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, "rrrr", RangeBoundaryType.Inclusive, null);

            Assert.IsFalse(inclusiveValidator.Validate("aaaa").IsValid);
            Assert.IsFalse(inclusiveValidator.Validate("bcccc").IsValid);
            Assert.IsFalse(inclusiveValidator.Validate("rrrra").IsValid);
            Assert.IsFalse(inclusiveValidator.Validate("zzzz").IsValid);

            RangeValidator exclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Exclusive, "rrrr", RangeBoundaryType.Exclusive, null);

            Assert.IsFalse(exclusiveValidator.Validate("aaaa").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("cccc").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("bcccc").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("rrrra").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("rrrr").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("zzzz").IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessWithStringLower()
        {
            RangeValidator validator = new RangeValidator(null, RangeBoundaryType.Ignore, "cccc", RangeBoundaryType.Inclusive, null);

            Assert.IsTrue(validator.Validate("aaaa").IsValid);
            Assert.IsTrue(validator.Validate("cccc").IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureWithStringLower()
        {
            RangeValidator inclusiveValidator = new RangeValidator(null, RangeBoundaryType.Ignore, "cccc", RangeBoundaryType.Inclusive, null);

            Assert.IsFalse(inclusiveValidator.Validate("dddd").IsValid);

            RangeValidator exclusiveValidator = new RangeValidator(null, RangeBoundaryType.Ignore, "cccc", RangeBoundaryType.Exclusive, null);

            Assert.IsFalse(exclusiveValidator.Validate("cccc").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("dddd").IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessWithStringUpper()
        {
            RangeValidator validator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Ignore, null);

            Assert.IsTrue(validator.Validate("cccc").IsValid);
            Assert.IsTrue(validator.Validate("dddd").IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureWithStringUpper()
        {
            RangeValidator inclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Ignore, null);

            Assert.IsFalse(inclusiveValidator.Validate("bbbb").IsValid);

            RangeValidator exclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Exclusive, null, RangeBoundaryType.Ignore, null);

            Assert.IsFalse(exclusiveValidator.Validate("bbbb").IsValid);
            Assert.IsFalse(exclusiveValidator.Validate("cccc").IsValid);
        }

        [TestMethod]
        public void NegatedFailureWithStringBeetweenLowerAndUpper()
        {
            RangeValidator inclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, "rrrr", RangeBoundaryType.Inclusive, null, true);

            Assert.IsFalse(inclusiveValidator.Validate("cccc").IsValid);
            Assert.IsFalse(inclusiveValidator.Validate("cccca").IsValid);
            Assert.IsFalse(inclusiveValidator.Validate("gggg").IsValid);
            Assert.IsFalse(inclusiveValidator.Validate("rrrr").IsValid);
        }

        [TestMethod]
        public void NegatedSuccessWithStringBeetweenLowerAndUpper()
        {
            RangeValidator inclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, "rrrr", RangeBoundaryType.Inclusive, null, true);

            Assert.IsTrue(inclusiveValidator.Validate("aaaa").IsValid);
            Assert.IsTrue(inclusiveValidator.Validate("bcccc").IsValid);
            Assert.IsTrue(inclusiveValidator.Validate("rrrra").IsValid);
            Assert.IsTrue(inclusiveValidator.Validate("zzzz").IsValid);

            RangeValidator exclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Exclusive, "rrrr", RangeBoundaryType.Exclusive, null, true);

            Assert.IsTrue(exclusiveValidator.Validate("aaaa").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("cccc").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("bcccc").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("rrrra").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("rrrr").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("zzzz").IsValid);
        }

        [TestMethod]
        public void NegatedFailureWithStringLower()
        {
            RangeValidator validator = new RangeValidator(null, RangeBoundaryType.Ignore, "cccc", RangeBoundaryType.Inclusive, null, true);

            Assert.IsFalse(validator.Validate("aaaa").IsValid);
            Assert.IsFalse(validator.Validate("cccc").IsValid);
        }

        [TestMethod]
        public void NegatedSuccessWithStringLower()
        {
            RangeValidator inclusiveValidator = new RangeValidator(null, RangeBoundaryType.Ignore, "cccc", RangeBoundaryType.Inclusive, null, true);

            Assert.IsTrue(inclusiveValidator.Validate("dddd").IsValid);

            RangeValidator exclusiveValidator = new RangeValidator(null, RangeBoundaryType.Ignore, "cccc", RangeBoundaryType.Exclusive, null, true);

            Assert.IsTrue(exclusiveValidator.Validate("cccc").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("dddd").IsValid);
        }

        [TestMethod]
        public void NegatedFailureWithStringUpper()
        {
            RangeValidator validator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Ignore, null, true);

            Assert.IsFalse(validator.Validate("cccc").IsValid);
            Assert.IsFalse(validator.Validate("dddd").IsValid);
        }

        [TestMethod]
        public void NegatedSuccessWithStringUpper()
        {
            RangeValidator inclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Ignore, null, true);

            Assert.IsTrue(inclusiveValidator.Validate("bbbb").IsValid);

            RangeValidator exclusiveValidator = new RangeValidator("cccc", RangeBoundaryType.Exclusive, null, RangeBoundaryType.Ignore, null, true);

            Assert.IsTrue(exclusiveValidator.Validate("bbbb").IsValid);
            Assert.IsTrue(exclusiveValidator.Validate("cccc").IsValid);
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            RangeValidator validator = new RangeValidator(10, RangeBoundaryType.Exclusive, 20, RangeBoundaryType.Inclusive);
            validator.MessageTemplate = "{0}|{1}|{2}|{3}|{4}|{5}|{6}";
            validator.Tag = "tag";
            object target = 24;
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
            RangeValidator validator = new RangeValidator(10, RangeBoundaryType.Exclusive, 20, RangeBoundaryType.Inclusive);
            validator.Tag = "tag";
            object target = 24;
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
            RangeValidator validator = new RangeValidator(10, RangeBoundaryType.Exclusive, 20, RangeBoundaryType.Inclusive, true);
            validator.Tag = "tag";
            object target = 15;
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
