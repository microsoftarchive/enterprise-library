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
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary>
    /// Specifies additional metadata for the FilteredFileNameEditor editor.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Property)]
    public sealed class FilteredFileNameEditorAttribute : Attribute
    {
        private string filter;

        /// <summary>
        /// Initialize a new instance of the <see cref="FilteredFileNameEditorAttribute"/> class with the <see cref="Type"/> containing the resources and the resource key.
        /// </summary>
        /// <param name="resourceType">The <see cref="Type"/> containing the resources.</param>
        /// <param name="resourceKey">The resource key.</param>
        public FilteredFileNameEditorAttribute(Type resourceType, string resourceKey)
        {
            if (null == resourceType) throw new ArgumentNullException("resourceType");

            this.filter = ResourceStringLoader.LoadString(resourceType.FullName, resourceKey, resourceType.Assembly); ;
        }

        /// <summary>
        /// Gets the filter for the dialog.
        /// </summary>
        /// <value>
        /// The filter for the dialog.
        /// </value>
        public string Filter
        {
            get { return this.filter; }
        }
    }
}
