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
    [TestClass]
    public class TypeConversionValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullPatternThrows()
        {
            new TypeConversionValidator(null);
        }

        [TestMethod]
        public void ConstructorWithTargetTypeCreatesCorrectInstance()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int));

            Assert.AreEqual(Resources.TypeConversionNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(typeof(int), validator.TargetType);
        }

        [TestMethod]
        public void ConstructorWithTargetTypeAndNegatedCreatesCorrectInstance()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), true);

            Assert.AreEqual(Resources.TypeConversionNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(typeof(int), validator.TargetType);
        }

        [TestMethod]
        public void ConstructorWithTargetTypeAndMessageTemplateCreatesCorrectInstance()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), "my message template");

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(typeof(int), validator.TargetType);
        }

        [TestMethod]
        public void ConstructorWithTargetTypeAndMessageTemplateAndNegatedCreatesCorrectInstance()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), "my message template", true);

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(typeof(int), validator.TargetType);
        }

        [TestMethod]
        public void NegatedRejectsNull()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNull()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), false);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedAcceptsStringWithIntType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), false);

            ValidationResults results = validator.Validate("12");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedRejectsStringWithIntType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), true);

            ValidationResults results = validator.Validate("12");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsStringWithIntType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), false);

            ValidationResults results = validator.Validate("a");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedAcceptsStringWithIntType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), true);

            ValidationResults results = validator.Validate("a");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsEmptyStringWithDateTimeType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(DateTime));

            ValidationResults results = validator.Validate(string.Empty);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsEmptyStringWithIntType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int));

            ValidationResults results = validator.Validate(string.Empty);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedAcceptsEmptyStringWithStringType()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(string));

            ValidationResults results = validator.Validate(string.Empty);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int));
            validator.MessageTemplate = "{0}-{1}-{2}-{3}";
            validator.Tag = "tag";
            object target = "not an int";
            string key = "key";

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, null, key, validationResults);

            Assert.IsFalse(validationResults.IsValid);
            ValidationResult validationResult = ValidationTestHelper.GetResultsList(validationResults)[0];
            Match match = TemplateStringTester.Match(validator.MessageTemplate, validationResult.Message);
            Assert.IsTrue(match.Success);
            Assert.IsTrue(match.Groups["param0"].Success);
            Assert.AreEqual(target.ToString(), match.Groups["param0"].Value);
            Assert.IsTrue(match.Groups["param1"].Success);
            Assert.AreEqual(key, match.Groups["param1"].Value);
            Assert.IsTrue(match.Groups["param2"].Success);
            Assert.AreEqual(validator.Tag, match.Groups["param2"].Value);
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual("System.Int32", match.Groups["param3"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int));
            validator.Tag = "tag";
            object target = "not an int";
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
            Assert.AreEqual("System.Int32", match.Groups["param3"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            TypeConversionValidator validator = new TypeConversionValidator(typeof(int), true);
            validator.Tag = "tag";
            object target = "10";
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
            Assert.AreEqual("System.Int32", match.Groups["param3"].Value);
        }
    }
}
