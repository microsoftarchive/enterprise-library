using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class OraclePackageDataViewModel : CollectionElementViewModel
    {
        public OraclePackageDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        public Property NameProperty
        {
            get { return Property("Name"); }
        }


        public Property PrefixProperty
        {
            get { return Property("Prefix"); }
        }
    }
}