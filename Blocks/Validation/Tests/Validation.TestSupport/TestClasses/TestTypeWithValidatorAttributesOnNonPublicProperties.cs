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
	public class TestTypeWithValidatorAttributesOnNonPublicOrStaticProperties
	{
		[MockValidator(false, MessageTemplate = "DefaultProperty")]
		String DefaultProperty
		{
			get { return null; }
		}

		[MockValidator(false, MessageTemplate = "InternalProperty")]
		internal int InternalProperty
		{
			get { return 0; }
		}

		[MockValidator(false, MessageTemplate = "ProtectedProperty")]
		[MockValidator(false, MessageTemplate = "ProtectedProperty")]
		protected virtual object ProtectedProperty
		{
			get { return null; }
		}

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "PublicProperty")]
		[MockValidator(false, MessageTemplate = "PublicProperty")]
		public object PublicProperty
		{
			get { return null; }
		}

		[MockValidator(false, MessageTemplate = "StaticPublicProperty")]
		public static object StaticPublicProperty
		{
			get { return null; }
		}
	}
}
