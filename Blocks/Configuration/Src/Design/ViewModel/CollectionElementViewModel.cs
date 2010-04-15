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
using System.Globalization;
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
    /// <summary>
    /// Represents a <see cref="ConfigurationElement"/> within a <see cref="ConfigurationElementCollection"/>.
    /// </summary>
    public class CollectionElementViewModel : ElementViewModel
    {
        readonly ElementCollectionViewModel containingCollection;
        readonly ConfigurationCollectionAttribute configurationCollectionAttribute;

        /// <summary>
        /// Initializes a new instance of <see cref="CollectionElementViewModel"/>.
        /// </summary>
        /// <param name="containingCollection">The collection containing this element.</param>
        /// <param name="thisElement">The <see cref="ConfigurationElement"/> modeled by the <see cref="CollectionElementViewModel"/>.</param>
        [InjectionConstructor]
        public CollectionElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : this(containingCollection, thisElement, Enumerable.Empty<Attribute>())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CollectionElementViewModel"/>.
        /// </summary>
        /// <param name="containingCollection">The collection containing this element.</param>
        /// <param name="thisElement">The <see cref="ConfigurationElement"/> modeled by the <see cref="CollectionElementViewModel"/>.</param>
        /// <param name="additionalAttributes">Additional <see cref="Attribute"/> items to apply when describing this <paramref name="thisElement"/>.</param>
        public CollectionElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement, IEnumerable<Attribute> additionalAttributes)
            :base(containingCollection, thisElement, additionalAttributes)
        {
            this.containingCollection = containingCollection;

            configurationCollectionAttribute = containingCollection.Attributes.OfType<ConfigurationCollectionAttribute>().First();
            
            var overrides = new Dictionary<Type, object>()
                             {
                                 {typeof (ElementCollectionViewModel), containingCollection},
                                 {typeof (CollectionElementViewModel), this}
                             };

            MoveUp = ContainingSection.CreateCommand<MoveUpCommand>(overrides);
            MoveDown = ContainingSection.CreateCommand<MoveDownCommand>(overrides);
        }



        /// <summary>
        /// Gets or sets the <see cref="CommandModel"/> to move this element logically up in the collection.
        /// </summary>
        public CommandModel MoveUp { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="CommandModel"/> to move this element logically down in the collection.
        /// </summary>
        public CommandModel MoveDown { get; protected set; }


        /// <summary>
        /// Creates or collections all the commands related to this <see cref="ElementViewModel"/>.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            return base.GetAllCommands().Union(
                new CommandModel[] { MoveUp, MoveDown });
        }

        ///<summary>
        /// Deletes this element.
        ///</summary>
        public override void Delete()
        {
            containingCollection.Delete(this);
            Dispose();
        }

        /// <summary>
        /// Gets a value indicating that this Element's <see cref="ElementViewModel.Path"/> is reliable 
        /// </summary>
        public override bool IsElementPathReliableXPath
        {
            get
            {
                if (ParentElement != null && !ParentElement.IsElementPathReliableXPath) return false;

                if (ConfigurationElement == null) return false;

                if (InheritedFromParentConfiguration) return false;

                return ConfigurationElementHasKeyProperties(ConfigurationElement.ElementInformation);
            }
        }


        /// <summary>
        /// Gets a string that can be appended to the parent's <see cref="ElementViewModel.Path"/> to compose a <see cref="ElementViewModel.Path"/> used to uniquely identify this <see cref="ElementViewModel"/>. <br/>
        /// </summary>
        protected override string GetLocalPathPart()
        {
            if (!ConfigurationElementHasKeyProperties(ConfigurationElement.ElementInformation))
            {
                return string.Format(CultureInfo.InvariantCulture, "[designtimeid={0}", ElementId);
            }

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
            return builder.ToString();
        }

        private static bool ConfigurationElementHasKeyProperties(ElementInformation elementInformation)
        {
            return elementInformation.Properties.Cast<PropertyInformation>().Any(x => x.IsKey);
        }
    }
}
