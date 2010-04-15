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
    /// Layout class used to display a vertical list of <see cref="ElementViewModel"/>'s.
    /// </summary>
    public class ElementListLayout : ViewModel
    {
        IEnumerable<ElementViewModel> elements;

        /// <summary>
        /// Initializes a new instance of <see cref="ElementListLayout"/>.
        /// </summary>
        /// <param name="elements">The <see cref="ElementViewModel"/> instances that should be displayed in the list.</param>
        public ElementListLayout(IEnumerable<ElementViewModel> elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// Gets the <see cref="ElementViewModel"/> instances that should be displayed in the list.
        /// </summary>
        /// <value>
        /// The <see cref="ElementViewModel"/> instances that should be displayed in the list.
        /// </value>
        public IEnumerable<ElementViewModel> Elements
        {
            get { return this.elements; }
        }
    }
}
