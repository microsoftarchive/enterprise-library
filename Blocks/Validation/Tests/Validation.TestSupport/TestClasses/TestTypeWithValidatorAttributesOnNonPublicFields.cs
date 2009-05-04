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
	public class TestTypeWithValidatorAttributesOnNonPublicOrStaticFields
	{
#pragma warning disable 414
		[MockValidator(false, MessageTemplate = "DefaultField")]
		String DefaultField = null;
#pragma warning restore 414

		[MockValidator(false, MessageTemplate = "InternalField")]
		internal int InternalField = 0;

		[MockValidator(false, MessageTemplate = "ProtectedField")]
		[MockValidator(false, MessageTemplate = "ProtectedField")]
		protected object ProtectedField = null;

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "PublicField")]
		[MockValidator(false, MessageTemplate = "PublicField")]
		public object PublicField = null;

		[MockValidator(false, MessageTemplate = "StaticPublicField")]
		public static object StaticPublicField = null;
	}
}
