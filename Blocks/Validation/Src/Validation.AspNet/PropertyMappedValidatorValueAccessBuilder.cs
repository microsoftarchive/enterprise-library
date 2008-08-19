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
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet
{
	internal class PropertyMappedValidatorValueAccessBuilder : MemberValueAccessBuilder
	{
		protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
		{
			throw new NotSupportedException();
		}

		protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
		{
			throw new NotSupportedException();
		}

		protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
		{
			return new PropertyMappedValidatorValueAccess(propertyInfo.Name);
		}
	}
}