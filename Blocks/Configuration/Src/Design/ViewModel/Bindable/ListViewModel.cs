using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class ListViewModel : ViewModel
    {
        IEnumerable<ViewModel> elements;

        public ListViewModel(IEnumerable<ViewModel> elements)
        {
            this.elements = elements;
        }

        public IEnumerable<ViewModel> Elements
        {
            get { return this.elements; }
        }
    }

    public class ElementListViewModel : ViewModel
    {
        IEnumerable<ElementViewModel> elements;

        public ElementListViewModel(IEnumerable<ElementViewModel> elements)
        {
            this.elements = elements;
        }

        public IEnumerable<ElementViewModel> Elements
        {
            get { return this.elements; }
        }
    }
}
