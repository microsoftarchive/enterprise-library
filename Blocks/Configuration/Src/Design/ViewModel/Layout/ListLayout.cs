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
    /// Layout class used to display a vertical list of <see cref="ViewModel"/>'s.
    /// </summary>
    /// <remarks>
    /// For a vertical list of <see cref="ElementViewModel"/> instances use <see cref="ElementListLayout"/>, 
    /// which will honor the collection change events for the displayed list.
    /// </remarks>
    /// <seealso cref="ElementListLayout"/>
    /// <seealso cref="TwoVerticalsLayout"/>
    /// <seealso cref="ThreeVerticalVisualsLayout"/>
    public class ListLayout : ViewModel
    {
        IEnumerable<ViewModel> elements;

        /// <summary>
        /// Initializes a new instance of <see cref="ListLayout"/>.
        /// </summary>
        /// <param name="elements">The <see cref="ViewModel"/> elements that should be displayed in a vertical list.</param>
        public ListLayout(IEnumerable<ViewModel> elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// Gets the <see cref="ViewModel"/> elements that should be displayed in a vertical list.
        /// </summary>
        /// <value>
        /// The <see cref="ViewModel"/> elements that should be displayed in a vertical list.
        /// </value>
        public IEnumerable<ViewModel> Elements
        {
            get { return this.elements; }
        }
    }
}
