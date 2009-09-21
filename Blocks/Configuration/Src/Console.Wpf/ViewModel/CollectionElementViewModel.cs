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
using System.Configuration;
using System.Windows.Input;

namespace Console.Wpf.ViewModel
{
    public class CollectionElementViewModel : ElementViewModel
    {
        private readonly ElementCollectionViewModel containingCollection;

        public CollectionElementViewModel(IServiceProvider serviceProvider, ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(serviceProvider, containingCollection, thisElement, Enumerable.Empty<Attribute>()) //where do these come from?
        {
            this.containingCollection = containingCollection;

            MoveUp = new DelegateCommand((o) => containingCollection.MoveUp(this), (o) => !containingCollection.IsFirst(this));
            MoveDown = new DelegateCommand((o) => containingCollection.MoveDown(this), (o) => !containingCollection.IsLast(this));
            DeleteCommand = new DelegateCommand((o) => this.Delete());
        }

        public ICommand MoveUp { get; protected set; }
        public ICommand MoveDown { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }

        public virtual void Delete()
        {
            containingCollection.Delete(this);
        }

    }
}
