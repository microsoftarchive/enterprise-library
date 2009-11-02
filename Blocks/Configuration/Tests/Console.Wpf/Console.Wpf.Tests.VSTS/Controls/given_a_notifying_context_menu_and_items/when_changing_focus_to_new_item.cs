using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls
{
    [TestClass]
    public class when_changing_focus_to_new_item : ContextMenuContext
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
}