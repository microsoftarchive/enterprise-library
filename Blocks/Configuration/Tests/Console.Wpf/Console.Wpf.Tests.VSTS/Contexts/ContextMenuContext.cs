using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Console.Wpf.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Console.Wpf.Tests.VSTS.Controls
{
    
    public abstract class ContextMenuContext : ArrangeActAssert
    {
        private SelectionNotifyingContextMenu contextMenu;

        protected override void Arrange()
        {
            base.Arrange();

            contextMenu = new SelectionNotifyingContextMenu();
            var menuItemOne = new SelectionNotifyingMenuItem();
            var menuItemTwo = new SelectionNotifyingMenuItem();
            contextMenu.Items.Add(menuItemOne);
            contextMenu.Items.Add(menuItemTwo);
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
