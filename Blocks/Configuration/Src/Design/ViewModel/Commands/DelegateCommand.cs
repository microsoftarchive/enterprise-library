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
using System.Windows.Input;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    internal class DelegateCommand : ICommand
    {
        private Action<object> command;
        private readonly Func<object, bool> canExecute;
        private List<WeakReference> _canExecuteChangedHandlers;

        public DelegateCommand(Action<object> command)
            : this(command, (o) => true)
        {
        }

        public DelegateCommand(Action<object> command, Func<object, bool> CanExecute)
        {
            this.command = command;
            canExecute = CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e")]
        protected void OnCanExecuteChanged(EventArgs e)
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                command(parameter);
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }
    }
}
