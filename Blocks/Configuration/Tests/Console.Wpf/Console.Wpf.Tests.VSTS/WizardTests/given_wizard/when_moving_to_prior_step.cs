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
    public class when_moving_to_prior_step : TestableWizardContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            Steps[0].SetIsValid(true);
            this.Wizard.Next();
        }

        protected override void Act()
        {
            this.Wizard.Previous();
        }

        [TestMethod]
        public void then_sets_current_to_prior_step()
        {
            Assert.AreSame(Steps[0], this.Wizard.CurrentStep);
        }
    }
}
