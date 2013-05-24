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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents a single category filter configuration settings.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CategoryFilterDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CategoryFilterDataDisplayName")]
    public class CategoryFilterData : LogFilterData
    {
        private const string categoryFilterModeProperty = "categoryFilterMode";
        private const string categoryFiltersProperty = "categoryFilters";

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
        /// </summary>
        public CategoryFilterData()
        {
            Type = typeof(CategoryFilter);
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
        /// </summary>
        /// <param name="categoryFilters">The collection of category names to filter.</param>
        /// <param name="categoryFilterMode">The mode of filtering.</param>
        public CategoryFilterData(NamedElementCollection<CategoryFilterEntry> categoryFilters,
                                  CategoryFilterMode categoryFilterMode)
            : this("category", categoryFilters, categoryFilterMode)
        {
        }

        /// <summary>
        /// <para>Initialize a new named instance of the <see cref="CategoryFilterData"/> class.</para>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="categoryFilters">The collection of category names to filter.</param>
        /// <param name="categoryFilterMode">The mode of filtering.</param>
        public CategoryFilterData(string name, NamedElementCollection<CategoryFilterEntry> categoryFilters,
                                  CategoryFilterMode categoryFilterMode)
            : base(name, typeof(CategoryFilter))
        {
            CategoryFilters = categoryFilters;
            CategoryFilterMode = categoryFilterMode;
        }

        /// <summary>
        /// One of <see cref="CategoryFilterMode"/> enumeration.
        /// </summary>
        [ConfigurationProperty(categoryFilterModeProperty)]
        [ResourceDescription(typeof(DesignResources), "CategoryFilterDataCategoryFilterModeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CategoryFilterDataCategoryFilterModeDisplayName")]
        public CategoryFilterMode CategoryFilterMode
        {
            get { return (CategoryFilterMode)this[categoryFilterModeProperty]; }
            set { this[categoryFilterModeProperty] = value; }
        }

        /// <summary>
        /// Collection of <see cref="CategoryFilterData"/>.
        /// </summary>
        [ConfigurationProperty(categoryFiltersProperty)]
        [ConfigurationCollection(typeof(CategoryFilterEntry))]
        [ResourceDescription(typeof(DesignResources), "CategoryFilterDataCategoryFiltersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CategoryFilterDataCategoryFiltersDisplayName")]
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [EnvironmentalOverrides(false)]
        [DesignTimeReadOnly(false)]
        public NamedElementCollection<CategoryFilterEntry> CategoryFilters
        {
            get { return (NamedElementCollection<CategoryFilterEntry>)base[categoryFiltersProperty]; }

            private set { base[categoryFiltersProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="ILogFilter" /> object represented by this configuration object.
        /// </summary>
        /// <returns>
        /// A <see cref="CategoryFilter"/>.
        /// </returns>
        public override ILogFilter BuildFilter()
        {
            return new CategoryFilter(this.Name, this.CategoryFilters.Select(cfe => cfe.Name).ToArray(), this.CategoryFilterMode);
        }
    }
}
