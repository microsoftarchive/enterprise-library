using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ValidatorDataViewModel: CollectionElementViewModel
    {
        public ValidatorDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new HierarchicalViewModel(
                this,
                ChildElement("Validators") != null ?
                    ChildElement("Validators").ChildElements :
                    Enumerable.Empty<ElementViewModel>())
                {
                    ColumnName = string.Format("Column{0}", this.AncesterElements().Where(x=>typeof(ValidatorData).IsAssignableFrom( x.ConfigurationType)).Count() + 3)
                };
        }
    }
}
