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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	/// <summary>
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.WmiTraceListenerData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public partial class WmiTraceListenerSetting : TraceListenerSetting
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="WmiTraceListenerSetting"/> class ith the configuration source element,
        /// the name of the listner and the trace output options.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the listener.</param>
        /// <param name="traceOutputOptions">The trace output opitons.</param>
		/// <param name="filter">The filter value.</param>
		public WmiTraceListenerSetting(WmiTraceListenerData sourceElement,
			string name,
			string traceOutputOptions,
			string filter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{ }

        /// <summary>
        /// Returns an enumeration of the published <see cref="WmiTraceListenerSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<WmiTraceListenerSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<WmiTraceListenerSetting>();
		}

        /// <summary>
        /// Returns the <see cref="WmiTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="WmiTraceListenerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static WmiTraceListenerSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<WmiTraceListenerSetting>(ApplicationName, SectionName, Name);
		}

        /// <summary>
        /// Saves the changes on the <see cref="WmiTraceListenerSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(System.Configuration.ConfigurationElement sourceElement)
		{
			return false;	// no changes to save
		}
	}
}
