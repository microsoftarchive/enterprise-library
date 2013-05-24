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
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Represents a bindable command that can be executed from within the designer.<br/>
    /// </summary>
    [DebuggerDisplay("Title: {Title}  Placement: {Placement}")]
    public abstract class CommandModel : ICommand, INotifyPropertyChanged, IDisposable
    {
        private readonly IUIServiceWpf uiService;
        private List<WeakReference> _canExecuteChangedHandlers;

        /// <summary>
        /// Initializes an instance of CommandModel from <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="commandAttribute"></param>
        /// <param name="uiService"></param>
        protected CommandModel(CommandAttribute commandAttribute, IUIServiceWpf uiService)
            : this(uiService)
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
        protected CommandModel(IUIServiceWpf uiService)
        {
            this.uiService = uiService;
            this.Browsable = true;
        }


        /// <summary>
        /// Service for displaying messages and dialogs to the user.
        /// </summary>
        protected IUIServiceWpf UIService
        {
            get { return this.uiService; }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.Design.IUIService.ShowError(System.Exception,System.String)", Justification = "Low-level error message")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool CanExecute(object parameter)
        {
            try
            {
                return InnerCanExecute(parameter);
            }
            catch (Exception ex)
            {
                uiService.ShowError(ex, string.Format(CultureInfo.CurrentCulture, "An error occurred while determining if the command {0} can execute:", Title));
            }

            return false;
        }

        /// <summary>
        /// When implemented by a child, determines if the command can execute.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected virtual bool InnerCanExecute(object parameter)
        {
            return false;
        }

        ///<summary>
        /// Invokes the <see cref="CanExecuteChanged"/> event.
        ///</summary>
        public void OnCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add{ WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);}
            remove{ WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);}
        }


        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Execute(object parameter)
        {
            try
            {
                InnerExecute(parameter);
            }
            catch (Exception ex)
            {
                uiService.ShowError(ex, string.Format(CultureInfo.CurrentCulture, Resources.ErrorExecutingCommand, Title, ex.Message));
            }
        }

        /// <summary>
        /// When implemented by a child, executes the command.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        protected virtual void InnerExecute(object parameter)
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

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="Dispose(bool)"/> was invoked through an explicit call to <see cref="Dispose()"/> instead of a finalizer call.</param>
        /// <filterpriority>2</filterpriority>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}
