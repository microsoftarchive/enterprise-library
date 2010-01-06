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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Represents a bindable command that can be executed from within the designer.<br/>
    /// </summary>
    [DebuggerDisplay("Title: {Title}  Placement: {Placement}")]
    public abstract class CommandModel : ICommand, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes an instance of CommandModel from <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="commandAttribute"></param>
        protected CommandModel(CommandAttribute commandAttribute)
            : this()
        {
            Title = commandAttribute.Title;
            HelpText = string.Empty;
            Placement = commandAttribute.CommandPlacement;
            ChildCommands = Enumerable.Empty<CommandModel>();
            KeyGesture = commandAttribute.KeyGesture;
        }

        /// <summary>
        /// Initializes an instance of CommandModel.
        /// </summary>
        protected CommandModel()
        {
            Browsable = true;
        }

        ///<summary>
        /// The logical placement of the command.
        ///</summary>
        public virtual CommandPlacement Placement
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Browsable
        { get; set; }

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public virtual string Title
        {
            get;
            private set;
        }

        //todo: this is not in use, delete?
        ///<summary>
        /// The icon to display for this command.
        ///</summary>
        public virtual Image Icon
        {
            get;
            private set;
        }

        ///<summary>
        /// The command's related help text.
        ///</summary>
        public virtual string HelpText
        {
            get; 
            private set;
        }

        ///<summary>
        /// Child <see cref="CommandModel"/> commands to this command.
        ///</summary>
        public virtual IEnumerable<CommandModel> ChildCommands
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines the key gesture used for this command.
        /// </summary>
        public virtual string KeyGesture { get; private set; }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        public virtual bool CanExecute(object parameter)
        {
            return false;
        }

        ///<summary>
        /// Invokes the <see cref="CanExecuteChanged"/> event.
        ///</summary>
        public virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        public virtual void Execute(object parameter)
        {
        }

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event for this class
        /// </summary>
        /// <param name="propertyName">The name of the property changing</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
