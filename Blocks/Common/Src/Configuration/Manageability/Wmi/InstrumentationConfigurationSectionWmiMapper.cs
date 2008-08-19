//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	///<summary>
	/// Represents a mapper for configuration to Wmi.
	///</summary>
	public static class InstrumentationConfigurationSectionWmiMapper
	{
        /// <summary>
        /// Generates Wmi objects for a configuration section.
        /// </summary>
        /// <param name="configurationObject">The configuraiton objects.</param>
        /// <param name="wmiSettings">The Wmi settings.</param>
		public static void GenerateWmiObjects(InstrumentationConfigurationSection configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new InstrumentationSetting(configurationObject,
					configurationObject.EventLoggingEnabled,
					configurationObject.PerformanceCountersEnabled,
					configurationObject.WmiEnabled));
		}

        /// <summary>
        /// Save changes made to Wmi objects.
        /// </summary>
        /// <param name="instrumentationSetting">
        /// The instrumentaiton settings.
        /// </param>
        /// <param name="sourceElement">
        /// The element to save.
        /// </param>
        /// <returns>true if the changes were saved; otherwise, false.</returns>
		public static bool SaveChanges(InstrumentationSetting instrumentationSetting, ConfigurationElement sourceElement)
		{
			InstrumentationConfigurationSection section = (InstrumentationConfigurationSection)sourceElement;

			section.EventLoggingEnabled = instrumentationSetting.EventLoggingEnabled;
			section.PerformanceCountersEnabled = instrumentationSetting.PerformanceCountersEnabled;
			section.WmiEnabled = instrumentationSetting.WmiEnabled;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(InstrumentationSetting));
		}
	}
}