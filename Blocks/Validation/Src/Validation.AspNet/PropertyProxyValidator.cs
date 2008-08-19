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
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.Compilation;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet
{
	/// <summary>
	/// Performs validation on a control's value using the validation specified on the property of <see cref="System.Type"/>.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class PropertyProxyValidator : BaseValidator, IValidationIntegrationProxy
	{
		/// <summary>
		/// Determines whether the content in the input control is valid.
		/// </summary>
		/// <returns><see langword="true"/> if the control is valid; otherwise, <see langword="false"/>.</returns>
		protected override bool EvaluateIsValid()
		{
			Validator validator = new ValidationIntegrationHelper(this).GetValidator();

			if (validator != null)
			{
				ValidationResults validationResults = validator.Validate(this);

				this.ErrorMessage = FormatErrorMessage(validationResults, this.DisplayMode);
				return validationResults.IsValid;
			}
			else
			{
				this.ErrorMessage = "";
				return true;
			}
		}

		internal static string FormatErrorMessage(ValidationResults results, ValidationSummaryDisplayMode displayMode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string errorsListStart;
			string errorStart;
			string errorEnd;
			string errorListEnd;

			switch (displayMode)
			{
				case ValidationSummaryDisplayMode.List:
					errorsListStart = string.Empty;
					errorStart = string.Empty;
					errorEnd = "<br/>";
					errorListEnd = string.Empty;
					break;

				case ValidationSummaryDisplayMode.SingleParagraph:
					errorsListStart = string.Empty;
					errorStart = string.Empty;
					errorEnd = " ";
					errorListEnd = "<br/>";
					break;

				default:
					errorsListStart = "<ul>";
					errorStart = "<li>";
					errorEnd = "</li>";
					errorListEnd = "</ul>";
					break;
			}
			if (!results.IsValid)
			{
				stringBuilder.Append(errorsListStart);
				foreach (ValidationResult validationResult in results)
				{
					stringBuilder.Append(errorStart);
					stringBuilder.Append(validationResult.Message);
					stringBuilder.Append(errorEnd);
				}
				stringBuilder.Append(errorListEnd);
			}

			return stringBuilder.ToString();
		}

		internal bool GetValue(out object value, out string valueAccessFailureMessage)
		{
			ValidationIntegrationHelper helper = new ValidationIntegrationHelper(this);

			return helper.GetValue(out value, out valueAccessFailureMessage);
		}

		private string sourceTypeName;
		/// <summary>
		/// Gets or sets the name of the type to use a source for validation specifications.
		/// </summary>
		public string SourceTypeName
		{
			get { return sourceTypeName; }
			set { sourceTypeName = value; }
		}

		private string propertyName;
		/// <summary>
		/// Gets or sets the name of the property to use as soource for validation specifications.
		/// </summary>
		public string PropertyName
		{
			get { return propertyName; }
			set { propertyName = value; }
		}

		private string rulesetName;
		/// <summary>
		/// Gets or sets the name of the ruleset to use when retrieving validation specifications.
		/// </summary>
		[DefaultValue("")]
		public string RulesetName
		{
			get { return rulesetName != null ? rulesetName : string.Empty; }
			set { rulesetName = value; }
		}

		private ValidationSpecificationSource specificationSource = ValidationSpecificationSource.Both;
		/// <summary>
		/// Gets or sets the <see cref="ValidationSpecificationSource"/> indicating where to get validation specifications from.
		/// </summary>
		[DefaultValue(ValidationSpecificationSource.Both)]
		public ValidationSpecificationSource SpecificationSource
		{
			get { return specificationSource; }
			set { specificationSource = value; }
		}

		private ValidationSummaryDisplayMode displayMode;
		/// <summary>
		/// Gets or sets the <see cref="ValidationSummaryDisplayMode"/> indicating how to format multiple validation results.
		/// </summary>
		public ValidationSummaryDisplayMode DisplayMode
		{
			get { return displayMode; }
			set { displayMode = value; }
		}

		/// <summary>
		/// Occurs when value conversion is required by the control to perform validation.
		/// </summary>
		/// <remarks>
		/// The ValueConvert event is raised when value conversion is required by the control to perform validation. 
		/// This event is used to provide a custom value conversion routine for an input control, 
		/// such as a <see cref="System.Web.UI.WebControls.TextBox"/> control.
		/// </remarks>
		/// <seealso cref="ValueConvertEventArgs"/>
		public event EventHandler<ValueConvertEventArgs> ValueConvert;

		#region IValidationIntegrationProxy Members

		object IValidationIntegrationProxy.GetRawValue()
		{
			return this.GetControlValidationValue(this.ControlToValidate);
		}

		MemberValueAccessBuilder IValidationIntegrationProxy.GetMemberValueAccessBuilder()
		{
			return new PropertyMappedValidatorValueAccessBuilder();
		}

		void IValidationIntegrationProxy.PerformCustomValueConversion(ValueConvertEventArgs e)
		{
			if (this.ValueConvert != null)
			{
				this.ValueConvert(this, e);
			}
		}

		bool IValidationIntegrationProxy.ProvidesCustomValueConversion
		{
			get { return this.ValueConvert != null; }
		}

		string IValidationIntegrationProxy.Ruleset
		{
			get { return this.RulesetName; }
		}

		ValidationSpecificationSource IValidationIntegrationProxy.SpecificationSource
		{
			get { return this.SpecificationSource; }
		}

		string IValidationIntegrationProxy.ValidatedPropertyName
		{
			get { return this.PropertyName; }
		}

		Type IValidationIntegrationProxy.ValidatedType
		{
			get
			{
				if (string.IsNullOrEmpty(this.sourceTypeName))
				{
					throw new InvalidOperationException(Resources.ExceptionNullSourceTypeName);
				}
				Type validatedType = BuildManager.GetType(this.SourceTypeName, false, false);
				if (validatedType == null)
				{
					throw new InvalidOperationException(
						string.Format(CultureInfo.CurrentUICulture,
							Resources.ExceptionInvalidSourceTypeName,
							this.sourceTypeName));
				}

				return validatedType;
			}
		}

		#endregion
	}
}