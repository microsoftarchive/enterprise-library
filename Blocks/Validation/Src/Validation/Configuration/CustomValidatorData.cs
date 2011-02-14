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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of custom <see cref="Validator"/> class.
	/// </summary>
	/// <remarks>
	/// Custom <see cref="Validator"/> classes must implement a constructor with with name and value collection parameters.
	/// </remarks>
	public partial class CustomValidatorData : ValidatorData
	{
		/// <summary>
		/// Creates the <see cref="Validator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="Validator"/>.</returns>
		/// <seealso cref="Validator"/>
		protected override Validator DoCreateValidator(Type targetType)
		{
			Validator validator
				= (Validator)Activator.CreateInstance(this.Type, this.Attributes);

			return validator;
		}
	}
}
