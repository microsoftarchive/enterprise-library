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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Layout class used to display two <see cref="ViewModel"/> instances vertically.
    /// </summary>
    /// <seealso cref="ThreeVerticalVisualsLayout"/>
    /// <seealso cref="ListLayout"/>
    /// <seealso cref="ElementListLayout"/>
    public class TwoVerticalsLayout : ViewModel
    {
        ViewModel first, second;

        /// <summary>
        /// Initializes a new instance of <see cref="TwoVerticalsLayout"/>.
        /// </summary>
        /// <param name="first">The first <see cref="ViewModel"/> instance.</param>
        /// <param name="second">The second <see cref="ViewModel"/> instance.</param>
        public TwoVerticalsLayout(ViewModel first, ViewModel second)
        {
            this.first = first;
            this.second = second;
        }

        /// <summary>
        /// Gets the first <see cref="ViewModel"/> instance.
        /// </summary>
        /// <value>
        /// The first <see cref="ViewModel"/> instance.
        /// </value>
        public ViewModel First
        {
            get { return first; }
        }

        /// <summary>
        /// Gets the second <see cref="ViewModel"/> instance.
        /// </summary>
        /// <value>
        /// The second <see cref="ViewModel"/> instance.
        /// </value>
        public ViewModel Second
        {
            get { return second; }
        }
    }
}
