using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls
{
    [TestClass]
    public class when_clearing_selection : ContextMenuContext
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
}