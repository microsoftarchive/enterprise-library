using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExceptionTypeDataViewModel : CollectionElementViewModel
    {
        public ExceptionTypeDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new HierarchicalViewModel(this, this.ChildElement("ExceptionHandlers").ChildElements)
                {
                    ColumnName = "Column1"
                };
        }
    }
}
