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

using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls
{
    [TestClass]
    public class when_changing_focus_to_new_top_level_item : ContextMenuContext
    {
        protected override void Arrange()
        {
            base.Arrange();   
            Selector.SetIsSelected(LastMenuItem, true);
        }

        protected override void Act()
        {
            Selector.SetIsSelected(FirstMenuItem, true);
        }

        [TestMethod]
        public void then_current_selection_matches_new_item()
        {
            Assert.AreSame(ContextMenu.CurrentSelection, FirstMenuItem);
        }
    }

    [TestClass]
    public class when_changing_focus_to_submenu_item : ContextMenuContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            Selector.SetIsSelected(FirstMenuItem.Items[0] as DependencyObject, true);
        }

        protected override void Act()
        {
            Selector.SetIsSelected(FirstMenuItem.Items[1] as DependencyObject, true);
        }

        [TestMethod]
        public void then_menu_item_selection_updated()
        {
            Assert.AreSame(FirstMenuItem.CurrentSelection, FirstMenuItem.Items[1]);
        }
    }
}
