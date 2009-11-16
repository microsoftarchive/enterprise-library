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
using System.Configuration;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Default Command Model Implementation for <see cref="ElementCollectionViewModel"/> instances.
    /// </summary>
    /// <remarks>
    /// This is the default add command created for any <see cref="ConfigurationElementCollection"/>.  
    /// 
    /// For homogeneous collections where only one type can ever be added, this command when invoked via <see cref="CommandModel.Execute"/>, 
    /// will add a new instance of that type to the collection.
    /// 
    /// For <see cref="PolymorphicConfigurationElementCollection{T}"/>, this command will contain a collection
    /// of the child <see cref="ConfigurationElement"/> add commands, generally one for each type that can be added.
    /// 
    /// To change the commands for a configuration element, <see cref="CommandAttribute"/>
    ///</remarks>
    public class DefaultElementCollectionAddCommand : CommandModel
    {
        readonly ElementCollectionViewModel collection;
        readonly CommandModel[] childCommands;
        readonly SectionViewModel section;

        ///<summary>
        /// Initializes an instance of DefaultElementCollectionAddCommand.  
        /// </summary>
        ///<param name="collection">The collection that will be affected by the add command.</param>
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

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public override string Title
        {
            get
            {
                return string.Format("_Add {0}", collection.Name);
            }
        }

        ///<summary>
        /// Child <see cref="CommandModel"/> commands to this command.
        ///</summary>
        public override IEnumerable<CommandModel> ChildCommands
        {
            get
            {
                return childCommands;
            }
        }

        ///<summary>
        /// The logical placement of the command.
        ///</summary>
        public override CommandPlacement Placement
        {
            get{ return CommandPlacement.ContextAdd; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
