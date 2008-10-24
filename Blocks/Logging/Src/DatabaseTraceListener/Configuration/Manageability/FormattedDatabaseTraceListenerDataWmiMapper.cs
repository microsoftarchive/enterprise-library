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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for <see cref="FormattedDatabaseTraceListenerData"/> configuration to Wmi.
    /// </summary>
	public static class FormattedDatabaseTraceListenerDataWmiMapper 
	{
        /// <summary>
        /// Creates the <see cref="FormattedDatabaseTraceListenerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(FormattedDatabaseTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new FormattedDatabaseTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.DatabaseInstanceName,
					configurationObject.WriteLogStoredProcName,
					configurationObject.AddCategoryStoredProcName,
					configurationObject.Formatter,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FormattedDatabaseTraceListenerSetting));
		}


        internal static bool SaveChanges(FormattedDatabaseTraceListenerSetting setting, ConfigurationElement sourceElement)
        {
            var element = (FormattedDatabaseTraceListenerData) sourceElement;
            element.AddCategoryStoredProcName = setting.AddCategoryStoredProcName;
            element.DatabaseInstanceName = setting.DatabaseInstanceName;
            element.Formatter = setting.Formatter;
            element.WriteLogStoredProcName = setting.WriteLogStoredProcName;

            SourceLevels filter;
            TraceOptions traceOptions;

            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }

            if(ParseHelper.TryParseEnum(setting.TraceOutputOptions, out traceOptions))
            {
                element.TraceOutputOptions = traceOptions;
            }

            return true;
        }
	}
}
