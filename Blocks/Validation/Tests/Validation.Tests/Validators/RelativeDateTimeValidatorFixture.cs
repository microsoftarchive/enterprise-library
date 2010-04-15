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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class RelativeDateTimeValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfValidatorWithIncosistentLowerUnitAndBoundThrows()
        {
            new RelativeDateTimeValidator(2, DateTimeUnit.None, RangeBoundaryType.Inclusive, 0, DateTimeUnit.None, RangeBoundaryType.Ignore, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfValidatorWithIncosistentUpperUnitAndBoundThrows()
        {
            new RelativeDateTimeValidator(0, DateTimeUnit.None, RangeBoundaryType.Ignore, 2, DateTimeUnit.None, RangeBoundaryType.Inclusive, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfValidatorWithLowerBoundHigherThanUpperBoundThrows()
        {
            new RelativeDateTimeValidator(45, DateTimeUnit.Day, RangeBoundaryType.Inclusive, 1, DateTimeUnit.Month, RangeBoundaryType.Inclusive, false);
        }

        [TestMethod]
        public void CreatingInstanceWithNegated()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, true);

            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatingInstanceWithNonNegated()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, false);

            Assert.AreEqual(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndNegatedCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, true);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndMessageTemplateCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, "my message template");

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndMessageTemplateAndNegatedCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, "my message template", true);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndRangeBoundaryTypeCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, RangeBoundaryType.Ignore);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndRangeBoundaryTypeAndNegatedCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, RangeBoundaryType.Ignore, true);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndRangeBoundaryTypeAndTemplateMessageCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, RangeBoundaryType.Ignore, "my template message");

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.UpperBoundType);
            Assert.AreEqual("my template message", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesAndRangeBoundaryTypeAndTemplateMessageAndNegatedCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, RangeBoundaryType.Ignore, "my template message", true);

            Assert.AreEqual(0, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.None, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Ignore, validator.UpperBoundType);
            Assert.AreEqual("my template message", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerValuesAndUpperValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, 7, DateTimeUnit.Year);

            Assert.AreEqual(3, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(7, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Year, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerValuesAndUpperValuesAndNegatedCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Hour, 7, DateTimeUnit.Year, true);

            Assert.AreEqual(3, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(7, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Year, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerAndUpperValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive);

            Assert.AreEqual(2, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Minute, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerAndUpperAndNegatedValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive, true);

            Assert.AreEqual(2, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Minute, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerAndUpperAndMessageTemplateValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive, "my message template");

            Assert.AreEqual(2, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Minute, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithLowerAndUpperAndMessageTemplateAndNegatedValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive, "my message template", true);

            Assert.AreEqual(2, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Minute, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void LowerBoundBiggerThanUpperBoundThrows()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                -7, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpperBoundBiggerThanLowerBoundThrows()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void LowerNoneUnitWithNonIgnoreBoundaryTypeThrows()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(5, DateTimeUnit.None, RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        public void NegatedRejectsNull()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(2, DateTimeUnit.Minute, true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNull()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(2, DateTimeUnit.Minute, true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeBetweenLowerAndUpper()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(2));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedFailureForDateTimeBetweenLowerAndUpper()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(2));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedSuccessForDateTimeGreaterThenUpperBound()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(8));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedSuccessForDateTimeSmallerThenLowerBound()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-10));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeGreaterThenUpperBound()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(8));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedFailureForDateTimeSmallerThenLowerBound()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-10));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForSameLowerBoundValueUsingInclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-5));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailuresForSameLowerBoundValueUsingExclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-5));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForSameUpperBoundValueUsingInclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(3));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailuresForSameUpperBoundValueUsingExclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Exclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(3));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedSuccessForSameLowerBoundValueUsingInclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-5));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedFailuresForSameLowerBoundValueUsingExclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-5));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedSuccessForSameUpperBoundValueUsingInclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(3));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedFailuresForSameUpperBoundValueUsingExclusiveBoundaryType()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive,
                                                                                3, DateTimeUnit.Day, RangeBoundaryType.Exclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(3));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void ResourceValuesHaveBeenDefined()
        {
            Assert.IsFalse(string.IsNullOrEmpty(Resources.RelativeDateTimeNegatedDefaultMessageTemplate));
            Assert.IsFalse(string.IsNullOrEmpty(Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate));
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeBetweenPositiveLowerAndPositiveUpper()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                5, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(4));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeBetweenNegativeLowerAndNegativeUpper()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                -3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-4));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedSuccessForDateTimeBetweenPositiveLowerAndPositiveUpper()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                5, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(4));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedSuccessForDateTimeBetweenNegativeLowerAndNegativeUpper()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                -3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, true);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-4));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeLowerPositiveInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                5, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(3));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeLowerNegativeInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                -3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-5));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperPositiveInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                5, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(5));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperNegativeInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                -3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-3));

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeLowerPositiveExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Day, RangeBoundaryType.Exclusive,
                                                                                5, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(3));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeLowerNegativeExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive,
                                                                                -3, DateTimeUnit.Day, RangeBoundaryType.Inclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-5));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperPositiveExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(3, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                5, DateTimeUnit.Day, RangeBoundaryType.Exclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(5));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperNegativeExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
                                                                                -3, DateTimeUnit.Day, RangeBoundaryType.Exclusive, false);

            ValidationResults results = validator.Validate(DateTime.Now.AddDays(-3));

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperOnlyPositiveInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(5, DateTimeUnit.Day, RangeBoundaryType.Inclusive);

            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(5)).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(4)).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-2)).IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperOnlyPositiveInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(5, DateTimeUnit.Day, RangeBoundaryType.Inclusive);

            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(6)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(7)).IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperOnlyNegativeInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive);

            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-5)).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-6)).IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperOnlyNegativeInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Inclusive);

            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(-2)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(3)).IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperOnlyInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(0, DateTimeUnit.None, RangeBoundaryType.Inclusive);

            Assert.IsTrue(validator.Validate(DateTime.Now).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-1)).IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperOnlyInclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(0, DateTimeUnit.None, RangeBoundaryType.Inclusive);

            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(1)).IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperOnlyPositiveExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(5, DateTimeUnit.Day, RangeBoundaryType.Exclusive);

            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(4)).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now).IsValid);
            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-2)).IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperOnlyPositiveExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(5, DateTimeUnit.Day, RangeBoundaryType.Exclusive);

            // this assertion is time dependent
            //Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(5)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(6)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(7)).IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperOnlyNegativeExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive);

            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-6)).IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperOnlyNegativeExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-5, DateTimeUnit.Day, RangeBoundaryType.Exclusive);

            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(-5)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(-2)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(3)).IsValid);
        }

        [TestMethod]
        public void NonNegatedSuccessForDateTimeUpperOnlyExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(0, DateTimeUnit.None, RangeBoundaryType.Exclusive);

            Assert.IsTrue(validator.Validate(DateTime.Now.AddDays(-1)).IsValid);
        }

        [TestMethod]
        public void NonNegatedFailureForDateTimeUpperOnlyExclusive()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(0, DateTimeUnit.None, RangeBoundaryType.Exclusive);

            Assert.IsFalse(validator.Validate(DateTime.Now.AddSeconds(1)).IsValid);
            Assert.IsFalse(validator.Validate(DateTime.Now.AddDays(1)).IsValid);
        }

        [TestMethod]
        public void CanValidateThroughNonGenericProtocol()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-1, DateTimeUnit.Day, RangeBoundaryType.Exclusive);

            Assert.IsFalse(validator.Validate((object)DateTime.Now).IsValid);
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-10, DateTimeUnit.Day, 20, DateTimeUnit.Year);
            validator.MessageTemplate = "{0}|{1}|{2}|{3}|{4}|{5}|{6}";
            validator.Tag = "tag";
            object target = DateTime.Now.AddDays(-20);
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
            Assert.AreEqual("-10", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("Day", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("20", match.Groups["param5"].Value);
            Assert.IsTrue(match.Groups["param6"].Success);
            Assert.AreEqual("Year", match.Groups["param6"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-10, DateTimeUnit.Day, 20, DateTimeUnit.Year);
            validator.Tag = "tag";
            object target = DateTime.Now.AddDays(-20);
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
            Assert.AreEqual("-10", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("Day", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("20", match.Groups["param5"].Value);
            Assert.IsTrue(match.Groups["param6"].Success);
            Assert.AreEqual("Year", match.Groups["param6"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            RelativeDateTimeValidator validator = new RelativeDateTimeValidator(-10, DateTimeUnit.Day, 20, DateTimeUnit.Year, true);
            validator.Tag = "tag";
            object target = DateTime.Now.AddDays(-2);
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
            Assert.AreEqual("-10", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("Day", match.Groups["param4"].Value);
            Assert.IsTrue(match.Groups["param5"].Success);
            Assert.AreEqual("20", match.Groups["param5"].Value);
            Assert.IsTrue(match.Groups["param6"].Success);
            Assert.AreEqual("Year", match.Groups["param6"].Value);
        }

        [TestMethod]
        public void ValidatesUsingTheCurrentDataAtTheTimeOfTheValidation()
        {
            var validator =
                new RelativeDateTimeValidator(-10, DateTimeUnit.Second, RangeBoundaryType.Exclusive, 0, DateTimeUnit.Day, RangeBoundaryType.Ignore);
            var pointInTime = DateTime.Now;

            Assert.IsTrue(validator.Validate(pointInTime).IsValid);

            Thread.Sleep(15000);

            Assert.IsFalse(validator.Validate(pointInTime).IsValid);
        }
    }
}
