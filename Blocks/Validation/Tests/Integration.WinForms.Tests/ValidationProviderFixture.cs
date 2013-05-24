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
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests
{
    [TestClass]
    public class ValidationProviderFixture
    {
        bool? validationStatusOnCallback;
        ValidationPerformedEventArgs validationPerformedEventArgs;
        ValueConvertEventArgs valueConvertEventArgs;

        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
            MockValidator<object>.ResetCaches();
            validationStatusOnCallback = null;
            valueConvertEventArgs = null;
            validationPerformedEventArgs = null;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void ValidationProviderCanRetrieveValidType()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixture).AssemblyQualifiedName;

            Type type = validationProvider.GetSourceType();

            Assert.AreSame(typeof(ValidationProviderFixture), type);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RetrievalOfInvalidTypeNameThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = "invalid type name";

            validationProvider.GetSourceType();
        }

        [TestMethod]
        public void ValidationProviderCanExtendControls()
        {
            IExtenderProvider validationProvider = new ValidationProvider();

            Assert.IsTrue(validationProvider.CanExtend(new MockControl()));
            Assert.IsFalse(validationProvider.CanExtend("a string"));
        }

        [TestMethod]
        public void DefaultsForProviderAndExtendedPropertiesAreCorrect()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            MockControl control = new MockControl();

            Assert.AreEqual(null, validationProvider.ErrorProvider);
            Assert.AreEqual("", validationProvider.RulesetName);
            Assert.AreEqual(null, validationProvider.SourceTypeName);
            Assert.AreEqual(ValidationSpecificationSource.All, validationProvider.SpecificationSource);
            Assert.AreEqual(@"{0}", validationProvider.ValidationResultFormat);
            Assert.AreEqual(false, validationProvider.GetPerformValidation(control));
            Assert.AreEqual(null, validationProvider.GetSourcePropertyName(control));
            Assert.AreEqual("Text", validationProvider.GetValidatedProperty(control));
        }

        [TestMethod]
        public void CanGetSetPropertiesForControl()
        {
            ValidationProvider validationProvider = new ValidationProvider();

            MockControl control = new MockControl();

            validationProvider.SetPerformValidation(control, true);
            validationProvider.SetSourcePropertyName(control, "property");
            validationProvider.SetValidatedProperty(control, "controlproperty");

            Assert.AreEqual(true, validationProvider.GetPerformValidation(control));
            Assert.AreEqual("property", validationProvider.GetSourcePropertyName(control));
            Assert.AreEqual("controlproperty", validationProvider.GetValidatedProperty(control));
        }

        [TestMethod]
        public void GettingAndSettingPropertiesOnNullControlThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();

            MockControl control = null;

            try
            {
                validationProvider.GetPerformValidation(control);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException) { }
            try
            {
                validationProvider.GetSourcePropertyName(control);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException) { }
            try
            {
                validationProvider.GetValidatedProperty(control);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException) { }
            try
            {
                validationProvider.SetPerformValidation(control, true);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException) { }
            try
            {
                validationProvider.SetSourcePropertyName(control, "");
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException) { }
            try
            {
                validationProvider.SetValidatedProperty(control, "");
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException) { }
        }

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void GetValidatorForProviderWithoutSourceTypeThrows()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();

        //    validationProvider.GetValidator("IntProperty");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void GetValidatorForProviderWithInvalidSourceTypeThrows()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();
        //    validationProvider.SourceTypeName = "an invalid type";

        //    validationProvider.GetValidator("IntProperty");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void GetValidatorForInvalidPropertyForProviderWithSourceTypeThrows()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();
        //    validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;

        //    validationProvider.GetValidator("InvalidProperty");
        //}

        //[TestMethod]
        //public void GetValidatorForValidPropertyNameForProviderWithSourceTypeUsesDefaultRuleset()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();
        //    validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;

        //    Validator validator = validationProvider.GetValidator("IntProperty");

        //    Assert.AreEqual(1, MockValidator<object>.CreatedValidators.Count);
        //    Assert.AreEqual("int property", MockValidator<object>.CreatedValidators[0].MessageTemplate);
        //}

        //[TestMethod]
        //public void GetValidatorForValidPropertyNameForProviderWithSourceTypeAndRulesetUsesSuppliedRuleset()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();
        //    validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
        //    validationProvider.RulesetName = "ruleset";

        //    Validator validator = validationProvider.GetValidator("IntProperty");

        //    Assert.AreEqual(1, MockValidator<object>.CreatedValidators.Count);
        //    Assert.AreEqual("int property ruleset", MockValidator<object>.CreatedValidators[0].MessageTemplate);
        //}

        //[TestMethod]
        //public void GetValidatorForValidPropertyNameForProviderWithSourceTypeAndRulesetWithoutRulesReturnsNull()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();
        //    validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
        //    validationProvider.RulesetName = "unknown ruleset";

        //    Validator validator = validationProvider.GetValidator("IntProperty");

        //    Assert.IsNull(validator);
        //}

        //[TestMethod]
        //public void GetValidatorForValidPropertyNameForProviderWithSourceTypeAndRulesetAndSourceUsesSuppliedRulesetAndSource()
        //{
        //    ValidationProvider validationProvider = new ValidationProvider();
        //    validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
        //    validationProvider.RulesetName = "ruleset";
        //    validationProvider.SpecificationSource = ValidationSpecificationSource.Configuration;

        //    Validator validator = validationProvider.GetValidator("IntProperty");

        //    Assert.AreEqual(0, MockValidator<object>.CreatedValidators.Count);	// there is no configuration for this ruleset
        //}

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PerformValidationOnNullControlThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();

            validationProvider.PerformValidation((Control)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PerformValidationOnNonExtendedControlThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            MockControl control = new MockControl();

            validationProvider.PerformValidation(control);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PerformValidationOnExtendedControlWithoutSourcePropertyThrows()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            MockControl control = new MockControl();

            validationProvider.SetPerformValidation(control, true);

            validationProvider.PerformValidation(control);
        }

        [TestMethod]
        public void ItemIsValidAfterPerformingSuccessfulValidation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "short";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.IsTrue(validatedControlItem.IsValid);
        }

        [TestMethod]
        public void ItemIsNotValidAfterPerformingUnsuccessfulValidation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "too long";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.IsFalse(validatedControlItem.IsValid);
        }

        [TestMethod]
        public void ItemIsValidAfterPerformingSuccessfulValidationIfPreviouslyUnsucessful()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "too long";
            validationProvider.PerformValidation(validatedControlItem);
            control.Text = "short";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.IsTrue(validatedControlItem.IsValid);
        }

        [TestMethod]
        public void CallbackIsInvokedAfterPerformingSuccessfulValidation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "short";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.IsNotNull(validationStatusOnCallback);
            Assert.AreSame(control, validationPerformedEventArgs.ValidatedControl);
            Assert.IsTrue(validationPerformedEventArgs.ValidationResults.IsValid);
        }

        [TestMethod]
        public void CallbackIsInvokedAfterPerformingUnsuccessfulValidation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "too long";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.IsNotNull(validationStatusOnCallback);
            Assert.AreSame(control, validationPerformedEventArgs.ValidatedControl);
            Assert.IsFalse(validationPerformedEventArgs.ValidationResults.IsValid);
        }

        [TestMethod]
        public void ErrorProviderHasNullErrorForControlAfterPerformingSuccessfulValidation()
        {
            ErrorProvider errorProvider = new ErrorProvider();
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ErrorProvider = errorProvider;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "short";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.AreEqual(string.Empty, errorProvider.GetError(control));
        }

        [TestMethod]
        public void ErrorProviderHasErrorForControlAfterPerformingUnsuccessfulValidation()
        {
            ErrorProvider errorProvider = new ErrorProvider();
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ErrorProvider = errorProvider;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "too long";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.AreNotEqual(string.Empty, errorProvider.GetError(control));

            control.Text = "short";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.AreEqual(string.Empty, errorProvider.GetError(control));
        }

        [TestMethod]
        public void ErrorProviderErrorForControlIsClearedAfterPerformingSuccessfulValidation()
        {
            ErrorProvider errorProvider = new ErrorProvider();
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ErrorProvider = errorProvider;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";

            control.Text = "too long";
            validationProvider.PerformValidation(validatedControlItem);

            Assert.AreNotEqual(string.Empty, errorProvider.GetError(control));
        }

        [TestMethod]
        public void ValidationProviderUsesDefaultMessageFormattingTemplateIfNoneSet()
        {
            ValidationProvider validationProvider = new ValidationProvider();

            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("message 1", null, null, "tag", null));

            string formattedResult = validationProvider.FormatErrorMessage(validationResults);

            Assert.AreEqual("message 1", formattedResult);
        }

        [TestMethod]
        public void ValidationProviderUsesSuppliedMessageFormattingTemplateIfSet()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.ValidationResultFormat = "{0} - {2}";

            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("message 1", null, null, "tag", null));

            string formattedResult = validationProvider.FormatErrorMessage(validationResults);

            Assert.AreEqual(string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", "message 1", "tag"),
                            formattedResult);
        }

        [TestMethod]
        public void ValidationProviderSeparatesLinesWhenFormattingMultipleMessages()
        {
            ValidationProvider validationProvider = new ValidationProvider();

            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("message 1", null, null, "tag", null));
            validationResults.AddResult(new ValidationResult("message 2", null, null, "tag", null));

            string formattedResult = validationProvider.FormatErrorMessage(validationResults);
            StringReader reader = new StringReader(formattedResult);

            Assert.AreEqual("message 1", reader.ReadLine());
            Assert.AreEqual("message 2", reader.ReadLine());
            Assert.AreEqual(null, reader.ReadLine());
        }

        [TestMethod]
        public void ValidationIsNotFiredOnValidatingEventIfExtendedControlDoesNotAllowIt()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";
            validatedControlItem.PerformValidation = false;

            control.Text = "too long";
            bool cancel = control.FireValidating();

            Assert.IsFalse(cancel);
            Assert.IsNull(validationStatusOnCallback);
            Assert.IsNull(validationPerformedEventArgs);
        }

        [TestMethod]
        public void ValidationIsFiredAndCancelArgsIsSetToFalseOnValidatingEventIfExtendedControlDoesAllowValidationAndSucceeds()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";
            validatedControlItem.PerformValidation = true;

            control.Text = "short";
            bool cancel = control.FireValidating();

            Assert.IsFalse(cancel);
            Assert.IsNotNull(validationStatusOnCallback);
        }

        [TestMethod]
        public void ValidationIsFiredAndCancelArgsIsSetToTrueOnValidatingEventIfExtendedControlDoesAllowValidationAndFails()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";
            validatedControlItem.PerformValidation = true;

            control.Text = "too long";
            bool cancel = control.FireValidating();

            Assert.IsTrue(cancel);
            Assert.IsNotNull(validationStatusOnCallback);
        }

        [TestMethod]
        public void ProviderWithTwoSuccessfullyValidatedControlsIsValid()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");

            control1.Text = "short";
            control2.Text = "short";

            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsTrue(validationProvider.IsValid);
        }

        [TestMethod]
        public void ProviderWithMixedValidationOutcomesIsInvalid()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");

            control1.Text = "short";
            control2.Text = "too long";

            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);
        }

        [TestMethod]
        public void ProviderWithTwoUnsuccessfullyValidatedControlsIsInvalid()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");

            control1.Text = "too long";
            control2.Text = "too long";

            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);
        }

        [TestMethod]
        public void ProviderWithTwoUnsuccesfullyValidatedControlsContinuesToBeInvalidAfterOneControlSucceeds()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");

            control1.Text = "too long";
            control2.Text = "too long";
            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);

            control1.Text = "short";
            validationProvider.PerformValidation(control1);

            Assert.IsFalse(validationProvider.IsValid);
        }

        [TestMethod]
        public void ProviderWithTwoUnsuccesfullyValidatedControlsBecomesValidAfterBothControlsSucceed()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");

            control1.Text = "too long";
            control2.Text = "too long";
            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);

            control1.Text = "short";
            validationProvider.PerformValidation(control1);

            Assert.IsFalse(validationProvider.IsValid);

            control2.Text = "short";
            validationProvider.PerformValidation(control2);

            Assert.IsTrue(validationProvider.IsValid);
        }

        [TestMethod]
        public void ValidatedControlIsUnregisteredOnDispose()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            ValidatedControlItem validatedControlItem
                = new ValidatedControlItem(validationProvider, control);
            validatedControlItem.SourcePropertyName = "StringProperty";
            validatedControlItem.PerformValidation = true;

            control.Text = "too long";
            bool cancel = control.FireValidating();

            Assert.IsTrue(cancel);
            Assert.IsNotNull(validationStatusOnCallback);

            validationStatusOnCallback = null;
            validatedControlItem.Dispose();

            cancel = control.FireValidating();

            Assert.IsFalse(cancel);
            Assert.IsNull(validationStatusOnCallback);
        }

        [TestMethod]
        public void ValidationProviderUnregistersControlsOnDispose()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control = new MockControl();
            validationProvider.SetSourcePropertyName(control, "StringProperty");
            validationProvider.SetPerformValidation(control, true);

            control.Text = "too long";
            bool cancel = control.FireValidating();

            Assert.IsTrue(cancel);
            Assert.IsNotNull(validationStatusOnCallback);

            validationStatusOnCallback = null;
            validationProvider.Dispose();

            cancel = control.FireValidating();

            Assert.IsFalse(cancel);
            Assert.IsNull(validationStatusOnCallback);
        }

        [TestMethod]
        public void ErrorsInErrorProviderForValidatedControlsAreClearedByValidatorProviderDispose()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            ErrorProvider errorProvider = new ErrorProvider();
            validationProvider.ErrorProvider = errorProvider;

            MockControl controlWithoutError = new MockControl();
            controlWithoutError.Text = "short";
            validationProvider.SetSourcePropertyName(controlWithoutError, "StringProperty");
            MockControl controlWithError = new MockControl();
            controlWithError.Text = "too long";
            validationProvider.SetSourcePropertyName(controlWithError, "StringProperty");
            MockControl controlWithoutValidation = new MockControl();
            controlWithoutValidation.Text = "too long";

            validationProvider.PerformValidation(controlWithoutError);
            validationProvider.PerformValidation(controlWithError);
            errorProvider.SetError(controlWithoutValidation, "an unmanaged error");

            Assert.AreEqual(string.Empty, errorProvider.GetError(controlWithoutError));
            Assert.AreEqual("validation", errorProvider.GetError(controlWithError));
            Assert.AreEqual("an unmanaged error", errorProvider.GetError(controlWithoutValidation));

            validationProvider.Dispose();

            Assert.AreEqual(string.Empty, errorProvider.GetError(controlWithoutError));
            Assert.AreEqual(string.Empty, errorProvider.GetError(controlWithError));
            Assert.AreEqual("an unmanaged error", errorProvider.GetError(controlWithoutValidation));
        }

        [TestMethod]
        public void ValidationProviderIsEnabledByDefault()
        {
            ValidationProvider validationProvider = new ValidationProvider();

            Assert.IsTrue(validationProvider.Enabled);
        }

        [TestMethod]
        public void ValidationWillNotBePerformedIfValidationProviderIsDisabled()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.Enabled = false; // set validation provider to disabled
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");

            control1.Text = "too long";

            validationProvider.PerformValidation(control1);

            Assert.IsTrue(validationProvider.IsValid);
        }

        [TestMethod]
        public void AutomaticValidationWillNotBePerformedIfValidationProviderIsDisabled()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.Enabled = false; // set validation provider to disabled
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            validationProvider.SetPerformValidation(control1, true);

            control1.Text = "too long";

            bool cancel = control1.FireValidating();

            Assert.IsTrue(validationProvider.IsValid);
            Assert.IsFalse(cancel);
        }

        [TestMethod]
        public void ValidationWillNotBePerformedIfValidationProviderIsDisabledAndCallbackWillNotBeInvoked()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            validationProvider.Enabled = false; // set validation provider to disabled
            validationProvider.ValidationPerformed += OnValidationPerformed;
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");

            control1.Text = "too long";

            validationProvider.PerformValidation(control1);

            Assert.IsTrue(validationProvider.IsValid);
            Assert.IsNull(validationPerformedEventArgs);
        }

        [TestMethod]
        public void InvalidValidationProviderBecomesValidWhenDisabled()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");

            control1.Text = "too long";

            validationProvider.PerformValidation(control1);

            Assert.IsFalse(validationProvider.IsValid);

            validationProvider.Enabled = false; // set validation provider to disabled

            Assert.IsTrue(validationProvider.IsValid);
        }

        [TestMethod]
        public void InvalidValidationProviderBecomesValidWhenDisabledAndRemainsValidWhenReenabled()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");

            control1.Text = "too long";

            validationProvider.PerformValidation(control1);

            Assert.IsFalse(validationProvider.IsValid);

            validationProvider.Enabled = false; // set validation provider to disabled

            Assert.IsTrue(validationProvider.IsValid);

            validationProvider.Enabled = true; // set validation provider to enabled

            Assert.IsTrue(validationProvider.IsValid);
        }

        [TestMethod]
        public void ExistingValidationErrorsAreClearedWhenDisablingValidationProvider()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            ErrorProvider errorProvider = new ErrorProvider();
            validationProvider.ErrorProvider = errorProvider;

            control1.Text = "too long";

            validationProvider.PerformValidation(control1);

            Assert.AreEqual("validation", errorProvider.GetError(control1));

            validationProvider.Enabled = false; // set validation provider to disabled

            Assert.AreEqual("", errorProvider.GetError(control1));
        }

        [TestMethod]
        public void ExistingValidationErrorsAreClearedWhenDisablingValidationProviderButNotSetAgainWhenEnablingTheValidationProvider()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "validation";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            ErrorProvider errorProvider = new ErrorProvider();
            validationProvider.ErrorProvider = errorProvider;

            control1.Text = "too long";

            validationProvider.PerformValidation(control1);

            Assert.AreEqual("validation", errorProvider.GetError(control1));

            validationProvider.Enabled = false; // set validation provider to disabled

            Assert.AreEqual("", errorProvider.GetError(control1));

            validationProvider.Enabled = true; // set validation provider to enabled

            Assert.AreEqual("", errorProvider.GetError(control1));
        }

        [TestMethod]
        public void ValueConvertingCallbackIsInvokedIfSetWhenValidatingAndTheConvertedValueIsUsedForValidation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.ValueConvert += OnValueConvert;
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "IntProperty");

            control1.Text = "5";

            validationProvider.PerformValidation(control1);

            Assert.IsNotNull(valueConvertEventArgs);
            Assert.AreEqual(typeof(int), valueConvertEventArgs.TargetType);
            Assert.AreEqual("5", valueConvertEventArgs.ValueToConvert);
            Assert.AreEqual(500, valueConvertEventArgs.ConvertedValue);
            //Assert.AreSame(control1, this.valueConvertEventArgs.ValueSource);
            Assert.AreEqual("IntProperty", valueConvertEventArgs.SourcePropertyName);

            Assert.AreEqual(500, MockValidator<object>.CreatedValidators[0].ValidatedTargets[0]);
        }

        [TestMethod]
        public void ValueConvertingCallbackSettingConversionErrorMessageWillCauseValidationToBeSkippedAndSetTheControlAsInvalid()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.ValueConvert += OnValueConvertFailing;
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "IntProperty");

            control1.Text = "5";

            validationProvider.PerformValidation(control1);

            Assert.IsNotNull(valueConvertEventArgs); // callback was called
            Assert.AreEqual(0,
                            MockValidator<object>.CreatedValidators[0].ValidatedTargets.Count); // nothing got validated
            Assert.IsFalse(validationProvider.IsValid); // but the provider is invalid
        }

        [TestMethod]
        public void ValueConvertingCallbackSettingConversionErrorMessageWillUseMessageOnErrorProviderIfExists()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.ValueConvert += OnValueConvertFailing;
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "IntProperty");
            ErrorProvider errorProvider = new ErrorProvider();
            validationProvider.ErrorProvider = errorProvider;

            control1.Text = "5";

            validationProvider.PerformValidation(control1);

            Assert.IsNotNull(valueConvertEventArgs); // callback was called
            Assert.AreEqual(0,
                            MockValidator<object>.CreatedValidators[0].ValidatedTargets.Count); // nothing got validated
            Assert.IsFalse(validationProvider.IsValid); // but the provider is invalid
            Assert.AreEqual("failed conversion", errorProvider.GetError(control1)); // and the error was set on the error provider
        }

        [TestMethod]
        public void RequestForExistingValidatedControlItemForMappedPropertyNameReturnsItem()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "IntProperty");

            Assert.AreSame(control1, validationProvider.GetExistingValidatedControlItem("StringProperty").Control);
        }

        [TestMethod]
        public void RequestForExistingValidatedControlItemForNonMappedPropertyNameReturnsNull()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "IntProperty");

            Assert.IsNull(validationProvider.GetExistingValidatedControlItem("NonMappedProperty"));
        }

        [TestMethod]
        public void CanPerformCrossFieldValidation()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            validationProvider.RulesetName = "cross field";
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "ExtraStringProperty");
            ValidatedControlItem validatedControlItem1 = validationProvider.GetExistingValidatedControlItem(control1);
            Validator validator = validatedControlItem1.Validator;

            control1.Text = "aaaaaa";
            control2.Text = "aaaabb";

            Assert.IsFalse(validator.Validate(validatedControlItem1).IsValid);

            control2.Text = "aaaaaa";

            Assert.IsTrue(validator.Validate(validatedControlItem1).IsValid);
        }

        [TestMethod]
        public void ValidationIsClearedForControlWhenControlPropertyIsSetAndErrorIsCleared()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            ValidatedControlItem controlItem1 = validationProvider.GetExistingValidatedControlItem(control1);
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");
            ValidatedControlItem controlItem2 = validationProvider.GetExistingValidatedControlItem(control2);
            ErrorProvider errorProvider = new ErrorProvider();
            validationProvider.ErrorProvider = errorProvider;

            control1.Text = "000000000001";
            control2.Text = "000000000001";

            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);
            Assert.IsFalse(controlItem1.IsValid);
            Assert.IsFalse(controlItem2.IsValid);
            Assert.AreEqual("string property", errorProvider.GetError(control1));
            Assert.AreEqual("string property", errorProvider.GetError(control2));

            validationProvider.ValidationPerformed += OnValidationPerformed;
            validationProvider.SetSourcePropertyName(control2, "IntProperty");

            Assert.IsFalse(validationProvider.IsValid);
            Assert.IsFalse(controlItem1.IsValid);
            Assert.IsTrue(controlItem2.IsValid);
            Assert.AreEqual("string property", errorProvider.GetError(control1));
            Assert.AreEqual(string.Empty, errorProvider.GetError(control2));
            Assert.IsNull(validationPerformedEventArgs);

            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);
            Assert.IsFalse(controlItem1.IsValid);
            Assert.IsFalse(controlItem2.IsValid);
            Assert.AreEqual("string property", errorProvider.GetError(control1));
            Assert.AreEqual("int property", errorProvider.GetError(control2));
            Assert.IsNotNull(validationPerformedEventArgs);
        }

        [TestMethod]
        public void ValidationIsClearedForAllControlsWhenProviderPropertyIsSetAndErrorIsCleared()
        {
            ValidationProvider validationProvider = new ValidationProvider();
            validationProvider.SourceTypeName = typeof(ValidationProviderFixtureTestClass).AssemblyQualifiedName;
            MockControl control1 = new MockControl();
            validationProvider.SetSourcePropertyName(control1, "StringProperty");
            ValidatedControlItem controlItem1 = validationProvider.GetExistingValidatedControlItem(control1);
            MockControl control2 = new MockControl();
            validationProvider.SetSourcePropertyName(control2, "StringProperty");
            ValidatedControlItem controlItem2 = validationProvider.GetExistingValidatedControlItem(control2);
            ErrorProvider errorProvider = new ErrorProvider();
            validationProvider.ErrorProvider = errorProvider;

            control1.Text = "000000000001";
            control2.Text = "000000000001";

            validationProvider.PerformValidation(control1);
            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);
            Assert.IsFalse(controlItem1.IsValid);
            Assert.IsFalse(controlItem2.IsValid);
            Assert.AreEqual("string property", errorProvider.GetError(control1));
            Assert.AreEqual("string property", errorProvider.GetError(control2));

            validationProvider.ValidationPerformed += OnValidationPerformed;
            validationProvider.RulesetName = "validation";

            Assert.IsTrue(validationProvider.IsValid);
            Assert.IsTrue(controlItem1.IsValid);
            Assert.IsTrue(controlItem2.IsValid);
            Assert.AreEqual(string.Empty, errorProvider.GetError(control1));
            Assert.AreEqual(string.Empty, errorProvider.GetError(control2));
            Assert.IsNull(validationPerformedEventArgs);

            validationProvider.PerformValidation(control2);

            Assert.IsFalse(validationProvider.IsValid);
            Assert.IsTrue(controlItem1.IsValid);
            Assert.IsFalse(controlItem2.IsValid);
            Assert.AreEqual(string.Empty, errorProvider.GetError(control1));
            Assert.AreEqual("validation", errorProvider.GetError(control2));
            Assert.IsNotNull(validationPerformedEventArgs);
        }

        void OnValidationPerformed(object source,
                                   ValidationPerformedEventArgs e)
        {
            validationStatusOnCallback = ((ValidationProvider)source).IsValid;
            validationPerformedEventArgs = e;
        }

        void OnValueConvert(object source,
                            ValueConvertEventArgs e)
        {
            valueConvertEventArgs = e;
            valueConvertEventArgs.ConvertedValue = 500;
        }

        void OnValueConvertFailing(object source,
                                   ValueConvertEventArgs e)
        {
            valueConvertEventArgs = e;
            valueConvertEventArgs.ConversionErrorMessage = "failed conversion";
        }

        public class ValidationProviderFixtureTestClass
        {
            string extraStringProperty;

            int intProperty;
            string stringProperty;

            public string ExtraStringProperty
            {
                get { return extraStringProperty; }
                set { extraStringProperty = value; }
            }

            [MockValidator(true, MessageTemplate = "int property")]
            [MockValidator(true, MessageTemplate = "int property ruleset", Ruleset = "ruleset")]
            [MockValidator(true, Ruleset = "failing")]
            [MockValidator(false, Ruleset = "non failing")]
            public int IntProperty
            {
                get { return intProperty; }
                set { intProperty = value; }
            }

            [MockValidator(true, MessageTemplate = "string property")]
            [MockValidator(false, Ruleset = "non failing")]
            [StringLengthValidator(5, MessageTemplate = "validation", Ruleset = "validation")]
            [PropertyComparisonValidator("ExtraStringProperty", ComparisonOperator.Equal,
                Ruleset = "cross field", MessageTemplate = "equal strings")]
            public string StringProperty
            {
                get { return stringProperty; }
                set { stringProperty = value; }
            }
        }
    }
}
