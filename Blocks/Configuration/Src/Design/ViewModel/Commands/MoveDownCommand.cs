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

using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    ///<summary>
    /// The <see cref="MoveDownCommand"/> moves an element downward in its containing collection.
    ///</summary>
    ///<remarks>
    /// This command is offered by a <see cref="CollectionElementViewModel"/> as a bindable command to invoke <see cref="CollectionElementViewModel.MoveDown"/></remarks>
    public class MoveDownCommand : CommandModel
    {
        ElementCollectionViewModel collection;
        CollectionElementViewModel element;

        ///<summary>
        /// Initializes a new instance of <see cref="MoveDownCommand"/>.
        ///</summary>
        ///<param name="collection">The collection containing the <paramref name="element"/> to move.</param>
        ///<param name="element">The element to move.</param>
        ///<param name="uiService">The user-interface service used to display messages and windows to the user.</param>
        public MoveDownCommand(ElementCollectionViewModel collection, CollectionElementViewModel element, IUIServiceWpf uiService)
            : base(uiService)
        {
            this.collection = collection;
            this.element = element;
        }

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public override string Title
        {
            get
            {
                return Resources.MoveElementDownCommandTitle;
            }
        }

        /// <summary>
        /// Moves the element downard in the collection.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        protected override void InnerExecute(object parameter)
        {
            collection.MoveDown(element);
            element.Select();
        }

        /// <summary>
        /// Determines if the command can execute.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if the element is not the last one in the collection.  
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        /// <param name="parameter">Not used</param>
        protected override bool InnerCanExecute(object parameter)
        {
            return !collection.IsLast(element);
        }


        /// <summary>
        /// Defines the key gesture used for this command (Ctrl-Down).
        /// </summary>
        public override string KeyGesture
        {
            get
            {
                return new KeyGestureConverter().ConvertToString(new KeyGesture(Key.Down, ModifierKeys.Control));
            }
        }
    }
}
