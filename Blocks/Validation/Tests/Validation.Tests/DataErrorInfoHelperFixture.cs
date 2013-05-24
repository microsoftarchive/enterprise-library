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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class DataErrorInfoHelperFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void MessageForEntireObjectIsEmpty()
        {
            var instance = new ValidatedClass();
            var helper = new DataErrorInfoHelper(instance);

            var message = helper.Error;

            Assert.AreEqual("", message);
        }

        [TestMethod]
        public void MessageForNullPropertyIsEmptyString()
        {
            var instance = new ValidatedClass();
            var helper = new DataErrorInfoHelper(instance);

            var message = helper[null];

            Assert.AreEqual("", message);
        }

        [TestMethod]
        public void MessageForNonExistingPropertyIsEmptyString()
        {
            var instance = new ValidatedClass();
            var helper = new DataErrorInfoHelper(instance);

            var message = helper["NonExisting"];

            Assert.AreEqual("", message);
        }

        [TestMethod]
        public void MessageForNonValidatedPropertyIsEmptyString()
        {
            var instance = new ValidatedClass();
            var helper = new DataErrorInfoHelper(instance);

            var message = helper["IntProperty"];

            Assert.AreEqual("", message);
        }

        [TestMethod]
        public void MessageForValidPropertyIsEmptyString()
        {
            var instance = new ValidatedClass { ValidatedIntProperty = 17 };
            var helper = new DataErrorInfoHelper(instance);

            var message = helper["ValidatedIntProperty"];

            Assert.AreEqual("", message);
        }

        [TestMethod]
        public void MessageForInvalidPropertyAccordingToOneRuleContainsTheMessageInTheRule()
        {
            var instance = new ValidatedClass { ValidatedIntProperty = 10 };
            var helper = new DataErrorInfoHelper(instance);

            var message = helper["ValidatedIntProperty"];

            Assert.IsFalse(message.Contains("invalid1"));
            Assert.IsTrue(message.Contains("invalid2"));
        }

        [TestMethod]
        public void MessageForInvalidPropertyAccordingToMultipleRulesContainsTheMessageInTheRules()
        {
            var instance = new ValidatedClass { ValidatedIntProperty = 5 };
            var helper = new DataErrorInfoHelper(instance);

            var message = helper["ValidatedIntProperty"];

            Assert.IsTrue(message.Contains("invalid1"));
            Assert.IsTrue(message.Contains("invalid2"));
        }

        [TestMethod]
        public void MessageForInvalidPropertyAccordingToRuleInRulesetContainsTheMessageInTheRule()
        {
            var instance = new ValidatedClass { ValidatedIntProperty = 5 };
            var helper = new DataErrorInfoHelper(instance, ValidationSpecificationSource.All, "ruleset");

            var message = helper["ValidatedIntProperty"];

            Assert.IsFalse(message.Contains("invalid1"));
            Assert.IsFalse(message.Contains("invalid2"));
            Assert.IsTrue(message.Contains("invalid-ruleset"));
        }
    }


    public class ValidatedClass
    {
        [RangeValidator(10, RangeBoundaryType.Inclusive, 20, RangeBoundaryType.Exclusive, MessageTemplate = "invalid1")]
        [RangeValidator(15, RangeBoundaryType.Inclusive, 25, RangeBoundaryType.Exclusive, MessageTemplate = "invalid2")]
        [RangeValidator(30, RangeBoundaryType.Inclusive, 40, RangeBoundaryType.Exclusive, MessageTemplate = "invalid-ruleset", Ruleset = "ruleset")]
        public int ValidatedIntProperty { get; set; }

        public int IntProperty { get; set; }
    }
}
