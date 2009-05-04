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
    public class MockClassWithValidatorAttributesOnProperties
	{
		public String PropertyWithNoAttributes
		{
			get { return null; }
		}

		[MockValidator(false, MessageTemplate = "PropertyWithSingleAttribute-Message1")]
		public int PropertyWithSingleAttribute
		{
			get { return 0; }
		}

		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributes-Message1")]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributes-Message2", Ruleset="RuleA")]
		public string PropertyWithMultipleAttributes
		{
			get { return null; }
		}
	}
}
