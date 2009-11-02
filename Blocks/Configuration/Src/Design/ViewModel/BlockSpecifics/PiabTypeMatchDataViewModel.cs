using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class PiabTypeMatchDataViewModel: CollectionElementViewModel
    {
        public PiabTypeMatchDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
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
    }
}
