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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Provides an implementation for <see cref="SystemDiagnosticsTraceListenerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
	public class SystemDiagnosticsTraceListenerDataManageabilityProvider
		: BasicCustomTraceListenerDataManageabilityProvider<SystemDiagnosticsTraceListenerData>
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="SystemDiagnosticsTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
		public SystemDiagnosticsTraceListenerDataManageabilityProvider()
		{
			SystemDiagnosticsTraceListenerDataWmiMapper.RegisterWmiTypes();
		}

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(SystemDiagnosticsTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			SystemDiagnosticsTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
