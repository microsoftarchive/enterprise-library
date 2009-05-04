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
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests
{
    [TestClass]
    public class PropertyMappedControlValueAccessFixture
    {
        object valueToConvert;

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GettingValueFromNonControlObjectThrows()
        {
            PropertyMappedControlValueAccess valueAccess = new PropertyMappedControlValueAccess("StringProperty");

            object value;
            string valueAccessFailureMessage;
            bool status = valueAccess.GetValue("not a control", out value, out valueAccessFailureMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GettingValueForNonMappedPropertyThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(PropertyMappedControlValueAccessFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "control text";
            validationProvider.SetSourcePropertyName(control, "StringProperty");

            PropertyMappedControlValueAccess valueAccess = new PropertyMappedControlValueAccess("NonMappedProperty");

            object value;
            string valueAccessFailureMessage;
            bool status = valueAccess.GetValue(validationProvider.GetExistingValidatedControlItem(control), out value, out valueAccessFailureMessage);
        }

        [TestMethod]
        public void GetValueFromSuppliedControl()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(PropertyMappedControlValueAccessFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "control text";
            validationProvider.SetSourcePropertyName(control, "StringProperty");

            PropertyMappedControlValueAccess valueAccess = new PropertyMappedControlValueAccess("StringProperty");

            object value;
            string valueAccessFailureMessage;
            bool status = valueAccess.GetValue(validationProvider.GetExistingValidatedControlItem(control), out value, out valueAccessFailureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual("control text", value);
        }

        [TestMethod]
        public void GetsValueFromOtherRegisteredControl()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(PropertyMappedControlValueAccessFixtureTestClass).AssemblyQualifiedName;
            MockControl control1 = new MockControl();
            control1.Text = "control1 text";
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            control2.Text = "control2 text";
            validationProvider.SetSourcePropertyName(control2, "ExtraStringProperty");

            PropertyMappedControlValueAccess valueAccess = new PropertyMappedControlValueAccess("ExtraStringProperty");

            object value;
            string valueAccessFailureMessage;
            bool status = valueAccess.GetValue(validationProvider.GetExistingValidatedControlItem(control1), out value, out valueAccessFailureMessage);

            Assert.AreEqual("control2 text", value);
        }

        [TestMethod]
        public void ValueConversionIsPerformedWhenRequestingValueFromValueAccess()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(PropertyMappedControlValueAccessFixtureTestClass).AssemblyQualifiedName;
            validationProvider.ValueConvert += OnValueConvert;
            MockControl control = new MockControl();
            control.Text = "control text";
            validationProvider.SetSourcePropertyName(control, "StringProperty");

            PropertyMappedControlValueAccess valueAccess = new PropertyMappedControlValueAccess("StringProperty");

            object value;
            string valueAccessFailureMessage;
            bool status = valueAccess.GetValue(validationProvider.GetExistingValidatedControlItem(control), out value, out valueAccessFailureMessage);

            Assert.AreEqual("converted control text", value);
            Assert.AreEqual("control text", valueToConvert);
        }

        [TestMethod]
        public void CanValidateThroughValueAccessValidatorUsingValueAccess()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(PropertyMappedControlValueAccessFixtureTestClass).AssemblyQualifiedName;
            MockControl control = new MockControl();
            control.Text = "control text";
            validationProvider.SetSourcePropertyName(control, "StringProperty");

            MockValidator valueValidator = new MockValidator(true, "message");
            Validator validator = new ValueAccessValidator(new PropertyMappedControlValueAccess("StringProperty"), valueValidator);

            ValidationResults validationResults
                = validator.Validate(validationProvider.GetExistingValidatedControlItem(control));

            Assert.IsFalse(validationResults.IsValid);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.AreEqual("StringProperty", resultsList[0].Key);
            Assert.AreEqual("message", resultsList[0].Message);
            Assert.AreEqual(null, resultsList[0].Tag);
            Assert.AreSame(validationProvider.GetExistingValidatedControlItem(control), resultsList[0].Target);
            Assert.AreSame(valueValidator, resultsList[0].Validator);
            Assert.AreEqual("control text", valueValidator.ValidatedTargets[0]);
        }

        void OnValueConvert(object source,
                            ValueConvertEventArgs args)
        {
            valueToConvert = args.ValueToConvert;
            args.ConvertedValue = "converted control text";
        }

        public class PropertyMappedControlValueAccessFixtureTestClass
        {
            string extraStringProperty;
            string stringProperty;

            public string ExtraStringProperty
            {
                get { return extraStringProperty; }
                set { extraStringProperty = value; }
            }

            public string StringProperty
            {
                get { return stringProperty; }
                set { stringProperty = value; }
            }
        }
    }
}
