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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Denotes an associated with another element.
    /// </summary>
    public interface IElementAssociation
    {
        /// <summary>
        /// When implemented, returns the associated <see cref="ElementViewModel"/>
        /// </summary>
        ElementViewModel AssociatedElement { get; }

        /// <summary>
        /// When implemented, returns an element name that may differ from <see cref="ElementViewModel.Name"/>
        /// </summary>
        string ElementName { get; }
    }
}
