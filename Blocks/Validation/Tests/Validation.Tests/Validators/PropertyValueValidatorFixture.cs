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
    public class PropertyValueValidatorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullPropertyNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>(null, valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithEmptyPropertyNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>("", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonExistingPropertyNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>("NonExistingProperty", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonPublicPropertyNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>("NonPublicProperty", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNonReadablePropertyNameThrows()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>("NonReadableProperty", valueValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullValueValidatorThrows()
        {
            MockValidator<object> valueValidator = null;
            new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>("PublicProperty", valueValidator);
        }

        [TestMethod]
        public void ValidatesValueProperty()
        {
            MockValidator<object> valueValidator = new MockValidator<object>(false);
            Validator validator
                = new PropertyValueValidator<PropertyValueValidatorFixtureTestClass>("PublicProperty", valueValidator);

            validator.Validate(new PropertyValueValidatorFixtureTestClass());

            Assert.AreEqual(PropertyValueValidatorFixtureTestClass.value, valueValidator.ValidatedTargets[0]);
        }

        public class PropertyValueValidatorFixtureTestClass
        {
            public const string value = "value";

            internal string NonPublicProperty
            {
                get { return null; }
            }

            public string NonReadableProperty
            {
                set { }
            }

            public string PublicProperty
            {
                get { return value; }
            }
        }
    }
}
