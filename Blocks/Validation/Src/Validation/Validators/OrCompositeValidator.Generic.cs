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
	public class OrCompositeValidator<T> : OrCompositeValidator, Validator<T>
	{
		public OrCompositeValidator(params Validator<T>[] validators)
			: base(validators)
		{ }

		ValidationResults Validator<T>.Validate(T target)
		{
			return ((Validator)this).Validate(target);
		}

		void Validator<T>.Validate(T target, ValidationResults validationResults)
		{
			((Validator)this).Validate(target, validationResults);
		}
	}
}
