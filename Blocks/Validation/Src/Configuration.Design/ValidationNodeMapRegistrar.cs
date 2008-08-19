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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	sealed class ValidationNodeMapRegistrar : NodeMapRegistrar
	{
		public ValidationNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			base.AddMultipleNodeMap(Resources.AndCompositeValidatorNodeName, typeof(AndCompositeValidatorNode), typeof(AndCompositeValidatorData));
			base.AddMultipleNodeMap(Resources.OrCompositeValidatorNodeName, typeof(OrCompositeValidatorNode), typeof(OrCompositeValidatorData));
			base.AddMultipleNodeMap(Resources.CustomValidatorNodeName, typeof(CustomValidatorNode), typeof(CustomValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectValidatorNodeName, typeof(ObjectValidatorNode), typeof(ObjectValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(ObjectCollectionValidatorNode), typeof(ObjectCollectionValidatorData));
			base.AddMultipleNodeMap(Resources.PropertyComparisonValidatorNodeName, typeof(PropertyComparisonValidatorNode), typeof(PropertyComparisonValidatorData));

			base.AddMultipleNodeMap(Resources.NotNullValidatorNodeName, typeof(NotNullValidatorNode), typeof(NotNullValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(RangeValidatorNode), typeof(RangeValidatorData));
			base.AddMultipleNodeMap(Resources.DateRangeValidatorNodeName, typeof(DateRangeValidatorNode), typeof(DateTimeRangeValidatorData));
			base.AddMultipleNodeMap(Resources.StringLengthValidatorNodeName, typeof(StringLengthValidatorNode), typeof(StringLengthValidatorData));
			base.AddMultipleNodeMap(Resources.RegexValidatorNodeName, typeof(RegexValidatorNode), typeof(RegexValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(TypeConversionValidatorNode), typeof(TypeConversionValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(EnumConversionValidatorNode), typeof(EnumConversionValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(RelativeDateTimeValidatorNode), typeof(RelativeDateTimeValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(ContainsCharactersValidatorNode), typeof(ContainsCharactersValidatorData));
			base.AddMultipleNodeMap(Resources.ObjectCollectionValidatorNodeName, typeof(DomainValidatorNode), typeof(DomainValidatorData));
		}
	}
}
