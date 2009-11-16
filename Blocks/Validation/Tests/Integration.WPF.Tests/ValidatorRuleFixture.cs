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
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWC = System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Tests
{
    [TestClass]
    public class ValidatorRuleFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingValidatorRuleWithNullBindingExpressionThrows()
        {
            new ValidatorRule(null);
        }

        [TestMethod]
        public void ValidatesBindingForInvalidStringValidatedProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string"));
        }

        [TestMethod]
        public void ValidatesBindingForStringValidatedProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "aaaaaa";

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));
        }

        [TestMethod]
        public void ValidatesBindingForInvalidConvertedValidatedProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedIntProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "1";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid int"));
        }

        [TestMethod]
        public void NonConvertedValueIsNotValidatedForConvertedValidatedProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding =
                new Binding("ValidatedIntProperty")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    ValidatesOnExceptions = true    // necessary for the conversion error to become an error!
                };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "aaaa";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsFalse(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid int"));
        }

        [TestMethod]
        public void ValidatesBindingForConvertedValidatedProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedIntProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "15";

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));
        }

        [TestMethod]
        public void ValidatesBindingForConvertedNonValidatedProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("NonValidatedProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "15";

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(15, instance.NonValidatedProperty);
        }


        [TestMethod]
        public void CanValidateWithExplicitlySetTypeAndProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            binding.ValidationRules.Add(new ValidatorRule(typeof(ValidatedObject), "ValidatedStringProperty"));

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string"));
        }
    }


}
