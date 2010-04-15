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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    [TestClass]
    public class when_creating_wizard : TestableWizardContext
    {
        private TestableWizard wizard;

        protected override void Act()
        {
            wizard = new TestableWizard(UIService.Object, Steps);
        }

        [TestMethod]
        public void then_starts_on_first_step()
        {
            Assert.AreEqual(Steps[0], wizard.CurrentStep);
        }

        [TestMethod]
        public void then_step_order_preserved()
        {
            CollectionAssert.AreEqual(Steps, wizard.Steps.ToArray());
        }

        [TestMethod]
        public void then_next_command_not_executable()
        {
            Assert.IsFalse(wizard.NextCommand.CanExecute(null));
        }

        [TestMethod]
        public void then_finish_command_not_executable()
        {
            Assert.IsFalse(wizard.FinishCommand.CanExecute(null));
        }
    }
}
