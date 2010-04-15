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
    public class when_all_steps_become_valid : TestableWizardContext
    {
        private CanExecuteChangedListener commandListener;

        protected override void Arrange()
        {
            base.Arrange();

            Steps[1].SetIsValid(true);
            Steps[2].SetIsValid(true);

            commandListener = new CanExecuteChangedListener();
            commandListener.Add(this.Wizard.FinishCommand);
        }

        protected override void Act()
        {
            Steps[0].SetIsValid(true);
        }

        [TestMethod]
        public void then_finish_command_canexecutechanged_fires()
        {
            Assert.IsTrue(commandListener.CanExecuteChangedFired(this.Wizard.FinishCommand));
        }
    }
}
