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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class DateTimeRangeValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateWithLowerHigerThanUpperBoundThrows()
        {
            DateTime lowerBound = new DateTime(2006, 1, 10);
            DateTime upperBound = new DateTime(2006, 1, 1);

            Validator validator = new DateTimeRangeValidator(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive);

            Assert.IsNotNull(validator);
            Assert.IsTrue(validator.Validate(lowerBound).IsValid);
        }

        [TestMethod]
        public void UpperArgumentConstructorCreatesCorrectValidator()
        {
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(upperBound);

            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(default(DateTime), validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void NegatedUpperArgumentConstructorCreatesCorrectValidator()
        {
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(upperBound, true);

            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(default(DateTime), validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void UpperAndLowerArgumentConstructorCreatesCorrectValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(lowerBound, upperBound);

            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(lowerBound, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void NegatedUpperAndLowerArgumentConstructorCreatesCorrectValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(lowerBound, upperBound, true);

            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(lowerBound, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void UpperAndLowerAndBoundTypeArgumentConstructorCreatesCorrectValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Inclusive);

            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(lowerBound, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void NegatedUpperAndLowerAndBoundTypeArgumentConstructorCreatesCorrectValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Inclusive, true);

            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(lowerBound, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void UpperAndLowerAndBoundTypeAndMessageTemplateArgumentConstructorCreatesCorrectValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Inclusive, "my message template");

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(lowerBound, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }

        [TestMethod]
        public void NegatedUpperAndLowerAndBoundTypeAndMessageTemplateArgumentConstructorCreatesCorrectValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidator validator = new DateTimeRangeValidator(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Inclusive, "my message template", true);

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(lowerBound, validator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.LowerBoundType);
            Assert.AreEqual(upperBound, validator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
        }
    }
}
