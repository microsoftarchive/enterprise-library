using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExceptionHandlerDataViewModel: CollectionElementViewModel
    {
        public ExceptionHandlerDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new HierarchicalViewModel(
                this, 
                Enumerable.Empty<ElementViewModel>())
                {
                    ColumnName = "Column2"
                };
        }
    }
}
