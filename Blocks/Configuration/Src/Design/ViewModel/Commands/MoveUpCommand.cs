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
    /// The <see cref="MoveUpCommand"/> moves an element upward in its containing collection.
    ///</summary>
    ///<remarks>
    /// This command is offered by a <see cref="CollectionElementViewModel"/> as a bindable command to invoke <see cref="CollectionElementViewModel.MoveDown"/></remarks>
    public class MoveUpCommand : CommandModel
    {
        ElementCollectionViewModel collection;
        CollectionElementViewModel element;

        ///<summary>
        /// Initializes a new instance of <see cref="MoveUpCommand"/>.
        ///</summary>
        ///<param name="collection">The collection containing the <paramref name="element"/> to move.</param>
        ///<param name="element">The element to move.</param>
        ///<param name="uiService">The user-interface service used to display messages and windows to the user.</param>
        public MoveUpCommand(ElementCollectionViewModel collection, CollectionElementViewModel element, IUIServiceWpf uiService)
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
                return Resources.MoveElementUpCommandTitle;
            }
        }

        /// <summary>
        /// Moves the element upwards in the collection.
        /// </summary>
        /// <param name="parameter">
        /// Not used
        /// </param>
        protected override void InnerExecute(object parameter)
        {
            collection.MoveUp(element);
            element.Select();
        }

        /// <summary>
        /// Determines if the element can be moved up.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if this element is not the first one in the collection.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        /// <param name="parameter">Not used</param>
        protected override bool InnerCanExecute(object parameter)
        {
            return !collection.IsFirst(element);
        }

        /// <summary>
        /// Defines the key gesture used for this command (Ctrl-Up).
        /// </summary>
        public override string KeyGesture
        {
            get
            {
                return new KeyGestureConverter().ConvertToString(new KeyGesture(Key.Up, ModifierKeys.Control));
            }
        }
    }
}
