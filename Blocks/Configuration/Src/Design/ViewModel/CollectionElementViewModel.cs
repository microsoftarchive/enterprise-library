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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;
using System.Diagnostics;

namespace Console.Wpf.ViewModel
{
    public class CollectionElementViewModel : ElementViewModel
    {
        readonly ElementCollectionViewModel containingCollection;
        readonly ConfigurationCollectionAttribute configurationCollectionAttribute;

        [InjectionConstructor]
        public CollectionElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement, Enumerable.Empty<Attribute>()) //where do these come from?
        {
            this.containingCollection = containingCollection;

            MoveUp = new DelegateCommand((o) => containingCollection.MoveUp(this), (o) => !containingCollection.IsFirst(this));
            MoveDown = new DelegateCommand((o) => containingCollection.MoveDown(this), (o) => !containingCollection.IsLast(this));

            configurationCollectionAttribute = containingCollection.Attributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();
            Debug.Assert(configurationCollectionAttribute != null);
        }

        public ICommand MoveUp { get; protected set; }
        public ICommand MoveDown { get; protected set; }

        public override void Delete()
        {
            containingCollection.Delete(this);
        }

        protected override string GetLocalPathPart()
        {
            StringBuilder builder = new StringBuilder();

            foreach (PropertyInformation property in ConfigurationElement.ElementInformation.Properties)
            {
                if (property.IsKey)
                {
                    builder.AppendFormat("@{0}='{1}' &", property.Name, property.Value);
                }
            }

            builder.Remove(builder.Length - 2, 2);
            builder.Insert(0, configurationCollectionAttribute.AddItemName + "[");
            builder.Append("]");
            return builder.ToString() ;
        }
    }
}
