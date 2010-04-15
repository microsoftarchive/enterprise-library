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
    /// Denotes an associated from a property to its logical containing (or parent) element.
    /// </summary>
    public interface ILogicalPropertyContainerElement
    {
        /// <summary>
        /// When implemented, returns the logical container (or parent) <see cref="ElementViewModel"/>.
        /// </summary>
        ElementViewModel ContainingElement { get; }

        /// <summary>
        /// When implemented, returns a display name for the logically containing element, which is used for e.g. validation results.<br/>
        /// </summary>
        string ContainingElementDisplayName { get; }
    }
}
