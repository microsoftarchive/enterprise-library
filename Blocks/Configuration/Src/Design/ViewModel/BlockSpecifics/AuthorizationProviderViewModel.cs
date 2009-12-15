using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class AuthorizationProviderDataViewModel: CollectionElementViewModel
    {
        public AuthorizationProviderDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new TwoColumnsViewModel(this, null)
                {
                    ColumnName = "Column0"
                };
        }
    }
}
