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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class GivenValidationAttributeValidatedElementOnPropertyWithoutValidationAttributes
    {
        private IValidatedElement validatedElement;

        [TestInitialize]
        public void Setup()
        {
            this.validatedElement =
                new ValidationAttributeValidatedElement(
                    StaticReflection.GetPropertyInfo((TypeWithValidationAttributes t)
                        => t.PropertyWithNoValidationAttributes));
        }

        [TestMethod]
        public void ThenReturnsConfiguredMember()
        {
            Assert.AreSame(
                StaticReflection.GetPropertyInfo((TypeWithValidationAttributes t) => t.PropertyWithNoValidationAttributes),
                this.validatedElement.MemberInfo);
        }

        [TestMethod]
        public void ThenReturnsTargetType()
        {
            Assert.AreSame(typeof(int), this.validatedElement.TargetType);
        }

        [TestMethod]
        public void ThenDoesNotIgnoreNulls()
        {
            Assert.IsFalse(this.validatedElement.IgnoreNulls);
            Assert.IsNull(this.validatedElement.IgnoreNullsMessageTemplate);
            Assert.IsNull(this.validatedElement.IgnoreNullsTag);
        }

        [TestMethod]
        public void ThenReturnsAndComposition()
        {
            Assert.AreEqual(CompositionType.And, this.validatedElement.CompositionType);
            Assert.IsNull(this.validatedElement.CompositionMessageTemplate);
            Assert.IsNull(this.validatedElement.CompositionTag);
        }

        [TestMethod]
        public void ThenReturnsNoValidationDescriptors()
        {
            Assert.AreEqual(0, this.validatedElement.GetValidatorDescriptors().Count());
        }
    }

    [TestClass]
    public class GivenValidationAttributeValidatedElementOnPropertyWithValidationAttributes
    {
        private IValidatedElement validatedElement;

        [TestInitialize]
        public void Setup()
        {
            this.validatedElement =
                new ValidationAttributeValidatedElement(
                    StaticReflection.GetPropertyInfo((TypeWithValidationAttributes t)
                        => t.PropertyWithSingleValidationAttribute));
        }

        [TestMethod]
        public void ThenReturnsConfiguredMember()
        {
            Assert.AreSame(
                StaticReflection.GetPropertyInfo((TypeWithValidationAttributes t) => t.PropertyWithSingleValidationAttribute),
                this.validatedElement.MemberInfo);
        }

        [TestMethod]
        public void ThenReturnsTargetType()
        {
            Assert.AreSame(typeof(int), this.validatedElement.TargetType);
        }

        [TestMethod]
        public void ThenDoesNotIgnoreNulls()
        {
            Assert.IsFalse(this.validatedElement.IgnoreNulls);
            Assert.IsNull(this.validatedElement.IgnoreNullsMessageTemplate);
            Assert.IsNull(this.validatedElement.IgnoreNullsTag);
        }

        [TestMethod]
        public void ThenReturnsAndComposition()
        {
            Assert.AreEqual(CompositionType.And, this.validatedElement.CompositionType);
            Assert.IsNull(this.validatedElement.CompositionMessageTemplate);
            Assert.IsNull(this.validatedElement.CompositionTag);
        }

        [TestMethod]
        public void ThenReturnsSingleValidationDescriptor()
        {
            Assert.AreEqual(1, this.validatedElement.GetValidatorDescriptors().Count());
        }

        [TestMethod]
        public void WhenCreatingAttributeFromReturnedValidationDescriptor_ThenCreatesValidationAttributeValidatorForTheSingleAttribute()
        {
            var validator = this.validatedElement.GetValidatorDescriptors().ElementAt(0).CreateValidator(null, null, null, null);

            Assert.IsInstanceOfType(validator, typeof(ValidationAttributeValidator));
            Assert.IsFalse(validator.Validate(100).IsValid);
            Assert.AreEqual("range", validator.Validate(100).ElementAt(0).Message);
        }
    }

    [TestClass]
    public class GivenValidationAttributeValidatedElementOnFieldWithMultipleValidationAttributes
    {
        private IValidatedElement validatedElement;

        [TestInitialize]
        public void Setup()
        {
            this.validatedElement =
                new ValidationAttributeValidatedElement(
                    StaticReflection.GetFieldInfo((TypeWithValidationAttributes t)
                        => t.FieldWithMultipleValidationAttributes));
        }

        [TestMethod]
        public void ThenReturnsConfiguredMember()
        {
            Assert.AreSame(
                StaticReflection.GetFieldInfo((TypeWithValidationAttributes t) => t.FieldWithMultipleValidationAttributes),
                this.validatedElement.MemberInfo);
        }

        [TestMethod]
        public void ThenReturnsTargetType()
        {
            Assert.AreSame(typeof(string), this.validatedElement.TargetType);
        }

        [TestMethod]
        public void ThenDoesNotIgnoreNulls()
        {
            Assert.IsFalse(this.validatedElement.IgnoreNulls);
            Assert.IsNull(this.validatedElement.IgnoreNullsMessageTemplate);
            Assert.IsNull(this.validatedElement.IgnoreNullsTag);
        }

        [TestMethod]
        public void ThenReturnsAndComposition()
        {
            Assert.AreEqual(CompositionType.And, this.validatedElement.CompositionType);
            Assert.IsNull(this.validatedElement.CompositionMessageTemplate);
            Assert.IsNull(this.validatedElement.CompositionTag);
        }

        [TestMethod]
        public void ThenReturnsSingleValidationDescriptor()
        {
            Assert.AreEqual(1, this.validatedElement.GetValidatorDescriptors().Count());
        }

        [TestMethod]
        public void WhenCreatingAttributeFromReturnedValidationDescriptor_ThenCreatesValidationAttributeValidatorForTheSingleAttribute()
        {
            var validator = this.validatedElement.GetValidatorDescriptors().ElementAt(0).CreateValidator(null, null, null, null);

            Assert.IsInstanceOfType(validator, typeof(ValidationAttributeValidator));
            Assert.IsFalse(validator.Validate("bbbbbbbbbbbbbbbbbbbb").IsValid);
            Assert.IsTrue(validator.Validate("bbbbbbbbbbbbbbbbbbbb").Any(vr => vr.Message == "length"));
            Assert.IsTrue(validator.Validate("bbbbbbbbbbbbbbbbbbbb").Any(vr => vr.Message == "regex"));
        }
    }

    [TestClass]
    public class GivenValidationAttributeValidatedTypeOnTypeWithValidationAttributes
    {
        private IValidatedType validatedType;

        [TestInitialize]
        public void Setup()
        {
            this.validatedType = new ValidationAttributeValidatedType(typeof(TypeWithValidationAttributes));
        }

        [TestMethod]
        public void ThenReturnsConfiguredMember()
        {
            Assert.AreSame(typeof(TypeWithValidationAttributes), this.validatedType.MemberInfo);
        }

        [TestMethod]
        public void ThenReturnsTargetType()
        {
            Assert.AreSame(typeof(TypeWithValidationAttributes), this.validatedType.TargetType);
        }

        [TestMethod]
        public void ThenDoesNotIgnoreNulls()
        {
            Assert.IsFalse(this.validatedType.IgnoreNulls);
            Assert.IsNull(this.validatedType.IgnoreNullsMessageTemplate);
            Assert.IsNull(this.validatedType.IgnoreNullsTag);
        }

        [TestMethod]
        public void ThenReturnsAndComposition()
        {
            Assert.AreEqual(CompositionType.And, this.validatedType.CompositionType);
            Assert.IsNull(this.validatedType.CompositionMessageTemplate);
            Assert.IsNull(this.validatedType.CompositionTag);
        }

        [TestMethod]
        public void ThenReturnsNoValidationDescriptors()
        {
            Assert.AreEqual(0, this.validatedType.GetValidatorDescriptors().Count());
        }

        [TestMethod]
        public void ThenReturnsNoSelfValidationMethods()
        {
            Assert.AreEqual(0, this.validatedType.GetSelfValidationMethods().Count());
        }

        [TestMethod]
        public void ThenReturnsNoValidatedMethods()
        {
            Assert.AreEqual(0, this.validatedType.GetValidatedMethods().Count());
        }

        [TestMethod]
        public void ThenReturnsValidatedProperties()
        {
            var validatedProperties = this.validatedType.GetValidatedProperties();

            Assert.AreEqual(2, validatedProperties.Count());
        }

        [TestMethod]
        public void ThenValidatedPropertiesContainTheExpectedDescriptors()
        {
            var validatedProperties = this.validatedType.GetValidatedProperties();

            Assert.AreEqual(
                0,
                validatedProperties
                    .Where(vp => vp.MemberInfo ==
                        StaticReflection.GetPropertyInfo(
                            (TypeWithValidationAttributes t) => t.PropertyWithNoValidationAttributes))
                    .First()
                    .GetValidatorDescriptors()
                    .Count());

            Assert.AreEqual(
                1,
                validatedProperties
                    .Where(vp => vp.MemberInfo ==
                        StaticReflection.GetPropertyInfo(
                            (TypeWithValidationAttributes t) => t.PropertyWithSingleValidationAttribute))
                    .First()
                    .GetValidatorDescriptors()
                    .Count());
        }

        [TestMethod]
        public void ThenReturnsValidatedFields()
        {
            var validatedFields = this.validatedType.GetValidatedFields();

            Assert.AreEqual(2, validatedFields.Count());
        }

        [TestMethod]
        public void ThenValidatedFieldsContainTheExpectedDescriptors()
        {
            var validatedFields = this.validatedType.GetValidatedFields();

            Assert.AreEqual(
                0,
                validatedFields
                    .Where(vf => vf.MemberInfo ==
                        StaticReflection.GetFieldInfo(
                            (TypeWithValidationAttributes t) => t.FieldWithNoValidationAttributes))
                    .First()
                    .GetValidatorDescriptors()
                    .Count());

            Assert.AreEqual(
                1,
                validatedFields
                    .Where(vf => vf.MemberInfo ==
                        StaticReflection.GetFieldInfo(
                            (TypeWithValidationAttributes t) => t.FieldWithMultipleValidationAttributes))
                    .First()
                    .GetValidatorDescriptors()
                    .Count());
        }
    }

    public class TypeWithValidationAttributes
    {
        public int PropertyWithNoValidationAttributes { get; set; }

        [Range(1, 20, ErrorMessage = "range")]
        public int PropertyWithSingleValidationAttribute { get; set; }

        public string FieldWithNoValidationAttributes;

        [StringLength(10, ErrorMessage = "length")]
        [RegularExpression("a*", ErrorMessage = "regex")]
        public string FieldWithMultipleValidationAttributes;
    }

    public static class StaticReflection
    {
        public static PropertyInfo GetPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return (PropertyInfo)((MemberExpression)expression.Body).Member;
        }

        public static FieldInfo GetFieldInfo<T, TField>(Expression<Func<T, TField>> expression)
        {
            return (FieldInfo)((MemberExpression)expression.Body).Member;
        }
    }

}
