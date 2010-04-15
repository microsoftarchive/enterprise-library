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

using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    [TestClass]
    public class when_moving_to_next_step_from_valid_step : TestableWizardContext
    {
        private PropertyChangedListener changeListener;
        private CanExecuteChangedListener commandListener;

        protected override void Arrange()
        {
            base.Arrange();

            Steps[0].SetIsValid(true);
            changeListener = new PropertyChangedListener(this.Wizard);
            commandListener = new CanExecuteChangedListener();
            commandListener.Add(this.Wizard.NextCommand);
            commandListener.Add(this.Wizard.PreviousCommand);
        }

        protected override void Act()
        {
            this.Wizard.Next();
        }

        [TestMethod]
        public void then_current_step_moves_to_next_step()
        {
            Assert.AreSame(Steps[1], this.Wizard.CurrentStep);
        }

        [TestMethod]
        public void then_previous_command_enabled()
        {
            Assert.IsTrue(this.Wizard.PreviousCommand.CanExecute(null));
        }

        [TestMethod]
        public void then_previous_command_canexecutechanged_fired()
        {
            Assert.IsTrue(commandListener.CanExecuteChangedFired(this.Wizard.PreviousCommand));
        }

        [TestMethod]
        public void then_next_command_canexecutechanged_fired()
        {
            Assert.IsTrue(commandListener.CanExecuteChangedFired(this.Wizard.NextCommand));
        }

        [TestMethod]
        public void then_current_propety_change_notify_fires()
        {
            Assert.IsTrue(changeListener.ChangedProperties.Contains("CurrentStep"));
        }
    }
}
