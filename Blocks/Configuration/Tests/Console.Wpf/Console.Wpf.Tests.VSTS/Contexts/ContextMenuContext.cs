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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Console.Wpf.Tests.VSTS.Controls
{
    
    public abstract class ContextMenuContext : ArrangeActAssert
    {
        private SelectionNotifyingContextMenu contextMenu;

        protected override void Arrange()
        {
            base.Arrange();

            contextMenu = new SelectionNotifyingContextMenu() { Name = "ContextMenu" };
            var firstMenu = new SelectionNotifyingMenuItem() { Name = "FirstMenu" };
            firstMenu.Items.Add(new SelectionNotifyingMenuItem() { Name = "FirstMenu_FirstSubMenu" });
            firstMenu.Items.Add(new SelectionNotifyingMenuItem() { Name = "FirstMenu_SecondSubMenu" });
            contextMenu.Items.Add(firstMenu);
            contextMenu.Items.Add(new SelectionNotifyingMenuItem() { Name = "SecondMenu" });
        }    

        protected SelectionNotifyingContextMenu ContextMenu
        {
            get { return contextMenu;}
        }

        protected SelectionNotifyingMenuItem LastMenuItem
        {
            get { return contextMenu.Items[1] as SelectionNotifyingMenuItem;}
        }

        protected SelectionNotifyingMenuItem FirstMenuItem
        {
            get { return contextMenu.Items[0] as SelectionNotifyingMenuItem; }
        }
    }
}
