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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    /// <summary>
    /// Represents a category on an ADM template file.
    /// </summary>
    public class AdmCategory
    {
        internal const String CategoryEndTemplate = "END CATEGORY\t; \"{0}\"";
        internal const String CategoryStartTemplate = "CATEGORY \"{0}\"";

        List<AdmCategory> categories;
        String name;
        List<AdmPolicy> policies;

        /// <summary>
        /// Initialize a new instance of the <see cref="AdmCategory"/> class.
        /// </summary>
        /// <param name="categoryName">
        /// The categor name.
        /// </param>
        public AdmCategory(String categoryName)
        {
            name = categoryName;

            categories = new List<AdmCategory>();
            policies = new List<AdmPolicy>();
        }

        /// <summary>
        /// Gest the list of sub categories.
        /// </summary>
        public IEnumerable<AdmCategory> Categories
        {
            get { return categories; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the list of policies in a category.
        /// </summary>
        public IEnumerable<AdmPolicy> Policies
        {
            get { return policies; }
        }

        internal void AddCategory(AdmCategory category)
        {
            categories.Add(category);
        }

        internal void AddPolicy(AdmPolicy policy)
        {
            policies.Add(policy);
        }

        internal void Write(TextWriter writer)
        {
            writer.WriteLine(String.Format(CultureInfo.InvariantCulture, CategoryStartTemplate, name));
            foreach (AdmCategory category in categories)
            {
                category.Write(writer);
            }
            foreach (AdmPolicy policy in policies)
            {
                policy.Write(writer);
            }
            writer.WriteLine(String.Format(CultureInfo.InvariantCulture, CategoryEndTemplate, name));
        }
    }
}
