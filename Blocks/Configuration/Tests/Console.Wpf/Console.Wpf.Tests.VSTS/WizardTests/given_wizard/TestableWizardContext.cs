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

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Moq;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    public abstract class TestableWizardContext : ArrangeActAssert
    {
        protected TestableWizard Wizard { get; private set; }
        protected TestableWizardStep[] Steps { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();
            UIService = CreateUIServiceMock();

            Steps = CreateSteps();

            Wizard = new TestableWizard(UIService.Object, Steps);

        }

        protected virtual Mock<IUIServiceWpf> CreateUIServiceMock()
        {
            return new Mock<IUIServiceWpf>();
        }

        protected Mock<IUIServiceWpf> UIService { get; private set; }

        protected virtual TestableWizardStep[] CreateSteps()
        {
            return new[] { new TestableWizardStep(), new TestableWizardStep(), new TestableWizardStep() };
        }
    }
}
