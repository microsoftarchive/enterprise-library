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
    public class ContainsCharactersValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullPatternThrows()
        {
            new ContainsCharactersValidator(null);
        }

        [TestMethod]
        public void CreatingInstanceWithNegated()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, true);

            Assert.AreEqual(Resources.ContainsCharactersNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, validator.ContainsCharacters);
        }

        [TestMethod]
        public void CreatingInstanceWithMessageTemplate()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, "my message template");

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, validator.ContainsCharacters);
        }

        [TestMethod]
        public void CreatingInstanceWithMessageTemplateAndNegated()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, "my message template", true);

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, validator.ContainsCharacters);
        }

        [TestMethod]
        public void CreatingInstanceWithNonNegated()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, false);

            Assert.AreEqual(Resources.ContainsCharactersNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, validator.ContainsCharacters);
        }

        [TestMethod]
        public void ConstructorWithCharacterSetCreatesCorrectInstance()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc");

            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(ContainsCharacters.Any, validator.ContainsCharacters);
        }

        [TestMethod]
        public void ConstructorWithCharacterSetAndContainsCharacterCreatesCorrectInstance()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All);

            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(ContainsCharacters.All, validator.ContainsCharacters);
        }

        [TestMethod]
        public void ConstructorWithCharacterSetAndContainsCharacterAndNegatedCreatesCorrectInstance()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, true);

            Assert.AreEqual("abc", validator.CharacterSet);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(ContainsCharacters.Any, validator.ContainsCharacters);
        }

        [TestMethod]
        public void NegatedRejectsNull()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNull()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, false);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedWithAnyCharactersAcceptsStringWithSomeCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, false);

            ValidationResults results = validator.Validate("abdab");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedWithAnyCharactersRejectsStringWithoutAnyCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, false);

            ValidationResults results = validator.Validate("hgty");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedWithAnyCharactersRejectsStringWithSomeCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, true);

            ValidationResults results = validator.Validate("abdab");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedWithAnyCharactersAcceptsStringWithoutAnyCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.Any, true);

            ValidationResults results = validator.Validate("hgty");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedWithAllCharactersRejectsStringWithSomeCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, false);

            ValidationResults results = validator.Validate("abdab");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedWithAllCharactersAcceptsStringWithAllCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, false);

            ValidationResults results = validator.Validate("asdfbc");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedWithAllCharactersAcceptsStringWithSomeCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, true);

            ValidationResults results = validator.Validate("abdab");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NegatedWithAllCharactersRejectsStringWithAllCharacters()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, true);

            ValidationResults results = validator.Validate("asdfbc");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void ResourceValuesHaveBeenDefined()
        {
            Assert.IsFalse(string.IsNullOrEmpty(Resources.ContainsCharactersNegatedDefaultMessageTemplate));
            Assert.IsFalse(string.IsNullOrEmpty(Resources.ContainsCharactersNonNegatedDefaultMessageTemplate));
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All);
            validator.MessageTemplate = "{0}|{1}|{2}|{3}|{4}";
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
            Assert.AreEqual("abc", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("All", match.Groups["param4"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All);
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
            Assert.AreEqual("abc", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("All", match.Groups["param4"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            ContainsCharactersValidator validator = new ContainsCharactersValidator("abc", ContainsCharacters.All, true);
            validator.Tag = "tag";
            object target = "blahc";
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
            Assert.AreEqual("abc", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual("All", match.Groups["param4"].Value);
        }
    }
}
