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
        }
    }
}