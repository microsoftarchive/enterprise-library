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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    [TestClass]
    public class when_wizard_steps_are_complete : WizardCompleteContext
    {
        protected override void Act()
        {
            while (wizard.CurrentStep != wizard.Steps.Last())
            {
                wizard.Next();
            }
        }

        [TestMethod]
        public void then_finish_command_can_be_executed()
        {
            Assert.IsTrue(wizard.FinishCommand.CanExecute(null));
        }
    }
}
