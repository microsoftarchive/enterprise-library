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
    public class when_clearing_selection_for_top_level_menu : ContextMenuContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            Selector.SetIsSelected(LastMenuItem, true);
        }

        protected override void Act()
        {
            Selector.SetIsSelected(LastMenuItem, false);
        }

        [TestMethod]
        public void then_no_item_is_currently_selected()
        {
            Assert.IsNull(ContextMenu.CurrentSelection);
        }
    }

    [TestClass]
    public class when_clearing_selection_for_sub_menu : ContextMenuContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            Selector.SetIsSelected(FirstMenuItem.Items[1] as DependencyObject, true);
        }

        protected override void Act()
        {
            Selector.SetIsSelected(FirstMenuItem.Items[1] as DependencyObject, false);
        }

        [TestMethod]
        public void then_no_item_is_currently_selected()
        {
            Assert.IsNull(FirstMenuItem.CurrentSelection);
        }
    }
}
