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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	internal class ReflectionMemberValueAccessBuilder : MemberValueAccessBuilder
	{
		protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
		{
			return new FieldValueAccess(fieldInfo);
		}

		protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
		{
			return new MethodValueAccess(methodInfo);
		}

		protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
		{
			return new PropertyValueAccess(propertyInfo);
		}
	}
}