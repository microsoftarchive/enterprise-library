using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Console.Wpf.Controls
{
    public class ContextMenuButton : ButtonBase
    {

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
