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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
    /// <summary>
    /// Represents a mapper for <see cref="TextFormatterData"/> configuration to Wmi.
    /// </summary>
    public static class TextFormatterDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="TextFormatterSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(TextFormatterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(new TextFormatterSetting(configurationObject, configurationObject.Name, configurationObject.Template));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(TextFormatterSetting));
        }

        internal static bool SaveChanges(TextFormatterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            TextFormatterData element = (TextFormatterData)sourceElement;

            element.Template = setting.Template;

            return true;
        }
    }
}