using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    public class MoveUpCommand : CommandModel
    {
        ElementCollectionViewModel collection;
        CollectionElementViewModel element;
        
        public MoveUpCommand(ElementCollectionViewModel collection, CollectionElementViewModel element)
        {
            this.collection = collection;
            this.element = element;
        }

        public override string Title
        {
            get
            {
                return "Move Up";
            }
        }

        public override void Execute(object parameter)
        {
            collection.MoveUp(element);
        }

        public override bool CanExecute(object parameter)
        {
            return !collection.IsFirst(element);
        }
    }
}
