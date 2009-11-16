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

using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWC = System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Tests
{
    [TestClass]
    public class ValidatorRuleAttachmentFixture
    {
        [TestMethod]
        public void SettingTheAttachedPropertyToTheNameOfABoundPropertyOnAnInitializedElementAddsValidatorRuleToTheReferencedBinding()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            var binding = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            textBox.EndInit();

            Validate.SetBindingFor(textBox, "Text");

            Assert.AreEqual(1, binding.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void SettingTheAttachedPropertyToTheNameOfABoundPropertyOnANonInitializedElementDoesNotAddValidatorRuleToTheReferencedBinding()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            var binding = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);

            Validate.SetBindingFor(textBox, "Text");

            Assert.AreEqual(0, binding.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void SettingTheAttachedPropertyToTheNameOfABoundPropertyOnANonInitializedElementAddsValidatorRuleToTheReferencedBindingOnInitialize()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            var binding = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);

            Validate.SetBindingFor(textBox, "Text");

            textBox.EndInit();

            Assert.AreEqual(1, binding.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void ClearingTheAttachedPropertyRemovesTheValidationRule()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            var binding = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            textBox.EndInit();

            Validate.SetBindingFor(textBox, "Text");

            Assert.AreEqual(1, binding.ValidationRules.OfType<ValidatorRule>().Count());

            textBox.ClearValue(Validate.BindingForProperty);

            Assert.AreEqual(0, binding.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void ChangingTheAttachedPropertyRemovesTheValidationRule()
        {
            var textBox = new TextBox();

            textBox.BeginInit();
            var binding1 = new Binding("ValidatedIntProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.MaxLinesProperty, binding1);
            var binding2 = new Binding("ValidatedIntProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.MaxLengthProperty, binding2);
            textBox.EndInit();

            Validate.SetBindingFor(textBox, "MaxLines");

            Assert.AreEqual(1, binding1.ValidationRules.OfType<ValidatorRule>().Count());
            Assert.AreEqual(0, binding2.ValidationRules.OfType<ValidatorRule>().Count());

            Validate.SetBindingFor(textBox, "MaxLength");

            Assert.AreEqual(0, binding1.ValidationRules.OfType<ValidatorRule>().Count());
            Assert.AreEqual(1, binding2.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void BindingForPropertyNamedInAttachedProperyIsValidatedForInvalidValue()
        {
            var instance = new ValidatedObject();

            var textBox = new TextBox();
            textBox.BeginInit();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnExceptions = true
            };
            PresentationTraceSources.SetTraceLevel(binding, PresentationTraceLevel.High);
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            textBox.EndInit();

            Validate.SetBindingFor(textBox, "Text");

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));

            textBox.Text = "bbbbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
        }

        [TestMethod]
        public void BindingForPropertyNamedInAttachedProperyIsValidatedForValidValue()
        {
            var instance = new ValidatedObject();

            var textBox = new TextBox();
            textBox.BeginInit();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnExceptions = true
            };
            PresentationTraceSources.SetTraceLevel(binding, PresentationTraceLevel.High);
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            textBox.EndInit();

            Validate.SetBindingFor(textBox, "Text");

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));

            textBox.Text = "aaaaaaaa";

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));
        }
    }
}
