//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	internal class CustomTraceListenerDataHelper
		: BasicCustomTraceListenerDataHelper
	{
		private static readonly ConfigurationProperty formatterProperty =
			new ConfigurationProperty(CustomTraceListenerData.formatterNameProperty,
										typeof(string),
										null,   // no reasonable default
										null,   // use default converter
										null,	// no validations
										ConfigurationPropertyOptions.None);

		internal CustomTraceListenerDataHelper(CustomTraceListenerData helpedCustomProviderData)
			: base(helpedCustomProviderData)
		{
			propertiesCollection.Add(formatterProperty);
		}

		protected override bool IsKnownPropertyName(string propertyName)
		{
			return base.IsKnownPropertyName(propertyName)
				|| CustomTraceListenerData.formatterNameProperty.Equals(propertyName);
		}
	}
}
