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
using System.Windows.Input;

namespace Console.Wpf.ViewModel
{
    /* move to nested protected internal of CommandViewModel later? */
    internal class DelegateCommand : ICommand
    {
        private Action<object> command;
        private readonly Func<object, bool> canExecute;

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

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler changed = CanExecuteChanged;
            if (changed != null) changed(this, e);
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
