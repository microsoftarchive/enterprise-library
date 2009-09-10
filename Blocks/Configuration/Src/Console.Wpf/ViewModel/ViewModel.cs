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

namespace Console.Wpf.ViewModel
{

    //base class for everything that can be rendered in the grid.
    //actual elements, collections, sections and even headers derive from this.
    public class ViewModel
    {
        public ViewModel()
        {
            Row = -1;
            Column = -1;
            RowSpan = 1;
            Column = 1;
        }

        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }

        /// <summary>
        /// Returns true if this <see cref="ViewModel"/> should be shown.
        /// </summary>
        public virtual bool IsShown
        {
            get
            {
                return (Row != -1 && Column != -1);
            }
        }
    }
}
