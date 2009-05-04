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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Represents a mapper for <see cref="FlatFileTraceListenerData"/> configuraiton to Wmi.   
    /// </summary>
	public static class FlatFileTraceListenerDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="FlatFileTraceListenerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(FlatFileTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new FlatFileTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.FileName,
					configurationObject.Header,
					configurationObject.Footer,
					configurationObject.Formatter,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}

		internal static bool SaveChanges(FlatFileTraceListenerSetting setting, ConfigurationElement sourceElement)
		{
			FlatFileTraceListenerData element = (FlatFileTraceListenerData)sourceElement;

			element.FileName = setting.FileName;
			element.Footer = setting.Footer;
			element.Formatter = setting.Formatter;
			element.Header = setting.Header;
		    SourceLevels filter;
            if (ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
		    element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FlatFileTraceListenerSetting));
		}
	}
}
