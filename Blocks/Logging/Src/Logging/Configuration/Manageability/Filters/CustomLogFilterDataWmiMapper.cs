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
    /// Represents a mapper for <see cref="CustomLogFilterData"/> configuration to Wmi.
    /// </summary>
    public static class CustomLogFilterDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="CustomFilterSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(CustomLogFilterData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomFilterSetting(data,
                                        data.Name,
                                        data.Type.AssemblyQualifiedName,
                                        CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomFilterSetting));
        }

        internal static bool SaveChanges(CustomFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            CustomLogFilterData element = (CustomLogFilterData)sourceElement;

            element.TypeName = setting.FilterType;
            CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);

            return true;
        }
    }
}
