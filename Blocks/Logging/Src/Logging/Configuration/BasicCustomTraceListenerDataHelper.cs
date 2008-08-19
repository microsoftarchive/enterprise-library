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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	internal class BasicCustomTraceListenerDataHelper
		: CustomProviderDataHelper<BasicCustomTraceListenerData>
	{
		private static readonly ConfigurationProperty traceListenerTypeProperty =
			new ConfigurationProperty(TraceListenerData.listenerDataTypeProperty,
										typeof(string),
										null,
										null,
										null,
										ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty traceOutputOptionsProperty =
			new ConfigurationProperty(TraceListenerData.traceOutputOptionsProperty,
										typeof(TraceOptions),
										TraceOptions.None,
										null,
										null,
										ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty initDataProperty =
			new ConfigurationProperty(SystemDiagnosticsTraceListenerData.initDataProperty,
										typeof(string),
										string.Empty,
										null,
										null,
										ConfigurationPropertyOptions.None);


		internal BasicCustomTraceListenerDataHelper(BasicCustomTraceListenerData helpedCustomProviderData)
			: base(helpedCustomProviderData)
		{
			propertiesCollection.Add(traceListenerTypeProperty);
			propertiesCollection.Add(traceOutputOptionsProperty);
			propertiesCollection.Add(initDataProperty);
		}

		protected override bool IsKnownPropertyName(string propertyName)
		{
			return base.IsKnownPropertyName(propertyName)
				|| TraceListenerData.listenerDataTypeProperty.Equals(propertyName)
				|| TraceListenerData.traceOutputOptionsProperty.Equals(propertyName)
				|| BasicCustomTraceListenerData.initDataProperty.Equals(propertyName);
		}
	}
}
