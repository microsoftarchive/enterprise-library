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
            element.Select();
        }

        public override bool CanExecute(object parameter)
        {
            return !collection.IsLast(element);
        }


        public override string KeyGesture
        {
            get
            {
                return new KeyGestureConverter().ConvertToString(new KeyGesture(Key.Down, ModifierKeys.Control));
            }
        }
    }
}
