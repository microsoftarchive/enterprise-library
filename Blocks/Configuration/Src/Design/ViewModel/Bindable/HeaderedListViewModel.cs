using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class HeaderedListViewModel : TwoVerticalVisualsViewModel
    {
        public HeaderedListViewModel(ElementViewModel containgElement)
            : base(new HeaderViewModel(containgElement.Name, containgElement.Commands), new ElementListViewModel(containgElement.ChildElements))
        {
        }
        public HeaderedListViewModel(ElementViewModel containgElement, IEnumerable<CommandModel> commands)
            : base(new HeaderViewModel(containgElement.Name, commands), new ElementListViewModel(containgElement.ChildElements))
        {
        }
    }
}
