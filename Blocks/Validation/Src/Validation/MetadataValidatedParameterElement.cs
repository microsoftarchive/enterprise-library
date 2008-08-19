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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	internal class MetadataValidatedParameterElement : IValidatedElement
	{
		private CompositionType compositionType = CompositionType.And;
		private bool ignoreNulls = false;
		private ParameterInfo parameterInfo;

		public IEnumerable<IValidatorDescriptor> GetValidatorDescriptors()
		{
			if (parameterInfo != null)
			{
				foreach (object attribute in parameterInfo.GetCustomAttributes(typeof(ValidatorAttribute), false))
				{
					yield return (IValidatorDescriptor)attribute;
				}
			}
		}

		public CompositionType CompositionType
		{
			get { return compositionType; }
		}

		public string CompositionMessageTemplate
		{
			get { return null; }
		}

		public string CompositionTag
		{
			get { return null; }
		}

		public bool IgnoreNulls
		{
			get { return ignoreNulls; }
		}

		public string IgnoreNullsMessageTemplate
		{
			get { return null; }
		}

		public string IgnoreNullsTag
		{
			get { return null; }
		}

		public MemberInfo MemberInfo
		{
			get { return null; }
		}

		public Type TargetType
		{
			get { return null; }
		}

		public void UpdateFlyweight(ParameterInfo parameterInfo)
		{
			this.parameterInfo = parameterInfo;
		}
	}
}
