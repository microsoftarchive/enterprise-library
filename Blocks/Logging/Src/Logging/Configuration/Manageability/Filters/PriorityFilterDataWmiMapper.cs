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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Represents a mapper for <see cref="PriorityFilterData"/> configuration to Wmi.
    /// </summary>
    public static class PriorityFilterDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="PriorityFilterSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(PriorityFilterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new PriorityFilterSetting(configurationObject,
                                          configurationObject.Name,
                                          configurationObject.MaximumPriority,
                                          configurationObject.MinimumPriority));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(PriorityFilterSetting));
        }

        internal static bool SaveChanges(PriorityFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            PriorityFilterData element = (PriorityFilterData)sourceElement;

            element.MaximumPriority = setting.MaximumPriority;
            element.MinimumPriority = setting.MinimumPriority;

            return true;
        }
    }
}
