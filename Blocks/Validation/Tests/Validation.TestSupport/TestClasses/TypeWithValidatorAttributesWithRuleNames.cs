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
	public class TypeWithValidatorAttributesWithRuleNames
	{
		[MockValidator(true, MessageTemplate = "PropertyWithMixedRulesInValidationAttributes1-NoRule")]
		[MockValidator(true, MessageTemplate = "PropertyWithMixedRulesInValidationAttributes2-NoRule")]
		[MockValidator(true, MessageTemplate = "PropertyWithMixedRulesInValidationAttributes-RuleA", Ruleset = "RuleA")]
		[MockValidator(true, MessageTemplate = "PropertyWithMixedRulesInValidationAttributes3-NoRule")]
		[MockValidator(true, MessageTemplate = "PropertyWithMixedRulesInValidationAttributes-RuleB", Ruleset = "RuleB")]
		public string PropertyWithMixedRulesInValidationAttributes
		{
			get { return null; }
		}

		[MockValidator(true, MessageTemplate = "FieldWithMixedRulesInValidationAttributes1-NoRule")]
		[MockValidator(true, MessageTemplate = "FieldWithMixedRulesInValidationAttributes2-NoRule")]
		[MockValidator(true, MessageTemplate = "FieldWithMixedRulesInValidationAttributes-RuleA", Ruleset = "RuleA")]
		[MockValidator(true, MessageTemplate = "FieldWithMixedRulesInValidationAttributes3-NoRule")]
		[MockValidator(true, MessageTemplate = "FieldWithMixedRulesInValidationAttributes-RuleB", Ruleset = "RuleB")]
		public string FieldWithMixedRulesInValidationAttributes = null;

		[MockValidator(true, MessageTemplate = "MethodWithMixedRulesInValidationAttributes1-NoRule")]
		[MockValidator(true, MessageTemplate = "MethodWithMixedRulesInValidationAttributes2-NoRule")]
		[MockValidator(true, MessageTemplate = "MethodWithMixedRulesInValidationAttributes-RuleA", Ruleset = "RuleA")]
		[MockValidator(true, MessageTemplate = "MethodWithMixedRulesInValidationAttributes3-NoRule")]
		[MockValidator(true, MessageTemplate = "MethodWithMixedRulesInValidationAttributes-RuleB", Ruleset = "RuleB")]
		public string MethodWithMixedRulesInValidationAttributes()
		{
			return null;
		}
	}
}
