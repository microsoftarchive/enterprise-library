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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class MockMemberAccessValidatorBuilderFactory : MemberAccessValidatorBuilderFactory
	{
		public Dictionary<string, ValueAccessValidatorBuilder> requestedMembers
			= new Dictionary<string, ValueAccessValidatorBuilder>();
		public Dictionary<string, CompositeValidatorBuilder> requestedTypes
			= new Dictionary<string, CompositeValidatorBuilder>();

		public MockMemberAccessValidatorBuilderFactory()
			: base()
		{ }

		public MockMemberAccessValidatorBuilderFactory(MemberValueAccessBuilder valueAccessBuilder)
			: base(valueAccessBuilder)
		{ }

		public override ValueAccessValidatorBuilder GetFieldValueAccessValidatorBuilder(FieldInfo fieldInfo,
			IValidatedElement validatedElement)
		{
			ValueAccessValidatorBuilder validatorBuilder = base.GetFieldValueAccessValidatorBuilder(fieldInfo,
				validatedElement);
			this.requestedMembers.Add(fieldInfo.DeclaringType.Name + "." + fieldInfo.Name, validatorBuilder);
			return validatorBuilder;
		}

		public override ValueAccessValidatorBuilder GetMethodValueAccessValidatorBuilder(MethodInfo methodInfo,
			IValidatedElement validatedElement)
		{
			ValueAccessValidatorBuilder validatorBuilder = base.GetMethodValueAccessValidatorBuilder(methodInfo,
				validatedElement);
			this.requestedMembers.Add(methodInfo.DeclaringType.Name + "." + methodInfo.Name, validatorBuilder);
			return validatorBuilder;
		}

		public override ValueAccessValidatorBuilder GetPropertyValueAccessValidatorBuilder(PropertyInfo propertyInfo,
			IValidatedElement validatedElement)
		{
			ValueAccessValidatorBuilder validatorBuilder = base.GetPropertyValueAccessValidatorBuilder(propertyInfo,
				validatedElement);
			this.requestedMembers.Add(propertyInfo.DeclaringType.Name + "." + propertyInfo.Name, validatorBuilder);
			return validatorBuilder;
		}

		public override CompositeValidatorBuilder GetTypeValidatorBuilder(Type type,
			IValidatedElement validatedElement)
		{
			CompositeValidatorBuilder validatorBuilder = base.GetTypeValidatorBuilder(type, validatedElement);
			this.requestedTypes.Add(type.Name, validatorBuilder);
			return validatorBuilder;
		}
	}
}
