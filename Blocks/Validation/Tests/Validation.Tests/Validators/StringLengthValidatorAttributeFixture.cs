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

using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class StringLengthValidatorAttributeFixture
    {
        [TestMethod]
        public void AttributeWithUpperBoundOnlyCreatesAppropriateValidator()
        {
            ValidatorAttribute attribute = new StringLengthValidatorAttribute(20);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(RangeBoundaryType.Ignore, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(20, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate, stringLengthValidator.MessageTemplate);
            Assert.AreEqual(false, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithUpperBoundOnlyAndNegatedCreatesAppropriateValidator()
        {
            ValueValidatorAttribute attribute = new StringLengthValidatorAttribute(20);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(RangeBoundaryType.Ignore, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(20, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNegatedDefaultMessageTemplate, stringLengthValidator.MessageTemplate);
            Assert.AreEqual(true, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsOnlyCreatesAppropriateValidator()
        {
            ValidatorAttribute attribute = new StringLengthValidatorAttribute(10, 20);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(20, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate, stringLengthValidator.MessageTemplate);
            Assert.AreEqual(false, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsOnlyAndNegatedCreatesAppropriateValidator()
        {
            ValueValidatorAttribute attribute = new StringLengthValidatorAttribute(10, 20);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(20, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNegatedDefaultMessageTemplate, stringLengthValidator.MessageTemplate);
            Assert.AreEqual(true, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndMessageOverrideCreatesAppropriateValidator()
        {
            ValidatorAttribute attribute = new StringLengthValidatorAttribute(10, 20);
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(20, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.UpperBoundType);
            Assert.AreEqual("overriden message template", stringLengthValidator.MessageTemplate);
            Assert.AreEqual(false, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndMessageOverrideAndNegatedCreatesAppropriateValidator()
        {
            ValueValidatorAttribute attribute = new StringLengthValidatorAttribute(10, 20);
            attribute.MessageTemplate = "overriden message template";
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(20, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, stringLengthValidator.UpperBoundType);
            Assert.AreEqual("overriden message template", stringLengthValidator.MessageTemplate);
            Assert.AreEqual(true, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndBoundTypesCreatesAppropriateValidator()
        {
            ValidatorAttribute attribute = new StringLengthValidatorAttribute(10, RangeBoundaryType.Exclusive, 0, RangeBoundaryType.Ignore);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(0, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, stringLengthValidator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate, stringLengthValidator.MessageTemplate);
            Assert.AreEqual(false, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndBoundTypesAndNegatedCreatesAppropriateValidator()
        {
            ValueValidatorAttribute attribute = new StringLengthValidatorAttribute(10, RangeBoundaryType.Exclusive, 0, RangeBoundaryType.Ignore);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(0, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, stringLengthValidator.UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNegatedDefaultMessageTemplate, stringLengthValidator.MessageTemplate);
            Assert.AreEqual(true, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndBoundTypesAndMessageTemplateCreatesAppropriateValidator()
        {
            ValidatorAttribute attribute = new StringLengthValidatorAttribute(10, RangeBoundaryType.Exclusive, 0, RangeBoundaryType.Ignore);
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(0, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, stringLengthValidator.UpperBoundType);
            Assert.AreEqual("my message template", stringLengthValidator.MessageTemplate);
            Assert.AreEqual(false, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithLowerAndUpperBoundsAndBoundTypesAndMessageTemplateAndNegatedCreatesAppropriateValidator()
        {
            ValueValidatorAttribute attribute = new StringLengthValidatorAttribute(10, RangeBoundaryType.Exclusive, 0, RangeBoundaryType.Ignore);
            attribute.Negated = true;
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            StringLengthValidator stringLengthValidator = validator as StringLengthValidator;
            Assert.IsNotNull(stringLengthValidator);
            Assert.AreEqual(10, stringLengthValidator.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, stringLengthValidator.LowerBoundType);
            Assert.AreEqual(0, stringLengthValidator.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, stringLengthValidator.UpperBoundType);
            Assert.AreEqual("my message template", stringLengthValidator.MessageTemplate);
            Assert.AreEqual(true, stringLengthValidator.Negated);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new StringLengthValidatorAttribute(10)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid("abcdefg"));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new StringLengthValidatorAttribute(10)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid("abcdefghijklm"));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new StringLengthValidatorAttribute(10)
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid("abcdefghijklm"));
        }
    }
}
