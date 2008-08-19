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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Represents an <see cref="ObjectValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Method
		| AttributeTargets.Parameter,
		AllowMultiple = true,
		Inherited = false)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019",
		Justification = "Fields are used internally")]
	public sealed class ObjectValidatorAttribute : ValidatorAttribute
	{
		private string targetRuleset;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidatorAttribute"/> class.</para>
		/// </summary>
		public ObjectValidatorAttribute()
			: this(string.Empty)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidatorAttribute"/> class with
		/// a specific ruleset.</para>
		/// </summary>
		/// <param name="targetRuleset">The target ruleset.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
		/// <seealso cref="ObjectValidator(Type, string)"/>
		public ObjectValidatorAttribute(string targetRuleset)
		{
			if (targetRuleset == null)
			{
				throw new ArgumentNullException("targetRuleset");
			}

			this.targetRuleset = targetRuleset;
		}

		/// <summary>
		/// Creates the <see cref="ObjectValidator"/> described by attribute.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="ObjectValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new ObjectValidator(targetType, targetRuleset);
		}
	}
}