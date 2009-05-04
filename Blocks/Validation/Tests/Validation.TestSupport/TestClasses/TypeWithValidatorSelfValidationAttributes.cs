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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class TypeWithValidatorSelfValidationAttributes
	{
		public void MethodWithoutSelfValidationAttributes(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("MethodWithoutSelfValidationAttributes", null, null, null, null));
		}

		[SelfValidation]
		public void MethodWithSelfValidationAttributes(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("MethodWithSelfValidationAttributes", null, null, null, null));
		}

		[SelfValidation]
		[SelfValidation]
		[SelfValidation]
		public void MethodWithMultipleSelfValidationAttributes(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("MethodWithMultipleSelfValidationAttributes", null, null, null, null));
		}

		[SelfValidation(Ruleset = "RuleA")]
		public void MethodWithSelfValidationAttributesWithRuleset(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("MethodWithSelfValidationAttributesWithRuleset", null, null, null, null));
		}
	}
}
