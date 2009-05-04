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
    public class FieldValueValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullFieldNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new FieldValueValidator<FieldValueValidatorFixtureTestClass>(null, valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithEmptyFieldNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new FieldValueValidator<FieldValueValidatorFixtureTestClass>("", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonExistingFieldNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new FieldValueValidator<FieldValueValidatorFixtureTestClass>("NonExistingField", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonPublicFieldNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new FieldValueValidator<FieldValueValidatorFixtureTestClass>("NonPublicField", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullValueValidatorThrows()
        {
            MockValidator<object> valueValidator = null;
            new FieldValueValidator<FieldValueValidatorFixtureTestClass>("PublicField", valueValidator);
        }

        [TestMethod]
        public void ValidatesValueField()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            Validator validator
                = new FieldValueValidator<FieldValueValidatorFixtureTestClass>("PublicField", valueValidator);

            validator.Validate(new FieldValueValidatorFixtureTestClass());

            Assert.AreEqual(FieldValueValidatorFixtureTestClass.value, valueValidator.ValidatedTargets[0]);
        }

        public class FieldValueValidatorFixtureTestClass
        {
            public const string value = "value";

            internal string NonPublicField = value;

            public string PublicField = value;
        }
    }
}
