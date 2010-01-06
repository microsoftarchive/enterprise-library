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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class CollectionElementViewModel : ElementViewModel
    {
        readonly ElementCollectionViewModel containingCollection;
        readonly ConfigurationCollectionAttribute configurationCollectionAttribute;


        public CollectionElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement, Enumerable.Empty<Attribute>()) //where do these come from?
        {
            this.containingCollection = containingCollection;

            MoveUp = new MoveUpCommand(containingCollection, this);
            MoveDown = new MoveDownCommand(containingCollection, this);

            configurationCollectionAttribute = containingCollection.Attributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();
            Debug.Assert(configurationCollectionAttribute != null);
        }

        public CommandModel MoveUp { get; protected set; }
        public CommandModel MoveDown { get; protected set; }


        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            return base.GetAllCommands().Union( 
                new CommandModel[]{ MoveUp, MoveDown });
        }

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
