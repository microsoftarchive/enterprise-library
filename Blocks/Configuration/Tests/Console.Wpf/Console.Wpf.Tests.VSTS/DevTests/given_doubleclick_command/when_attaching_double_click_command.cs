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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows;

namespace Console.Wpf.Tests.VSTS.DevTests.given_doubleclick_command
{
    
    [TestClass]
    public class when_attaching_double_click_command : ArrangeActAssert
    {
        DoubleClickControl subject;
        Command command;

        protected override void Arrange()
        {
            subject = new DoubleClickControl();
            command = new Command();

            DoubleClickCommand.SetCommand(subject, command);
            DoubleClickCommand.SetCommandParameter(subject, 1);
        }

        protected override void Act()
        {
            subject.DoDoubleClick();
        }

        [TestMethod]
        public void then_command_was_executed()
        {
            Assert.IsNotNull(command.ParameterForExecute);
            Assert.AreEqual(1, command.ParameterForExecute);
        }

        private class DoubleClickControl : Control
        {
            public void DoDoubleClick()
            {
                base.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Control.MouseDoubleClickEvent
                    });
            }
        }

        private class Command : ICommand
        {
            public object ParameterForExecute;

            public bool CanExecute(object parameter)
            {
                return true;    
            }

            public event EventHandler CanExecuteChanged;

            private void InvokeCanExecuteChanged(EventArgs e)
            {
                EventHandler changed = CanExecuteChanged;
                if (changed != null) changed(this, e);
            }

            public void Execute(object parameter)
            {
                ParameterForExecute = parameter;
            }
        }
    }
}
