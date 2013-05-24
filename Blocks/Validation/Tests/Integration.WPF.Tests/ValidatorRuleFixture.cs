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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWC = System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Tests
{
    [TestClass]
    public class ValidatorRuleFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingValidatorRuleWithNullBindingExpressionThrows()
        {
            new ValidatorRule(null);
        }

        [TestMethod]
        public void ValidatesBindingForPropertyInvalidStringValidatedProperty()
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
        public void ValidatesBindingForPropertyStringValidatedProperty()
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
        public void ValidatesBindingForPropertyInvalidConvertedValidatedProperty()
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
        public void ValidatesBindingForPropertyConvertedValidatedProperty()
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
        public void ValidatesBindingForPropertyConvertedNonValidatedProperty()
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingSourceTypeToNullThrows()
        {
            new ValidatorRule { SourceType = null };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SettingSourceTypeOnARuleInitializedWithABindingExpressionThrows()
        {
            var textBox = new TextBox();
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, new Binding("NonValidatedProperty"));
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);

            new ValidatorRule(bindingExpression) { SourceType = typeof(ValidatedObject) };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingSourcePropertyNameToNullThrows()
        {
            new ValidatorRule { SourcePropertyName = null };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SettingSourcePropertyNameOnARuleInitializedWithABindingExpressionThrows()
        {
            var textBox = new TextBox();
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, new Binding("NonValidatedProperty"));
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);

            new ValidatorRule(bindingExpression) { SourcePropertyName = "NonValidatedProperty" };
        }

        [TestMethod]
        public void CanValidateWithExplicitlySetTypeAndProperty()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            binding.ValidationRules.Add(
                new ValidatorRule
                {
                    SourceType = typeof(ValidatedObject),
                    SourcePropertyName = "ValidatedStringProperty"
                });

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string"));
        }

        [TestMethod]
        public void ValidatingWithARuleWithNoTypeSetThrows()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            binding.ValidationRules.Add(
                new ValidatorRule
                {
                    SourcePropertyName = "ValidatedStringProperty"
                });

            try
            {
                textBox.Text = "bbbb";
                Assert.Fail("should have thrown");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void ValidatingWithARuleWithNoSourcePropertySetThrows()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            binding.ValidationRules.Add(
                new ValidatorRule
                {
                    SourceType = typeof(ValidatedObject)
                });

            try
            {
                textBox.Text = "bbbb";
                Assert.Fail("should have thrown");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void ValidatingWithARuleWithInvalidSourcePropertySetThrows()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("ValidatedStringProperty") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            binding.ValidationRules.Add(
                new ValidatorRule
                {
                    SourceType = typeof(ValidatedObject),
                    SourcePropertyName = "Invalid"
                });

            try
            {
                textBox.Text = "bbbb";
                Assert.Fail("should have thrown");
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void ValidatesBindingForPropertyInvalidStringValidatedPropertyWithMultipleSources()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding =
                new Binding("MultipleSourceValidatedStringProperty")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(
                new ValidatorRule(bindingExpression)
                {
                    ValidationSpecificationSource = ValidationSpecificationSource.All
                });

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string: vab"));
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string: data annotations"));
        }

        [TestMethod]
        public void ValidatesBindingForPropertyInvalidStringValidatedPropertyWithFilteredSources()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding =
                new Binding("MultipleSourceValidatedStringProperty")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(
                new ValidatorRule(bindingExpression)
                {
                    ValidationSpecificationSource = ValidationSpecificationSource.Attributes
                });

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string: vab"));
            Assert.IsFalse(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string: data annotations"));
        }


        [TestMethod]
        public void ValidatesBindingForPropertyInvalidStringValidatedPropertyUsingDefaultRuleset()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("MultipleRulesetValidatedStringProperty")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression));

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string default"));
            Assert.IsFalse(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string ruleset"));
        }

        [TestMethod]
        public void ValidatesBindingForPropertyInvalidStringValidatedPropertyUsingNonDefaultRuleset()
        {
            var instance = new ValidatedObject { };

            var textBox = new TextBox();
            textBox.DataContext = instance;
            var binding = new Binding("MultipleRulesetValidatedStringProperty")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            binding.ValidationRules.Add(new ValidatorRule(bindingExpression) { RulesetName = "A" });

            textBox.Text = "bbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
            Assert.AreEqual(1, SWC.Validation.GetErrors(textBox).Count);
            Assert.IsFalse(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string default"));
            Assert.IsTrue(SWC.Validation.GetErrors(textBox)[0].ErrorContent.ToString().Contains("invalid string ruleset"));
        }
    }
}
