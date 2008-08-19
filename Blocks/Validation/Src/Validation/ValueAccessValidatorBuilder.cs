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

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	internal class ValueAccessValidatorBuilder : CompositeValidatorBuilder
	{
		private ValueAccess valueAccess;

		public ValueAccessValidatorBuilder(ValueAccess valueAccess, IValidatedElement validatedElement)
			: base(validatedElement)
		{
			this.valueAccess = valueAccess;
		}

		protected override Validator DoGetValidator()
		{
			return new ValueAccessValidator(this.valueAccess, base.DoGetValidator());
		}

		#region test only properties

		internal ValueAccess ValueAccess
		{
			get { return this.valueAccess; }
		}

		#endregion
	}
}
