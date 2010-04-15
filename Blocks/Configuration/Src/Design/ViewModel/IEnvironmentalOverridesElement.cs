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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Interface that contains the necessary information needed to environmentally override a property.
    /// </summary>
    public interface IEnvironmentalOverridesElement
    {
        /// <summary>
        /// Gets a value indicating that this element's path is reliable.
        /// </summary>
        /// <remarks>
        /// The element's xpath is used in serializes and deserializing overrides for an element and must be reliable
        /// for the element to participate in the environmental overrides capability.
        /// </remarks>
        bool IsElementPathReliableXPath { get; }
    }
}
