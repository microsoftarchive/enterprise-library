using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class PiabPropertyMatchDataViewModel: CollectionElementViewModel
    {
        public PiabPropertyMatchDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        public Property IgnoreCaseProperty
        {
            get { return Property("IgnoreCase"); }
        }

        public Property MatchProperty
        {
            get { return Property("Match"); }
        }

        public Property MatchOptionProperty
        {
            get { return Property("MatchOption"); }
        }
    }
}
