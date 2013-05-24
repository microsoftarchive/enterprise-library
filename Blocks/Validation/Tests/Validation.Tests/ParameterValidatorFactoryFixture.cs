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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    /// <summary>
    /// Tests for the class that builds validators based on the attributes
    /// supplied in method parameters.
    /// </summary>
    [TestClass]
    public class ParameterValidatorFactoryFixture
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
        public void ShouldBuildEmptyCompositeWithNoValidationAttributes()
        {
            ParameterInfo paramToValidate = GetParameterInfo("MethodWithNoValidators", "firstParam");
            Validator validator = ParameterValidatorFactory.CreateValidator(paramToValidate);
            Assert.IsNotNull(validator);
            Assert.IsTrue(validator is AndCompositeValidator);
            List<Validator> validators = new List<Validator>(((AndCompositeValidator)validator).Validators);
            Assert.AreEqual(0, validators.Count);
        }

        [TestMethod]
        public void ShouldBuildOneValidatorWithOneValidationAttribute()
        {
            ParameterInfo paramToValidate = GetParameterInfo("MethodWithOneValidator", "foo");
            Validator v = ParameterValidatorFactory.CreateValidator(paramToValidate);
            Assert.IsNotNull(v);
            Assert.IsTrue(v is NotNullValidator);
        }

        [TestMethod]
        public void ShouldUseAnExternalRuleSet()
        {
            ParameterInfo paramToValidate = GetParameterInfo("MethodWithObjectValidator", "objectToValidate");
            Validator v = ParameterValidatorFactory.CreateValidator(paramToValidate);
            Assert.IsNotNull(v);
            Assert.IsTrue(v is ObjectValidator);
            ObjectValidator validator = v as ObjectValidator;
            Assert.AreEqual(validator.TargetRuleset, "RuleSetA");
        }

        [TestMethod]
        public void ShouldBuildThreeValidatorsWithThreeValidationAttributes()
        {
            ParameterInfo paramToValidate = GetParameterInfo("MethodWithMultipleValidators", "id");
            Validator v = ParameterValidatorFactory.CreateValidator(paramToValidate);
            Assert.IsNotNull(v);
            Assert.IsTrue(v is AndCompositeValidator);
            List<Validator> validators = new List<Validator>(((AndCompositeValidator)v).Validators);
            Assert.AreEqual(3, validators.Count);
            Assert.IsTrue(
                validators.Exists(
                    delegate(Validator v1)
                    {
                        return v1 is NotNullValidator;
                    }));

            Assert.IsTrue(
                validators.Exists(
                    delegate(Validator v1)
                    {
                        return v1 is StringLengthValidator;
                    }));

            Assert.IsTrue(
                validators.Exists(
                    delegate(Validator v1)
                    {
                        return v1 is RegexValidator;
                    }));
        }

        [TestMethod]
        public void ShouldObserveTheIgnoreNullsAttribute()
        {
            Validator validatorWithIgnoreNulls
                = ParameterValidatorFactory.CreateValidator(GetParameterInfo("MethodWithIgnoreNullAttribute", "id"));
            Validator validatorWithoutIgnoreNulls
                = ParameterValidatorFactory.CreateValidator(GetParameterInfo("MethodWithoutIgnoreNullAttribute", "id"));

            Assert.IsTrue(validatorWithIgnoreNulls.Validate(null).IsValid);
            Assert.IsFalse(validatorWithoutIgnoreNulls.Validate(null).IsValid);

            Assert.IsTrue(validatorWithIgnoreNulls.Validate(new string('c', 30)).IsValid);
            Assert.IsTrue(validatorWithoutIgnoreNulls.Validate(new string('c', 30)).IsValid);

            Assert.IsFalse(validatorWithIgnoreNulls.Validate(new string('c', 60)).IsValid);
            Assert.IsFalse(validatorWithoutIgnoreNulls.Validate(new string('c', 60)).IsValid);
        }

        [TestMethod]
        public void ShouldObserveTheCompositionTypeAttribute()
        {
            Validator validatorWithAndComposition
                = ParameterValidatorFactory.CreateValidator(GetParameterInfo("MethodWithAndComposition", "id"));
            Validator validatorWithOrComposition
                = ParameterValidatorFactory.CreateValidator(GetParameterInfo("MethodWithOrComposition", "id"));

            Assert.IsFalse(validatorWithAndComposition.Validate(null).IsValid);
            Assert.IsFalse(validatorWithOrComposition.Validate(null).IsValid);

            Assert.IsTrue(validatorWithAndComposition.Validate(new string('c', 30)).IsValid);
            Assert.IsTrue(validatorWithOrComposition.Validate(new string('c', 30)).IsValid);

            Assert.IsFalse(validatorWithAndComposition.Validate(new string('&', 30)).IsValid);
            Assert.IsTrue(validatorWithOrComposition.Validate(new string('&', 30)).IsValid);

            Assert.IsFalse(validatorWithAndComposition.Validate(new string('c', 60)).IsValid);
            Assert.IsTrue(validatorWithOrComposition.Validate(new string('c', 60)).IsValid);

            Assert.IsFalse(validatorWithAndComposition.Validate(new string('&', 60)).IsValid);
            Assert.IsFalse(validatorWithOrComposition.Validate(new string('&', 60)).IsValid);
        }


        static ParameterInfo GetParameterInfo(string method,
                                              string paramName)
        {
            MemberInfo[] members = typeof(ISomeMethodsToValidate).GetMember(method);
            MethodInfo methodInfo = (MethodInfo)members[0];
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
            {
                if (paramInfo.Name == paramName)
                {
                    return paramInfo;
                }
            }
            throw new InvalidOperationException(
                string.Format("The method {0} does not exist or does not have a parameter named {1}",
                              method, paramName));
        }

        interface ISomeMethodsToValidate
        {
            int MethodWithMultipleValidators(
                [NotNullValidator] [StringLengthValidator(1, RangeBoundaryType.Inclusive, 50, RangeBoundaryType.Inclusive)] [RegexValidator(@"[A-Za-z_][A-Za-z0-9_]*")] string id);

            int MethodWithNoValidators(string firstParam);

            int MethodWithObjectValidator(
                [ObjectValidator("RuleSetA")] object objectToValidate);

            int MethodWithOneValidator(
                [NotNullValidator] string foo);

            int MethodWithIgnoreNullAttribute(
                [IgnoreNulls]
                [StringLengthValidator(1, RangeBoundaryType.Inclusive, 50, RangeBoundaryType.Inclusive)] 
                [RegexValidator(@"[A-Za-z_][A-Za-z0-9_]*")] 
                string id);

            int MethodWithoutIgnoreNullAttribute(
                [StringLengthValidator(1, RangeBoundaryType.Inclusive, 50, RangeBoundaryType.Inclusive)] 
                [RegexValidator(@"[A-Za-z_][A-Za-z0-9_]*")] 
                string id);

            int MethodWithAndComposition(
                [ValidatorComposition(CompositionType.And)] 
                [StringLengthValidator(1, RangeBoundaryType.Inclusive, 50, RangeBoundaryType.Inclusive)] 
                [RegexValidator(@"[A-Za-z_][A-Za-z0-9_]*")] 
                string id);

            int MethodWithOrComposition(
                [ValidatorComposition(CompositionType.Or)] 
                [StringLengthValidator(1, RangeBoundaryType.Inclusive, 50, RangeBoundaryType.Inclusive)] 
                [RegexValidator(@"[A-Za-z_][A-Za-z0-9_]*")] 
                string id);
        }
    }
}
