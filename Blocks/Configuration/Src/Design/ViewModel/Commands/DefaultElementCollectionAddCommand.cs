using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel
{
    public class DefaultElementCollectionAddCommand : CommandModel
    {
        readonly ElementCollectionViewModel collection;
        readonly CommandModel[] childCommands;
        readonly SectionViewModel section;

        public DefaultElementCollectionAddCommand(ElementCollectionViewModel collection)
        {
            this.collection = collection;
            this.section = collection.ContainingSection;

            if (this.collection.IsPolymorphicCollection)
            {
                childCommands = this.collection.PolymorphicCollectionElementTypes
                                                .SelectMany(x => section.CreateCollectionElementAddCommand(x, collection))
                                                .ToArray();
            }
            else
            {
                childCommands =  section.CreateCollectionElementAddCommand(collection.CollectionElementType, collection).ToArray();
            }


        }

        public override IEnumerable<CommandModel> ChildCommands
        {
            get
            {
                return childCommands;
            }
        }

        public override CommandPlacement Placement
        {
            get{ return CommandPlacement.ContextAdd; }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
