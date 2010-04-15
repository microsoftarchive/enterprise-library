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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    [TestClass]
    public class when_calling_finish : TestableWizardContext
    {
        private bool closeInvoked;

        protected override void Arrange()
        {
            base.Arrange();
            Array.ForEach(Steps, s => s.SetIsValid(true));
            this.Wizard.OnCloseAction = () => closeInvoked = true;
        }

        protected override void Act()
        {
            this.Wizard.DoFinish();
        }

        [TestMethod]
        public void then_all_steps_executed()
        {
            Assert.IsTrue(Steps.All(s => s.ExecuteCalled));
        }

        [TestMethod]
        public void then_close_delegate_invoked()
        {
            Assert.IsTrue(closeInvoked);
        }
    }
}
