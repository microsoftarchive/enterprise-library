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
using System.Globalization;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Represents a property contained within a collection editor and provides
    /// logic to return the appropriate logical contained element <see cref="ILogicalPropertyContainerElement"/>.
    /// </summary>
    /// <seealso cref="ElementProperty"/>
    public class CollectionEditorContainedElementProperty : ElementProperty, ILogicalPropertyContainerElement
    {
        ElementViewModel logicalParentElement;

        /// <summary>
        /// Creates a new instnace of <see cref="CollectionEditorContainedElementProperty"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="parent">The parent element view model.</param>
        /// <param name="declaringProperty">The declaring property descriptor.</param>
        public CollectionEditorContainedElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, Enumerable.Empty<Attribute>())
        {
            logicalParentElement = parent.ParentElement.ParentElement;
        }

        #region ILogicalPropertyContainerElement Members

        ElementViewModel ILogicalPropertyContainerElement.ContainingElement
        {
            get { return logicalParentElement; }
        }

        string ILogicalPropertyContainerElement.ContainingElementDisplayName
        {
            get
            {
                return string.Format(
                  CultureInfo.CurrentCulture, Resources.CollectionEditorContainedElementPropertyDisplayNameFormat,
                  logicalParentElement.Name,
                  DeclaringElement.ParentElement.DeclaringProperty.DisplayName);
            }
        }

        #endregion
    }
}
