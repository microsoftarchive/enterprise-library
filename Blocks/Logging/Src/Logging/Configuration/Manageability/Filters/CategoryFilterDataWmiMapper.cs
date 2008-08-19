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
    /// Represents a mapper for <see cref="CategoryFilterData"/> confituration to Wmi.
    /// </summary>
    public static class CategoryFilterDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="CategoryFilterSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(CategoryFilterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            string[] categoryFilters = new string[configurationObject.CategoryFilters.Count];
            for (int i = 0; i < configurationObject.CategoryFilters.Count; i++)
            {
                categoryFilters[i]
                    = configurationObject.CategoryFilters.Get(i).Name;
            }

            wmiSettings.Add(
                new CategoryFilterSetting(configurationObject,
                                          configurationObject.Name,
                                          configurationObject.CategoryFilterMode.ToString(),
                                          categoryFilters));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CategoryFilterSetting));
        }

        internal static bool SaveChanges(CategoryFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            CategoryFilterData element = (CategoryFilterData)sourceElement;

            element.CategoryFilterMode = ParseHelper.ParseEnum<CategoryFilterMode>(setting.CategoryFilterMode, false);
            element.CategoryFilters.Clear();
            foreach (string name in setting.CategoryFilters)
            {
                element.CategoryFilters.Add(new CategoryFilterEntry(name));
            }

            return true;
        }
    }
}