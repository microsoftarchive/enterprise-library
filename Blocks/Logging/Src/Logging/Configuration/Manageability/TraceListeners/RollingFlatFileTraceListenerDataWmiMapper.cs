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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Represents a mapper for a <see cref="RollingFlatFileTraceListener"/> configuration to Wmi.
    /// </summary>
	public static class RollingFlatFileTraceListenerDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="RollingFlatFileTraceListenerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(RollingFlatFileTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new RollingFlatFileTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.FileName,
					configurationObject.Header,
					configurationObject.Footer,
					configurationObject.Formatter,
					configurationObject.RollFileExistsBehavior.ToString(),
					configurationObject.RollInterval.ToString(),
					configurationObject.RollSizeKB,
					configurationObject.TimeStampPattern,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}

		internal static bool SaveChanges(RollingFlatFileTraceListenerSetting setting,
			ConfigurationElement sourceElement)
		{
			RollingFlatFileTraceListenerData element = (RollingFlatFileTraceListenerData)sourceElement;

			element.FileName = setting.FileName;
			element.Footer = setting.Footer;
			element.Formatter = setting.Formatter;
			element.Header = setting.Header;
			element.RollFileExistsBehavior
				= ParseHelper.ParseEnum<RollFileExistsBehavior>(setting.RollFileExistsBehavior, false);
			element.RollInterval
				= ParseHelper.ParseEnum<RollInterval>(setting.RollInterval, false);
			element.RollSizeKB = setting.RollSizeKB;
			element.TimeStampPattern = setting.TimeStampPattern;
			element.TraceOutputOptions
				= ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);

		    SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(RollingFlatFileTraceListenerSetting));
		}
	}
}
