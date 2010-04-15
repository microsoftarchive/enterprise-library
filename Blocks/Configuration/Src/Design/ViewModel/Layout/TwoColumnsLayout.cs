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
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Layout class used to display two <see cref="ViewModel"/> instances horizontally.
    /// </summary>
    public class TwoColumnsLayout : ViewModel
    {
        ViewModel left;
        ViewModel right;
        int columnIndex;

        /// <summary>
        /// Initializes a new instance of <see cref="TwoColumnsLayout"/>.
        /// </summary>
        /// <param name="left">The <see cref="ViewModel"/> instance that should be shown on the left.</param>
        /// <param name="right">The <see cref="ViewModel"/> instance that should be shown on the right.</param>
        /// <param name="columnIndex">The 0-based column index that should be used to display and resize the <paramref name="left"/> element. </param>
        public TwoColumnsLayout(ViewModel left, ViewModel right, int columnIndex)
        {
            this.left = left;
            this.right = right;
            this.columnIndex = columnIndex;
        }

        /// <summary>
        /// Gets the <see cref="ViewModel"/> instance that should be shown on the left.
        /// </summary>
        /// <value>
        /// The <see cref="ViewModel"/> instance that should be shown on the left.
        /// </value>
        public ViewModel Left
        {
            get { return left; }
        }

        /// <summary>
        /// Gets the <see cref="ViewModel"/> instance that should be shown on the right.
        /// </summary>
        /// <value>
        /// The <see cref="ViewModel"/> instance that should be shown on the right.
        /// </value>
        public ViewModel Right
        {
            get { return right; }
        }

        /// <summary>
        /// Gets the 0-based column index that should be used to display and resize the <see cref="Left"/> element. 
        /// </summary>
        /// <value>
        /// The 0-based column index that should be used to display and resize the <see cref="Left"/> element. 
        /// </value>
        public string ColumnName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Column{0}", columnIndex); }
        }

    }
}
