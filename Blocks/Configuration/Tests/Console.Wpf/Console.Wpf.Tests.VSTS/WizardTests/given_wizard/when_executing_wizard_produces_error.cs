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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    [TestClass]
    public class when_executing_wizard_produces_error : TestableWizardContext
    {

        protected override Mock<IUIServiceWpf> CreateUIServiceMock()
        {
            var mock = base.CreateUIServiceMock();
            mock.Setup(
                x =>
                x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<System.Windows.MessageBoxButton>()))
                .Verifiable();

            return mock;
        }

        protected override void Arrange()
        {
            base.Arrange();
            Array.ForEach(Steps, s => s.SetIsValid(true));

            this.Wizard.ExecuteAction = () => { throw new AmbiguousMatchException("TestException"); };
        }

        protected override void Act()
        {
            this.Wizard.DoFinish();
        }

        [TestMethod]
        public void then_shows_error_in_dialog()
        {
            UIService.VerifyAll();
        }
    }
}
