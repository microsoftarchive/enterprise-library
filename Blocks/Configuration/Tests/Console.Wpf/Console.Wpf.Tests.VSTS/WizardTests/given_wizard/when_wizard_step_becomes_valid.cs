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
    public class when_wizard_step_becomes_valid : TestableWizardContext
    {
        protected override void Act()
        {
            Steps[0].SetIsValid(true);
        }

        [TestMethod]
        public void then_next_command_executable()
        {
            Assert.IsTrue(Wizard.NextCommand.CanExecute(null));
        }
    }
}
