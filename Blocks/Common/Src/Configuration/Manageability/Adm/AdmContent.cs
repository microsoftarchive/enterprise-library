//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    /// <summary>
    /// Represents the contents of an ADM template file.
    /// </summary>
    public class AdmContent
    {
        List<AdmCategory> categories;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="AdmContentBuilder"/> class.
        /// </summary>
        public AdmContent()
        {
            categories = new List<AdmCategory>();
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public IEnumerable<AdmCategory> Categories
        {
            get { return categories; }
        }

        /// <summary>
        /// Add a category to the content
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void AddCategory(AdmCategory category)
        {
            categories.Add(category);
        }

        /// <summary>
        /// Writes the contents represented by the receiver to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> to where the contents should be written.</param>
        public void Write(TextWriter writer)
        {
            foreach (AdmCategory category in categories)
            {
                category.Write(writer);
            }
        }
    }
}
