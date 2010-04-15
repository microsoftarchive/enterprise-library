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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    [TestClass]
    public class when_moving_to_last_step : TestableWizardContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            foreach(var step in Steps)
            {
                step.SetIsValid(true);
            }
        }

        protected override void Act()
        {
            this.Wizard.Next();
            this.Wizard.Next();
        }

        [TestMethod]
        public void then_previous_command_executable()
        {
            Assert.IsTrue(this.Wizard.PreviousCommand.CanExecute(null));
        }

        [TestMethod]
        public void then_next_command_disabled()
        {
            Assert.IsFalse(this.Wizard.NextCommand.CanExecute(null));
        }
    }
}
