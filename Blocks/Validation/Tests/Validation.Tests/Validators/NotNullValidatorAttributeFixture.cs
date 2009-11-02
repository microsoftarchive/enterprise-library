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
    public class NotNullValidatorAttributeFixture
    {
        [TestMethod]
        public void CanCreateNonNegatedValidatorWithAttribute()
        {
            ValidatorAttribute validatorAttribute = new NotNullValidatorAttribute();

            Validator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual(Resources.NonNullNonNegatedValidatorDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, ((NotNullValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateNegatedValidatorWithAttribute()
        {
            ValueValidatorAttribute validatorAttribute = new NotNullValidatorAttribute();
            validatorAttribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual(Resources.NonNullNegatedValidatorDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, ((NotNullValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateValidatorWithAttributeAndMessageOverride()
        {
            string messageTemplateOverride = "overriden message template";

            ValueValidatorAttribute validatorAttribute = new NotNullValidatorAttribute();
            validatorAttribute.MessageTemplate = messageTemplateOverride;

            Validator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual(messageTemplateOverride, validator.MessageTemplate);
            Assert.AreEqual(false, ((NotNullValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateValidatorWithAttributeAndMessageOverrideAndNegated()
        {
            string messageTemplateOverride = "overriden message template";

            ValueValidatorAttribute validatorAttribute = new NotNullValidatorAttribute();
            validatorAttribute.Negated = true;
            validatorAttribute.MessageTemplate = messageTemplateOverride;

            Validator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual(messageTemplateOverride, validator.MessageTemplate);
            Assert.AreEqual(true, ((NotNullValidator)validator).Negated);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new NotNullValidatorAttribute()
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid(new object()));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new NotNullValidatorAttribute()
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid(null));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new NotNullValidatorAttribute()
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid(null));
        }
    }
}
