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
	public class TestTypeWithValidatorAttributesOnNonPublicOrStaticMethods
	{
		[MockValidator(false, MessageTemplate = "DefaultMethod")]
		String DefaultMethod()
		{
			return null;
		}

		[MockValidator(false, MessageTemplate = "InternalMethod")]
		internal int InternalMethod()
		{
			return 0;
		}

		[MockValidator(false, MessageTemplate = "ProtectedMethod")]
		[MockValidator(false, MessageTemplate = "ProtectedMethod")]
		protected virtual object ProtectedMethod()
		{
			return null;
		}

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "PublicMethod")]
		[MockValidator(false, MessageTemplate = "PublicMethod")]
		public object PublicMethod()
		{
			return null;
		}

		[MockValidator(false, MessageTemplate = "StaticPublicMethod")]
		public static object StaticPublicMethod()
		{
			return null;
		}
	}
}
