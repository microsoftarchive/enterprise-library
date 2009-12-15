using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    public class MoveDownCommand : CommandModel
    {
        ElementCollectionViewModel collection;
        CollectionElementViewModel element;

        public MoveDownCommand(ElementCollectionViewModel collection, CollectionElementViewModel element)
        {
            this.collection = collection;
            this.element = element;
        }

        public override string Title
        {
            get
            {
                return "Move Down";
            }
        }

        public override void Execute(object parameter)
        {
            collection.MoveDown(element);
        }

        public override bool CanExecute(object parameter)
        {
            return !collection.IsLast(element);
        }
    }
}
