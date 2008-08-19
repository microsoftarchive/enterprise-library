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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.CategoryFilterData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public partial class CategoryFilterSetting : LogFilterSetting
    {
        string categoryFilterMode;
        string[] categoryFilters;

        /// <summary>
        /// Initialize a new instance of the <see cref="CategoryFilterSetting"/> class with 
        /// the filter configuration, the name of the filter, the filter mode and the categories.
        /// </summary>
        /// <param name="sourceElement">The filter configuration.</param>
        /// <param name="name">The name of the filter.</param>
        /// <param name="categoryFilterMode">The filter mode.</param>
        /// <param name="categoryFilters">The list of filters.</param>
        public CategoryFilterSetting(CategoryFilterData sourceElement,
                                     string name,
                                     string categoryFilterMode,
                                     string[] categoryFilters)
            : base(sourceElement, name)
        {
            this.categoryFilterMode = categoryFilterMode;
            this.categoryFilters = categoryFilters;
        }

        /// <summary>
        /// Gets the name of the filter mode value for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string CategoryFilterMode
        {
            get { return categoryFilterMode; }
            set { categoryFilterMode = value; }
        }

        /// <summary>
        /// Gets the set of filtered categories for the represented configuration element.
        /// </summary>
        /// <remarks>
        /// The categories are encoded as an string array of category names.
        /// </remarks>
        [ManagementConfiguration]
        public string[] CategoryFilters
        {
            get { return categoryFilters; }
            set { categoryFilters = value; }
        }

        /// <summary>
        /// Returns the <see cref="CategoryFilterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CategoryFilterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static CategoryFilterSetting BindInstance(string ApplicationName,
                                                         string SectionName,
                                                         string Name)
        {
            return BindInstance<CategoryFilterSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="CategoryFilterSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<CategoryFilterSetting> GetInstances()
        {
            return GetInstances<CategoryFilterSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="CategoryFilterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CategoryFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}