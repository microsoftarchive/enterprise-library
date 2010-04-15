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
    public class when_moving_to_next_step_from_invalid_step : TestableWizardContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            Steps[0].SetIsValid(false);
        }

        protected override void Act()
        {
            this.Wizard.Next();
        }

        [TestMethod]
        public void then_next_current_does_not_change()
        {
            Assert.AreSame(Steps[0], this.Wizard.CurrentStep);
        }
    }
}
