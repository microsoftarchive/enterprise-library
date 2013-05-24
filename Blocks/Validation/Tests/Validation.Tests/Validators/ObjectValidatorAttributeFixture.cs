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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class ObjectValidatorAttributeFixture
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttributeCreatedWithNullTargetRulesetThrows()
        {
            new ObjectValidatorAttribute(null);
        }

        [TestMethod]
        public void AttributeCreatesValidatorWithDefaultRulesetIfNoRulesetIsSupplied()
        {
            ObjectValidatorAttributeFixtureTestClass target = new ObjectValidatorAttributeFixtureTestClass();
            ValidatorAttribute validatorAttribute = new ObjectValidatorAttribute();

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute)
                    .CreateValidator(
                        typeof(ObjectValidatorAttributeFixtureTestClass),
                        null,
                        null,
                        ValidationFactory.DefaultCompositeValidatorFactory);
            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectValidatorAttributeFixtureTestClass"));
            Assert.AreEqual(null, resultsMapping["ObjectValidatorAttributeFixtureTestClass"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectValidatorAttributeFixtureTestClass"].Tag);
            Assert.AreSame(target, resultsMapping["ObjectValidatorAttributeFixtureTestClass"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectValidatorAttributeFixtureTestClass-Property"));
            Assert.AreEqual("Property", resultsMapping["ObjectValidatorAttributeFixtureTestClass-Property"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectValidatorAttributeFixtureTestClass-Property"].Tag);
            Assert.AreSame(target, resultsMapping["ObjectValidatorAttributeFixtureTestClass-Property"].Target);
        }

        [TestMethod]
        public void AttributeCreatesValidatorWithSuppliedRulesetIfRulesetIsSupplied()
        {
            ObjectValidatorAttributeFixtureTestClass target = new ObjectValidatorAttributeFixtureTestClass();
            ValidatorAttribute validatorAttribute = new ObjectValidatorAttribute("RuleB");

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute)
                    .CreateValidator(
                        typeof(ObjectValidatorAttributeFixtureTestClass),
                        null,
                        null,
                        ValidationFactory.DefaultCompositeValidatorFactory);
            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectValidatorAttributeFixtureTestClass-Property-RuleB"));
            Assert.AreEqual("Property", resultsMapping["ObjectValidatorAttributeFixtureTestClass-Property-RuleB"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectValidatorAttributeFixtureTestClass-Property-RuleB"].Tag);
            Assert.AreSame(target, resultsMapping["ObjectValidatorAttributeFixtureTestClass-Property-RuleB"].Target);
        }

        [MockValidator(true, MessageTemplate = "ObjectValidatorAttributeFixtureTestClass")]
        public class ObjectValidatorAttributeFixtureTestClass
        {
            [MockValidator(true, MessageTemplate = "ObjectValidatorAttributeFixtureTestClass-Property")]
            [MockValidator(true, MessageTemplate = "ObjectValidatorAttributeFixtureTestClass-Property-RuleB", Ruleset = "RuleB")]
            public string Property
            {
                get { return null; }
            }
        }

        [TestMethod]
        public void AttributeWithNullRulesetCannotBeUsedAsValidationAttribute()
        {
            ValidationAttribute attribute = new ObjectValidatorAttribute();

            try
            {
                attribute.IsValid("");
                Assert.Fail("should have thrown");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void AttributeWithNonNullRulesetReturnsValid()
        {
            ValidationAttribute attribute = new ObjectValidatorAttribute() { Ruleset = "some ruleset" };

            Assert.IsTrue(attribute.IsValid(""));
        }
    }
}
