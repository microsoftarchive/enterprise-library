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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	/// <summary>
	/// Represents a mapper for <see cref="XmlTraceListenerData"/> configuration to Wmi.
	/// </summary>
	public static class XmlTraceListenerDataWmiMapper
	{
		/// <summary>
		/// Creates the <see cref="XmlTraceListenerSetting"/> instances that describe the 
		/// configurationObject.
		/// </summary>
		/// <param name="configurationObject">The configuration object for instances that must be managed.</param>
		/// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(XmlTraceListenerData configurationObject,
											  ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new XmlTraceListenerSetting(configurationObject,
											configurationObject.Name,
											configurationObject.FileName,
											configurationObject.TraceOutputOptions.ToString(),
											configurationObject.Filter.ToString()));
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(XmlTraceListenerSetting));
		}

		internal static bool SaveChanges(XmlTraceListenerSetting setting,
										 ConfigurationElement sourceElement)
		{
			XmlTraceListenerData element = (XmlTraceListenerData)sourceElement;

			element.FileName = setting.FileName;
			element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);

		    SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
			return true;
		}
	}
}