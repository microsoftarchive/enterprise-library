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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.given_context_menu_button
{
    class TestableContextMenuButton : ContextMenuButton
    {
        public void DoClick()
        {
            base.OnClick();
        }
    }

    public abstract class TestableContextMenuContext : ArrangeActAssert
    {
        private TestableContextMenuButton menuButton;
        protected ContextMenuButton MenuButton
        {
            get { return menuButton; }
        }

        protected override void Arrange()
        {
            base.Arrange();
            menuButton = new TestableContextMenuButton() { ContextMenu = ArrangeContextMenu()  };
        }

        public virtual ContextMenu ArrangeContextMenu()
        {
            return null;
        }

        public void InvokeClick()
        {
            menuButton.DoClick();
        }
    }

    [TestClass]
    public class when_clicking_the_button_with_menu : TestableContextMenuContext
    {
        public override ContextMenu ArrangeContextMenu()
        {
            return new ContextMenu();
        }

        protected override void Act()
        {
            InvokeClick();
        }

        [TestMethod]
        public void then_context_menu_is_opened()
        {
            Assert.IsTrue(MenuButton.ContextMenu.IsOpen);
        }

        [TestMethod]
        public void then_placement_target_is_button()
        {
            Assert.AreSame(MenuButton, MenuButton.ContextMenu.PlacementTarget);
        }
    }

    [TestClass]
    public class when_clicking_button_with_no_menu : TestableContextMenuContext
    {
        [TestMethod]
        public void then_invoke_does_not_throw()
        {
            InvokeClick();
        }

    }

    [TestClass]
    public class when_clicking_button_and_targeting_element : ArrangeActAssert
    {
        private TestableContextMenuButton menuButton;
        private FrameworkElement targetElement;

        protected override void Arrange()
        {
            base.Arrange();

            menuButton = new TestableContextMenuButton();
            targetElement = new FrameworkElement();
            targetElement.ContextMenu = new ContextMenu();

            menuButton.TargetElement = targetElement;
        }

        protected override void Act()
        {
            menuButton.DoClick();
        }

        [TestMethod]
        public void then_target_element_context_menu_opens()
        {
            Assert.IsTrue(targetElement.ContextMenu.IsOpen);
        }
    }
}
