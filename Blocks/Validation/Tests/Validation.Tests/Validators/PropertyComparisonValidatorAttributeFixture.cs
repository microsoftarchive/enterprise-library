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
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class PropertyComparisonValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingInstanceWithNullPropertyNameThrows()
        {
            new PropertyComparisonValidatorAttribute(null, ComparisonOperator.Equal);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeWithInvalidPropertyNameThrowsWhenCreatingValidator()
        {
            MemberValueAccessBuilder builder = new ReflectionMemberValueAccessBuilder();
            IValidatorDescriptor validatorAttribute = new PropertyComparisonValidatorAttribute("InvalidProperty", ComparisonOperator.Equal);

            validatorAttribute.CreateValidator(typeof(PropertyComparisonValidatorAttributeFixtureTestClass),
                                               typeof(PropertyComparisonValidatorAttributeFixtureTestClass),
                                               builder,
                                               null);
        }

        [TestMethod]
        public void CreatesAppropriateValidator()
        {
            MemberValueAccessBuilder builder = new ReflectionMemberValueAccessBuilder();
            PropertyComparisonValidatorAttribute validatorAttribute = new PropertyComparisonValidatorAttribute("PublicProperty", ComparisonOperator.NotEqual);
            validatorAttribute.Negated = true;

            PropertyComparisonValidator validator = ((IValidatorDescriptor)validatorAttribute).CreateValidator(typeof(PropertyComparisonValidatorAttributeFixtureTestClass),
                                                                                                               typeof(PropertyComparisonValidatorAttributeFixtureTestClass),
                                                                                                               builder,
                                                                                                               null) as PropertyComparisonValidator;

            Assert.IsNotNull(validator);
            Assert.AreEqual("PublicProperty", ((PropertyValueAccess)validator.ValueAccess).PropertyInfo.Name);
            Assert.AreEqual(ComparisonOperator.NotEqual, validator.ComparisonOperator);
            Assert.AreEqual(true, validator.Negated);
        }

        public class PropertyComparisonValidatorAttributeFixtureTestClass
        {
            string publicProperty;

            public string PublicProperty
            {
                get { return publicProperty; }
                set { publicProperty = value; }
            }
        }

        [TestMethod]
        public void AttributeWithNullRulesetCannotBeUsedAsValidationAttribute()
        {
            ValidationAttribute attribute =
                new PropertyComparisonValidatorAttribute("proeprty", ComparisonOperator.NotEqual);

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
            ValidationAttribute attribute =
                new PropertyComparisonValidatorAttribute("proeprty", ComparisonOperator.NotEqual)
                {
                    Ruleset = "ruleset"
                };

            Assert.IsTrue(attribute.IsValid(""));
        }
    }
}
