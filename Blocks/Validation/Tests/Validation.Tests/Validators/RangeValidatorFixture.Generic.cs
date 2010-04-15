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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class GenericRangeValidatorFixture
    {
        [TestMethod]
        public void OneArgumentConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator<string> validator = new RangeValidator<string>("zzzz");

            RangeChecker<string> rangeChecker = validator.RangeChecker;
            Assert.AreEqual(null, rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeChecker.LowerBoundType);
            Assert.AreEqual("zzzz", rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
        }

        [TestMethod]
        public void TwoArgumentsConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator<string> validator = new RangeValidator<string>("aaaa", "zzzz");

            RangeChecker<string> rangeChecker = validator.RangeChecker;
            Assert.AreEqual("aaaa", rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.LowerBoundType);
            Assert.AreEqual("zzzz", rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
        }

        [TestMethod]
        public void AllArgumentsConstructorCreatesCorrectRangeChecker()
        {
            RangeValidator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Ignore);

            RangeChecker<string> rangeChecker = validator.RangeChecker;
            Assert.AreEqual("aaaa", rangeChecker.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeChecker.LowerBoundType);
            Assert.AreEqual("zzzz", rangeChecker.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeChecker.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
        }

        [TestMethod]
        public void CreatedInstanceWithMessageTemplateHasProvidedMessageTemplate()
        {
            RangeValidator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Ignore, "message template override");

            Assert.AreEqual("message template override", validator.MessageTemplate);
        }

        [TestMethod]
        public void CreatedInstanceWithNullMessageTemplateHasDefaultMessageTemplate()
        {
            RangeValidator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Ignore, null);

            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
        }

        [TestMethod]
        public void ReturnsSuccessForValueInRange()
        {
            Validator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate("bbbb");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsFailureForValueOutsideRange()
        {
            Validator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Inclusive);

            ValidationResults validationResults = validator.Validate("0000");

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        // TODO fix with templated message.
        public void ReturnsFailureWithOverridenMessageForValueOutsideRange()
        {
            string message = "overrinde message";

            Validator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Inclusive, message);

            ValidationResults validationResults = validator.Validate("0000");

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual(message, resultsList[0].Message);
        }

        [TestMethod]
        public void NegatedRejectsNullForReferenceType()
        {
            Validator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Inclusive, "bbbb", RangeBoundaryType.Inclusive, "test", true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNullForReferenceType()
        {
            Validator<string> validator = new RangeValidator<string>("aaaa", RangeBoundaryType.Inclusive, "bbbb", RangeBoundaryType.Inclusive, "test");

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedRejectsNullForValueType()
        {
            Validator<int> validator = new RangeValidator<int>(0, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Inclusive, "test", true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNullForValueType()
        {
            Validator<int> validator = new RangeValidator<int>(0, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Inclusive, "test");

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }
    }
}
