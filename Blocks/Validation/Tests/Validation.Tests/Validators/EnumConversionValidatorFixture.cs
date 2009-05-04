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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    enum MockEnumValidator
    {
        MyEnumValue,
        AnotherEnumValue
    }

    [TestClass]
    public class EnumConversionValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullPatternThrows()
        {
            new EnumConversionValidator(null);
        }

        [TestMethod]
        public void CreatingInstanceWithNegated()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), true);

            Assert.AreEqual(Resources.EnumConversionNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(typeof(MockEnumValidator), validator.EnumType);
        }

        [TestMethod]
        public void CreatingInstanceWithMessageTemplate()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), "my message template");

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(typeof(MockEnumValidator), validator.EnumType);
        }

        [TestMethod]
        public void CreatingInstanceWithNonNegated()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), false);

            Assert.AreEqual(Resources.EnumConversionNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(typeof(MockEnumValidator), validator.EnumType);
        }

        [TestMethod]
        public void ConstructorWithEnumTypeCreatesCorrectInstance()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator));

            Assert.AreEqual(typeof(MockEnumValidator), validator.EnumType);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithEnumTypeAndNegatedCreatesCorrectInstance()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), true);

            Assert.AreEqual(typeof(MockEnumValidator), validator.EnumType);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void NegatedRejectsNull()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNull()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), false);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedAcceptsStringWithEnumValue()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), false);

            ValidationResults results = validator.Validate("MyEnumValue");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedRejectsStringWithEnumValue()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), true);

            ValidationResults results = validator.Validate("MyEnumValue");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsStringWithoutEnumValue()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), false);

            ValidationResults results = validator.Validate("xyz");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedAcceptsStringWithoutEnumValue()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), true);

            ValidationResults results = validator.Validate("xyz");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void ResourceValuesHaveBeenDefined()
        {
            Assert.IsFalse(string.IsNullOrEmpty(Resources.EnumConversionNonNegatedDefaultMessageTemplate));
            Assert.IsFalse(string.IsNullOrEmpty(Resources.EnumConversionNegatedDefaultMessageTemplate));
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator));
            validator.MessageTemplate = "{0}|{1}|{2}|{3}";
            validator.Tag = "tag";
            object target = "blah";
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsTrue(match.Groups["param0"].Success);
            Assert.AreEqual(target, match.Groups["param0"].Value);
            Assert.IsTrue(match.Groups["param1"].Success);
            Assert.AreEqual(key, match.Groups["param1"].Value);
            Assert.IsTrue(match.Groups["param2"].Success);
            Assert.AreEqual(validator.Tag, match.Groups["param2"].Value);
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual(typeof(MockEnumValidator).Name, match.Groups["param3"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator));
            validator.Tag = "tag";
            object target = "blah";
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsFalse(match.Groups["param0"].Success);
            Assert.IsFalse(match.Groups["param1"].Success);
            Assert.IsFalse(match.Groups["param2"].Success);
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual(typeof(MockEnumValidator).Name, match.Groups["param3"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            EnumConversionValidator validator = new EnumConversionValidator(typeof(MockEnumValidator), true);
            validator.Tag = "tag";
            object target = MockEnumValidator.MyEnumValue.ToString();
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsFalse(match.Groups["param0"].Success);
            Assert.IsFalse(match.Groups["param1"].Success);
            Assert.IsFalse(match.Groups["param2"].Success);
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual(typeof(MockEnumValidator).Name, match.Groups["param3"].Value);
        }
    }
}
