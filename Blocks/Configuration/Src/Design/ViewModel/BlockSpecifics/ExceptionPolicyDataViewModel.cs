using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExceptionPolicyDataViewModel : CollectionElementViewModel
    {
        public ExceptionPolicyDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new HierarchicalViewModel(
                this, 
                this.ChildElement("ExceptionTypes").ChildElements)
                {
                    ColumnName = "Column0"
                };
        }
    }
}
