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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValidationReflectorHelperFixture
    {
        [TestMethod]
        public void CanGetAttributesForTypeWithNoAttributes()
        {
            var attributes = ValidationReflectionHelper.GetCustomAttributes(typeof(TypeWithNoAttributes), typeof(BaseAttribute), false);

            Assert.AreEqual(0, attributes.Length);
        }

        [TestMethod]
        public void CanGetAttributesForTypeWithAttributesOnTheActualType()
        {
            var attributes = ValidationReflectionHelper.GetCustomAttributes(typeof(TypeWithAttributes), typeof(BaseAttribute), false);

            Assert.AreEqual(2, attributes.Length);
        }

        [TestMethod]
        public void CanGetAttributesForTypeWithAttributesOnMetadataType()
        {
            var attributes = ValidationReflectionHelper.GetCustomAttributes(typeof(TypeWithAttributesOnMetadataType), typeof(BaseAttribute), false);

            Assert.AreEqual(2, attributes.Length);
        }

        [TestMethod]
        public void CanGetAttributesForPropertyOnTypeWithNoMetadataType()
        {
            var property = typeof(TypeWithPropertyWithAttributes).GetProperty("MyProperty");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(property, typeof(BaseAttribute), false);

            Assert.AreEqual(1, attributes.Length);
        }

        [TestMethod]
        public void CanGetAttributesForPropertyOnTypeWithMetadataTypeWithNoMatchingProperty()
        {
            var property = typeof(TypeWithMetadataTypeWithNoMatchingProperty).GetProperty("MyProperty");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(property, typeof(BaseAttribute), false);

            Assert.AreEqual(0, attributes.Length);
        }

        [TestMethod]
        public void CanGetAttributesForPropertyOnTypeWithMetadataTypeWithMatchingProperty()
        {
            var property = typeof(TypeWithMetadataTypeWithMatchingProperty).GetProperty("MyProperty");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(property, typeof(BaseAttribute), false);

            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual("from metadata", ((DerivedAttribute)attributes[0]).Description);
        }

        [TestMethod]
        public void GetsAttributesFromOriginalPropertyIfTypeHasMetadataTypeWithNoMatchingProperty()
        {
            var property =
                typeof(TypeWithPropertyWithAttributesAndMetadataTypeWithNoMatchingProperty).GetProperty("MyProperty");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(property, typeof(BaseAttribute), false);

            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual("from main", ((DerivedAttribute)attributes[0]).Description);
        }

        [TestMethod]
        public void GetsAttributesFromThePropertyOnTheMetadataTypeOnlyIfBothTheActualPropertyAndTheMatchingMetadataPropertyHaveAttributes()
        {
            var property = typeof(TypeWithMetadataTypeWithMatchingPropertyAndAttributesOnBothProperties).GetProperty("MyProperty");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(property, typeof(BaseAttribute), false);

            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual("from metadata", ((DerivedAttribute)attributes[0]).Description);
        }

        [TestMethod]
        public void CanGetAttributesForFieldOnTypeWithMetadataTypeWithMatchingField()
        {
            var field = typeof(TypeWithMetadataTypeWithMatchingField).GetField("MyField");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(field, typeof(BaseAttribute), false);

            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual("from metadata", ((DerivedAttribute)attributes[0]).Description);
        }

        [TestMethod]
        public void CanGetAttributesForMethodOnTypeWithMetadataTypeWithMatchingMethod()
        {
            var method = typeof(TypeWithMetadataTypeWithMatchingMethod).GetMethod("MyMethod");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(method, typeof(BaseAttribute), false);

            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual("from metadata", ((DerivedAttribute)attributes[0]).Description);
        }

        [TestMethod]
        public void MatchesMethodsOnMetadataTypeBasedOnParameterTypes()
        {
            var method = typeof(TypeWithMetadataTypeWithMatchingMethodWithDifferentSignatures).GetMethod("MyMethod");
            var attributes = ValidationReflectionHelper.GetCustomAttributes(method, typeof(BaseAttribute), false);

            Assert.AreEqual(0, attributes.Length);
        }
    }

    public class TypeWithNoAttributes { }

    [Derived]
    [Derived]
    public class TypeWithAttributes { }

    [MetadataType(typeof(TypeWithAttributesOnMetadataTypeMetadata))]
    public class TypeWithAttributesOnMetadataType
    {
        [Derived]
        [Derived]
        private class TypeWithAttributesOnMetadataTypeMetadata
        {
        }
    }

    public class TypeWithPropertyWithAttributes
    {
        [Derived]
        public int MyProperty { get; set; }
    }

    [MetadataType(typeof(TypeWithMetadataTypeWithNoMatchingPropertyMetadata))]
    public class TypeWithMetadataTypeWithNoMatchingProperty
    {
        private class TypeWithMetadataTypeWithNoMatchingPropertyMetadata
        {
        }

        public int MyProperty { get; set; }
    }

    [MetadataType(typeof(TypeWithMetadataTypeWithMatchingPropertyMetadata))]
    public class TypeWithMetadataTypeWithMatchingProperty
    {
        private class TypeWithMetadataTypeWithMatchingPropertyMetadata
        {
            [Derived(Description = "from metadata")]
            public int MyProperty { get; set; }
        }

        public int MyProperty { get; set; }
    }

    [MetadataType(typeof(TypeWithPropertyWithAttributesAndMetadataTypeWithNoMatchingPropertyMetadata))]
    public class TypeWithPropertyWithAttributesAndMetadataTypeWithNoMatchingProperty
    {
        private class TypeWithPropertyWithAttributesAndMetadataTypeWithNoMatchingPropertyMetadata
        {
        }

        [Derived(Description = "from main")]
        public int MyProperty { get; set; }
    }

    [MetadataType(typeof(TypeWithMetadataTypeWithMatchingPropertyAndAttributesOnBothPropertiesMetadata))]
    public class TypeWithMetadataTypeWithMatchingPropertyAndAttributesOnBothProperties
    {
        private class TypeWithMetadataTypeWithMatchingPropertyAndAttributesOnBothPropertiesMetadata
        {
            [Derived(Description = "from metadata")]
            public int MyProperty { get; set; }
        }

        [Derived(Description = "from main")]
        public int MyProperty { get; set; }
    }

    [MetadataType(typeof(TypeWithMetadataTypeWithMatchingFieldMetadata))]
    public class TypeWithMetadataTypeWithMatchingField
    {
        private class TypeWithMetadataTypeWithMatchingFieldMetadata
        {
            // Unused field warning - used for reflection only
#pragma warning disable 649
            [Derived(Description = "from metadata")]
            public int MyField;
#pragma warning restore 649
        }

        public int MyField;
    }

    [MetadataType(typeof(TypeWithMetadataTypeWithMatchingMethodMetadata))]
    public class TypeWithMetadataTypeWithMatchingMethod
    {
        private class TypeWithMetadataTypeWithMatchingMethodMetadata
        {
            [Derived(Description = "from metadata")]
            public int MyMethod()
            {
                return 0;
            }
        }

        public int MyMethod()
        {
            return 0;
        }
    }

    [MetadataType(typeof(TypeWithMetadataTypeWithMatchingMethodWithDifferentSignaturesMetadata))]
    public class TypeWithMetadataTypeWithMatchingMethodWithDifferentSignatures
    {
        private class TypeWithMetadataTypeWithMatchingMethodWithDifferentSignaturesMetadata
        {
            [Derived(Description = "from metadata")]
            public int MyMethod(string ignored)
            {
                return 0;
            }
        }

        public int MyMethod()
        {
            return 0;
        }
    }

    public class BaseAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DerivedAttribute : BaseAttribute
    {
        public string Description { get; set; }
    }
}
