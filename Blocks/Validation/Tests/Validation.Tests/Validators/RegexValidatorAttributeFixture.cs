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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class RegexValidatorAttributeFixture
    {
        const string RegexResourceName1 = "Regex1";
        const string RegexResourceName2 = "Regex2";
        const string RegexResourceName3 = "Regex3";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullPatternThrows()
        {
            new RegexValidatorAttribute(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullPatternAndNegatedThrows()
        {
            new RegexValidatorAttribute(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullResourceTypePatternThrows()
        {
            Type resourceType = null;
            new RegexValidatorAttribute(RegexResourceName1, resourceType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullResourceTypeAndNullResourceNamePatternThrows()
        {
            Type resourceType = null;
            new RegexValidatorAttribute(null, resourceType);
        }

        [TestMethod]
        public void AttributeWithPatternCreatesValidatorWithPatternDefaultMessageAndNoneOptions()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute("pattern");

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(null, regexValidator.PatternResourceName);
            Assert.AreEqual(null, regexValidator.PatternResourceType);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternCreatesValidatorWithPatternDefaultMessageAndNoneOptions()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources));

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternCreatesValidatorWithPatternDefaultMessageAndNoneOptionsAndNegated()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute("pattern");
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternAndNegatedCreatesValidatorWithPatternDefaultMessageAndNoneOptions()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources));
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternAndMessageOverrideCreatesValidatorWithPatternOverridenMessageAndNullOptions()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute("pattern");
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternAndMessageTemplateCreatesValidatorWithPatternDefaultMessageAndNoneOptions()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources));
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternAndMessageOverrideCreatesValidatorWithPatternOverridenMessageAndNullOptionsAndNegated()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute("pattern", RegexOptions.None);
            attribute.Negated = true;
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternAndMessageTemplateAndNegatedCreatesValidatorWithPatternDefaultMessageAndNoneOptions()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources));
            attribute.Negated = true;
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.None, regexValidator.Options);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternAndOptionsCreatesValidatorWithPatternDefaultMessageAndOptions()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute("pattern", RegexOptions.Multiline);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options.Value);
            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternAndOptionsCreatesValidator()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Multiline);

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options);
            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternAndOptionsCreatesValidatorWithPatternDefaultMessageAndOptionsAndNegated()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute("pattern", RegexOptions.Multiline);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options.Value);
            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternAndOptionsAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Multiline);
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options);
            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternOptionsAndMessageOverrideCreatesValidatorWithPatternOverridenMessageAndOptions()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute("pattern", RegexOptions.Multiline);
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options.Value);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithResourcePatternAndOptionsAndMessageTemplateCreatesValidator()
        {
            ValidatorAttribute attribute = new RegexValidatorAttribute(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Multiline);
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual(null, regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(RegexResourceName1, regexValidator.PatternResourceName);
            Assert.AreEqual(typeof(Properties.Resources), regexValidator.PatternResourceType);
            Assert.AreEqual(false, regexValidator.Negated);
        }

        [TestMethod]
        public void AttributeWithPatternOptionsAndMessageOverrideCreatesValidatorWithPatternOverridenMessageAndOptionsAndNegated()
        {
            ValueValidatorAttribute attribute = new RegexValidatorAttribute("pattern", RegexOptions.Multiline);
            attribute.Negated = true;
            attribute.MessageTemplate = "overriden message template";

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null, null);
            Assert.IsNotNull(validator);

            RegexValidator regexValidator = validator as RegexValidator;
            Assert.IsNotNull(regexValidator);

            Assert.AreEqual("pattern", regexValidator.Pattern);
            Assert.AreEqual(RegexOptions.Multiline, regexValidator.Options.Value);
            Assert.AreEqual("overriden message template", regexValidator.MessageTemplate);
            Assert.AreEqual(true, regexValidator.Negated);
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttributeForValidValue()
        {
            ValidationAttribute attribute =
                new RegexValidatorAttribute("^a*$")
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsTrue(attribute.IsValid("aaaaaaaaaa"));
        }

        [TestMethod]
        public void CanUseAttributeAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new RegexValidatorAttribute("^a*$")
                {
                    MessageTemplate = "template {1}"
                };

            Assert.IsFalse(attribute.IsValid("bbbbbb"));
            Assert.AreEqual("template name", attribute.FormatErrorMessage("name"));
        }

        [TestMethod]
        public void ValidatingWithValidatorAttributeWithARulesetSkipsValidation()
        {
            ValidationAttribute attribute =
                new RegexValidatorAttribute("^a*$")
                {
                    MessageTemplate = "template {1}",
                    Ruleset = "some ruleset"
                };

            Assert.IsTrue(attribute.IsValid("bbbbbb"));
        }
    }
}
