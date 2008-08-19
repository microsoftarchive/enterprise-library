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

using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Contains category filter section.
    /// </summary>
    public class CategoryFilterSettings
    {
        private CategoryFilterMode categoryFilterMode;

        private NamedElementCollection<CategoryFilterEntry> categoryFilters;

        /// <summary>
        /// Gets and sets the mode in which to filter.
        /// </summary>
        public CategoryFilterMode CategoryFilterMode
        {
            get { return categoryFilterMode; }
            set { categoryFilterMode = value; }
        }

        /// <summary>
        /// Gets and sets the collection of categories to filter.
        /// </summary>
        public NamedElementCollection<CategoryFilterEntry> CategoryFilters
        {
            get { return categoryFilters; }            
        }

        /// <summary>
        /// Creates a new <c>CategoryFilterSettings</c> object with initial section.
        /// </summary>
        /// <param name="categoryFilterMode">The mode in which to filter.</param>
        /// <param name="categoryFilters">The collection of categories to filter.</param>
        public CategoryFilterSettings(CategoryFilterMode categoryFilterMode, NamedElementCollection<CategoryFilterEntry> categoryFilters)
        {
            this.categoryFilterMode = categoryFilterMode;
            this.categoryFilters = categoryFilters;
        }

        /// <summary>
        /// Returns a user friendly formatted string which illustrates the filter section.
        /// </summary>
        /// <returns>See summary.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (CategoryFilterMode == CategoryFilterMode.AllowAllExceptDenied)
            {
                sb.Append(Resources.CategoryFilterSummaryAllow);
            }
            else
            {
                sb.Append(Resources.CategoryFilterSummaryDeny);
            }
            sb.Append(": ");

            bool first = true;
            foreach (CategoryFilterEntry categoryFilter in CategoryFilters)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.Append(categoryFilter.Name);
                first = false;
            }

            return sb.ToString();
        }
    }
}