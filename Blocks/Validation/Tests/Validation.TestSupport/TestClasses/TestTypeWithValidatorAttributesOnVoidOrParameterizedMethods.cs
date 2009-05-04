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
	public class TestTypeWithValidatorAttributesOnVoidOrParameterizedMethods
	{
		[MockValidator(false, MessageTemplate = "VoidMethodWithoutParameters")]
		public void VoidMethodWithoutParameters()
		{
		}

		[MockValidator(false, MessageTemplate = "VoidMethodWithParameters")]
		public void VoidMethodWithParameters(object param1)
		{
		}

		[MockValidator(false, MessageTemplate = "VoidMethodWithParamParameters")]
		public void VoidMethodWithParamParameters(params object[] param1)
		{
		}

		[MockValidator(false, MessageTemplate = "NonVoidMethodWithoutParameters")]
		public object NonVoidMethodWithoutParameters()
		{
			return null;
		}

		[MockValidator(false, MessageTemplate = "NonVoidMethodWithParameters")]
		public object NonVoidMethodWithParameters(object param1)
		{
			return null;
		}

		[MockValidator(false, MessageTemplate = "NonVoidMethodWithParamParameters")]
		public object NonVoidMethodWithParamParameters(params object[] param1)
		{
			return null;
		}
	}
}
