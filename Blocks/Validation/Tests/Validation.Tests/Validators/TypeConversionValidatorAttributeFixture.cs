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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class TypeConversionValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullPatternThrows()
        {
            new TypeConversionValidatorAttribute(null);
        }

        [TestMethod]
        public void AttributeWithTargetTypeCreatesValidator()
        {
            ValidatorAttribute attribute = new TypeConversionValidatorAttribute(typeof(double));

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            TypeConversionValidator typedValidator = validator as TypeConversionValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(typeof(double), typedValidator.TargetType);
        }

        [TestMethod]
        public void AttributeWithTargetTypeAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new TypeConversionValidatorAttribute(typeof(double));
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            TypeConversionValidator typedValidator = validator as TypeConversionValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual(typeof(double), typedValidator.TargetType);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new TypeConversionValidatorAttribute(typeof(double))
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid((2.0d).ToString(CultureInfo.CurrentCulture)));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new TypeConversionValidatorAttribute(typeof(double))
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
                new TypeConversionValidatorAttribute(typeof(double))
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid("abcdefghijklm"));
        }
    }
}
