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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ConfigurationValidatedElementFixture
    {
        [TestMethod]
        public void NewInstanceIsClean()
        {
            ConfigurationValidatedElement validatedElement = new ConfigurationValidatedElement();

            Assert.IsNull(((IValidatedElement)validatedElement).TargetType);
            Assert.IsNull(((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator
                = ((IValidatedElement)validatedElement).GetValidatorDescriptors().GetEnumerator();
            Assert.IsNotNull(validatorDescriptorsEnumerator);
            Assert.IsFalse(validatorDescriptorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedPropertyReferenceReturnsCorrectValues()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("Property");
            propertyReference.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference.Validators.Get("validator1").MessageTemplate = "property validator 1 message";
            propertyReference.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference.Validators.Get("validator2").MessageTemplate = "property validator 2 message";

            ConfigurationValidatedElement validatedElement = new ConfigurationValidatedElement();
            PropertyInfo propertyInfo = typeof(ConfigurationValidatedElementFixtureTestClass).GetProperty("Property");

            validatedElement.UpdateFlyweight(propertyReference, propertyInfo);

            Assert.AreSame(typeof(string), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(propertyInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(null, ((IValidatedElement)validatedElement).CompositionMessageTemplate);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);
            Assert.AreEqual(null, ((IValidatedElement)validatedElement).IgnoreNullsMessageTemplate);

            IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator
                = ((IValidatedElement)validatedElement).GetValidatorDescriptors().GetEnumerator();
            Assert.IsNotNull(validatorDescriptorsEnumerator);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("property validator 1 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("property validator 2 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsFalse(validatorDescriptorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedFieldReferenceReturnsCorrectValues()
        {
            ValidatedFieldReference fieldReference = new ValidatedFieldReference("Field");
            fieldReference.Validators.Add(new MockValidatorData("validator1", false));
            fieldReference.Validators.Get("validator1").MessageTemplate = "field validator 1 message";
            fieldReference.Validators.Add(new MockValidatorData("validator2", false));
            fieldReference.Validators.Get("validator2").MessageTemplate = "field validator 2 message";
            fieldReference.Validators.Add(new MockValidatorData("validator3", false));
            fieldReference.Validators.Get("validator3").MessageTemplate = "field validator 3 message";

            ConfigurationValidatedElement validatedElement = new ConfigurationValidatedElement();
            FieldInfo fieldInfo = typeof(ConfigurationValidatedElementFixtureTestClass).GetField("Field");

            validatedElement.UpdateFlyweight(fieldReference, fieldInfo);

            Assert.AreSame(typeof(int), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(fieldInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator
                = ((IValidatedElement)validatedElement).GetValidatorDescriptors().GetEnumerator();
            Assert.IsNotNull(validatorDescriptorsEnumerator);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("field validator 1 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("field validator 2 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("field validator 3 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsFalse(validatorDescriptorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedMethodReferenceReturnsCorrectValues()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("Method");
            methodReference.Validators.Add(new MockValidatorData("validator1", false));
            methodReference.Validators.Get("validator1").MessageTemplate = "method validator 1 message";
            methodReference.Validators.Add(new MockValidatorData("validator2", false));
            methodReference.Validators.Get("validator2").MessageTemplate = "method validator 2 message";
            methodReference.Validators.Add(new MockValidatorData("validator3", false));
            methodReference.Validators.Get("validator3").MessageTemplate = "method validator 3 message";
            methodReference.Validators.Add(new MockValidatorData("validator4", false));
            methodReference.Validators.Get("validator4").MessageTemplate = "method validator 4 message";

            ConfigurationValidatedElement validatedElement = new ConfigurationValidatedElement();
            MethodInfo methodInfo = typeof(ConfigurationValidatedElementFixtureTestClass).GetMethod("Method");

            validatedElement.UpdateFlyweight(methodReference, methodInfo);

            Assert.AreSame(typeof(DateTime), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(methodInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator
                = ((IValidatedElement)validatedElement).GetValidatorDescriptors().GetEnumerator();
            Assert.IsNotNull(validatorDescriptorsEnumerator);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("method validator 1 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("method validator 2 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("method validator 3 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("method validator 4 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsFalse(validatorDescriptorsEnumerator.MoveNext());
        }

        // just to show how it would work if the flyweight was updated - shouldn't happen in normal use
        [TestMethod]
        public void IterationContinuesAfterFlyweightUpdate()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("Property");
            propertyReference.Validators.Add(new MockValidatorData("validator1", false));
            propertyReference.Validators.Get("validator1").MessageTemplate = "property validator 1 message";
            propertyReference.Validators.Add(new MockValidatorData("validator2", false));
            propertyReference.Validators.Get("validator2").MessageTemplate = "property validator 2 message";
            ValidatedFieldReference fieldReference = new ValidatedFieldReference("Field");
            fieldReference.Validators.Add(new MockValidatorData("validator1", false));
            fieldReference.Validators.Get("validator1").MessageTemplate = "field validator 1 message";
            fieldReference.Validators.Add(new MockValidatorData("validator2", false));
            fieldReference.Validators.Get("validator2").MessageTemplate = "field validator 2 message";
            fieldReference.Validators.Add(new MockValidatorData("validator3", false));
            fieldReference.Validators.Get("validator3").MessageTemplate = "field validator 3 message";

            ConfigurationValidatedElement validatedElement = new ConfigurationValidatedElement();
            PropertyInfo propertyInfo = typeof(ConfigurationValidatedElementFixtureTestClass).GetProperty("Property");
            FieldInfo fieldInfo = typeof(ConfigurationValidatedElementFixtureTestClass).GetField("Field");

            validatedElement.UpdateFlyweight(propertyReference, propertyInfo);
            Assert.AreSame(typeof(string), ((IValidatedElement)validatedElement).TargetType);
            IEnumerator<IValidatorDescriptor> propertyValidatorDescriptorsEnumerator
                = ((IValidatedElement)validatedElement).GetValidatorDescriptors().GetEnumerator();
            Assert.IsTrue(propertyValidatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("property validator 1 message",
                            ((MockValidatorData)propertyValidatorDescriptorsEnumerator.Current).MessageTemplate);

            validatedElement.UpdateFlyweight(fieldReference, fieldInfo);
            Assert.AreSame(typeof(int), ((IValidatedElement)validatedElement).TargetType);
            IEnumerator<IValidatorDescriptor> fieldValidatorDescriptorsEnumerator
                = ((IValidatedElement)validatedElement).GetValidatorDescriptors().GetEnumerator();
            Assert.IsTrue(fieldValidatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("field validator 1 message",
                            ((MockValidatorData)fieldValidatorDescriptorsEnumerator.Current).MessageTemplate);

            Assert.IsTrue(propertyValidatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("property validator 2 message",
                            ((MockValidatorData)propertyValidatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(fieldValidatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("field validator 2 message",
                            ((MockValidatorData)fieldValidatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsFalse(propertyValidatorDescriptorsEnumerator.MoveNext());
            Assert.IsTrue(fieldValidatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("field validator 3 message",
                            ((MockValidatorData)fieldValidatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsFalse(fieldValidatorDescriptorsEnumerator.MoveNext());
        }

        public class ConfigurationValidatedElementFixtureTestClass
        {
            public int Field;

            public string Property
            {
                get { return null; }
            }

            public DateTime Method()
            {
                return default(DateTime);
            }
        }
    }
}
