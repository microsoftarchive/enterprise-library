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
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class RelativeDateTimeValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfAttributeWithIncosistentLowerUnitAndBoundThrows()
        {
            new RelativeDateTimeValidatorAttribute(2, DateTimeUnit.None, RangeBoundaryType.Inclusive, 0, DateTimeUnit.None, RangeBoundaryType.Ignore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionOfAttributeWithIncosistentUpperUnitAndBoundThrows()
        {
            new RelativeDateTimeValidatorAttribute(0, DateTimeUnit.None, RangeBoundaryType.Ignore, 2, DateTimeUnit.None, RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        public void ConstructorWithUpperValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(3, DateTimeUnit.Hour);

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(3, DateTimeUnit.Hour);
            validatorAttribute.Negated = true;

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
        public void ConstructorWithUpperValuesAndRangeBoundaryTypeCreatesCorrectInstance()
        {
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(3, DateTimeUnit.Hour, RangeBoundaryType.Ignore);

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(3, DateTimeUnit.Hour, RangeBoundaryType.Ignore);
            validatorAttribute.Negated = true;

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
        public void ConstructorWithLowerValuesAndUpperValuesCreatesCorrectInstance()
        {
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(3, DateTimeUnit.Hour, 7, DateTimeUnit.Year);

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(3, DateTimeUnit.Hour, 7, DateTimeUnit.Year);
            validatorAttribute.Negated = true;

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                                           3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive);

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                                           3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive);
            validatorAttribute.Negated = true;

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                                           3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive);
            validatorAttribute.MessageTemplate = "my message template";

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

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
            RelativeDateTimeValidatorAttribute validatorAttribute = new RelativeDateTimeValidatorAttribute(2, DateTimeUnit.Minute, RangeBoundaryType.Inclusive,
                                                                                                           3, DateTimeUnit.Hour, RangeBoundaryType.Exclusive);
            validatorAttribute.Negated = true;
            validatorAttribute.MessageTemplate = "my message template";

            RelativeDateTimeValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null) as RelativeDateTimeValidator;
            Assert.IsNotNull(validator);

            Assert.AreEqual(2, validator.LowerBound);
            Assert.AreEqual(DateTimeUnit.Minute, validator.LowerUnit);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validator.LowerBoundType);
            Assert.AreEqual(3, validator.UpperBound);
            Assert.AreEqual(DateTimeUnit.Hour, validator.UpperUnit);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validator.UpperBoundType);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new RelativeDateTimeValidatorAttribute(10, DateTimeUnit.Day, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid(DateTime.Today.AddDays(5)));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new RelativeDateTimeValidatorAttribute(10, DateTimeUnit.Day, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid(DateTime.Today.AddDays(15)));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new RelativeDateTimeValidatorAttribute(10, DateTimeUnit.Day, RangeBoundaryType.Inclusive)
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid(DateTime.Today.AddDays(15)));
        }
    }
}
