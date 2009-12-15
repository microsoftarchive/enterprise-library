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

using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace Console.Wpf.Tests.VSTS.Controls
{
    [TestClass]
    public class when_setting_focus_to_a_menu_item : ContextMenuContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            Assert.IsNull(ContextMenu.CurrentSelection);
        }

        protected override void Act()
        {
            Selector.SetIsSelected(LastMenuItem, true);
        }

        [TestMethod]
        public void then_current_selection_matches_selected_item()
        {
            Assert.AreSame(ContextMenu.CurrentSelection, LastMenuItem);
        }
    }

    [TestClass]
    public class when_setting_focus_to_a_child_menu_item : ContextMenuContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            Assert.IsNull(FirstMenuItem.CurrentSelection);
        }

        protected override void Act()
        {
            Selector.SetIsSelected(FirstMenuItem.Items[0] as DependencyObject, true);
        }

        [TestMethod]
        public void then_current_selection_matches_selected_item()
        {
            Assert.AreSame(FirstMenuItem.CurrentSelection, FirstMenuItem.Items[0]);
        }

        [TestMethod]
        public void then_root_menu_current_not_updated()
        {
            Assert.IsNull(ContextMenu.CurrentSelection);
        }
    }
}
