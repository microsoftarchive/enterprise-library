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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests
{
    [TestClass]
    public class ValidatedControlItemFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            MockValidator<object>.ResetCaches();
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void ValidatedControlItemCanGetValueFromDefaultProperty()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "value";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            object value;
            string failureMessage;
            bool status = validatedControlItem.GetValue(out value, out failureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual("value", value);
            Assert.AreEqual(null, failureMessage);
        }

        [TestMethod]
        public void ValidatedControlItemCanGetValueFromNonDefaultProperty()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "value";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.ValidatedPropertyName = "IntControlProperty";
            validatedControlItem.SourcePropertyName = "IntProperty";

            object value;
            string failureMessage;
            bool status = validatedControlItem.GetValue(out value, out failureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual(5, value);
            Assert.AreEqual(null, failureMessage);
        }

        [TestMethod]
        public void ValidatedControlItemCanGetConvertedValueFromDefaultProperty()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "5";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "IntProperty";

            object value;
            string failureMessage;
            bool status = validatedControlItem.GetValue(out value, out failureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual(5, value);
            Assert.AreEqual(null, failureMessage);
        }

        [TestMethod]
        public void ValidatedControlItemCanGetConvertedValueUsingCustomVersionFromDefaultProperty()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.ValueConvert += OnValueConvert;
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "5";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "IntProperty";

            object value;
            string failureMessage;
            bool status = validatedControlItem.GetValue(out value, out failureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual("converted value", value);
            Assert.AreEqual(null, failureMessage);
        }

        [TestMethod]
        public void ValidatedControlItemCanConvertedGetValueFromNonDefaultProperty()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "value";
            control.IntControlProperty = 5;
            control.StringControlProperty = "5";
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.ValidatedPropertyName = "StringControlProperty";
            validatedControlItem.SourcePropertyName = "DoubleProperty";

            object value;
            string failureMessage;
            bool status = validatedControlItem.GetValue(out value, out failureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual(5d, value);
            Assert.AreEqual(null, failureMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RequestForValueFromInvalidNonDefaultPropertyThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "value";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.ValidatedPropertyName = "InvalidControlProperty";
            validatedControlItem.SourcePropertyName = "IntProperty";

            object value;
            string failureMessage;
            bool status = validatedControlItem.GetValue(out value, out failureMessage);
        }

        [TestMethod]
        public void RetunsExpectedValuesForIValidationIntegrationProxyImplementation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.RulesetName = "ruleset";
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            validationProvider.SpecificationSource = ValidationSpecificationSource.Configuration;
            MockControl control = new MockControl();
            control.Text = "value";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.ValidatedPropertyName = "Text";
            validatedControlItem.SourcePropertyName = "IntProperty";

            IValidationIntegrationProxy proxy = validatedControlItem;

            Assert.AreSame(typeof(ValidatedControlItemFixtureTestClass), proxy.ValidatedType);
            Assert.AreEqual("IntProperty", proxy.ValidatedPropertyName);
            Assert.AreEqual(ValidationSpecificationSource.Configuration, proxy.SpecificationSource);
            Assert.AreEqual("ruleset", proxy.Ruleset);
            Assert.AreSame(typeof(PropertyMappedControlValueAccessBuilder), proxy.GetMemberValueAccessBuilder().GetType());
        }

        [TestMethod]
        public void CanValidateInstanceWithOwnValidator()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidatedControlItemFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "value";
            control.IntControlProperty = 5;
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            Validator validator = validatedControlItem.Validator;
            Assert.IsNotNull(validator);

            ValidationResults validationResults = validator.Validate(validatedControlItem);

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("StringProperty message", resultsList[0].Message);
            Assert.AreEqual("value", MockValidator<object>.CreatedValidators[0].ValidatedTargets[0]);
        }

        void OnValueConvert(object source,
                            ValueConvertEventArgs e)
        {
            e.ConvertedValue = "converted value";
        }

        public class ValidatedControlItemFixtureTestClass
        {
            public double DoubleProperty
            {
                get { return 0d; }
            }

            public int IntProperty
            {
                get { return 0; }
            }

            [MockValidator(true, MessageTemplate = "StringProperty message")]
            public string StringProperty
            {
                get { return null; }
            }
        }
    }
}
