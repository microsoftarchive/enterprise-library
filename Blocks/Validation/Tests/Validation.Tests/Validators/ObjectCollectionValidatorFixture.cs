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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class ObjectCollectionValidatorFixture
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
        public void ValidatorCreatedWithNullTypeThrows()
        {
            new ObjectCollectionValidator((Type)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatorCreatedWithNullRulesetThrows()
        {
            new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass), null);
        }

        [TestMethod]
        public void ValidationOfNullLogsNoError()
        {
            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = validator.Validate(null);

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ValidationOfNonCollectionLogsError()
        {
            object target = 10;
            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey(Resources.ObjectCollectionValidatorTargetNotCollection));
            Assert.AreEqual("key", resultsMapping[Resources.ObjectCollectionValidatorTargetNotCollection].Key);
            Assert.AreEqual(null, resultsMapping[Resources.ObjectCollectionValidatorTargetNotCollection].Tag);
            Assert.AreSame(this, resultsMapping[Resources.ObjectCollectionValidatorTargetNotCollection].Target);
            Assert.AreSame(validator, resultsMapping[Resources.ObjectCollectionValidatorTargetNotCollection].Validator);
        }

        [TestMethod]
        public void ValidationOfCollectionWithNullLogsNoError()
        {
            object target = new ObjectCollectionValidatorFixtureReferencedTestClass[] { null };
            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ValidationOfCollectionWithObjectsOfNonCompatibleTypeLogsError()
        {
            object target = new object[] { 10 };
            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey(Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection));
            Assert.AreEqual(null, resultsMapping[Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection].Key);
            Assert.AreEqual(null, resultsMapping[Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection].Tag);
            Assert.AreEqual(10, resultsMapping[Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection].Target);
            Assert.AreSame(validator, resultsMapping[Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection].Validator);
        }

        [TestMethod]
        public void ValidationIsPerformedOnCollectionElementsUsingDefaultRuleset()
        {
            object instance1 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object instance2 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object target = new object[] { instance1, instance2 };

            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(4, resultsList.Count);

            Assert.AreEqual("ReferencedObject", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Key);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(instance1, resultsList[0].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Key);
            Assert.AreEqual(null, resultsList[1].Tag);
            Assert.AreSame(instance1, resultsList[1].Target);

            Assert.AreEqual("ReferencedObject", resultsList[2].Message);
            Assert.AreEqual(null, resultsList[2].Key);
            Assert.AreEqual(null, resultsList[2].Tag);
            Assert.AreSame(instance2, resultsList[2].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[3].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[3].Key);
            Assert.AreEqual(null, resultsList[3].Tag);
            Assert.AreSame(instance2, resultsList[3].Target);
        }

        [TestMethod]
        public void NullElementsInCollectionAreSkipped()
        {
            object instance1 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object instance2 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object target = new object[] { instance1, null, instance2 };

            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(4, resultsList.Count);

            Assert.AreEqual("ReferencedObject", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Key);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(instance1, resultsList[0].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Key);
            Assert.AreEqual(null, resultsList[1].Tag);
            Assert.AreSame(instance1, resultsList[1].Target);

            Assert.AreEqual("ReferencedObject", resultsList[2].Message);
            Assert.AreEqual(null, resultsList[2].Key);
            Assert.AreEqual(null, resultsList[2].Tag);
            Assert.AreSame(instance2, resultsList[2].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[3].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[3].Key);
            Assert.AreEqual(null, resultsList[3].Tag);
            Assert.AreSame(instance2, resultsList[3].Target);
        }

        [TestMethod]
        public void ValidationIsPerformedOnCollectionElementsUsingSuppliedRuleset()
        {
            object instance1 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object instance2 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object target = new object[] { instance1, instance2 };

            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass), "RuleB");

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(2, resultsList.Count);

            Assert.AreEqual("PropertyInReferencedObject-RuleB", resultsList[0].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[0].Key);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(instance1, resultsList[0].Target);

            Assert.AreEqual("PropertyInReferencedObject-RuleB", resultsList[1].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Key);
            Assert.AreEqual(null, resultsList[1].Tag);
            Assert.AreSame(instance2, resultsList[1].Target);
        }

        [TestMethod]
        public void ValidatesUsingTheRulesForTheBaseTypeIfNotValidatingTheActualType()
        {
            object instance1 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object instance2 = new DerivedObjectCollectionValidatorFixtureReferencedTestClass();
            object target = new object[] { instance1, instance2 };

            Validator validator = new ObjectCollectionValidator(typeof(ObjectCollectionValidatorFixtureReferencedTestClass));

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(4, resultsList.Count);

            Assert.AreEqual("ReferencedObject", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Key);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(instance1, resultsList[0].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Key);
            Assert.AreEqual(null, resultsList[1].Tag);
            Assert.AreSame(instance1, resultsList[1].Target);

            Assert.AreEqual("ReferencedObject", resultsList[2].Message);
            Assert.AreEqual(null, resultsList[2].Key);
            Assert.AreEqual(null, resultsList[2].Tag);
            Assert.AreSame(instance2, resultsList[2].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[3].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[3].Key);
            Assert.AreEqual(null, resultsList[3].Tag);
            Assert.AreSame(instance2, resultsList[3].Target);
        }

        [TestMethod]
        public void ValidatesUsingTheRulesForTheActualTypeIfValidatingTheActualType()
        {
            // TODO - change to avoid relying on reflection order
            object instance1 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object instance2 = new DerivedObjectCollectionValidatorFixtureReferencedTestClass();
            object target = new object[] { instance1, instance2 };

            Validator validator = new ObjectCollectionValidator();

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(6, resultsList.Count);

            Assert.AreEqual("ReferencedObject", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Key);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(instance1, resultsList[0].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Key);
            Assert.AreEqual(null, resultsList[1].Tag);
            Assert.AreSame(instance1, resultsList[1].Target);

            Assert.AreEqual("DerivedReferencedObject", resultsList[2].Message);
            Assert.AreEqual(null, resultsList[2].Key);
            Assert.AreEqual(null, resultsList[2].Tag);
            Assert.AreSame(instance2, resultsList[2].Target);

            Assert.AreEqual("PropertyInDerivedReferencedObject", resultsList[3].Message);
            Assert.AreEqual("PropertyInDerivedReferencedObject", resultsList[3].Key);
            Assert.AreEqual(null, resultsList[3].Tag);
            Assert.AreSame(instance2, resultsList[3].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[4].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[4].Key);
            Assert.AreEqual(null, resultsList[4].Tag);
            Assert.AreSame(instance2, resultsList[4].Target);

            Assert.AreEqual("PropertyInDerivedReferencedObject-DataAnnotations", resultsList[5].Message);
            Assert.AreEqual("PropertyInDerivedReferencedObject", resultsList[5].Key);
            Assert.AreEqual(null, resultsList[5].Tag);
            Assert.AreSame(instance2, resultsList[5].Target);
        }

        [TestMethod]
        public void ValidatesUsingTheRulesForTheActualTypeWithASpecifiedValidatorFactoryIfValidatingTheActualType()
        {
            // TODO - change to avoid relying on reflection order
            object instance1 = new ObjectCollectionValidatorFixtureReferencedTestClass();
            object instance2 = new DerivedObjectCollectionValidatorFixtureReferencedTestClass();
            object target = new object[] { instance1, instance2 };

            Validator validator =
                new ObjectCollectionValidator(new AttributeValidatorFactory());

            ValidationResults validationResults = new ValidationResults();
            validator.DoValidate(target, this, "key", validationResults); // setting the currentTarget and the key

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(5, resultsList.Count);

            Assert.AreEqual("ReferencedObject", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Key);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(instance1, resultsList[0].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[1].Key);
            Assert.AreEqual(null, resultsList[1].Tag);
            Assert.AreSame(instance1, resultsList[1].Target);

            Assert.AreEqual("DerivedReferencedObject", resultsList[2].Message);
            Assert.AreEqual(null, resultsList[2].Key);
            Assert.AreEqual(null, resultsList[2].Tag);
            Assert.AreSame(instance2, resultsList[2].Target);

            Assert.AreEqual("PropertyInDerivedReferencedObject", resultsList[3].Message);
            Assert.AreEqual("PropertyInDerivedReferencedObject", resultsList[3].Key);
            Assert.AreEqual(null, resultsList[3].Tag);
            Assert.AreSame(instance2, resultsList[3].Target);

            Assert.AreEqual("PropertyInReferencedObject", resultsList[4].Message);
            Assert.AreEqual("PropertyInReferencedObject", resultsList[4].Key);
            Assert.AreEqual(null, resultsList[4].Tag);
            Assert.AreSame(instance2, resultsList[4].Target);
        }

        [MockValidator(true, MessageTemplate = "ReferencedObject")]
        public class ObjectCollectionValidatorFixtureReferencedTestClass
        {
            [MockValidator(true, MessageTemplate = "PropertyInReferencedObject")]
            [MockValidator(true, MessageTemplate = "PropertyInReferencedObject-RuleB", Ruleset = "RuleB")]
            public string PropertyInReferencedObject
            {
                get { return null; }
            }
        }

        [MockValidator(true, MessageTemplate = "DerivedReferencedObject")]
        public class DerivedObjectCollectionValidatorFixtureReferencedTestClass : ObjectCollectionValidatorFixtureReferencedTestClass
        {
            [MockValidator(true, MessageTemplate = "PropertyInDerivedReferencedObject")]
            [MockValidator(true, MessageTemplate = "PropertyInDerivedReferencedObject-RuleA", Ruleset = "RuleA")]
            [StringLength(5, ErrorMessage = "PropertyInDerivedReferencedObject-DataAnnotations")]
            public string PropertyInDerivedReferencedObject
            {
                get { return "a long string"; }
            }
        }
    }
}
