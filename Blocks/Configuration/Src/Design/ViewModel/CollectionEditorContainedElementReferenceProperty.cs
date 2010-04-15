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
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{

    /// <summary>
    /// The <see cref="CollectionEditorContainedElementReferenceProperty"/> represents
    /// a reference property contained within a collection editor.
    /// </summary>
    /// <remarks>
    /// Elements within a collection editor need to report their associated element differently.  
    /// This class handles reporting the appropriate <see cref="ILogicalPropertyContainerElement"/>
    /// when contained within a collection editor.
    /// </remarks>
    /// <seealso cref="CollectionEditorContainedElementProperty"/>
    public class CollectionEditorContainedElementReferenceProperty : ElementReferenceProperty, ILogicalPropertyContainerElement
    {
        ElementViewModel logicalParentElement;


        /// <summary>
        /// Creates a new instnace of <see cref="CollectionEditorContainedElementProperty"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="lookup">The <see cref="ElementLookup"/> service to use when locating elements.</param>
        /// <param name="parent">The parent element view model.</param>
        /// <param name="declaringProperty">The declaring property descriptor.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated by ancestor class")]
        public CollectionEditorContainedElementReferenceProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, lookup, parent, declaringProperty)
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
