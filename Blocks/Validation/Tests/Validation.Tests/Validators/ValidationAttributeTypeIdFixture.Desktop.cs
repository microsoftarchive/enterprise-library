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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class ValidationAttributeTypeIdFixture
    {
        [TestMethod]
        public void ValidationAttributesWithAllowMultipleOverrideTypeId()
        {
            var baseAttributeType = typeof(BaseValidationAttribute);
            var assembly = baseAttributeType.Assembly;

            foreach (var validationAttributeType
                in assembly.GetExportedTypes().Where(t => baseAttributeType.IsAssignableFrom(t) && !t.IsAbstract))
            {
                var attributeUsage =
                    (AttributeUsageAttribute)Attribute.GetCustomAttribute(validationAttributeType, typeof(AttributeUsageAttribute));

                if (attributeUsage != null && attributeUsage.AllowMultiple)
                {
                    Assert.IsNotNull(
                        validationAttributeType.GetProperty("TypeId", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public),
                        "TypeId is not overridden in " + validationAttributeType.FullName);
                }
            }
        }

        [TestMethod]
        public void MultipleInstancesCanBeRetrievedUsingComponentModel()
        {
            var property = TypeDescriptor.GetProperties(typeof(TypeWithValidationAttributes))["ValidatedProperty"];

            var baseAttributeType = typeof(BaseValidationAttribute);
            var assembly = baseAttributeType.Assembly;

            foreach (var validationAttributeType
                in assembly.GetExportedTypes().Where(t => baseAttributeType.IsAssignableFrom(t) && !t.IsAbstract))
            {
                var attributeUsage =
                    (AttributeUsageAttribute)Attribute.GetCustomAttribute(validationAttributeType, typeof(AttributeUsageAttribute));

                if (attributeUsage != null && attributeUsage.AllowMultiple)
                {
                    Assert.AreEqual(
                        2,
                        property.Attributes.OfType<Attribute>().Where(a => a.GetType() == validationAttributeType).Count(),
                        "Couldn't retrieve attributes for " + validationAttributeType.Name);
                }
            }
        }

        [HasSelfValidation]
        public class TypeWithValidationAttributes
        {
            [RelativeDateTimeValidator(10, DateTimeUnit.Day)]
            [RelativeDateTimeValidator(10, DateTimeUnit.Day)]
            [RangeValidator(0, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive)]
            [RangeValidator(0, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive)]
            [DateTimeRangeValidator("2011-01-01T00:00:00")]
            [DateTimeRangeValidator("2011-01-01T00:00:00")]
            [EnumConversionValidator(typeof(BindingFlags))]
            [EnumConversionValidator(typeof(BindingFlags))]
            [PropertyComparisonValidator("other", ComparisonOperator.Equal)]
            [PropertyComparisonValidator("other", ComparisonOperator.Equal)]
            [IgnoreNulls]
            [IgnoreNulls]
            [TypeConversionValidator(typeof(object))]
            [TypeConversionValidator(typeof(object))]
            [ContainsCharactersValidator("abc")]
            [ContainsCharactersValidator("abc")]
            [ValidatorComposition(CompositionType.And)]
            [ValidatorComposition(CompositionType.And)]
            [NotNullValidator]
            [NotNullValidator]
            [ObjectCollectionValidator]
            [ObjectCollectionValidator]
            [ObjectValidator]
            [ObjectValidator]
            [StringLengthValidator(10)]
            [StringLengthValidator(10)]
            [RegexValidator("test")]
            [RegexValidator("test")]
            [DomainValidator]
            [DomainValidator]
            public int ValidatedProperty { get; set; }

            [SelfValidation]
            [SelfValidation]
            public void SelfValidation() { }
        }
    }
}
