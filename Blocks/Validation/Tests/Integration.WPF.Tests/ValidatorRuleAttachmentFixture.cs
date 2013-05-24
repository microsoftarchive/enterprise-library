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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWC = System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Tests
{
    [TestClass]
    public class ValidatorRuleAttachmentFixture
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

            Validate.SetBindingForProperty(textBox, "Text");

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

            Validate.SetBindingForProperty(textBox, "Text");

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

            Validate.SetBindingForProperty(textBox, "Text");

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

            Validate.SetBindingForProperty(textBox, "Text");

            Assert.AreEqual(1, binding.ValidationRules.OfType<ValidatorRule>().Count());

            textBox.ClearValue(Validate.BindingForPropertyProperty);

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

            Validate.SetBindingForProperty(textBox, "MaxLines");

            Assert.AreEqual(1, binding1.ValidationRules.OfType<ValidatorRule>().Count());
            Assert.AreEqual(0, binding2.ValidationRules.OfType<ValidatorRule>().Count());

            Validate.SetBindingForProperty(textBox, "MaxLength");

            Assert.AreEqual(0, binding1.ValidationRules.OfType<ValidatorRule>().Count());
            Assert.AreEqual(1, binding2.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void BindingForPropertyPropertyNamedInAttachedProperyIsValidatedForInvalidValue()
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

            Validate.SetBindingForProperty(textBox, "Text");

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));

            textBox.Text = "bbbbbb";

            Assert.IsTrue(SWC.Validation.GetHasError(textBox));
        }

        [TestMethod]
        public void BindingForPropertyPropertyNamedInAttachedProperyIsValidatedForValidValue()
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

            Validate.SetBindingForProperty(textBox, "Text");

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));

            textBox.Text = "aaaaaaaa";

            Assert.IsFalse(SWC.Validation.GetHasError(textBox));
        }

        [TestMethod]
        public void CanSetAValidationSpecificationSourceBeforeSettingTheBindingForPropertyProperty()
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

            Validate.SetUsingSource(textBox, ValidationSpecificationSource.Attributes);
            Validate.SetBindingForProperty(textBox, "Text");

            var rule = binding.ValidationRules.OfType<ValidatorRule>().FirstOrDefault();

            Assert.AreEqual(ValidationSpecificationSource.Attributes, rule.ValidationSpecificationSource);
        }

        [TestMethod]
        public void CanSetAValidationSpecificationSourceAfterSettingTheBindingForPropertyProperty()
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

            Validate.SetBindingForProperty(textBox, "Text");
            Validate.SetUsingSource(textBox, ValidationSpecificationSource.Attributes);

            var rule = binding.ValidationRules.OfType<ValidatorRule>().FirstOrDefault();

            Assert.AreEqual(ValidationSpecificationSource.Attributes, rule.ValidationSpecificationSource);
        }

        [TestMethod]
        public void CanSetARulesetNameBeforeSettingTheBindingForPropertyProperty()
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

            Validate.SetUsingRulesetName(textBox, "Ruleset");
            Validate.SetBindingForProperty(textBox, "Text");

            var rule = binding.ValidationRules.OfType<ValidatorRule>().FirstOrDefault();

            Assert.AreEqual("Ruleset", rule.RulesetName);
        }

        [TestMethod]
        public void CanSetARulesetNameAfterSettingTheBindingForPropertyProperty()
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

            Validate.SetBindingForProperty(textBox, "Text");
            Validate.SetUsingRulesetName(textBox, "Ruleset");

            var rule = binding.ValidationRules.OfType<ValidatorRule>().FirstOrDefault();

            Assert.AreEqual("Ruleset", rule.RulesetName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SettingTheAttachedPropertyToAValueThatIsNotADependencyPropertyNameThrows()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            textBox.EndInit();

            Validate.SetBindingForProperty(textBox, "InvalidPropertyName");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SettingTheAttachedPropertyTheNameOfAnUnboundDependencyPropertyThrows()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            textBox.EndInit();

            Validate.SetBindingForProperty(textBox, "Text");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SettingTheAttachedPropertyTheNameOfAnDependencyPropertyWithAComplexPathBindingThrows()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, new Binding("Complex.Path"));
            textBox.EndInit();

            Validate.SetBindingForProperty(textBox, "Text");
        }


        [TestMethod]
        public void AttachingValidationToPropertyWithMultiBindingAddsRulesToTheLeafLevelBindings()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            var binding1 = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            var binding2 = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(
                textBox,
                TextBox.TextProperty,
                new MultiBinding
                {
                    Bindings = { binding1, binding2 },
                    Converter = new TestConverter()
                });
            textBox.EndInit();

            Validate.SetBindingForProperty(textBox, "Text");

            Assert.AreEqual(1, binding1.ValidationRules.OfType<ValidatorRule>().Count());
            Assert.AreEqual(1, binding2.ValidationRules.OfType<ValidatorRule>().Count());
        }

        [TestMethod]
        public void AttachingValidationToPropertyWithPriorityBindingAddsRulesToTheLeafLevelBindings()
        {
            var textBox = new TextBox();
            textBox.BeginInit();
            var binding1 = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            var binding2 = new Binding("ValidatedStringProperty")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(
                textBox,
                TextBox.TextProperty,
                new PriorityBinding
                {
                    Bindings = { binding1, binding2 }
                });
            textBox.EndInit();

            Validate.SetBindingForProperty(textBox, "Text");

            Assert.AreEqual(1, binding1.ValidationRules.OfType<ValidatorRule>().Count());
            Assert.AreEqual(1, binding2.ValidationRules.OfType<ValidatorRule>().Count());
        }

        private class TestConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }
}
