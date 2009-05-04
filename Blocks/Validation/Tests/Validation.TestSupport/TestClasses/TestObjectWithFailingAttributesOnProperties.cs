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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class TestObjectWithFailingAttributesOnProperties
	{
		private string failingProperty1;
		[MockValidator(true, MessageTemplate = "message1")]
		[MockValidator(true, MessageTemplate = "message1-RuleA", Ruleset = "RuleA")]
        [MockValidator(true, MessageTemplate = "message1-RuleB", Ruleset = "RuleB")]
		public string FailingProperty1
		{
			get { return failingProperty1; }
			set { failingProperty1 = value; }
		}
	
		[MockValidator(true, MessageTemplate = "message2")]
		public string FailingProperty2
		{
			get { return "failing2"; }
		}

		[MockValidator(true, MessageTemplate = "non readable")]
		public string NonReadableProperty
		{
			set { }
		}

		public string PropertyWithoutAttributes
		{
			get { return "withoutAttributes"; }
		}
	}
}
