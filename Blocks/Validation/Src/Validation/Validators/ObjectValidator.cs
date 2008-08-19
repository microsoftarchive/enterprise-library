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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Performs validation on objects by applying the validation rules specified for a supplied type.
	/// </summary>
	/// <seealso cref="ValidationFactory"/>
	[ConfigurationElementType(typeof(ObjectValidatorData))]
	public class ObjectValidator : Validator
	{
		private Type targetType;
		private string targetRuleset;
		private Validator targetTypeValidator;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type.</para>
		/// </summary>
		/// <param name="targetType">The target type</param>
		/// <remarks>
		/// The default ruleset for <paramref name="targetType"/> will be used.
		/// </remarks>
		/// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
		public ObjectValidator(Type targetType)
			: this(targetType, string.Empty)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type
		/// using the supplied ruleset.</para>
		/// </summary>
		/// <param name="targetType">The target type</param>
		/// <param name="targetRuleset">The ruleset to use.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
		public ObjectValidator(Type targetType, string targetRuleset)
			: base(null, null)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetRuleset == null)
			{
				throw new ArgumentNullException("targetRuleset");
			}

			this.targetType = targetType;
			this.targetRuleset = targetRuleset;
			this.targetTypeValidator = ValidationFactory.CreateValidator(this.targetType, this.targetRuleset);
		}

		/// <summary>
		/// Validates by applying the validation rules for the target type specified for the receiver.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
		/// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
		/// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
		/// <remarks>
		/// If <paramref name="objectToValidate"/> is <see langword="null"/> validation is ignored.
		/// <para/>
		/// A referece to an instance of a type not compatible with the configured target type
		/// causes a validation failure.
		/// </remarks>
		protected internal override void DoValidate(object objectToValidate,
			object currentTarget,
			string key,
			ValidationResults validationResults)
		{
			if (objectToValidate != null)
			{
				if (this.targetType.IsAssignableFrom(objectToValidate.GetType()))
				{
					// reset the current target and the key
					this.targetTypeValidator.DoValidate(objectToValidate, objectToValidate, null, validationResults);
				}
				else
				{
					// unlikely
					this.LogValidationResult(validationResults, Resources.ObjectValidatorInvalidTargetType, currentTarget, key);
				}
			}
		}

		/// <summary>
		/// Gets the message template to use when logging results no message is supplied.
		/// </summary>
		protected override string DefaultMessageTemplate
		{
			get { return null; }
		}

		#region test only properties

		internal Type TargetType
		{
			get { return this.targetType; }
		}

		internal string TargetRuleset
		{
			get { return this.targetRuleset; }
		}

		#endregion
	}
}