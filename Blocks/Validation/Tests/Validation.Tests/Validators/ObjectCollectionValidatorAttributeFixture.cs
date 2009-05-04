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
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class ObjectCollectionValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttributeCreatedWithNullTargetTypeThrows()
        {
            new ObjectCollectionValidatorAttribute(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttributeCreatedWithNullTargetRulesetThrows()
        {
            new ObjectCollectionValidatorAttribute(typeof(ObjectCollectionValidatorAttributeFixtureTestClass), null);
        }

        [TestMethod]
        public void AttributeCreatesValidatorWithDefaultRulesetIfNoRulesetIsSupplied()
        {
            object instance = new ObjectCollectionValidatorAttributeFixtureTestClass();
            object target = new object[] { instance };
            ValidatorAttribute validatorAttribute
                = new ObjectCollectionValidatorAttribute(typeof(ObjectCollectionValidatorAttributeFixtureTestClass));

            Validator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null);
            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass"));
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass"].Tag);
            Assert.AreSame(instance, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass-Property"));
            Assert.AreEqual("Property", resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Tag);
            Assert.AreSame(instance, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Target);
        }

        [TestMethod]
        public void AttributeCreatesValidatorWithSuppliedRulesetIfRulesetIsSupplied()
        {
            object instance = new ObjectCollectionValidatorAttributeFixtureTestClass();
            object target = new object[] { instance };
            ValidatorAttribute validatorAttribute = new ObjectCollectionValidatorAttribute(typeof(ObjectCollectionValidatorAttributeFixtureTestClass), "RuleB");

            Validator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null);
            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass-Property-RuleB"));
            Assert.AreEqual("Property", resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property-RuleB"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property-RuleB"].Tag);
            Assert.AreSame(instance, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property-RuleB"].Target);
        }

        [TestMethod]
        public void CanApplyAttributeToCollectionClass()
        {
            ObjectCollectionValidatorAttributeFixtureCollectionTestClass collection = new ObjectCollectionValidatorAttributeFixtureCollectionTestClass();
            ObjectCollectionValidatorAttributeFixtureTestClass element = new ObjectCollectionValidatorAttributeFixtureTestClass();
            collection.Add(element);

            Validator validator = ValidationFactory.CreateValidator<ObjectCollectionValidatorAttributeFixtureCollectionTestClass>();
            ValidationResults validationResults = validator.Validate(collection);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass"));
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass"].Tag);
            Assert.AreSame(element, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass-Property"));
            Assert.AreEqual("Property", resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Tag);
            Assert.AreSame(element, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Target);
        }

        [ObjectCollectionValidator(typeof(ObjectCollectionValidatorAttributeFixtureTestClass))]
        public class ObjectCollectionValidatorAttributeFixtureCollectionTestClass : List<ObjectCollectionValidatorAttributeFixtureTestClass> {}

        [MockValidator(true, MessageTemplate = "ObjectCollectionValidatorAttributeFixtureTestClass")]
        public class ObjectCollectionValidatorAttributeFixtureTestClass
        {
            [MockValidator(true, MessageTemplate = "ObjectCollectionValidatorAttributeFixtureTestClass-Property")]
            [MockValidator(true, MessageTemplate = "ObjectCollectionValidatorAttributeFixtureTestClass-Property-RuleB", Ruleset = "RuleB")]
            public string Property
            {
                get { return null; }
            }
        }
    }
}
