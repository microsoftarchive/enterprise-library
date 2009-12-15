using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ValidationRulesetDataViewModel: CollectionElementViewModel
    {
        public ValidationRulesetDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new TwoColumnsViewModel(
                this,
                new ListViewModel(new ViewModel[]{
                    new HierarchicalViewModel(ChildElement("Validators"), ChildElement("Validators").ChildElements),
                    new ElementListViewModel(ChildElement("Properties").ChildElements),
                    new ElementListViewModel(ChildElement("Fields").ChildElements),
                    new ElementListViewModel(ChildElement("Methods").ChildElements)
                }))
                {
                    ColumnName = "Column1"
                };
        }
    }
}
