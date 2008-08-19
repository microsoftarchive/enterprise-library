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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	internal class MetadataValidatorBuilder : ValidatorBuilderBase
	{
		public MetadataValidatorBuilder()
			: base()
		{ }

		public MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
			: base(memberAccessValidatorFactory)
		{ }

		public Validator CreateValidator(Type type, string ruleset)
		{
			return CreateValidator(new MetadataValidatedType(type, ruleset));
		}

		#region test only methods

		internal Validator CreateValidatorForType(Type type, string ruleset)
		{
			return CreateValidatorForValidatedElement(new MetadataValidatedType(type, ruleset), 
				this.GetCompositeValidatorBuilderForType);
		}

		internal Validator CreateValidatorForProperty(PropertyInfo propertyInfo, string ruleset)
		{
			return CreateValidatorForValidatedElement(new MetadataValidatedElement(propertyInfo, ruleset), 
				this.GetCompositeValidatorBuilderForProperty);
		}

		internal Validator CreateValidatorForField(FieldInfo fieldInfo, string ruleset)
		{
			return CreateValidatorForValidatedElement(new MetadataValidatedElement(fieldInfo, ruleset), 
				this.GetCompositeValidatorBuilderForField);
		}

		internal Validator CreateValidatorForMethod(MethodInfo methodInfo, string ruleset)
		{
			return CreateValidatorForValidatedElement(new MetadataValidatedElement(methodInfo, ruleset),
				this.GetCompositeValidatorBuilderForMethod);
		}

		#endregion
	}
}