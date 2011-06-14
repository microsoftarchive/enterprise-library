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

        [TestMethod]
        public void AttributeWithDomainInCtorReturnSameDomainObjects()
        {
            Assert.AreEqual(1, new DomainValidatorAttribute(1).Domain1);
            Assert.AreEqual(2, new DomainValidatorAttribute(1, 2).Domain2);
            Assert.AreEqual(3, new DomainValidatorAttribute(1, 2, 3).Domain3);
            Assert.AreEqual(4, new DomainValidatorAttribute(1, 2, 3, 4).Domain4);
            Assert.AreEqual(5, new DomainValidatorAttribute(1, 2, 3, 4, 5).Domain5);
            Assert.AreEqual(6, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6).Domain6);
            Assert.AreEqual(7, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7).Domain7);
            Assert.AreEqual(8, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8).Domain8);
            Assert.AreEqual(9, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9).Domain9);
            Assert.AreEqual(10, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Domain10);
            Assert.AreEqual(11, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Domain11);
            Assert.AreEqual(12, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Domain12);
            Assert.AreEqual(13, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Domain13);
            Assert.AreEqual(14, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Domain14);
            Assert.AreEqual(15, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Domain15);
            Assert.AreEqual(16, new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Domain16);
        }

        [TestMethod]
        public void AttributeWithDomainInCtorReturnOtherObjectAsNull()
        {
            Assert.IsNull(new DomainValidatorAttribute(1).Domain2);
            Assert.IsNull(new DomainValidatorAttribute(1, 2).Domain3);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3).Domain4);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4).Domain5);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5).Domain6);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6).Domain7);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7).Domain8);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8).Domain9);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9).Domain10);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Domain11);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Domain12);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Domain13);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Domain14);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Domain15);
            Assert.IsNull(new DomainValidatorAttribute(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Domain16);
        }
    }
}
