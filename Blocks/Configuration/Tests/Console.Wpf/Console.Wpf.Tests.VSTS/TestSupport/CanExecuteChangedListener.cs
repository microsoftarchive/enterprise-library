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
using System.Windows.Input;

namespace Console.Wpf.Tests.VSTS.TestSupport
{
    public class CanExecuteChangedListener
    {
        Dictionary<ICommand, bool> commandList = new Dictionary<ICommand, bool>();
        private EventHandler canExecuteChangedHandler;

        public void Add(ICommand command)
        {
            commandList.Add(command, false);

            // need to hold onto our own reference in case subscriber is using WeakEventPattern to support WPF expectations
            canExecuteChangedHandler = CanExecuteChangedHandler;  
            command.CanExecuteChanged += canExecuteChangedHandler;
        }

        private void CanExecuteChangedHandler(object sender, EventArgs e)
        {
            var command = sender as ICommand;
            if (command != null)
            {
                commandList[command] = true;
            }
        }

        public bool CanExecuteChangedFired(ICommand command)
        {
            return commandList[command];
        }

        public void Reset()
        {
            foreach (var key in commandList.Keys)
            {
                commandList[key] = false;
            }
        }
    }
}
