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
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class MethodReturnValueValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullMethodNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>(null, valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithEmptyMethodNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonExistingMethodNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("NonExistingMethod", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonPublicMethodNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("NonPublicMethod", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithVoidMethodNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("VoidMmethod", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithMethodWithParametersNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("PublicMethodWithParameters", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullValueValidatorThrows()
        {
            MockValidator<object> valueValidator = null;
            new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("PublicMethod", valueValidator);
        }

        [TestMethod]
        public void ValidatesValueMethod()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            Validator validator
                = new MethodReturnValueValidator<MethodReturnValueValidatorFixtureTestClass>("PublicMethod", valueValidator);

            validator.Validate(new MethodReturnValueValidatorFixtureTestClass());

            Assert.AreEqual(MethodReturnValueValidatorFixtureTestClass.value, valueValidator.ValidatedTargets[0]);
        }

        public class MethodReturnValueValidatorFixtureTestClass
        {
            public const string value = "value";

            internal string NonPublicMethod()
            {
                return value;
            }

            public string PublicMethod()
            {
                return value;
            }

            public string PublicMethodWithParameters(string param1,
                                                     string param2)
            {
                return value;
            }

            public void VoidMethod() {}
        }
    }
}
