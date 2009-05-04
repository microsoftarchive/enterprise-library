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
	[AttributeUsage(AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Method
		| AttributeTargets.Class
		| AttributeTargets.Parameter,
		AllowMultiple = true,
		Inherited = false)]
	public class MockValidatorAttribute : ValidatorAttribute
	{
		private bool returnFailure;

		public MockValidatorAttribute(bool returnFailure)
		{
			this.returnFailure = returnFailure;
		}

		protected override Validator DoCreateValidator(Type targetType)
		{
			return new MockValidator<object>(this.returnFailure);
		}
	}
}
