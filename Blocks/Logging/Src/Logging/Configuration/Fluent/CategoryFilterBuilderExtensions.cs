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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="ILoggingConfigurationOptions"/> extensions to configure <see cref="CategoryFilter"/> instances.
    /// </summary>
    /// <seealso cref="CategoryFilter"/>
    /// <seealso cref="CategoryFilterData"/>
    public static class CategoryFilterBuilderExtensions
    {
        /// <summary>
        /// Adds an <see cref="CategoryFilter"/> instance to the logging configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="categoryFilterName">Name of the <see cref="CategoryFilter"/> instance added to configuration.</param>
        /// <seealso cref="CategoryFilter"/>
        /// <seealso cref="CategoryFilterData"/>
        public static ILoggingConfigurationFilterOnCategory FilterOnCategory(this ILoggingConfigurationOptions context, string categoryFilterName)
        {
            if (categoryFilterName == null)
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "categoryFilterName");

            return new FilterOnCategoryBuilder(context, categoryFilterName);
        }

        private class FilterOnCategoryBuilder : LoggingConfigurationExtension, ILoggingConfigurationFilterOnCategory
        {
            CategoryFilterData categoryFilter;

            public FilterOnCategoryBuilder(ILoggingConfigurationOptions context, string logFilterName)
                :base(context)
            {
                categoryFilter = new CategoryFilterData()
                {
                    Name = logFilterName
                };

                LoggingSettings.LogFilters.Add(categoryFilter);
            }

            public ILoggingConfigurationOptions DenyAllCategoriesExcept(params string[] categories)
            {
                if (categories == null)
                    throw new ArgumentNullException("categories");

                categoryFilter.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
                AddCategoriesToFilter(categories);

                return base.LoggingOptions;
            }

            public ILoggingConfigurationOptions AllowAllCategoriesExcept(params string[] categories)
            {
                if (categories == null)
                    throw new ArgumentNullException("categories");

                categoryFilter.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
                AddCategoriesToFilter(categories);

                return base.LoggingOptions;
            }

            private void AddCategoriesToFilter(string[] categories)
            {
                categoryFilter.CategoryFilters.Clear();
                foreach (string category in categories)
                {
                    categoryFilter.CategoryFilters.Add(new CategoryFilterEntry(category));
                }
            }
        }
    }
}
