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
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Layout class that can be used to visualize an <see cref="ElementViewModel"/> and its children in a hierarchical layout.
    /// </summary>
    public class HierarchicalLayout  : TwoColumnsLayout
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HierarchicalLayout"/>.
        /// </summary>
        /// <param name="current">The <see cref="ElementViewModel"/> instance that should be displayed on the left side.</param>
        /// <param name="children">The set of <see cref="ElementViewModel"/> instances that should be displayed as <paramref name="current"/>'s children.</param>
        /// <param name="columnIndex">The 0-based column index that should be used to display and resize the <paramref name="current"/> element. </param>
        public HierarchicalLayout(ElementViewModel current, IEnumerable<ElementViewModel> children, int columnIndex)
            : base(current, new ElementListLayout(children), columnIndex)
        {
        }
    }
}
