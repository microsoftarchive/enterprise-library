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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class GivenAValidationAttributeValidatorConfiguredWithNoAttributes
    {
        private Validator validator;

        [TestInitialize]
        public void Setup()
        {
            this.validator = new ValidationAttributeValidator();
        }

        [TestMethod]
        public void WhenValidatingValue_ThenRetunsValidResult()
        {
            var results = this.validator.Validate(new object());

            Assert.IsTrue(results.IsValid);
        }
    }

    [TestClass]
    public class GivenValidationAttributeValidatorConfiguredWithRangeAttributeForIntBoundaries
    {
        private Validator validator;

        [TestInitialize]
        public void Setup()
        {
            var attribute = new RangeAttribute(5, 10);
            attribute.ErrorMessage = "test message";

            this.validator = new ValidationAttributeValidator(attribute);
        }

        [TestMethod]
        public void WhenValidatingValueWithinBounds_ThenRetunsValidResult()
        {
            var results = this.validator.Validate(7);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void WhenValidatingValueOutOfBounds_ThenRetunsInvalidResult()
        {
            var results = this.validator.Validate(2);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual("test message", results.ElementAt(0).Message);
        }

        [TestMethod]
        public void WhenValidatingAValueOfAnNonCompatibleType_ThenReturnsInvalidResult()
        {
            var results = this.validator.Validate("a string");

            Assert.IsFalse(results.IsValid);
        }
    }

    [TestClass]
    public class GivenValidationAttributeValidatorConfiguredWithTwoStringValidationAttributes
    {
        private Validator validator;

        [TestInitialize]
        public void Setup()
        {
            var attribute1 = new StringLengthAttribute(5);
            attribute1.ErrorMessage = "length";
            var attribute2 = new RegularExpressionAttribute("a*");
            attribute2.ErrorMessage = "regex";

            this.validator = new ValidationAttributeValidator(attribute1, attribute2);
        }

        [TestMethod]
        public void WhenValidatingAValidValue_ThenRetunsValidResult()
        {
            var results = this.validator.Validate("aaa");

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void WhenValidatingAValueInvalidForFirstValidator_ThenRetunsInvalidResultWithSingleResult()
        {
            var results = this.validator.Validate("aaaaaaaaa");

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("length", results.ElementAt(0).Message);
        }

        [TestMethod]
        public void WhenValidatingAValueInvalidForSecondValidator_ThenRetunsInvalidResultWithSingleResult()
        {
            var results = this.validator.Validate("bbb");

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("regex", results.ElementAt(0).Message);
        }

        [TestMethod]
        public void WhenValidatingAValueInvalidForBothValidators_ThenRetunsInvalidResultWithTwoResults()
        {
            var results = this.validator.Validate("bbbbbbb");

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("length", results.ElementAt(0).Message);
            Assert.AreEqual("regex", results.ElementAt(1).Message);
        }

        [TestMethod]
        public void WhenValidatingAValueOfAnNonCompatibleType_ThenReturnsInvalidResultsForEachValidator()
        {
            var results = this.validator.Validate(1);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
        }
    }

    [TestClass]
    public class GivenANullValidationAttributeEnumerable
    {
        private IEnumerable<ValidationAttribute> attributes = null;

        [TestMethod]
        public void WhenCreatingAValidationAttributeValidator_ThenAnExceptionIsThrown()
        {
            try
            {
                new ValidationAttributeValidator(attributes);
                Assert.Fail("should have thrown ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }
    }

    [TestClass]
    public class GivenANullValidationAttribute
    {
        private ValidationAttribute attribute = null;

        [TestMethod]
        public void WhenCreatingAValidationAttributeValidator_ThenAnExceptionIsThrown()
        {
            try
            {
                new ValidationAttributeValidator(attribute);
                Assert.Fail("should have thrown ArgumentException");
            }
            catch (ArgumentException)
            {
                // expected
            }
        }
    }
}
