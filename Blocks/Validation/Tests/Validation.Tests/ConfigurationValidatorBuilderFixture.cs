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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ConfigurationValidatorBuilderFixture
    {
        MockMemberAccessValidatorBuilderFactory mockFactory;
        ConfigurationValidatorBuilder builder;
        DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
            mockFactory = new MockMemberAccessValidatorBuilderFactory();
            configurationSource = new DictionaryConfigurationSource();
            builder =
                ConfigurationValidatorBuilder.FromConfiguration(
                    configurationSource,
                    mockFactory,
                    ValidationFactory.DefaultCompositeValidatorFactory);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void CreateValidatorForPropertyReferenceWithoutValidatorsReturnsNull()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("PublicProperty");

            Validator validator = builder.CreateValidatorForProperty(typeof(TestClass), propertyReference);

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void CreateValidatorForNonPublicPropertyReferenceWithValidatorsReturnsNull()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("NonPublicProperty");
            propertyReference.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            propertyReference.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForProperty(typeof(TestClass), propertyReference);

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForWriteOnlyPropertyReferenceWithValidatorsReturnsNull()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("WriteOnlyPublicProperty");
            propertyReference.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            propertyReference.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForProperty(typeof(TestClass), propertyReference);

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForPropertyReferenceWithValidatorsReturnsValueAccessValidator()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("PublicProperty");
            propertyReference.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            propertyReference.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForProperty(typeof(TestClass), propertyReference);

            Assert.IsNotNull(validator);
            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder = mockFactory.requestedMembers["TestClass.PublicProperty"];
            Assert.AreSame(valueAccessValidatorBuilder.BuiltValidator, validator);
            Assert.AreEqual(2, valueAccessValidatorBuilder.ValueValidators.Count);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder.ValueValidators[0]).MessageTemplate);
            Assert.AreEqual("validator 2 message", ((MockValidator<object>)valueAccessValidatorBuilder.ValueValidators[1]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForFieldReferenceWithoutValidatorsReturnsNull()
        {
            ValidatedFieldReference fieldReference = new ValidatedFieldReference("PublicField");

            Validator validator = builder.CreateValidatorForField(typeof(TestClass), fieldReference);

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void CreateValidatorForNonPublicFieldReferenceWithValidatorsReturnsNull()
        {
            ValidatedFieldReference fieldReference = new ValidatedFieldReference("NonPublicField");
            fieldReference.Validators.Add(new MockValidatorData("validator1", false));
            fieldReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            fieldReference.Validators.Add(new MockValidatorData("validator2", false));
            fieldReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForField(typeof(TestClass), fieldReference);

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForFieldReferenceWithValidatorsReturnsValueAccessValidator()
        {
            ValidatedFieldReference fieldReference = new ValidatedFieldReference("PublicField");
            fieldReference.Validators.Add(new MockValidatorData("validator1", false));
            fieldReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            fieldReference.Validators.Add(new MockValidatorData("validator2", false));
            fieldReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForField(typeof(TestClass), fieldReference);

            Assert.IsNotNull(validator);
            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder = mockFactory.requestedMembers["TestClass.PublicField"];
            Assert.AreSame(valueAccessValidatorBuilder.BuiltValidator, validator);
            Assert.AreEqual(2, valueAccessValidatorBuilder.ValueValidators.Count);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder.ValueValidators[0]).MessageTemplate);
            Assert.AreEqual("validator 2 message", ((MockValidator<object>)valueAccessValidatorBuilder.ValueValidators[1]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForMethodReferenceWithoutValidatorsReturnsNull()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("PublicMethod");

            Validator validator = builder.CreateValidatorForMethod(typeof(TestClass), methodReference);

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void CreateValidatorForNonPublicMethodReferenceWithValidatorsReturnsNull()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("NonPublicMethod");
            methodReference.Validators.Add(new MockValidatorData("validator1", false));
            methodReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference.Validators.Add(new MockValidatorData("validator2", false));
            methodReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForMethod(typeof(TestClass), methodReference);

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForVoidMethodReferenceWithValidatorsReturnsNull()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("PublicVoidMethod");
            methodReference.Validators.Add(new MockValidatorData("validator1", false));
            methodReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference.Validators.Add(new MockValidatorData("validator2", false));
            methodReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForMethod(typeof(TestClass), methodReference);

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForMethodWithArgumentsReferenceWithValidatorsReturnsNull()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("PublicMethodWithArguments");
            methodReference.Validators.Add(new MockValidatorData("validator1", false));
            methodReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference.Validators.Add(new MockValidatorData("validator2", false));
            methodReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForMethod(typeof(TestClass), methodReference);

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForMethodReferenceWithValidatorsReturnsValueAccessValidator()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("PublicMethod");
            methodReference.Validators.Add(new MockValidatorData("validator1", false));
            methodReference.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference.Validators.Add(new MockValidatorData("validator2", false));
            methodReference.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForMethod(typeof(TestClass), methodReference);

            Assert.IsNotNull(validator);
            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder.BuiltValidator, validator);
            Assert.AreEqual(2, valueAccessValidatorBuilder.ValueValidators.Count);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder.ValueValidators[0]).MessageTemplate);
            Assert.AreEqual("validator 2 message", ((MockValidator<object>)valueAccessValidatorBuilder.ValueValidators[1]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForEmptyRuleReturnsEmptyCompositeValidator()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
        }

        [TestMethod]
        public void CreateValidatorForRuleWithPropertyReferenceReturnsCompositeValidatorWithPropertyValueAccess()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("PublicProperty");
            ruleData.Properties.Add(propertyReference1);
            ValidatedPropertyReference propertyReference2 = new ValidatedPropertyReference("SecondPublicProperty");
            ruleData.Properties.Add(propertyReference2);
            propertyReference1.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            propertyReference2.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(2, validators.Count);
            Assert.AreEqual(2, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicProperty"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validators[0]);
            Assert.AreEqual(1, valueAccessValidatorBuilder1.ValueValidators.Count);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder2 = mockFactory.requestedMembers["TestClass.SecondPublicProperty"];
            Assert.AreSame(valueAccessValidatorBuilder2.BuiltValidator, validators[1]);
            Assert.AreEqual(1, valueAccessValidatorBuilder2.ValueValidators.Count);
            Assert.AreEqual("validator 2 message", ((MockValidator<object>)valueAccessValidatorBuilder2.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForRuleWithInvalidPropertyReferenceIgnoresInvalidProperty()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("PublicProperty");
            ruleData.Properties.Add(propertyReference1);
            ValidatedPropertyReference propertyReference2 = new ValidatedPropertyReference("NonPublicProperty");
            ruleData.Properties.Add(propertyReference2);
            propertyReference1.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            propertyReference2.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());

            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicProperty"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForRuleWithFieldReferenceReturnsCompositeValidatorWithFieldValueAccess()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();
            ValidatedFieldReference fieldReference1 = new ValidatedFieldReference("PublicField");
            ruleData.Fields.Add(fieldReference1);
            ValidatedFieldReference fieldReference2 = new ValidatedFieldReference("SecondPublicField");
            ruleData.Fields.Add(fieldReference2);
            fieldReference1.Validators.Add(new MockValidatorData("validator1", false));
            fieldReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            fieldReference2.Validators.Add(new MockValidatorData("validator2", false));
            fieldReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(2, validators.Count);
            Assert.AreEqual(2, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicField"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validators[0]);
            Assert.AreEqual(1, valueAccessValidatorBuilder1.ValueValidators.Count);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder2 = mockFactory.requestedMembers["TestClass.SecondPublicField"];
            Assert.AreSame(valueAccessValidatorBuilder2.BuiltValidator, validators[1]);
            Assert.AreEqual(1, valueAccessValidatorBuilder2.ValueValidators.Count);
            Assert.AreEqual("validator 2 message", ((MockValidator<object>)valueAccessValidatorBuilder2.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForRuleWithInvalidFieldReferenceIgnoresInvalidField()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();
            ValidatedFieldReference fieldReference1 = new ValidatedFieldReference("PublicField");
            ruleData.Fields.Add(fieldReference1);
            ValidatedFieldReference fieldReference2 = new ValidatedFieldReference("NonPublicField");
            ruleData.Fields.Add(fieldReference2);
            fieldReference1.Validators.Add(new MockValidatorData("validator1", false));
            fieldReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            fieldReference2.Validators.Add(new MockValidatorData("validator2", false));
            fieldReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicField"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForRuleWithMethodReferenceReturnsCompositeValidatorWithMethodValueAccess()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("SecondPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(2, validators.Count);
            Assert.AreEqual(2, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validators[0]);
            Assert.AreEqual(1, valueAccessValidatorBuilder1.ValueValidators.Count);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessValidatorBuilder2 = mockFactory.requestedMembers["TestClass.SecondPublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder2.BuiltValidator, validators[1]);
            Assert.AreEqual(1, valueAccessValidatorBuilder2.ValueValidators.Count);
            Assert.AreEqual("validator 2 message", ((MockValidator<object>)valueAccessValidatorBuilder2.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForRuleWithInvalidMethodReferenceIgnoresInvalidMethod()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData();
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForRule(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForNonExistingRuleReturnsEmptyValidator()
        {
            ValidatedTypeReference typeReference = new ValidatedTypeReference();

            Validator validator = builder.CreateValidator(typeof(TestClass), typeReference, "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
        }

        [TestMethod]
        public void CreateValidatorForExistingRuleReturnsNonEmptyValidatorBasedOnRuleDefinition()
        {
            ValidatedTypeReference typeReference = new ValidatedTypeReference();
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidator(typeof(TestClass), typeReference, "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForExistingTypeReturnsNonEmptyValidatorBasedOnRuleDefinition()
        {
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidator(typeof(TestClass), typeReference, "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForDefaultRuleForExistingTypeReturnsNonEmptyValidatorBasedOnDefaultRuleDefinition()
        {
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            typeReference.DefaultRuleset = "ruleset1";
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidator(typeof(TestClass), typeReference, string.Empty);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForDefaultRuleForExistingTypeWithoutDefaultRuleSetReturnsEmptyValidator()
        {
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidator(typeof(TestClass), typeReference, string.Empty);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForDefaultRuleForExistingTypeNonExistingDefaultRuleSetReturnsEmptyValidator()
        {
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            typeReference.DefaultRuleset = "ruleset2";
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidator(typeof(TestClass), typeReference, string.Empty);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorWithMissingConfigurationSectionReturnsEmptyValidator()
        {
            Validator validator = builder.CreateValidator(typeof(TestClass), "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForDefaultRuleForNonExistingTypeInConfigurationRetunsEmptyValidator()
        {
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);

            Validator validator = builder.CreateValidator(typeof(TestClass), string.Empty);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForNonExistingTypeInConfigurationReturnsEmptyValidator()
        {
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);

            Validator validator = builder.CreateValidator(typeof(TestClass), "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            IList<Validator> validators = ValidationTestHelper.CreateListFromEnumerable(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CreateValidatorForExistingTypeInConfigurationRetunsAppropriateValidatorBasedOnRule()
        {
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            settings.Types.Add(typeReference);
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            // Note: Use a local builder here since the assumption was made about how and when the
            //       the builder uses the configuration source....
            var localBuilder =
                ConfigurationValidatorBuilder.FromConfiguration(
                    configurationSource,
                    mockFactory,
                    ValidationFactory.DefaultCompositeValidatorFactory);
            Validator validator = localBuilder.CreateValidator(typeof(TestClass), "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForDefaultPropertyForExistingTypeInConfigurationRetunsAppropriateValidatorBasedOnRule()
        {
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            settings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "ruleset1";
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            // Note: Use a local builder here since the assumption was made about how and when the
            //       the builder uses the configuration source....
            var localBuilder =
                ConfigurationValidatorBuilder.FromConfiguration(
                    configurationSource,
                    mockFactory,
                    ValidationFactory.DefaultCompositeValidatorFactory);
            Validator validator = localBuilder.CreateValidator(typeof(TestClass), string.Empty);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorPropertyForExistingTypeInConfigurationRetunsAppropriateValidatorBasedOnRule()
        {
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            settings.Types.Add(typeReference);
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            typeReference.Rulesets.Add(ruleData);
            ValidatedMethodReference methodReference1 = new ValidatedMethodReference("PublicMethod");
            ruleData.Methods.Add(methodReference1);
            ValidatedMethodReference methodReference2 = new ValidatedMethodReference("NonPublicMethod");
            ruleData.Methods.Add(methodReference2);
            methodReference1.Validators.Add(new MockValidatorData("validator1", false));
            methodReference1.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            methodReference2.Validators.Add(new MockValidatorData("validator2", false));
            methodReference2.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            // Note: Use a local builder here since the assumption was made about how and when the
            //       the builder uses the configuration source....
            var localBuilder =
                ConfigurationValidatorBuilder.FromConfiguration(
                    configurationSource,
                    mockFactory,
                    ValidationFactory.DefaultCompositeValidatorFactory);
            Validator validator = localBuilder.CreateValidator(typeof(TestClass), "ruleset1");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            ValueAccessValidatorBuilder valueAccessValidatorBuilder1 = mockFactory.requestedMembers["TestClass.PublicMethod"];
            Assert.AreSame(valueAccessValidatorBuilder1.BuiltValidator, validator);
            Assert.AreEqual("validator 1 message", ((MockValidator<object>)valueAccessValidatorBuilder1.ValueValidators[0]).MessageTemplate);
        }

        [TestMethod]
        public void CreateValidatorForTypeWithoutValidatorConfigurationReturnsNull()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            Validator validator = builder.CreateValidatorForType(typeof(TestClass), ruleData);

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithMultipleValidatorConfigurations()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData("ruleset1");
            ruleData.Validators.Add(new MockValidatorData("validator1", false));
            ruleData.Validators.Get("validator1").MessageTemplate = "validator 1 message";
            ruleData.Validators.Add(new MockValidatorData("validator2", false));
            ruleData.Validators.Get("validator2").MessageTemplate = "validator 2 message";

            Validator validator = builder.CreateValidatorForType(typeof(TestClass), ruleData);

            Assert.IsNotNull(validator);

            CompositeValidatorBuilder compositeValidatorBuilder = mockFactory.requestedTypes["TestClass"];

            Assert.IsNotNull(compositeValidatorBuilder);
            Assert.AreSame(compositeValidatorBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, compositeValidatorBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, compositeValidatorBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = ValidationTestHelper.CreateMockValidatorsMapping(compositeValidatorBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("validator 1 message"));
            Assert.IsTrue(valueValidators.ContainsKey("validator 2 message"));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithMultipleValidationConfigurationWithProvidedRulesetThroughTopLevelFactoryMethod()
        {
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(TestClass));
            settings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "RuleB";
            ValidationRulesetData ruleDataA = new ValidationRulesetData("RuleA");
            typeReference.Rulesets.Add(ruleDataA);
            ruleDataA.Validators.Add(new MockValidatorData("validator1", false));
            ruleDataA.Validators.Get("validator1").MessageTemplate = "Message1-RuleA";
            ruleDataA.Validators.Add(new MockValidatorData("validator2", false));
            ruleDataA.Validators.Get("validator2").MessageTemplate = "Message2-RuleA";
            ruleDataA.Validators.Add(new MockValidatorData("validator3", false));
            ruleDataA.Validators.Get("validator3").MessageTemplate = "Message3-RuleA";
            ValidationRulesetData ruleDataB = new ValidationRulesetData("RuleB");
            typeReference.Rulesets.Add(ruleDataB);
            ruleDataB.Validators.Add(new MockValidatorData("validator1", false));
            ruleDataB.Validators.Get("validator1").MessageTemplate = "Message1-RuleB";
            ruleDataB.Validators.Add(new MockValidatorData("validator2", false));
            ruleDataB.Validators.Get("validator2").MessageTemplate = "Message1-RuleB";

            // Note: Use a local builder here since the assumption was made about how and when the
            //       the builder uses the configuration source....
            var localBuilder =
                ConfigurationValidatorBuilder.FromConfiguration(
                    configurationSource,
                    mockFactory,
                    ValidationFactory.DefaultCompositeValidatorFactory);
            Validator validator = localBuilder.CreateValidator(typeof(TestClass), "RuleA");
            Assert.IsNotNull(validator);
            CompositeValidatorBuilder compositeValidatorBuilder = mockFactory.requestedTypes["TestClass"];

            Assert.IsNotNull(compositeValidatorBuilder);
            Assert.AreEqual(false, compositeValidatorBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, compositeValidatorBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = ValidationTestHelper.CreateMockValidatorsMapping(compositeValidatorBuilder.ValueValidators);
            Assert.AreEqual(3, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("Message1-RuleA"));
            Assert.IsTrue(valueValidators.ContainsKey("Message2-RuleA"));
            Assert.IsTrue(valueValidators.ContainsKey("Message3-RuleA"));
        }

        public class TestClass
        {
#pragma warning disable 414
            String NonPublicField = null;
#pragma warning restore 414

            public String PublicField = null;

            public String SecondPublicField = null;

            String NonPublicProperty
            {
                get { return null; }
            }

            public String PublicProperty
            {
                get { return null; }
            }

            public String SecondPublicProperty
            {
                get { return null; }
            }

            public String WriteOnlyPublicProperty
            {
                set { }
            }

            String NonPublicMethod()
            {
                return null;
            }

#pragma warning restore 414

            public String PublicMethod()
            {
                return null;
            }

            public String PublicMethodWithArguments(object argument)
            {
                return null;
            }

            public String PublicMethodWithParamArguments(params object[] arguments)
            {
                return null;
            }

            public void PublicVoidMethod() { }

            public String SecondPublicMethod()
            {
                return null;
            }
        }
    }
}
