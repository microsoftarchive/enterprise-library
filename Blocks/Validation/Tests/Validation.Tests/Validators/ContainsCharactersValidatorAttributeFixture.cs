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
    public class ContainsCharactersValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullPatternThrows()
        {
            new ContainsCharactersValidatorAttribute(null);
        }

        [TestMethod]
        public void AttributeWithCharacterSetCreatesValidator()
        {
            ValidatorAttribute attribute = new ContainsCharactersValidatorAttribute("abc");

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            ContainsCharactersValidator typedValidator = validator as ContainsCharactersValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.ContainsCharactersNonNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual("abc", typedValidator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.Any, typedValidator.ContainsCharacters);
            Assert.AreEqual(false, typedValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithCharacterSetAndMessageOverrideCreatesValidator()
        {
            ValidatorAttribute attribute = new ContainsCharactersValidatorAttribute("abc");
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            ContainsCharactersValidator typedValidator = validator as ContainsCharactersValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual("abc", typedValidator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.Any, typedValidator.ContainsCharacters);
            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual("overriden message template", typedValidator.MessageTemplate);
        }

        [TestMethod]
        public void AttributeWithCharacterSetAndContainsCharactersCreatesValidator()
        {
            ValidatorAttribute attribute = new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            ContainsCharactersValidator typedValidator = validator as ContainsCharactersValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.ContainsCharactersNonNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual("abc", typedValidator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, typedValidator.ContainsCharacters);
            Assert.AreEqual(false, typedValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithCharacterSetAndContainsCharactersAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            ContainsCharactersValidator typedValidator = validator as ContainsCharactersValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.ContainsCharactersNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual("abc", typedValidator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, typedValidator.ContainsCharacters);
            Assert.AreEqual(true, typedValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithCharacterSetAndContainsCharactersAndMessageTemplateCreatesValidator()
        {
            ValidatorAttribute attribute = new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All);
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            ContainsCharactersValidator typedValidator = validator as ContainsCharactersValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual("my message template", typedValidator.MessageTemplate);
            Assert.AreEqual("abc", typedValidator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, typedValidator.ContainsCharacters);
            Assert.AreEqual(false, typedValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithCharacterSetAndContainsCharactersAndMessageTemplateAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All);
            attribute.Negated = true;
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            ContainsCharactersValidator typedValidator = validator as ContainsCharactersValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual("my message template", typedValidator.MessageTemplate);
            Assert.AreEqual("abc", typedValidator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, typedValidator.ContainsCharacters);
            Assert.AreEqual(true, typedValidator.Negated);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid("cccaaabbb"));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All)
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid("bcd"));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new ContainsCharactersValidatorAttribute("abc", ContainsCharacters.All)
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid("bcd"));
        }
    }
}
