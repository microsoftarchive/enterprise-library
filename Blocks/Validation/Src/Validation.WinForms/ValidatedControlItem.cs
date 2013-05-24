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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
    /// <summary>
    /// Represents the behavior necessary to provide integration with the Validation Application Block and a control item.
    /// </summary>
    public sealed class ValidatedControlItem : IValidationIntegrationProxy
    {
        Control control;
        bool isValid;
        bool performValidation;
        string sourcePropertyName;
        string validatedPropertyName;
        readonly ValidationProvider validationProvider;
        Validator validator;

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidatedControlItem"/> class with a <see cref="validationProvider"/> and a <see cref="Control"/>.
        /// </summary>
        /// <param name="validationProvider">The validation provider.</param>
        /// <param name="control">The control to validate.</param>
        public ValidatedControlItem(ValidationProvider validationProvider,
                                    Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            this.validationProvider = validationProvider;
            this.control = control;
            this.control.Validating += OnValidating;
            isValid = true;
            validatedPropertyName = ValidationProvider.DefaultValidatedProperty;
        }

        /// <summary>
        /// Gets the control to validate.
        /// </summary>
        /// <value>
        /// The control to validate.
        /// </value>
        public Control Control
        {
            get { return control; }
        }

        ///<summary>
        /// Gets if the control is valid.
        ///</summary>
        /// <value>
        /// true if the control is valid; otherwise, false.
        /// </value>
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }

        /// <summary>
        /// Gets if the validation is performed.
        /// </summary>
        /// <value>
        /// true if the validation is performed; otherwise, false.
        /// </value>
        public bool PerformValidation
        {
            get { return performValidation; }
            set { performValidation = value; }
        }

        bool IValidationIntegrationProxy.ProvidesCustomValueConversion
        {
            get { return validationProvider.ProvidesCustomValueConversion; }
        }

        string IValidationIntegrationProxy.Ruleset
        {
            get { return validationProvider.RulesetName; }
        }

        ///<summary>
        /// Gets the source property name.
        ///</summary>
        /// <value>
        /// The source property name.
        /// </value>
        public string SourcePropertyName
        {
            get { return sourcePropertyName; }
            set { sourcePropertyName = value; }
        }

        ValidationSpecificationSource IValidationIntegrationProxy.SpecificationSource
        {
            get { return validationProvider.SpecificationSource; }
        }

        ///<summary>
        /// Gets the validated property name.
        ///</summary>
        /// <value>
        /// The validated property name.
        /// </value>
        public string ValidatedPropertyName
        {
            get { return validatedPropertyName; }
            set { validatedPropertyName = value; }
        }

        string IValidationIntegrationProxy.ValidatedPropertyName
        {
            get { return SourcePropertyName; }
        }

        Type IValidationIntegrationProxy.ValidatedType
        {
            get { return validationProvider.GetSourceType(); }
        }

        /// <summary>
        /// Gets the <see cref="ValidationProvider"/> for the control.
        /// </summary>
        /// <value>
        /// The <see cref="ValidationProvider"/> for the control.
        /// </value>
        public ValidationProvider ValidationProvider
        {
            get { return validationProvider; }
        }

        /// <summary>
        /// Gets the <see cref="Validator"/> for the control.
        /// </summary>
        /// <value>
        /// The <see cref="Validator"/> for the control.
        /// </value>
        public Validator Validator
        {
            get
            {
                if (validator == null)
                {
                    validator = new ValidationIntegrationHelper(this).GetValidator();
                }

                return validator;
            }
        }

        internal void ClearValidation()
        {
            validator = null;
            isValid = true;
        }

        ///<summary>
        /// Allows the item to dispose of any unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            if (control != null)
            {
                control.Validating -= OnValidating;
                control = null;
            }
            validator = null;
        }

        MemberValueAccessBuilder IValidationIntegrationProxy.GetMemberValueAccessBuilder()
        {
            return new PropertyMappedControlValueAccessBuilder();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1304", Justification = "Warning on call to System.Type.InvokeMember")]
        object IValidationIntegrationProxy.GetRawValue()
        {
            if (ValidationProvider.DefaultValidatedProperty.Equals(validatedPropertyName))
            {
                return control.Text;
            }
            else
            {
                try
                {
                    return control.GetType().InvokeMember(validatedPropertyName,
                                                          BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                                                          null,
                                                          control,
                                                          null);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentCulture,
                                      Resources.ExceptionValidatedControlPropertyNotFound,
                                      validatedPropertyName,
                                      control.Name),
                        e);
                }
            }
        }

        /// <summary>
        /// Get the value and the failure message from validation.
        /// </summary>
        /// <param name="value">The value from validation.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns>true if the value passed validation; otherwise, false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Type of value is unknown")]
        public bool GetValue(out object value,
                             out string failureMessage)
        {
            ValidationIntegrationHelper helper = new ValidationIntegrationHelper(this);
            return helper.GetValue(out value, out failureMessage);
        }

        void OnValidating(object source,
                          CancelEventArgs e)
        {
            if (PerformValidation)
            {
                validationProvider.PerformValidation(this);
                e.Cancel = !IsValid;
            }
        }

        void IValidationIntegrationProxy.PerformCustomValueConversion(ValueConvertEventArgs e)
        {
            validationProvider.PerformCustomValueConversion(e);
        }
    }
}
