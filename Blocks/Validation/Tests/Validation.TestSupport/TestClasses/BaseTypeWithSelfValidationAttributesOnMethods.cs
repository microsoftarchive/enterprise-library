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
	public class BaseTypeWithSelfValidationAttributesOnMethods
	{
		[SelfValidation]
		public void InheritedPublicMethodWithSelfValidationAttributeAndMatchingSignature(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("InheritedPublicMethodWithSelfValidationAttributeAndMatchingSignature", null, null, null, null));
		}

		[SelfValidation]
		private void InheritedPrivateMethodWithSelfValidationAttributeAndMatchingSignature(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("InheritedPrivateMethodWithSelfValidationAttributeAndMatchingSignature", null, null, null, null));
		}

		[SelfValidation]
		private void InheritedProtectedMethodWithSelfValidationAttributeAndMatchingSignature(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("InheritedProtectedMethodWithSelfValidationAttributeAndMatchingSignature", null, null, null, null));
		}

		[SelfValidation]
		public virtual void OverridenPublicMethodWithMatchingSignatureAndSelfValidationAttributeOnBaseButNotOnOverride(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("OverridenPublicMethodWithMatchingSignatureAndSelfValidationAttributeOnBaseButNotOnOverride", null, null, null, null));
		}

		[SelfValidation]
		public virtual void OverridenPublicMethodWithMatchingSignatureAndSelfValidationAttributeOnBaseAndOnOverride(ValidationResults validationResults)
		{
			validationResults.AddResult(new ValidationResult("OverridenPublicMethodWithMatchingSignatureAndSelfValidationAttributeOnBaseAndOnOverride", null, null, null, null));
		}
	}
}
