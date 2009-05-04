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
	[MockValidator(false, MessageTemplate = "DerivedTestTypeWithValidatorAttributesOnMembersAndType-Message1")]
	[MockValidator(false, MessageTemplate = "DerivedTestTypeWithValidatorAttributesOnMembersAndType-Message2")]
	[MockValidator(false, MessageTemplate = "DerivedTestTypeWithValidatorAttributesOnMembersAndType-Message1-RuleA", Ruleset = "RuleA")]
	[MockValidator(false, MessageTemplate = "DerivedTestTypeWithValidatorAttributesOnMembersAndType-Message2-RuleA", Ruleset = "RuleA")]
	[MockValidator(false, MessageTemplate = "DerivedTestTypeWithValidatorAttributesOnMembersAndType-Message3-RuleA", Ruleset = "RuleA")]
	[MockValidator(false, MessageTemplate = "DerivedTestTypeWithValidatorAttributesOnMembersAndType-Message1-RuleB", Ruleset = "RuleB")]
	public class DerivedTestTypeWithValidatorAttributesOnMembersAndType : BaseTestTypeWithValidatorAttributes
	{
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesOverride-Message1")]
		public override object PropertyWithMultipleAttributes
		{
			get
			{
				return base.PropertyWithMultipleAttributes;
			}
		}

		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesOverride-Message1")]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesOverride-Message1-RuleA", Ruleset = "RuleA")]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesOverride-Message2-RuleA", Ruleset = "RuleA")]
		public override object MethodWithMultipleAttributes()
		{
			return base.PropertyWithMultipleAttributes;
		}
	}
}
