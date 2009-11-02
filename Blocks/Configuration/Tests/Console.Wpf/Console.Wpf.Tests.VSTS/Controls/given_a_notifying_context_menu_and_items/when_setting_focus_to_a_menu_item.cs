using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
}