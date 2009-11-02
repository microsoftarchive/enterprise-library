using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Console.Wpf.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.given_context_menu_button
{
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

        private class TestableContextMenuButton : ContextMenuButton
        {
            public void DoClick()
            {
                base.OnClick();
            }
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
}
