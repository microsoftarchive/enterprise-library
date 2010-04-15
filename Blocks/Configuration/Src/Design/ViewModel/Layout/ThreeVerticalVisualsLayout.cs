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
    /// Layout class used to display three <see cref="ViewModel"/> instances vertically.
    /// </summary>
    public class ThreeVerticalVisualsLayout : TwoVerticalsLayout
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ThreeVerticalVisualsLayout"/>.
        /// </summary>
        /// <param name="first">The first <see cref="ViewModel"/> instance.</param>
        /// <param name="second">The second <see cref="ViewModel"/> instance.</param>
        /// <param name="third">The third <see cref="ViewModel"/> instance.</param>
        public ThreeVerticalVisualsLayout(ViewModel first, ViewModel second, ViewModel third)
            :base(first, new TwoVerticalsLayout(second, third))
        {

        }
    }
}
