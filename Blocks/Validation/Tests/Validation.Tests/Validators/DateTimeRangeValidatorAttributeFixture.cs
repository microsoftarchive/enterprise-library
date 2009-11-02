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
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class DateTimeRangeValidatorAttributeFixture
    {
        [TestMethod]
        public void AttributeWithUpperBoundOnlyCreatesAppropriateValidator()
        {
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(upperBound);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithUpperBoundOnlyAndNegatedCreatesAppropriateValidator()
        {
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(upperBound);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringUpperBoundOnlyCreatesAppropriateValidator()
        {
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("2006-01-10T00:00:00");

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringUpperBoundOnlyAndNegatedCreatesAppropriateValidator()
        {
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("2006-01-10T00:00:00");
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsOnlyCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, upperBound);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsOnlyAndNegatedCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, upperBound);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringLowerAndUpperBoundsOnlyCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 20);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("2006-01-01T00:00:00", "2006-01-20T00:00:00");

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringLowerAndUpperBoundsOnlyAndNegatedCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 20);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("2006-01-01T00:00:00", "2006-01-20T00:00:00");
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndMessageOverrideCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, upperBound);
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual("overriden message template", rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndMessageOverrideAndNegatedCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, upperBound);
            attribute.Negated = true;
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, rangeValidator.UpperBoundType);
            Assert.AreEqual("overriden message template", rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndBoundTypesCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = default(DateTime);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Ignore);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndBoundTypesAndNegatedCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = default(DateTime);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Ignore);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringLowerAndUpperBoundsAndBoundTypesCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = default(DateTime);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("2006-01-01T00:00:00", RangeBoundaryType.Exclusive, "", RangeBoundaryType.Ignore);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringLowerAndUpperBoundsAndBoundTypesAndNegatedCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = default(DateTime);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("2006-01-01T00:00:00", RangeBoundaryType.Exclusive, "", RangeBoundaryType.Ignore);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringLowerAndUpperBoundsAndBoundTypesAndMessageTemplateCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = default(DateTime);
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Ignore);
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.UpperBoundType);
            Assert.AreEqual("my message template", rangeValidator.MessageTemplate);
            Assert.AreEqual(false, rangeValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithStringLowerAndUpperBoundsAndBoundTypesAndMessageTemplateAndNegatedCreatesAppropriateValidator()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = default(DateTime);
            ValueValidatorAttribute attribute = new DateTimeRangeValidatorAttribute(lowerBound, RangeBoundaryType.Exclusive, upperBound, RangeBoundaryType.Ignore);
            attribute.Negated = true;
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;
            Assert.IsNotNull(rangeValidator);
            Assert.AreEqual(lowerBound, rangeValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, rangeValidator.LowerBoundType);
            Assert.AreEqual(upperBound, rangeValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, rangeValidator.UpperBoundType);
            Assert.AreEqual("my message template", rangeValidator.MessageTemplate);
            Assert.AreEqual(true, rangeValidator.Negated);
        }

        [TestMethod]
        public void ConstructorCallWithNullStringDateUsesDefaultDateTimeValue()
        {
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute((string)null);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;

            Assert.AreEqual(default(DateTime), rangeValidator.UpperBound);
        }

        [TestMethod]
        public void ConstructorCallWithEmptyStringDateUsesDefaultDateTimeValue()
        {
            ValidatorAttribute attribute = new DateTimeRangeValidatorAttribute("");

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            RangeValidator<DateTime> rangeValidator = validator as RangeValidator<DateTime>;

            Assert.AreEqual(default(DateTime), rangeValidator.UpperBound);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCallWithInvalidStringDateThrows()
        {
            new DateTimeRangeValidatorAttribute("an invalid date string");
        }

        public class TestClass
        {
            [DateTimeRangeValidator("2006-01-01T00:00:00", "2006-01-01T00:00:00")]
            public DateTime DateTime;
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new DateTimeRangeValidatorAttribute(new DateTime(2000, 1, 1), new DateTime(2000, 1, 31))
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid(new DateTime(2000, 1, 15)));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new DateTimeRangeValidatorAttribute(new DateTime(2000, 1, 1), new DateTime(2000, 1, 31))
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid(new DateTime(2001, 1, 15)));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new DateTimeRangeValidatorAttribute(new DateTime(2000, 1, 1), new DateTime(2000, 1, 31))
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid(new DateTime(2001, 1, 15)));
        }
    }
}
