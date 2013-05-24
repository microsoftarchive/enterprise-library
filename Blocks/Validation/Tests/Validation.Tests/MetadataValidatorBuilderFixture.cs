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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class MetadataValidatorBuilderFixture
    {
        MockMemberAccessValidatorBuilderFactory mockFactory;
        MetadataValidatorBuilder builder;
        ReflectionMemberValueAccessBuilder valueAccessBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
            valueAccessBuilder = new ReflectionMemberValueAccessBuilder();
            mockFactory = new MockMemberAccessValidatorBuilderFactory(valueAccessBuilder);
            builder = new MetadataValidatorBuilder(mockFactory, ValidationFactory.DefaultCompositeValidatorFactory);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        #region test cases for individual properties

        [TestMethod]
        public void CreateValidatorForPropertyWithoutAttributesReturnsNull()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(BaseTestTypeWithValidatorAttributes).GetProperty("PropertyWithNoAttributes"), "");

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CanCreateValidatorForPropertyWithSingleValidatorAttribute()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(BaseTestTypeWithValidatorAttributes).GetProperty("PropertyWithSingleAttribute"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithSingleAttribute-Message1"));
        }

        [TestMethod]
        public void CanCreateValidatorForPropertyWithMultipleValidatorAttributes()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(BaseTestTypeWithValidatorAttributes).GetProperty("PropertyWithMultipleAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributes-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributes-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForPropertyWithMultipleValidatorAttributesAndValidationModifierAttributes()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(BaseTestTypeWithValidatorAttributes).GetProperty("PropertyWithMultipleAttributesAndValidationModifier"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(true, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.Or, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributesAndValidationModifier-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributesAndValidationModifier-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForPropertyWithMultipleValidatorAttributesAndValidationCompositionAttributeForAnd()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(BaseTestTypeWithValidatorAttributes).GetProperty("PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForOverriddenPropertyConsideringOnlyAttributesInTheOverride()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(DerivedTestTypeWithValidatorAttributes).GetProperty("PropertyWithMultipleAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributes.PropertyWithMultipleAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMultipleAttributesOverride-Message1"));
        }

        [TestMethod]
        public void ValidatorAttributesOnPropertyWithNoRuleSpecifiedAreUsedIfNoRuleNameIsProvided()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(TypeWithValidatorAttributesWithRuleNames).GetProperty("PropertyWithMixedRulesInValidationAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["TypeWithValidatorAttributesWithRuleNames.PropertyWithMixedRulesInValidationAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(3, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMixedRulesInValidationAttributes1-NoRule"));
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMixedRulesInValidationAttributes2-NoRule"));
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMixedRulesInValidationAttributes3-NoRule"));
        }

        [TestMethod]
        public void ValidatorAttributesOnPropertyWithRuleSpecifiedAreUsedIfRuleNameIsProvided()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(TypeWithValidatorAttributesWithRuleNames).GetProperty("PropertyWithMixedRulesInValidationAttributes"), "RuleA");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["TypeWithValidatorAttributesWithRuleNames.PropertyWithMixedRulesInValidationAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithMixedRulesInValidationAttributes-RuleA"));
        }

        #endregion

        #region test cases for individual fields

        [TestMethod]
        public void CreateValidatorForFieldWithoutAttributesReturnsNull()
        {
            Validator validator = builder.CreateValidatorForField(typeof(BaseTestTypeWithValidatorAttributes).GetField("FieldWithNoAttributes"), "");

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CanCreateValidatorForFieldWithSingleValidatorAttribute()
        {
            Validator validator = builder.CreateValidatorForField(typeof(BaseTestTypeWithValidatorAttributes).GetField("FieldWithSingleAttribute"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.FieldWithSingleAttribute"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithSingleAttribute-Message1"));
        }

        [TestMethod]
        public void CanCreateValidatorForFieldWithMultipleValidatorAttributes()
        {
            Validator validator = builder.CreateValidatorForField(typeof(BaseTestTypeWithValidatorAttributes).GetField("FieldWithMultipleAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMultipleAttributes-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMultipleAttributes-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForFieldWithMultipleValidatorAttributesAndValidationModifierAttribute()
        {
            Validator validator = builder.CreateValidatorForField(typeof(BaseTestTypeWithValidatorAttributes).GetField("FieldWithMultipleAttributesAndValidationModifier"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributesAndValidationModifier"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(true, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.Or, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMultipleAttributesAndValidationModifier-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMultipleAttributesAndValidationModifier-Message2"));
        }

        [TestMethod]
        public void ValidatorAttributesOnFieldWithNoRuleSpecifiedAreUsedIfNoRuleNameIsProvided()
        {
            Validator validator = builder.CreateValidatorForField(typeof(TypeWithValidatorAttributesWithRuleNames).GetField("FieldWithMixedRulesInValidationAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["TypeWithValidatorAttributesWithRuleNames.FieldWithMixedRulesInValidationAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(3, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMixedRulesInValidationAttributes1-NoRule"));
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMixedRulesInValidationAttributes2-NoRule"));
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMixedRulesInValidationAttributes3-NoRule"));
        }

        [TestMethod]
        public void ValidatorAttributesOnFieldWithRuleSpecifiedAreUsedIfRuleNameIsProvided()
        {
            Validator validator = builder.CreateValidatorForField(typeof(TypeWithValidatorAttributesWithRuleNames).GetField("FieldWithMixedRulesInValidationAttributes"), "RuleA");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["TypeWithValidatorAttributesWithRuleNames.FieldWithMixedRulesInValidationAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("FieldWithMixedRulesInValidationAttributes-RuleA"));
        }

        #endregion

        #region test cases for individual methods

        [TestMethod]
        public void CreateValidatorForMethodWithoutAttributesReturnsNull()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(BaseTestTypeWithValidatorAttributes).GetMethod("MethodWithNoAttributes"), "");

            Assert.IsNull(validator);
            Assert.AreEqual(0, mockFactory.requestedMembers.Count);
        }

        [TestMethod]
        public void CanCreateValidatorForMethodWithSingleValidatorAttribute()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(BaseTestTypeWithValidatorAttributes).GetMethod("MethodWithSingleAttribute"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.MethodWithSingleAttribute"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithSingleAttribute-Message1"));
        }

        [TestMethod]
        public void CanCreateValidatorForMethodWithMultipleValidatorAttributes()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(BaseTestTypeWithValidatorAttributes).GetMethod("MethodWithMultipleAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.MethodWithMultipleAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMultipleAttributes-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMultipleAttributes-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForMethodWithMultipleValidatorAttributesAndValidationModifierAttribute()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(BaseTestTypeWithValidatorAttributes).GetMethod("MethodWithMultipleAttributesAndValidationModifier"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.MethodWithMultipleAttributesAndValidationModifier"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(true, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.Or, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMultipleAttributesAndValidationModifier-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMultipleAttributesAndValidationModifier-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForOverriddenMethodConsideringOnlyAttributesInTheOverride()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(DerivedTestTypeWithValidatorAttributes).GetMethod("MethodWithMultipleAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributes.MethodWithMultipleAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMultipleAttributesOverride-Message1"));
        }

        [TestMethod]
        public void ValidatorAttributesOnMethodWithNoRuleSpecifiedAreUsedIfNoRuleNameIsProvided()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(TypeWithValidatorAttributesWithRuleNames).GetMethod("MethodWithMixedRulesInValidationAttributes"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["TypeWithValidatorAttributesWithRuleNames.MethodWithMixedRulesInValidationAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(3, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMixedRulesInValidationAttributes1-NoRule"));
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMixedRulesInValidationAttributes2-NoRule"));
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMixedRulesInValidationAttributes3-NoRule"));
        }

        [TestMethod]
        public void ValidatorAttributesOnMethodWithRuleSpecifiedAreUsedIfRuleNameIsProvided()
        {
            Validator validator = builder.CreateValidatorForMethod(typeof(TypeWithValidatorAttributesWithRuleNames).GetMethod("MethodWithMixedRulesInValidationAttributes"), "RuleA");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["TypeWithValidatorAttributesWithRuleNames.MethodWithMixedRulesInValidationAttributes"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("MethodWithMixedRulesInValidationAttributes-RuleA"));
        }

        #endregion

        #region test cases for methods with self validation

        // TODO move into metadata validated type

        //[TestMethod]
        //public void SelfValidatorRequestOnMethodWithoutSelfValidatorAttributesReturnsNull()
        //{
        //    Validator validator = this.builder.CreateSelfValidationValidatorForMethod(typeof(TypeWithValidatorSelfValidationAttributes).GetMethod("MethodWithoutSelfValidationAttributes"), "");

        //    Assert.IsNull(validator);
        //}

        //[TestMethod]
        //public void SelfValidatorRequestOnMethodWithSelfValidatorAttributeReturnsExpectedValidator()
        //{
        //    Validator validator = this.builder.CreateSelfValidationValidatorForMethod(typeof(TypeWithValidatorSelfValidationAttributes).GetMethod("MethodWithSelfValidationAttributes"), "");

        //    Assert.IsNotNull(validator);

        //    ValidationResults validationResults = validator.Validate(new TypeWithValidatorSelfValidationAttributes());
        //    IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);

        //    Assert.AreEqual(1, resultsMapping.Count);
        //    Assert.IsTrue(resultsMapping.ContainsKey("MethodWithSelfValidationAttributes"));
        //}

        //[TestMethod]
        //public void SelfValidatorRequestOnMethodWithMultipleSelfValidatorAttributeCreatesSingleSelfValidator()
        //{
        //    Validator validator = this.builder.CreateSelfValidationValidatorForMethod(typeof(TypeWithValidatorSelfValidationAttributes).GetMethod("MethodWithMultipleSelfValidationAttributes"), "");

        //    Assert.IsNotNull(validator);

        //    ValidationResults validationResults = validator.Validate(new TypeWithValidatorSelfValidationAttributes());
        //    IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);

        //    Assert.AreEqual(1, resultsMapping.Count);
        //    Assert.IsTrue(resultsMapping.ContainsKey("MethodWithMultipleSelfValidationAttributes"));
        //}

        //[TestMethod]
        //public void SelfValidatorRequestProvidingARulesetNameReturnsExpectedValidator()
        //{
        //    Validator validator = this.builder.CreateSelfValidationValidatorForMethod(typeof(TypeWithValidatorSelfValidationAttributes).GetMethod("MethodWithSelfValidationAttributesWithRuleset"), "");
        //    Validator validatorFromRuleset = this.builder.CreateSelfValidationValidatorForMethod(typeof(TypeWithValidatorSelfValidationAttributes).GetMethod("MethodWithSelfValidationAttributesWithRuleset"), "RuleA");

        //    Assert.IsNull(validator);
        //    Assert.IsNotNull(validatorFromRuleset);

        //    ValidationResults validationResults = validatorFromRuleset.Validate(new TypeWithValidatorSelfValidationAttributes());
        //    IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);

        //    Assert.AreEqual(1, resultsMapping.Count);
        //    Assert.IsTrue(resultsMapping.ContainsKey("MethodWithSelfValidationAttributesWithRuleset"));
        //}

        #endregion

        #region test cases for all properties

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnProperties()
        {
            Validator validator = builder.CreateValidator(typeof(BaseTestTypeWithValidatorAttributesOnProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(3, allValidators.Count);

            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithMultipleAttributes = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnInheritedAndOverridenProperties()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(3, allValidators.Count);

            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithMultipleAttributes = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributesOnProperties.PropertyWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithSingleAttribute.BuiltValidator));
            Assert.AreEqual(1, valueAccessBuilderPropertyWithMultipleAttributes.ValueValidators.Count);
            Assert.AreEqual("PropertyWithMultipleAttributesOverride-Message1", ((MockValidator<object>)valueAccessBuilderPropertyWithMultipleAttributes.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnNewProperties()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnNewProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(3, allValidators.Count);

            // because of the way properties work, new properties hide inherited properties.
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithMultipleAttributes = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributesOnNewProperties.PropertyWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithSingleAttribute.BuiltValidator));
            Assert.AreEqual(1, valueAccessBuilderPropertyWithMultipleAttributes.ValueValidators.Count);
            Assert.AreEqual("PropertyWithMultipleAttributesNew-Message1", ((MockValidator<object>)valueAccessBuilderPropertyWithMultipleAttributes.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnProperties.PropertyWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderPropertyWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void NonPublicOrStaticPropertiesAreIgnoredWhenCreatingValidator()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithValidatorAttributesOnNonPublicOrStaticProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("TestTypeWithValidatorAttributesOnNonPublicOrStaticProperties.PublicProperty"));
        }

        [TestMethod]
        public void SetterOnlyPropertiesAreIgnoredWhenCreatingValidator()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithValidatorAttributesOnSetterOnlyProperties), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(2, allValidators.Count);

            Assert.AreEqual(2, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("TestTypeWithValidatorAttributesOnSetterOnlyProperties.PropertyWithGetter"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("TestTypeWithValidatorAttributesOnSetterOnlyProperties.PropertyWithGetterAndSetter"));
        }

        [TestMethod]
        public void OnlyPropertiesWithRuleNameAreConsideredIfRuleNameIsSpecifiedOnCreation() { }

        #endregion

        #region test cases for all fields

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnFields()
        {
            Validator validator = builder.CreateValidator(typeof(BaseTestTypeWithValidatorAttributesOnFields), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(3, allValidators.Count);

            ValueAccessValidatorBuilder valueAccessBuilderFieldWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnFields.FieldWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderFieldWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderFieldWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderFieldWithMultipleAttributes = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnFields.FieldWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderFieldWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderFieldWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderFieldWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnFields.FieldWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderFieldWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderFieldWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnInheritedNewFields()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnNewFields), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(4, allValidators.Count);

            ValueAccessValidatorBuilder valueAccessBuilderFieldWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnFields.FieldWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderFieldWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderFieldWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderFieldWithMultipleAttributes = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnFields.FieldWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderFieldWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderFieldWithSingleAttribute.BuiltValidator));
            Assert.AreEqual(2, valueAccessBuilderFieldWithMultipleAttributes.ValueValidators.Count);
            ValueAccessValidatorBuilder valueAccessBuilderNewFieldWithMultipleAttributes = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributesOnNewFields.FieldWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderNewFieldWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderNewFieldWithMultipleAttributes.BuiltValidator));
            Assert.AreEqual(1, valueAccessBuilderNewFieldWithMultipleAttributes.ValueValidators.Count);
            Assert.AreEqual("FieldWithMultipleAttributesNew-Message1", ((MockValidator<object>)valueAccessBuilderNewFieldWithMultipleAttributes.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessBuilderFieldWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnFields.FieldWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderFieldWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderFieldWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void NonPublicOrStaticFieldsAreIgnoredWhenCreatingValidator()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithValidatorAttributesOnNonPublicOrStaticFields), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("TestTypeWithValidatorAttributesOnNonPublicOrStaticFields.PublicField"));
        }

        #endregion

        #region test cases for all methods

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnMethods()
        {
            Validator validator = builder.CreateValidator(typeof(BaseTestTypeWithValidatorAttributesOnMethods), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(3, allValidators.Count);

            Assert.AreEqual(3, mockFactory.requestedMembers.Count);
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderMethodWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithMultipleAttributes = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderMethodWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnInheritedAndOverridenMethods()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnMethods), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(3, allValidators.Count);

            ValueAccessValidatorBuilder valueAccessBuilderMethodWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderMethodWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithMultipleAttributes = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributesOnMethods.MethodWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderMethodWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithSingleAttribute.BuiltValidator));
            Assert.AreEqual(1, valueAccessBuilderMethodWithMultipleAttributes.ValueValidators.Count);
            Assert.AreEqual("MethodWithMultipleAttributesOverride-Message1", ((MockValidator<object>)valueAccessBuilderMethodWithMultipleAttributes.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnNewMethods()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnNewMethods), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            AndCompositeValidator compositeValidator = validator as AndCompositeValidator;
            IList<Validator> allValidators = ValidationTestHelper.CreateListFromEnumerable<Validator>(compositeValidator.Validators);
            Assert.AreEqual(4, allValidators.Count);

            ValueAccessValidatorBuilder valueAccessBuilderMethodWithSingleAttribute = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithSingleAttribute"];
            Assert.IsNotNull(valueAccessBuilderMethodWithSingleAttribute);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithSingleAttribute.BuiltValidator));
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithMultipleAttributes = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributesOnNewMethods.MethodWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderMethodWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithSingleAttribute.BuiltValidator));
            Assert.AreEqual(1, valueAccessBuilderMethodWithMultipleAttributes.ValueValidators.Count);
            Assert.AreEqual("MethodWithMultipleAttributesNew-Message1", ((MockValidator<object>)valueAccessBuilderMethodWithMultipleAttributes.ValueValidators[0]).MessageTemplate);
            ValueAccessValidatorBuilder valueAccessBuilderNewMethodWithMultipleAttributes = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithMultipleAttributes"];
            Assert.IsNotNull(valueAccessBuilderNewMethodWithMultipleAttributes);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderNewMethodWithMultipleAttributes.BuiltValidator));
            Assert.AreEqual(2, valueAccessBuilderNewMethodWithMultipleAttributes.ValueValidators.Count);
            ValueAccessValidatorBuilder valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMethods.MethodWithMultipleAttributesAndValidationModifier"];
            Assert.IsNotNull(valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier);
            Assert.IsTrue(allValidators.Contains(valueAccessBuilderMethodWithMultipleAttributesAndValidationModifier.BuiltValidator));
        }

        [TestMethod]
        public void NonPublicOrStaticMethodsAreIgnoredWhenCreatingValidator()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithValidatorAttributesOnNonPublicOrStaticMethods), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());
            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("TestTypeWithValidatorAttributesOnNonPublicOrStaticMethods.PublicMethod"));
        }

        [TestMethod]
        public void VoidOrParameterizedMethodsAreIgnoredWhenCreatingValidator()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithValidatorAttributesOnVoidOrParameterizedMethods), "");

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ValueAccessValidator), validator.GetType());

            Assert.AreEqual(1, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("TestTypeWithValidatorAttributesOnVoidOrParameterizedMethods.NonVoidMethodWithoutParameters"));
        }

        #endregion

        #region test cases for all methods with self validation

        [TestMethod]
        public void CanCreateValidatorForTypeWithoutSelfValidationAttributesOnMethods()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithoutSelfValidationAttributesOnMethods), "");

            ValidationResults validationResults = validator.Validate(new TestTypeWithoutSelfValidationAttributesOnMethods());

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithSelfValidationAttributesOnMethodsAndInheritedMethods()
        {
            // only public inherited methods can be retrieved through reflection

            Validator validator = builder.CreateValidator(typeof(DerivedTypeWithSelfValidationAttributesOnMethods), "");

            ValidationResults validationResults = validator.Validate(new DerivedTypeWithSelfValidationAttributesOnMethods());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("PublicMethodWithSelfValidationAttributeAndMatchingSignature"));
            Assert.IsFalse(resultsMapping.ContainsKey("PublicMethodWithMatchingSignatureButWithoutSelfValidationAttribute"));
            Assert.IsTrue(resultsMapping.ContainsKey("PrivateMethodWithSelfValidationAttributeAndMatchingSignature"));
            Assert.IsFalse(resultsMapping.ContainsKey("PublicMethodWithSelfValidationAttributeAndWithInvalidParameters"));
            Assert.IsFalse(resultsMapping.ContainsKey("PublicMethodWithSelfValidationAttributeAndWithoutReturnType"));
            Assert.IsTrue(resultsMapping.ContainsKey("InheritedPublicMethodWithSelfValidationAttributeAndMatchingSignature"));
            Assert.IsFalse(resultsMapping.ContainsKey("InheritedPrivateMethodWithSelfValidationAttributeAndMatchingSignature"));
            Assert.IsFalse(resultsMapping.ContainsKey("InheritedProtectedMethodWithSelfValidationAttributeAndMatchingSignature"));
            Assert.IsFalse(resultsMapping.ContainsKey("OverridenPublicMethodWithMatchingSignatureAndSelfValidationAttributeOnBaseButNotOnOverride"));
            Assert.IsTrue(resultsMapping.ContainsKey("OverridenPublicMethodWithMatchingSignatureAndSelfValidationAttributeOnBaseAndOnOverride-Derived"));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithSelfValidationAttributesOnMethodsWithRuleset()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTypeWithSelfValidationAttributesOnMethods), "RuleA");

            ValidationResults validationResults = validator.Validate(new DerivedTypeWithSelfValidationAttributesOnMethods());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("PublicMethodWithSelfValidationAttributeAndMatchingSignature"));
        }

        [TestMethod]
        public void SelfValidationAttributesAreNotScannedIfTheHasSelfValidationAttributeIsNotPresent()
        {
            {
                Validator validatorForTypeWithoutHasSelfValidation
                    = builder.CreateValidator(typeof(TypeWithSelfValidationAttributesOnMethodsButNoHasSelfValidationAttributeOnType), "");

                ValidationResults validationResultsForTypeWithoutHasSelfValidation
                    = validatorForTypeWithoutHasSelfValidation.Validate(new TypeWithSelfValidationAttributesOnMethodsButNoHasSelfValidationAttributeOnType());

                Assert.IsTrue(validationResultsForTypeWithoutHasSelfValidation.IsValid);
            }

            {
                Validator validatorForTypeWithHasSelfValidation =
                    builder.CreateValidator(typeof(TypeWithSelfValidationAttributesOnMethodsAndHasSelfValidationAttributeOnType), "");

                ValidationResults validationResultsForTypeWithHasSelfValidation
                    = validatorForTypeWithHasSelfValidation.Validate(new TypeWithSelfValidationAttributesOnMethodsAndHasSelfValidationAttributeOnType());

                Assert.IsFalse(validationResultsForTypeWithHasSelfValidation.IsValid);
                IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResultsForTypeWithHasSelfValidation);
                Assert.AreEqual(1, resultsMapping.Count);
                Assert.IsTrue(resultsMapping.ContainsKey("self validation"));
            }
        }

        public class TypeWithSelfValidationAttributesOnMethodsButNoHasSelfValidationAttributeOnType
        {
            [SelfValidation]
            public void SelfValidation(ValidationResults validationResults)
            {
                validationResults.AddResult(new ValidationResult("self validation", this, null, null, null));
            }
        }

        [HasSelfValidation]
        public class TypeWithSelfValidationAttributesOnMethodsAndHasSelfValidationAttributeOnType
        {
            [SelfValidation]
            public void SelfValidation(ValidationResults validationResults)
            {
                validationResults.AddResult(new ValidationResult("self validation", this, null, null, null));
            }
        }

        [TestMethod]
        public void InheritedHasSelfValidationAttributesAreNotConsidered()
        {
            Validator validatorForTypeWithoutHasSelfValidation
                = builder.CreateValidator(typeof(TypeWithSelfValidationAttributesOnMethodsDerivedFromTypeWithHasSelfValidationAttribute), "");

            ValidationResults validationResultsForTypeWithoutHasSelfValidation
                = validatorForTypeWithoutHasSelfValidation.Validate(new TypeWithSelfValidationAttributesOnMethodsDerivedFromTypeWithHasSelfValidationAttribute());

            Assert.IsTrue(validationResultsForTypeWithoutHasSelfValidation.IsValid);
        }

        public class TypeWithSelfValidationAttributesOnMethodsDerivedFromTypeWithHasSelfValidationAttribute
            : TypeWithSelfValidationAttributesOnMethodsAndHasSelfValidationAttributeOnType
        {
            [SelfValidation]
            public void DerivedSelfValidation(ValidationResults validationResults)
            {
                validationResults.AddResult(new ValidationResult("self validation", this, null, null, null));
            }
        }

        #endregion

        #region test cases for attributes on the type

        [TestMethod]
        public void CreateValidatorForTypeWithoutAttributesReturnsNull()
        {
            Validator validator = builder.CreateValidatorForType(typeof(TypeWithoutValidationAttributesAtTheTypeLevel), "");

            Assert.IsNull(validator);
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithMultipleValidationAttributes()
        {
            Validator validator = builder.CreateValidatorForType(typeof(TypeWithMultipleValidationAttributesAtTheTypeLevel), "");

            Assert.IsNotNull(validator);

            CompositeValidatorBuilder compositeValidatorBuilder = mockFactory.requestedTypes["TypeWithMultipleValidationAttributesAtTheTypeLevel"];

            Assert.IsNotNull(compositeValidatorBuilder);
            Assert.AreSame(compositeValidatorBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, compositeValidatorBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, compositeValidatorBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(compositeValidatorBuilder.ValueValidators);
            Assert.AreEqual(2, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("TypeWithMultipleValidationAttributesAtTheTypeLevel-Message1"));
            Assert.IsTrue(valueValidators.ContainsKey("TypeWithMultipleValidationAttributesAtTheTypeLevel-Message2"));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithMultipleValidationAttributesWithProvidedRuleset()
        {
            Validator validator = builder.CreateValidatorForType(typeof(TypeWithMultipleValidationAttributesAtTheTypeLevel), "RuleA");

            Assert.IsNotNull(validator);

            CompositeValidatorBuilder compositeValidatorBuilder = mockFactory.requestedTypes["TypeWithMultipleValidationAttributesAtTheTypeLevel"];

            Assert.IsNotNull(compositeValidatorBuilder);
            Assert.AreSame(compositeValidatorBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, compositeValidatorBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, compositeValidatorBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(compositeValidatorBuilder.ValueValidators);
            Assert.AreEqual(3, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("TypeWithMultipleValidationAttributesAtTheTypeLevel-Message1-RuleA"));
            Assert.IsTrue(valueValidators.ContainsKey("TypeWithMultipleValidationAttributesAtTheTypeLevel-Message2-RuleA"));
            Assert.IsTrue(valueValidators.ContainsKey("TypeWithMultipleValidationAttributesAtTheTypeLevel-Message3-RuleA"));
        }

        #endregion

        #region combined test cases

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnMethodsFieldsAndProperties()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributes), "");

            Assert.IsNotNull(validator);

            Assert.AreEqual(10, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("DerivedTestTypeWithValidatorAttributes.PropertyWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithSingleAttribute"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.MethodWithSingleAttribute"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("DerivedTestTypeWithValidatorAttributes.MethodWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.MethodWithMultipleAttributesAndValidationModifier"));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnMembersAndType()
        {
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnMembersAndType), "");

            Assert.IsNotNull(validator);

            Assert.AreEqual(10, mockFactory.requestedMembers.Count);
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("DerivedTestTypeWithValidatorAttributesOnMembersAndType.PropertyWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithSingleAttribute"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.MethodWithSingleAttribute"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("DerivedTestTypeWithValidatorAttributesOnMembersAndType.MethodWithMultipleAttributes"));
            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.MethodWithMultipleAttributesAndValidationModifier"));

            Assert.AreEqual(1, mockFactory.requestedTypes.Count);
            Assert.IsTrue(mockFactory.requestedTypes.ContainsKey("DerivedTestTypeWithValidatorAttributesOnMembersAndType"));
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnMethodsFieldsAndPropertiesWithProvidedRuleset()
        {
            Regex messageValidationRegex = new Regex(".*-RuleA");
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributes), "RuleA");

            Assert.IsNotNull(validator);

            Assert.AreEqual(4, mockFactory.requestedMembers.Count);

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"];
                Assert.AreEqual(1, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
            }

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"];
                Assert.AreEqual(2, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[1]).MessageTemplate));
            }

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"];
                Assert.AreEqual(2, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[1]).MessageTemplate));
            }

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("DerivedTestTypeWithValidatorAttributes.MethodWithMultipleAttributes"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributes.MethodWithMultipleAttributes"];
                Assert.AreEqual(2, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[1]).MessageTemplate));
            }
        }

        [TestMethod]
        public void CanCreateValidatorForTypeWithValidationAttributesOnMembersAndTypeWithProvidedRuleset()
        {
            Regex messageValidationRegex = new Regex(".*-RuleA");
            Validator validator = builder.CreateValidator(typeof(DerivedTestTypeWithValidatorAttributesOnMembersAndType), "RuleA");

            Assert.IsNotNull(validator);

            Assert.AreEqual(1, mockFactory.requestedTypes.Count);

            Assert.IsTrue(mockFactory.requestedTypes.ContainsKey("DerivedTestTypeWithValidatorAttributesOnMembersAndType"));
            {
                CompositeValidatorBuilder compositeValidatorBuilder = mockFactory.requestedTypes["DerivedTestTypeWithValidatorAttributesOnMembersAndType"];
                Assert.AreEqual(3, compositeValidatorBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)compositeValidatorBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)compositeValidatorBuilder.ValueValidators[1]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)compositeValidatorBuilder.ValueValidators[2]).MessageTemplate));
            }

            Assert.AreEqual(4, mockFactory.requestedMembers.Count);

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithSingleAttribute"];
                Assert.AreEqual(1, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
            }

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.PropertyWithMultipleAttributesAndValidationModifier"];
                Assert.AreEqual(2, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[1]).MessageTemplate));
            }

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributes.FieldWithMultipleAttributes"];
                Assert.AreEqual(2, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[1]).MessageTemplate));
            }

            Assert.IsTrue(mockFactory.requestedMembers.ContainsKey("DerivedTestTypeWithValidatorAttributesOnMembersAndType.MethodWithMultipleAttributes"));
            {
                ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["DerivedTestTypeWithValidatorAttributesOnMembersAndType.MethodWithMultipleAttributes"];
                Assert.AreEqual(2, valueAccessBuilder.ValueValidators.Count);
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[0]).MessageTemplate));
                Assert.IsTrue(messageValidationRegex.IsMatch(((MockValidator<object>)valueAccessBuilder.ValueValidators[1]).MessageTemplate));
            }
        }

        #endregion

        #region test cases with buddy classes

        [TestMethod]
        public void CanCreateValidatorForPropertyWithSingleValidatorAttributeOnMetadataType()
        {
            Validator validator = builder.CreateValidatorForProperty(typeof(BaseTestTypeWithValidatorAttributesOnMetadataType).GetProperty("PropertyWithSingleAttribute"), "");

            Assert.IsNotNull(validator);

            ValueAccessValidatorBuilder valueAccessBuilder = mockFactory.requestedMembers["BaseTestTypeWithValidatorAttributesOnMetadataType.PropertyWithSingleAttribute"];

            Assert.IsNotNull(valueAccessBuilder);
            Assert.AreSame(valueAccessBuilder.BuiltValidator, validator);
            Assert.AreEqual(false, valueAccessBuilder.IgnoreNulls);
            Assert.AreEqual(CompositionType.And, valueAccessBuilder.CompositionType);
            IDictionary<string, MockValidator<object>> valueValidators = CreateValidatorsMapping(valueAccessBuilder.ValueValidators);
            Assert.AreEqual(1, valueValidators.Count);
            Assert.IsTrue(valueValidators.ContainsKey("PropertyWithSingleAttribute-Message1-Metadata"));
        }

        [TestMethod]
        public void CanCreateValidatorFromSelfValidationAttributesOnMetadataType()
        {
            Validator validator = builder.CreateValidator(typeof(TestTypeWithoutSelfValidationAttributesOnMethodsInTheMetadataType), "");

            ValidationResults validationResults = validator.Validate(new TestTypeWithoutSelfValidationAttributesOnMethodsInTheMetadataType());

            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual("validation from self-validation method", validationResults.ElementAt(0).Message);
        }

        [MetadataType(typeof(BaseTestTypeWithValidatorAttributesOnMetadataTypeMetadata))]
        public class BaseTestTypeWithValidatorAttributesOnMetadataType
        {

            private class BaseTestTypeWithValidatorAttributesOnMetadataTypeMetadata
            {
                [MockValidator(false, MessageTemplate = "PropertyWithSingleAttribute-Message1-Metadata")]
                [MockValidator(false, MessageTemplate = "PropertyWithSingleAttribute-Message1-RuleA-Metadata", Ruleset = "RuleA")]
                public int PropertyWithSingleAttribute { get; set; }
            }

            public int PropertyWithSingleAttribute
            {
                get { return 0; }
            }
        }

        [MetadataType(typeof(TestTypeWithoutSelfValidationAttributesOnMethodsInTheMetadataTypeMetadata))]
        public class TestTypeWithoutSelfValidationAttributesOnMethodsInTheMetadataType
        {
            [HasSelfValidation]
            private class TestTypeWithoutSelfValidationAttributesOnMethodsInTheMetadataTypeMetadata
            {
                [SelfValidation]
                public void SelfValidate(ValidationResults validationResults)
                {
                }
            }

            public void SelfValidate(ValidationResults validationResults)
            {
                validationResults.AddResult(
                    new ValidationResult("validation from self-validation method", null, null, null, null));
            }
        }

        #endregion

        #region misc test cases

        [TestMethod]
        public void SuppliesAppropriateTypeParametersWhenCreatingValidatorsThroughAttributes()
        {
            ParameterCapturingValidatorAttribute.providedTypes.Clear();

            Validator validator = builder.CreateValidator(typeof(TestTypeWithParameterCapturingValidatorAttributes), string.Empty);

            Assert.AreEqual(4, ParameterCapturingValidatorAttribute.providedTypes.Count);
            Assert.AreSame(typeof(TestTypeWithParameterCapturingValidatorAttributes), ParameterCapturingValidatorAttribute.providedTypes["type"]);
            Assert.AreSame(typeof(MetadataValidatorBuilderFixture), ParameterCapturingValidatorAttribute.providedTypes["property"]);
            Assert.AreSame(typeof(MetadataValidatorBuilderFixture), ParameterCapturingValidatorAttribute.providedTypes["field"]);
            Assert.AreSame(typeof(MetadataValidatorBuilderFixture), ParameterCapturingValidatorAttribute.providedTypes["method"]);
        }

        public class ParameterCapturingValidatorAttribute : ValidatorAttribute
        {
            public static Dictionary<string, Type> providedTypes = new Dictionary<string, Type>();

            protected override Validator DoCreateValidator(Type targetType)
            {
                providedTypes.Add(MessageTemplate, targetType);
                return new MockValidator(false);
            }
        }

        [ParameterCapturingValidator(MessageTemplate = "type")]
        public class TestTypeWithParameterCapturingValidatorAttributes
        {
            [ParameterCapturingValidator(MessageTemplate = "field")]
            public MetadataValidatorBuilderFixture Field;

            [ParameterCapturingValidator(MessageTemplate = "property")]
            public MetadataValidatorBuilderFixture Property
            {
                get { return null; }
            }

            [ParameterCapturingValidator(MessageTemplate = "method")]
            public MetadataValidatorBuilderFixture Method()
            {
                return null;
            }
        }

        [TestMethod]
        public void ValueAccessBuilderIsSuppliedToValidatorDescriptor()
        {
            MockValueAccessBuilderCapturingAttribute.suppliedValueAccessBuilders.Clear();

            builder.CreateValidator(typeof(ValueAccessBuilderIsSuppliedToValidatorDescriptorTestClass), "");

            Assert.AreEqual(1, MockValueAccessBuilderCapturingAttribute.suppliedValueAccessBuilders.Count);
            Assert.AreSame(valueAccessBuilder, MockValueAccessBuilderCapturingAttribute.suppliedValueAccessBuilders[0]);
        }

        [MockValueAccessBuilderCapturing]
        public class ValueAccessBuilderIsSuppliedToValidatorDescriptorTestClass { }

        public class MockValueAccessBuilderCapturingAttribute : MockValidatorAttribute
        {
            public static IList<MemberValueAccessBuilder> suppliedValueAccessBuilders = new List<MemberValueAccessBuilder>();

            public MockValueAccessBuilderCapturingAttribute()
                : base(false) { }

            protected override Validator DoCreateValidator(
                Type targetType,
                Type ownerType,
                MemberValueAccessBuilder memberValueAccessBuilder,
                ValidatorFactory validatorFactory)
            {
                suppliedValueAccessBuilders.Add(memberValueAccessBuilder);
                return base.DoCreateValidator(targetType, ownerType, memberValueAccessBuilder, validatorFactory);
            }
        }

        #endregion

        #region utility methods

        // cannot rely on sort order for reflection, use a mapping
        IDictionary<string, MockValidator<object>> CreateValidatorsMapping(IList<Validator> validators)
        {
            return ValidationTestHelper.CreateMockValidatorsMapping(validators);
        }

        #endregion
    }
}
