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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="StringLengthValidator"/>.
	/// </summary>
	/// <seealso cref="StringLengthValidator"/>
	/// <seealso cref="ValidatorData"/>
	public class StringLengthValidatorData : RangeValidatorData<int>
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="StringLengthValidatorData"/> class.</para>
		/// </summary>
		public StringLengthValidatorData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="StringLengthValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public StringLengthValidatorData(string name)
			: base(name, typeof(StringLengthValidator))
		{ }

		/// <summary>
		/// Creates the <see cref="StringLengthValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="StringLengthValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new StringLengthValidator(this.LowerBound, 
				this.LowerBoundType, 
				this.UpperBound, 
				this.UpperBoundType,
				Negated);
		}
	}
}