using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class CategoryFilterEntryViewModel: CollectionElementViewModel
    {
        public CategoryFilterEntryViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        public Property NameProperty
        {
            get { return Property("Name"); }
        }
    }
}
