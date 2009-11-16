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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    /// <summary>
    /// Sites and opens the <see cref="ContextMenu"/> on a button when it is clicked.  This
    /// enables showing the context menu on left-click gestures.
    /// </summary>
    public class ContextMenuButton : ButtonBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click"/> routed event. 
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();
            var menu = this.ContextMenu;
            if (menu != null)
            {
                menu.PlacementTarget = this;
                menu.IsOpen = true;
            }
        }
    }
}
