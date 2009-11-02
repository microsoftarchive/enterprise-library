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
    public class DomainValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullPatternThrows()
        {
            new DomainValidatorAttribute(null);
        }

        [TestMethod]
        public void AttributeWithIntDomainCreatesValidator()
        {
            object[] domain = new object[] { 1, 2, 3 };
            ValidatorAttribute attribute = new DomainValidatorAttribute(domain);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void AttributeWithIntDomainInConstructorCreatesValidator()
        {
            object[] domain = new object[] { 1, 2, 3 };
            ValidatorAttribute attribute = new DomainValidatorAttribute(1, 2, 3);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void AttributeWithIntDomainAndNegatedCreatesValidator()
        {
            object[] domain = new object[] { 1, 2, 3 };

            DomainValidatorAttribute attribute = new DomainValidatorAttribute(domain);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.DomainNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void AttributeWithStringDomainCreatesValidator()
        {
            object[] domain = new object[] { "a", "b", "c" };
            ValidatorAttribute attribute = new DomainValidatorAttribute(domain);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void AttributeWithStringDomainAndNegatedCreatesValidator()
        {
            object[] domain = new object[] { "a", "b", "c" };

            DomainValidatorAttribute attribute = new DomainValidatorAttribute(domain);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(Resources.DomainNegatedDefaultMessageTemplate, typedValidator.MessageTemplate);
            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void AttributeWithStringDomainAndNegatedAndMessageTemplateCreatesValidator()
        {
            object[] domain = new object[] { "a", "b", "c" };

            DomainValidatorAttribute attribute = new DomainValidatorAttribute(domain);
            attribute.Negated = true;
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual("my message template", typedValidator.MessageTemplate);
            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void AttributeWithStringDomainAndMessageTemplateCreatesValidator()
        {
            object[] domain = new object[] { "a", "b", "c" };

            DomainValidatorAttribute attribute = new DomainValidatorAttribute(domain);
            attribute.MessageTemplate = "my message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            DomainValidator<object> typedValidator = validator as DomainValidator<object>;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual("my message template", typedValidator.MessageTemplate);
            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(3, typedValidator.Domain.Count);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new DomainValidatorAttribute(new object[] { "a", "b", "c" })
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid("a"));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new DomainValidatorAttribute(new object[] { "a", "b", "c" })
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid("z"));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new DomainValidatorAttribute(new object[] { "a", "b", "c" })
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid("z"));
        }
    }
}
