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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for <see cref="LoggingSettings"/> configuration to Wmi.
    /// </summary>
    public static class LoggingSettingsWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="LoggingBlockSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(LoggingSettings configurationObject,
            ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new LoggingBlockSetting(
                    configurationObject,
                    configurationObject.DefaultCategory,
                    configurationObject.LogWarningWhenNoCategoriesMatch,
                    configurationObject.TracingEnabled,
                    configurationObject.RevertImpersonation));
        }

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="traceSourceData">The <see cref="TraceSourceData" /> instance to copy properties./></param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        /// <param name="sourceKind">A plain category source.</param>
        public static void GenerateTraceSourceDataWmiObjects(TraceSourceData traceSourceData,
            ICollection<ConfigurationSetting> wmiSettings, string sourceKind)
        {
            string[] referencedTraceListeners = new string[traceSourceData.TraceListeners.Count];
            for (int i = 0; i < traceSourceData.TraceListeners.Count; i++)
            {
                referencedTraceListeners[i]
                    = traceSourceData.TraceListeners.Get(i).Name;
            }

            wmiSettings.Add(
                new TraceSourceSetting(traceSourceData,
                    traceSourceData.Name,
                    traceSourceData.DefaultLevel.ToString(),
                    referencedTraceListeners,
                    sourceKind));
        }

        internal static bool SaveChanges(LoggingBlockSetting setting, ConfigurationElement sourceElement)
        {
            LoggingSettings section = (LoggingSettings)sourceElement;

            section.DefaultCategory = setting.DefaultCategory;
            section.LogWarningWhenNoCategoriesMatch = setting.LogWarningWhenNoCategoriesMatch;
            section.TracingEnabled = setting.TracingEnabled;
            section.RevertImpersonation = setting.RevertImpersonation;

            return true;
        }

        internal static bool SaveChanges(TraceSourceSetting setting, ConfigurationElement sourceElement)
        {
            TraceSourceData element = (TraceSourceData)sourceElement;

            element.DefaultLevel = ParseHelper.ParseEnum<SourceLevels>(setting.DefaultLevel, false);
            element.TraceListeners.Clear();
            foreach (string traceListenerName in setting.TraceListeners)
            {
                element.TraceListeners.Add(new TraceListenerReferenceData(traceListenerName));
            }

            return true;
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LoggingBlockSetting),
                typeof(TraceSourceSetting));
        }
    }
}
