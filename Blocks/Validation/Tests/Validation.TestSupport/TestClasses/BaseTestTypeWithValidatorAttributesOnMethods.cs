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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class BaseTestTypeWithValidatorAttributesOnMethods
	{
		public String MethodWithNoAttributes()
		{
			return null;
		}

		[MockValidator(false, MessageTemplate = "MethodWithSingleAttribute-Message1")]
		public int MethodWithSingleAttribute()
		{
			return 0;
		}

		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributes-Message1")]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributes-Message2")]
		public virtual object MethodWithMultipleAttributes()
		{
			return null;
		}

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesAndValidationModifier-Message1")]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesAndValidationModifier-Message2")]
		public object MethodWithMultipleAttributesAndValidationModifier()
		{
			return null;
		}
	}
}
