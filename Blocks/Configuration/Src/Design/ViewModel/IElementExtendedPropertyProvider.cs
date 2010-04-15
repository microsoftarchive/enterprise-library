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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="IElementExtendedPropertyProvider"/> interface allows
    /// an implementer to provide additional properties to <see cref="ElementViewModel"/>s.
    /// </summary>
    /// <remarks>
    /// Extender providers must be added to the <see cref="ElementLookup"/> 
    /// through <see cref="ElementLookup.AddElement"/> or <see cref="ElementLookup.AddCustomElement"/> to begin
    /// extending <see cref="ElementViewModel"/> items.
    /// </remarks>
    public interface IElementExtendedPropertyProvider
    {
        /// <summary>
        /// Gets a value that indicates this extender provides <see cref="Property"/>
        /// elements to a <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="subject">The <see cref="ElementViewModel"/> to determine properties for.</param>
        /// <returns>
        /// Should return <see langword="true"/> if this extender provides properties for the <paramref name="subject"/>.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        bool CanExtend(ElementViewModel subject);

        /// <summary>
        /// Returns the set of <see cref="Property"/> elements to add to the <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="subject">The <see cref="ElementViewModel"/> to provide properties for.</param>
        /// <returns>
        /// Returns the set of additional <see cref="Property"/> items to add to the <paramref name="subject"/>.
        /// </returns>
        IEnumerable<Property> GetExtendedProperties(ElementViewModel subject);

    }
}
