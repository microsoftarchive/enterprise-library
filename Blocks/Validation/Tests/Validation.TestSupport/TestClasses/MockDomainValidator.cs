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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	[ConfigurationElementType(typeof(DomainValidatorData))]
	public class MockDomainValidator : MockValidator<object>
	{
		public MockDomainValidator(NameValueCollection attributes)
			: base(GetReturnFailure(attributes))
		{ }

		private static bool GetReturnFailure(NameValueCollection attributes)
		{
			string returnFailureString = attributes.Get("returnFailure");
			if (returnFailureString != null)
			{
				return bool.Parse(returnFailureString);
			}
			return false;
		}
	}
}
