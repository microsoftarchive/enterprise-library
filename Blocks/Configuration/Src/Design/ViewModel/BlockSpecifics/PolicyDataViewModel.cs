using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class PolicyDataViewModel : CollectionElementViewModel
    {
        public PolicyDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {

        }

        protected override object CreateBindable()
        {
            return new HorizontalListViewModel(
                        new ElementListViewModel(this.ChildElement("MatchingRules").ChildElements),
                        new TwoColumnsViewModel(this, null) { ColumnName = "Column1" },
                        new ElementListViewModel(this.ChildElement("Handlers").ChildElements)
                        )
                        {
                            Root = false
                        };

            //return new StackedHierarchiesViewModel(this, 
            //                this.ChildElement("MatchingRules").ChildElements,
            //                this.ChildElement("Handlers").ChildElements);
        }
    }
}
