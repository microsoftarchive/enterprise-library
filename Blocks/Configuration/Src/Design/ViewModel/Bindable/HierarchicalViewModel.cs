using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class HierarchicalViewModel  : TwoColumnsViewModel
    {
        public HierarchicalViewModel(ElementViewModel current, IEnumerable<ElementViewModel> children)
            :base(current, new ElementListViewModel(children))
        {
        }
    }
}
