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
    public class ObjectCollectionValidatorAttributeFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new ConfigurationValidatorFactory(new SystemConfigurationSource(false)));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingNullTargetRulesetThrows()
        {
            new ObjectCollectionValidatorAttribute().TargetRuleset = null;
        }

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

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, ValidationFactory.DefaultCompositeValidatorFactory);
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

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, ValidationFactory.DefaultCompositeValidatorFactory);
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

        [TestMethod]
        public void ValidatesUsingTargetTypeRulesIfTypeIsSpecified()
        {
            object instance = new DerivedObjectCollectionValidatorAttributeFixtureTestClass();
            object target = new object[] { instance };
            ValidatorAttribute validatorAttribute
                = new ObjectCollectionValidatorAttribute(typeof(ObjectCollectionValidatorAttributeFixtureTestClass));

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, ValidationFactory.DefaultCompositeValidatorFactory);
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
        public void ValidatesUsingActualTypeRulesIfTypeIsSpecified()
        {
            object instance = new DerivedObjectCollectionValidatorAttributeFixtureTestClass();
            object target = new object[] { instance };
            ValidatorAttribute validatorAttribute = new ObjectCollectionValidatorAttribute();

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute).CreateValidator(null, null, null, ValidationFactory.DefaultCompositeValidatorFactory);
            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("DerivedObjectCollectionValidatorAttributeFixtureTestClass"));
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass"].Key);
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass"].Tag);
            Assert.AreSame(instance, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass-Property"));
            Assert.AreEqual("Property", resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Tag);
            Assert.AreSame(instance, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"));
            Assert.AreEqual("DerivedProperty", resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"].Key);
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"].Tag);
            Assert.AreSame(instance, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty-DataAnnotations"));
            Assert.AreEqual("DerivedProperty", resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty-DataAnnotations"].Key);
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty-DataAnnotations"].Tag);
            Assert.AreSame(instance, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty-DataAnnotations"].Target);
        }

        [TestMethod]
        public void ValidatesUsingTheSuppliedFactory()
        {
            object instance = new DerivedObjectCollectionValidatorAttributeFixtureTestClass();
            object target = new object[] { instance };
            ValidatorAttribute validatorAttribute = new ObjectCollectionValidatorAttribute();

            Validator validator =
                ((IValidatorDescriptor)validatorAttribute)
                    .CreateValidator(null, null, null, new AttributeValidatorFactory());
            ValidationResults validationResults = validator.Validate(target);

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);

            Assert.IsTrue(resultsMapping.ContainsKey("DerivedObjectCollectionValidatorAttributeFixtureTestClass"));
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass"].Key);
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass"].Tag);
            Assert.AreSame(instance, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("ObjectCollectionValidatorAttributeFixtureTestClass-Property"));
            Assert.AreEqual("Property", resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Key);
            Assert.AreEqual(null, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Tag);
            Assert.AreSame(instance, resultsMapping["ObjectCollectionValidatorAttributeFixtureTestClass-Property"].Target);

            Assert.IsTrue(resultsMapping.ContainsKey("DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"));
            Assert.AreEqual("DerivedProperty", resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"].Key);
            Assert.AreEqual(null, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"].Tag);
            Assert.AreSame(instance, resultsMapping["DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty"].Target);
        }

        [ObjectCollectionValidator(typeof(ObjectCollectionValidatorAttributeFixtureTestClass))]
        public class ObjectCollectionValidatorAttributeFixtureCollectionTestClass : List<ObjectCollectionValidatorAttributeFixtureTestClass> { }

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

        [MockValidator(true, MessageTemplate = "DerivedObjectCollectionValidatorAttributeFixtureTestClass")]
        public class DerivedObjectCollectionValidatorAttributeFixtureTestClass : ObjectCollectionValidatorAttributeFixtureTestClass
        {
            [MockValidator(true, MessageTemplate = "DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty")]
            [MockValidator(true, MessageTemplate = "DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty-RuleB", Ruleset = "RuleB")]
            [StringLength(5, ErrorMessage = "DerivedObjectCollectionValidatorAttributeFixtureTestClass-DerivedProperty-DataAnnotations")]
            public string DerivedProperty
            {
                get { return "some long string"; }
            }
        }
    }
}
