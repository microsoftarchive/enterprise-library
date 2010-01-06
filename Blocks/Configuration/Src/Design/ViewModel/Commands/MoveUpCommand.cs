//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

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
            element.Select();
        }

        public override bool CanExecute(object parameter)
        {
            return !collection.IsFirst(element);
        }

        public override string KeyGesture
        {
            get
            {
                return new KeyGestureConverter().ConvertToString(new KeyGesture(Key.Up, ModifierKeys.Control));
            }
        }
    }
}
