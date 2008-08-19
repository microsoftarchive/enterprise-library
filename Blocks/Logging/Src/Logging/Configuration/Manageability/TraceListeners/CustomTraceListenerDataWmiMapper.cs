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
    /// Represents a mapper for <see cref="CustomTraceListenerData"/> configuration to Wmi.
    /// </summary>
    public static class CustomTraceListenerDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="CustomTraceListenerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(CustomTraceListenerData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomTraceListenerSetting(data,
                                               data.Name,
                                               data.Type.AssemblyQualifiedName,
                                               data.InitData,
                                               CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes),
                                               data.TraceOutputOptions.ToString(),
											   data.Filter.ToString(),
                                               data.Formatter));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomTraceListenerSetting));
        }

        internal static bool SaveChanges(CustomTraceListenerSetting setting,
                                         ConfigurationElement sourceElement)
        {
            BasicCustomTraceListenerData element = (BasicCustomTraceListenerData)sourceElement;

            element.TypeName = setting.ListenerType;
            CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);
            if (element is CustomTraceListenerData)
            {
                ((CustomTraceListenerData)element).Formatter = setting.Formatter;
            }
            element.InitData = setting.InitData;
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