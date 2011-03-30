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
    public class RangeValidatorAttributeFixture
    {
        // creation
        // both null throws
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithBothParametersIgnoredThrows()
        {
            new RangeValidatorAttribute("1", RangeBoundaryType.Ignore, "2", RangeBoundaryType.Ignore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullNonIgnoredLowerBoundThrows()
        {
            new RangeValidatorAttribute(null, RangeBoundaryType.Inclusive, "2", RangeBoundaryType.Ignore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullNonIgnoredUpperBoundThrows()
        {
            new RangeValidatorAttribute("1", RangeBoundaryType.Ignore, null, RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        public void ConstructorSetsProperties()
        {
            RangeValidatorAttribute attribute1
                = new RangeValidatorAttribute(null, RangeBoundaryType.Ignore, "1", RangeBoundaryType.Inclusive);
            RangeValidatorAttribute attribute2
                = new RangeValidatorAttribute(2.0d, RangeBoundaryType.Exclusive, 0d, RangeBoundaryType.Ignore);
            RangeValidatorAttribute attribute3
                = new RangeValidatorAttribute(1, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Exclusive);

            Assert.AreEqual(null, attribute1.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, attribute1.LowerBoundType);
            Assert.AreEqual("1", attribute1.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, attribute1.UpperBoundType);

            Assert.AreEqual(2.0d, attribute2.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, attribute2.LowerBoundType);
            Assert.AreEqual(0d, attribute2.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, attribute2.UpperBoundType);

            Assert.AreEqual(1, attribute3.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, attribute3.LowerBoundType);
            Assert.AreEqual(2, attribute3.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, attribute3.UpperBoundType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCallWithNullBoundTypeThrows()
        {
            new RangeValidatorAttribute(null, "1", RangeBoundaryType.Ignore, "2.0d", RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCallWithNonIComparableBoundTypeThrows()
        {
            new RangeValidatorAttribute(typeof(object), "1", RangeBoundaryType.Ignore, "2.0d", RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCallWithNonISODateTimeThrows()
        {
            new RangeValidatorAttribute(typeof(DateTime), "1", RangeBoundaryType.Ignore, "2.0d", RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        public void ConstructorCallWithISODateTimeSetsProperties()
        {
            RangeValidatorAttribute attribute
                = new RangeValidatorAttribute(typeof(DateTime),
                                              null, RangeBoundaryType.Ignore,
                                              "2006-01-10T00:00:00", RangeBoundaryType.Exclusive);

            Assert.AreEqual(null, attribute.EffectiveLowerBound);
            Assert.AreEqual(null, attribute.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, attribute.LowerBoundType);
            Assert.AreEqual(new DateTime(2006, 1, 10), attribute.EffectiveUpperBound);
            Assert.AreEqual("2006-01-10T00:00:00", attribute.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, attribute.UpperBoundType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCallWithInvalidDecimalStringThrows()
        {
            new RangeValidatorAttribute(typeof(Decimal),
                                        "0lfkjda.0", RangeBoundaryType.Ignore,
                                        "1000.5", RangeBoundaryType.Exclusive);
        }

        [TestMethod]
        public void ConstructorCallWithDecimalStringSetsProperties()
        {
            RangeValidatorAttribute attribute
                = new RangeValidatorAttribute(typeof(Decimal),
                                              "0.0", RangeBoundaryType.Ignore,
                                              "1000.5", RangeBoundaryType.Exclusive);

            Assert.AreEqual(new Decimal(0, 0, 0, false, 0), attribute.EffectiveLowerBound);
            Assert.AreEqual("0.0", attribute.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, attribute.LowerBoundType);
            Assert.AreEqual(new Decimal(10005, 0, 0, false, 1), attribute.EffectiveUpperBound);
            Assert.AreEqual("1000.5", attribute.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, attribute.UpperBoundType);
        }

        [TestMethod]
        public void DecimalAttributeWithRangeParametersCreatesValidator()
        {
            ValidatorAttribute attribute = new RangeValidatorAttribute(typeof(Decimal),
                                                                       "0.0", RangeBoundaryType.Inclusive,
                                                                       "200.10", RangeBoundaryType.Exclusive);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(typeof(Decimal), null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator typedValidator = validator as RangeValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(new Decimal(0), typedValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, typedValidator.LowerBoundType);
            Assert.AreEqual(new Decimal(2001, 0, 0, false, 1), typedValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, typedValidator.UpperBoundType);
        }

        [TestMethod]
        public void DateTimeAttributeWithRangeParametersAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new RangeValidatorAttribute(typeof(DateTime),
                                                                            "2006-01-10T00:00:00", RangeBoundaryType.Inclusive,
                                                                            "2006-01-20T00:00:00", RangeBoundaryType.Exclusive);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(typeof(DateTime), null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator typedValidator = validator as RangeValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(new DateTime(2006, 01, 10), typedValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, typedValidator.LowerBoundType);
            Assert.AreEqual(new DateTime(2006, 01, 20), typedValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, typedValidator.UpperBoundType);
        }

        [TestMethod]
        public void IntAttributeWithRangeParametersAndMessageTemplateCreatesValidator()
        {
            ValidatorAttribute attribute = new RangeValidatorAttribute(2, RangeBoundaryType.Inclusive, 5, RangeBoundaryType.Exclusive);
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(typeof(int), null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator typedValidator = validator as RangeValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual("my message template", typedValidator.MessageTemplate);
            Assert.AreEqual(2, typedValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, typedValidator.LowerBoundType);
            Assert.AreEqual(5, typedValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, typedValidator.UpperBoundType);
        }

        [TestMethod]
        public void DoubleAttributeWithRangeParametersAndMessageTemplateAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new RangeValidatorAttribute(2.0d, RangeBoundaryType.Inclusive, 5.6d, RangeBoundaryType.Exclusive);
            attribute.Negated = true;
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(typeof(Double), null, null, null);
            Assert.IsNotNull(validator);

            RangeValidator typedValidator = validator as RangeValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual("my message template", typedValidator.MessageTemplate);
            Assert.AreEqual(2.0d, typedValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, typedValidator.LowerBoundType);
            Assert.AreEqual(5.6d, typedValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, typedValidator.UpperBoundType);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new RangeValidatorAttribute(0, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid(5));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new RangeValidatorAttribute(0, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid(100));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingValueOfWrongTypeThrows()
        {
            ValidationAttribute attribute =
                new RangeValidatorAttribute(0, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}"
                };

            try
            {
                attribute.IsValid("a string");
                Assert.Fail();
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new RangeValidatorAttribute(0, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid(100));
        }
    }
}
