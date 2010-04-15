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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard_command
{
    [TestClass]
    public class when_executing_command_with_valid_wizard : ContainerContext
    {
        private WizardCommand command;
        private Window viewCreated;
        private Mock<IResolver<WizardModel>> resolver;

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowDialog(It.IsAny<WizardView>()))
                .Callback((Window w) => { viewCreated = w; })
                .Returns(true).Verifiable("ShowDialog not invoked.");

            resolver = new Mock<IResolver<WizardModel>>();
            resolver.Setup(x => x.Resolve(It.Is<Type>(t => t == typeof (MockWizard))))
                .Returns(new MockWizard(UIServiceMock.Object))
                .Verifiable("IResolver not invoked");

            command = new WizardCommand(
                new WizardCommandAttribute(typeof(WizardCommand)) { WizardType = typeof(MockWizard) },
                UIServiceMock.Object,
                resolver.Object
                );
        }

        protected override void Act()
        {
            viewCreated = null;
            command.Execute(null);
        }

        [TestMethod]
        public void then_show_dialog_invoked()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_supplies_instance_of_wizard_to_view()
        {
            Assert.IsInstanceOfType(viewCreated.DataContext, typeof(MockWizard));
        }

        [TestMethod]
        public void then_resolver_invoked()
        {
            resolver.Verify();
        }
    }
}
