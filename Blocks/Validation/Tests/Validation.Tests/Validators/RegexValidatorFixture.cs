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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class RegexValidatorFixture
    {
        const string RegexPattern = "^a+$";
        const string RegexResourceName1 = "Regex1";
        const string RegexResourceName2 = "Regex2";
        const string RegexResourceName3 = "Regex3";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullPatternThrows()
        {
            new RegexValidator(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullResourceNamePatternThrows()
        {
            new RegexValidator(null, typeof(Resources));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullResourceTypePatternThrows()
        {
            Type resourceType = null;
            new RegexValidator(RegexResourceName1, resourceType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullResourceTypeAndNullResourceNamePatternThrows()
        {
            Type resourceType = null;
            new RegexValidator(null, resourceType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullPatternAndNegatedThrows()
        {
            new RegexValidator(null, true);
        }

        [TestMethod]
        public void ReturnFailureForNullString()
        {
            Validator<string> validator = new RegexValidator(RegexPattern);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void ReturnFailureForNullStringUsingResourcePattern()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources));

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void NegatedReturnFailureForNullString()
        {
            Validator<string> validator = new RegexValidator(RegexPattern, true);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RegexValidatorNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void NegatedReturnFailureForNullStringUsingResourcePattern()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), true);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RegexValidatorNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void ReturnFailureForNullStringThroughNonGenericInterface()
        {
            Validator validator = new RegexValidator(RegexPattern);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForNullStringThroughNonGenericInterfaceUsingResourcePattern()
        {
            Validator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources));

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnFailureForNullStringThroughNonGenericInterface()
        {
            Validator validator = new RegexValidator(RegexPattern, true);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnFailureForNullStringThroughNonGenericInterfaceUsingResourcePattern()
        {
            Validator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), true);

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForEmptyStringIfRegexDoesNotAllowThem()
        {
            Validator<string> validator = new RegexValidator(RegexPattern);

            ValidationResults validationResults = validator.Validate("");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForEmptyStringIfResourceRegexDoesNotAllowThem()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources));

            ValidationResults validationResults = validator.Validate("");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnSuccessForEmptyStringIfRegexDoesNotAllowThem()
        {
            Validator<string> validator = new RegexValidator(RegexPattern, true);

            ValidationResults validationResults = validator.Validate("");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnSuccessForEmptyStringIfResourceRegexDoesNotAllowThem()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), true);

            ValidationResults validationResults = validator.Validate("");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnSuccessForEmptyStringIfRegexDoesAllowThem()
        {
            Validator<string> validator = new RegexValidator("^a*$");

            ValidationResults validationResults = validator.Validate("");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnSuccessForEmptyStringIfResourceRegexDoesAllowThem()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName2, typeof(Properties.Resources));

            ValidationResults validationResults = validator.Validate("");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnFailureForEmptyStringIfRegexDoesAllowThem()
        {
            Validator<string> validator = new RegexValidator("^a*$", true);

            ValidationResults validationResults = validator.Validate("");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnFailureForEmptyStringIfResourceRegexDoesAllowThem()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName2, typeof(Properties.Resources), true);

            ValidationResults validationResults = validator.Validate("");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForMatchingString()
        {
            Validator<string> validator = new RegexValidator(RegexPattern);

            ValidationResults validationResults = validator.Validate("aaaaaaaa");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnsSuccessForMatchingStringUsingResourceRegex()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources));

            ValidationResults validationResults = validator.Validate("aaaaaaaa");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForMatchingString()
        {
            Validator<string> validator = new RegexValidator(RegexPattern, true);

            ValidationResults validationResults = validator.Validate("aaaaaaaa");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnsFailureForMatchingStringUsingResourceRegex()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), true);

            ValidationResults validationResults = validator.Validate("aaaaaaaa");

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForNonMatchingString()
        {
            Validator<string> validator = new RegexValidator(RegexPattern);

            ValidationResults validationResults = validator.Validate("asdfg");

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void ReturnFailureForNonMatchingStringUsingResourceRegex()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources));

            ValidationResults validationResults = validator.Validate("asdfg");

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, resultsList[0].Message));
        }

        [TestMethod]
        public void NegatedReturnSuccessForNonMatchingString()
        {
            Validator<string> validator = new RegexValidator(RegexPattern, true);

            ValidationResults validationResults = validator.Validate("asdfg");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void NegatedReturnSuccessForNonMatchingStringUsingResourceRegex()
        {
            Validator<string> validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), true);

            ValidationResults validationResults = validator.Validate("asdfg");

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ValidatorUsesOptionsIfSpecified()
        {
            Validator<string> validatorWithoutSinglelineOption = new RegexValidator("^.*$");
            Validator<string> validatorWithSinglelineOption = new RegexValidator("^.*$", RegexOptions.Singleline);

            Assert.IsFalse(validatorWithoutSinglelineOption.Validate("first line\nlast line").IsValid);
            Assert.IsTrue(validatorWithSinglelineOption.Validate("first line\nlast line").IsValid);
        }

        [TestMethod]
        public void ValidatorUsesOptionsIfSpecifiedUsingResourceRegex()
        {
            Validator<string> validatorWithoutSinglelineOption = new RegexValidator(RegexResourceName3, typeof(Properties.Resources));
            Validator<string> validatorWithSinglelineOption = new RegexValidator(RegexResourceName3, typeof(Properties.Resources), RegexOptions.Singleline);

            Assert.IsFalse(validatorWithoutSinglelineOption.Validate("first line\nlast line").IsValid);
            Assert.IsTrue(validatorWithSinglelineOption.Validate("first line\nlast line").IsValid);
        }

        [TestMethod]
        public void NegatedValidatorUsesOptionsIfSpecified()
        {
            Validator<string> validatorWithoutSinglelineOption = new RegexValidator("^.*$", true);
            Validator<string> validatorWithSinglelineOption = new RegexValidator("^.*$", RegexOptions.Singleline, true);

            Assert.IsTrue(validatorWithoutSinglelineOption.Validate("first line\nlast line").IsValid);
            Assert.IsFalse(validatorWithSinglelineOption.Validate("first line\nlast line").IsValid);
        }

        [TestMethod]
        public void NegatedValidatorUsesOptionsIfSpecifiedUsingResourceRegex()
        {
            Validator<string> validatorWithoutSinglelineOption = new RegexValidator(RegexResourceName3, typeof(Properties.Resources), true);
            Validator<string> validatorWithSinglelineOption = new RegexValidator(RegexResourceName3, typeof(Properties.Resources), RegexOptions.Singleline, true);

            Assert.IsTrue(validatorWithoutSinglelineOption.Validate("first line\nlast line").IsValid);
            Assert.IsFalse(validatorWithSinglelineOption.Validate("first line\nlast line").IsValid);
        }

        [TestMethod]
        public void CreatedInstanceWithoutMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", RegexOptions.Singleline);

            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void CreatedResourceInstanceWithoutMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Singleline);

            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void CreatedInstanceWithMessageTemplateHasCustomTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", "my message template");

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void CreatedResourceInstanceWithMessageTemplateHasCustomTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), "my message template");

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void CreatedInstanceWithMessageTemplateAndNegatedHasCustomTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", "my message template", true);

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatedResourceInstanceWithMessageTemplateAndNegatedHasCustomTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), "my message template", true);

            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void NegatedCreatedInstanceWithoutMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", RegexOptions.Singleline, true);

            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void NegatedCreatedResourceInstanceWithoutMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Singleline, true);

            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatedInstanceWithMessageTemplateHasProvidedTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", RegexOptions.Singleline, "message template override");

            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void CreatedResourceInstanceWithMessageTemplateHasProvidedTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Singleline, "message template override");

            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void NegatedCreatedInstanceWithMessageTemplateHasProvidedTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", RegexOptions.Singleline, "message template override", true);

            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatedInstanceWithNullMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", RegexOptions.Singleline, null);

            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void NegatedCreatedInstanceWithNullMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator("^.*$", RegexOptions.Singleline, null, true);

            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void NegatedCreatedResourceInstanceWithMessageTemplateHasProvidedTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Singleline, "message template override", true);

            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatedResourceInstanceWithNullMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Singleline, null);

            Assert.AreEqual(Resources.RegexValidatorNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void NegatedCreatedResourceInstanceWithNullMessageTemplateHasDefaultTemplate()
        {
            RegexValidator validator = new RegexValidator(RegexResourceName1, typeof(Properties.Resources), RegexOptions.Singleline, null, true);

            Assert.AreEqual(Resources.RegexValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            RegexValidator validator = new RegexValidator("^[ab]+$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            validator.MessageTemplate = "{0}|{1}|{2}|{3}|{4}";
            validator.Tag = "tag";
            object target = "abc";
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
            Assert.AreEqual("^[ab]+$", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual((RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToString(), match.Groups["param4"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            RegexValidator validator = new RegexValidator("^[ab]+$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            validator.Tag = "tag";
            object target = "abc";
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
            Assert.AreEqual("^[ab]+$", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual((RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToString(), match.Groups["param4"].Value);
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            RegexValidator validator = new RegexValidator("^[ab]+$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace, true);
            validator.Tag = "tag";
            object target = "ab";
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
            Assert.AreEqual("^[ab]+$", match.Groups["param3"].Value);
            Assert.IsTrue(match.Groups["param4"].Success);
            Assert.AreEqual((RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToString(), match.Groups["param4"].Value);
        }
    }
}
