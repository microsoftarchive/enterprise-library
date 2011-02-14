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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="RelativeDateTimeValidatorData"/>.
	/// </summary>
	public partial class RelativeDateTimeValidatorData : RangeValidatorData<int>
	{
		/// <summary>
		/// Creates the <see cref="RelativeDateTimeValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="RelativeDateTimeValidator"/>.</returns>	
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new RelativeDateTimeValidator(LowerBound, LowerUnit, LowerBoundType, UpperBound, UpperUnit, UpperBoundType, Negated);
		}
	}
}
