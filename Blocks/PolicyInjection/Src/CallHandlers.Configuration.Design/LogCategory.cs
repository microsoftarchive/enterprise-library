//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// Stores a single entry in the list of log categories configured by the <see cref="LogCallHandlerNode"/>.
    /// </summary>
    public class LogCategory
    {
        string categoryName;

        /// <summary>
        /// Create a new <see cref="LogCategory" /> with an empty string.
        /// </summary>
        public LogCategory()
        {
        }

        /// <summary>
        /// Create a new <see cref="LogCategory" /> with the given string.
        /// </summary>
        /// <param name="categoryName"><see cref="System.String" /> to use as the category.</param>
        public LogCategory(string categoryName)
        {
            this.categoryName = categoryName;
        }

        /// <summary>
        /// The category name.
        /// </summary>
        /// <value>Gets or sets the category name.</value>
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        /// <summary>
        /// Convert this node to a string.
        /// </summary>
        /// <returns><paramref name="CategoryName"/> of this instance.</returns>
        public override string ToString()
        {
            return categoryName;
        }
    }
}
