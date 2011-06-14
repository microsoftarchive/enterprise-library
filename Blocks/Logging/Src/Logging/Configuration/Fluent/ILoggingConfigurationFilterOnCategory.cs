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

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="CategoryFilter"/> instance.
    /// </summary>
    /// <see cref="CategoryFilter"/>
    /// <see cref="CategoryFilterData"/>
    public interface ILoggingConfigurationFilterOnCategory : IFluentInterface
    {
        /// <summary>
        /// Specifies that logging is enabled for the specified categories.<br/>
        /// Disabled for all other categories.
        /// </summary>
        /// <param name="categories">The categories for which logging should be enabled.</param>
        /// <returns>Fluent interface for further configuring logging settings.</returns>
        /// <see cref="CategoryFilter"/>
        /// <see cref="CategoryFilterData"/>
        ILoggingConfigurationOptions AllowAllCategoriesExcept(params string[] categories);

        /// <summary>
        /// Specifies that logging is disabled for the specified categories.<br/>
        /// Enabled for all other categories.
        /// </summary>
        /// <param name="categories">The categories for which logging should be disabled.</param>
        /// <returns>Fluent interface for further configuring logging settings.</returns>
        /// <see cref="CategoryFilter"/>
        /// <see cref="CategoryFilterData"/>
        ILoggingConfigurationOptions DenyAllCategoriesExcept(params string[] categories);
    }
}
