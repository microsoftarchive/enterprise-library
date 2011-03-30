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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents a single category filter configuration settings.
    /// </summary>
    partial class CategoryFilterData
    {
        /// <summary>
        /// One of <see cref="CategoryFilterMode"/> enumeration.
        /// </summary>
        public CategoryFilterMode CategoryFilterMode
        {
            get;
            set;
        }


        private readonly NamedElementCollection<CategoryFilterEntry> categoryFilters = new NamedElementCollection<CategoryFilterEntry>();

        /// <summary>
        /// Collection of <see cref="CategoryFilterData"/>.
        /// </summary>
        public NamedElementCollection<CategoryFilterEntry> CategoryFilters
        {
            get { return this.categoryFilters; }
        }
    }
}
