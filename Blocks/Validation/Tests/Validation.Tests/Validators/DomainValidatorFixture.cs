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
    public class DomainValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullPatternThrows()
        {
            new DomainValidator<object>((List<object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfValidatorWithNullArrayPatternThrows()
        {
            new DomainValidator<object>(true, (object[])null);
        }

        [TestMethod]
        public void CreatingInstanceWithNegated()
        {
            DomainValidator<object> validator = new DomainValidator<object>(true);

            Assert.AreEqual(Resources.DomainNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CreatingInstanceWithNonNegated()
        {
            DomainValidator<object> validator = new DomainValidator<object>(false);

            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
        }

        [TestMethod]
        public void ConstructorWithEmptyDomainCreatesCorrectInstance()
        {
            List<object> domain = new List<object>();
            DomainValidator<object> validator = new DomainValidator<object>(domain);

            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(0, validator.Domain.Count);
        }

        [TestMethod]
        public void ConstructorWithDomainAsGenericListCreatesCorrectInstance()
        {
            List<object> domain = new List<object>(new object[] { 1, 2, 3 });
            DomainValidator<object> validator = new DomainValidator<object>(domain);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(3, validator.Domain.Count);
            foreach (var item in domain)
            {
                Assert.IsTrue(validator.Domain.Contains(item));
            }
        }

        [TestMethod]
        public void ConstructorWithDomainAsGenericListAndNegatedCreatesCorrectInstance()
        {
            List<object> domain = new List<object>(new object[] { 1, 2, 3 });
            DomainValidator<object> validator = new DomainValidator<object>(domain, true);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(Resources.DomainNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(3, validator.Domain.Count);
            foreach (var item in domain)
            {
                Assert.IsTrue(validator.Domain.Contains(item));
            }
        }

        [TestMethod]
        public void ConstructorWithDomainAsArrayCreatesCorrectInstance()
        {
            object[] domain = new object[] { 1, 2, 3 };
            DomainValidator<object> validator = new DomainValidator<object>(false, 1, 2, 3);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(domain.Length, validator.Domain.Count);
            Assert.AreEqual(3, validator.Domain.Count);
        }

        [TestMethod]
        public void ConstructorWithDomainAsArrayAndNegatedCreatesCorrectInstance()
        {
            object[] domain = new object[] { 1, 2, 3 };
            DomainValidator<object> validator = new DomainValidator<object>(true, false, true, false);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(Resources.DomainNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(domain.Length, validator.Domain.Count);
            Assert.AreEqual(3, validator.Domain.Count);
        }

        [TestMethod]
        public void ConstructorWithDomainAsArrayInConstructorCreatesCorrectInstance()
        {
            object[] domain = new object[] { 1, 2, 3 };
            DomainValidator<object> validator = new DomainValidator<object>(false, domain);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(Resources.DomainNonNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(domain.Length, validator.Domain.Count);
            Assert.AreEqual(3, validator.Domain.Count);
        }

        [TestMethod]
        public void ConstructorWithDomainAsArrayInConstructorAndNegatedCreatesCorrectInstance()
        {
            object[] domain = new object[] { 1, 2, 3 };
            DomainValidator<object> validator = new DomainValidator<object>(true, domain);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual(Resources.DomainNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(domain.Length, validator.Domain.Count);
            Assert.AreEqual(3, validator.Domain.Count);
        }

        [TestMethod]
        public void ConstructorWithDomainAsArrayCreatesAndMessageTemplateCorrectInstance()
        {
            object[] domain = new object[] { 1, 2, 3 };
            DomainValidator<object> validator = new DomainValidator<object>("my message template", 1, 2, 3);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(false, validator.Negated);
            Assert.AreEqual(domain.Length, validator.Domain.Count);
            Assert.AreEqual(3, validator.Domain.Count);
        }

        [TestMethod]
        public void ConstructorWithDomainAsListCreatesAndMessageTemplateCorrectInstance()
        {
            List<object> domain = new List<object>(new object[] { 1, 2, 3 });
            DomainValidator<object> validator = new DomainValidator<object>(domain, "my message template", true);

            Assert.IsNotNull(validator.Domain);
            Assert.AreEqual("my message template", validator.MessageTemplate);
            Assert.AreEqual(true, validator.Negated);
            Assert.AreEqual(domain.Count, validator.Domain.Count);
            Assert.AreEqual(3, validator.Domain.Count);
        }

        [TestMethod]
        public void NegatedRejectsNull()
        {
            DomainValidator<object> validator = new DomainValidator<object>(true);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsNull()
        {
            DomainValidator<object> validator = new DomainValidator<object>(false);

            ValidationResults results = validator.Validate(null);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedAcceptsStringIncludedInTheDomain()
        {
            DomainValidator<string> validator = new DomainValidator<string>(false, "a", "b", "c");

            ValidationResults results = validator.Validate("b");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsStringNotIncludedInTheDomain()
        {
            DomainValidator<string> validator = new DomainValidator<string>(false, "a", "b", "c");

            ValidationResults results = validator.Validate("d");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedRejectsStringIncludedInTheDomain()
        {
            DomainValidator<string> validator = new DomainValidator<string>(true, "a", "b", "c");

            ValidationResults results = validator.Validate("b");

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedAcceptsStringNotIncludedInTheDomain()
        {
            DomainValidator<string> validator = new DomainValidator<string>(true, "a", "b", "c");

            ValidationResults results = validator.Validate("d");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedAcceptsIntIncludedInTheDomain()
        {
            DomainValidator<int> validator = new DomainValidator<int>(false, 1, 2, 3);

            ValidationResults results = validator.Validate(2);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void NonNegatedRejectsIntNotIncludedInTheDomain()
        {
            DomainValidator<int> validator = new DomainValidator<int>(false, 1, 2, 3);

            ValidationResults results = validator.Validate(4);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedRejectsIntIncludedInTheDomain()
        {
            DomainValidator<int> validator = new DomainValidator<int>(true, 1, 2, 3);

            ValidationResults results = validator.Validate(2);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void NegatedAcceptsIntNotIncludedInTheDomain()
        {
            DomainValidator<int> validator = new DomainValidator<int>(true, 1, 2, 3);

            ValidationResults results = validator.Validate(4);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void ResourceValuesHaveBeenDefined()
        {
            Assert.IsFalse(string.IsNullOrEmpty(Resources.DomainNegatedDefaultMessageTemplate));
            Assert.IsFalse(string.IsNullOrEmpty(Resources.DomainNonNegatedDefaultMessageTemplate));
        }

        [TestMethod]
        public void SuppliesAppropriateParametersToMessageTemplate()
        {
            DomainValidator<int> validator = new DomainValidator<int>(false, 1, 2, 3, 4);
            validator.MessageTemplate = "{0}-{1}-{2}";
            validator.Tag = "tag";
            object target = 24;
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
        }

        public void SuppliesAppropriateParametersToDefaultMessage()
        {
            DomainValidator<int> validator = new DomainValidator<int>(false, 1, 2, 3, 4);
            validator.Tag = "tag";
            object target = 24;
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
        }

        public void SuppliesAppropriateParametersToDefaultNegatedMessage()
        {
            DomainValidator<int> validator = new DomainValidator<int>(true, 1, 2, 3, 4);
            validator.Tag = "tag";
            object target = 1;
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
        }
    }
}
