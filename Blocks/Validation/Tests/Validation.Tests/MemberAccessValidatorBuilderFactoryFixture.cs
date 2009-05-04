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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class MemberAccessValidatorBuilderFactoryFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValueAccessBuilderForNullPropertyThrows()
        {
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetPropertyValueAccessValidatorBuilder(null,
                                                                                                   new MockValidatedElement(false, null, null, CompositionType.And, null, null));
        }

        [TestMethod]
        public void CanGetValueAccessBuilderForProperty()
        {
            PropertyInfo propertyInfo = typeof(MemberAccessValidatorBuilderFactoryTestClass).GetProperty("Property");
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetPropertyValueAccessValidatorBuilder(propertyInfo,
                                                                                                   new MockValidatedElement(true, null, null, CompositionType.Or, null, null));

            Assert.IsNotNull(builder);
            Assert.AreEqual(true, builder.IgnoreNulls);
            Assert.AreEqual(CompositionType.Or, builder.CompositionType);
            Assert.AreSame(typeof(PropertyValueAccess), builder.ValueAccess.GetType());
            Assert.AreSame(propertyInfo, ((PropertyValueAccess)builder.ValueAccess).PropertyInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValueAccessBuilderForNullFieldThrows()
        {
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetFieldValueAccessValidatorBuilder(null,
                                                                                                new MockValidatedElement(false, null, null, CompositionType.And, null, null));
        }

        [TestMethod]
        public void CanGetValueAccessBuilderForField()
        {
            FieldInfo fieldInfo = typeof(MemberAccessValidatorBuilderFactoryTestClass).GetField("Field");
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetFieldValueAccessValidatorBuilder(fieldInfo,
                                                                                                new MockValidatedElement(true, null, null, CompositionType.Or, null, null));

            Assert.IsNotNull(builder);
            Assert.AreEqual(true, builder.IgnoreNulls);
            Assert.AreEqual(CompositionType.Or, builder.CompositionType);
            Assert.AreSame(typeof(FieldValueAccess), builder.ValueAccess.GetType());
            Assert.AreSame(fieldInfo, ((FieldValueAccess)builder.ValueAccess).FieldInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValueAccessBuilderForNullMethodThrows()
        {
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetMethodValueAccessValidatorBuilder(null,
                                                                                                 new MockValidatedElement(false, null, null, CompositionType.And, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetValueAccessBuilderForVoidMethodThrows()
        {
            MethodInfo methodInfo = typeof(MemberAccessValidatorBuilderFactoryTestClass).GetMethod("VoidMethod");
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetMethodValueAccessValidatorBuilder(methodInfo,
                                                                                                 new MockValidatedElement(false, null, null, CompositionType.And, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetValueAccessBuilderForMethodWithParametersThrows()
        {
            MethodInfo methodInfo = typeof(MemberAccessValidatorBuilderFactoryTestClass).GetMethod("MethodWithParameters");
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetMethodValueAccessValidatorBuilder(methodInfo,
                                                                                                 new MockValidatedElement(false, null, null, CompositionType.And, null, null));
        }

        [TestMethod]
        public void CanGetValueAccessBuilderForZeroParametersNotVoidMethod()
        {
            MethodInfo methodInfo = typeof(MemberAccessValidatorBuilderFactoryTestClass).GetMethod("Method");
            ValueAccessValidatorBuilder builder
                = new MemberAccessValidatorBuilderFactory().GetMethodValueAccessValidatorBuilder(methodInfo,
                                                                                                 new MockValidatedElement(true, null, null, CompositionType.Or, null, null));

            Assert.IsNotNull(builder);
            Assert.AreEqual(true, builder.IgnoreNulls);
            Assert.AreEqual(CompositionType.Or, builder.CompositionType);
            Assert.AreSame(typeof(MethodValueAccess), builder.ValueAccess.GetType());
            Assert.AreSame(methodInfo, ((MethodValueAccess)builder.ValueAccess).MethodInfo);
        }

        class MemberAccessValidatorBuilderFactoryTestClass
        {
            public string Field = null;

            public string Property
            {
                get { return null; }
            }

            public string Method()
            {
                return null;
            }

            public string MethodWithParameters(string parameter)
            {
                return null;
            }

            public void VoidMethod() {}
        }
    }
}
