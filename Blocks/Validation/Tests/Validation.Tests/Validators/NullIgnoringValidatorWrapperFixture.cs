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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class NullIgnoringValidatorWrapperFixture
    {
        [TestMethod]
        public void WrapperIgnoresNullValues()
        {
            var validator = new TestValidator();
            var wrapperValidator = new NullIgnoringValidatorWrapper(validator);
            object value = null;

            var rawResults = validator.Validate(value);
            var wrapperResults = wrapperValidator.Validate(value);

            Assert.IsFalse(rawResults.IsValid);
            Assert.AreEqual("Invalid value", rawResults.ElementAt(0).Message);
            Assert.IsTrue(wrapperResults.IsValid);
        }

        [TestMethod]
        public void WrapperFailsIfNotNullValueFailsForWrappedValidator()
        {
            var validator = new TestValidator();
            var wrapperValidator = new NullIgnoringValidatorWrapper(validator);
            object value = "some value";

            var rawResults = validator.Validate(value);
            var wrapperResults = wrapperValidator.Validate(value);

            Assert.IsFalse(rawResults.IsValid);
            Assert.AreEqual("Invalid value", rawResults.ElementAt(0).Message);
            Assert.IsFalse(wrapperResults.IsValid);
            Assert.AreEqual("Invalid value", wrapperResults.ElementAt(0).Message);
        }

        [TestMethod]
        public void WrapperSucceedsIfNotNullValueSucceedsForWrappedValidator()
        {
            var validator = new TestValidator();
            var wrapperValidator = new NullIgnoringValidatorWrapper(validator);
            object value = "test value";

            var rawResults = validator.Validate(value);
            var wrapperResults = wrapperValidator.Validate(value);

            Assert.IsTrue(rawResults.IsValid);
            Assert.IsTrue(wrapperResults.IsValid);
        }

        public class TestValidator : ValueValidator
        {
            public TestValidator()
                : base("", "", false)
            { }

            protected override string DefaultNonNegatedMessageTemplate
            {
                get { return ""; }
            }

            protected override string DefaultNegatedMessageTemplate
            {
                get { return ""; }
            }

            public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
            {
                if ((string)objectToValidate != "test value")
                {
                    LogValidationResult(validationResults, "Invalid value", currentTarget, key);
                }
            }
        }


    }
}
