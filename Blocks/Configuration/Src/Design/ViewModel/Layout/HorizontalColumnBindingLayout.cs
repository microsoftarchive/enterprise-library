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
    /// Layout class used to display an <see cref="ElementViewModel"/> instance in a horizontally aligned grid column.
    /// </summary>
    /// <remarks>
    /// Most configuration section models show a top-level grid with column names.<br/>
    /// <see cref="ElementViewModel"/> instances that should be shown as part of this top-level grid should be contained in this <see cref="HorizontalColumnBindingLayout"/> to 
    /// participate in resizing behavior.
    /// </remarks>
    public class HorizontalColumnBindingLayout : TwoColumnsLayout
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HorizontalColumnBindingLayout"/>.
        /// </summary>
        /// <param name="element">The <see cref="ElementViewModel"/> isntance that should be displayed.</param>
        /// <param name="columnIndex">The 0-based column index that should be used to display and resize the <paramref name="element"/>. </param>
        public HorizontalColumnBindingLayout(ViewModel element, int columnIndex)
            : base(element, null, columnIndex)
        {

        }
    }
}
