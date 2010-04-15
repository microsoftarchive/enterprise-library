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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Console.Wpf.Tests.VSTS.WizardTests.given_class_with_wizard_attribute;
using System.Windows;
using Console.Wpf.Tests.VSTS.WizardTests.given_wizard_command;
using Console.Wpf.Tests.VSTS.Mocks;

[assembly: WizardCommand(typeof(MockCommandModel), WizardType = typeof(MockWizard))]

namespace Console.Wpf.Tests.VSTS.WizardTests.given_class_with_wizard_attribute
{
    [TestClass]
    public class when_instatiating_command_with_invalid_wizard_type : ContainerContext
    {
        private Mock<IResolver<WizardModel>> resolver;

        protected override void Arrange()
        {
            base.Arrange();

            resolver = new Mock<IResolver<WizardModel>>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void then_throws_an_argument_exception()
        {
            var command = new WizardCommand(
                new WizardCommandAttribute(typeof(WizardCommand)) { WizardType = typeof(string) },
                UIServiceMock.Object,
                resolver.Object
                );
        }
    }

    [TestClass]
    public class when_requesting_wizard_commands : ContainerContext
    {
        private MenuCommandService commandService;
        private IEnumerable<CommandModel> commands;

        protected override void Arrange()
        {
            base.Arrange();

            commandService = Container.Resolve<MenuCommandService>();
        }

        protected override void Act()
        {
            commands = commandService.GetCommands(CommandPlacement.WizardMenu);
        }

        [TestMethod]
        public void then_finds_wizard_command()
        {
            Assert.IsTrue(commands.OfType<MockCommandModel>().Any());
        }

        [TestMethod]
        public void then_wizard_command_can_execute_by_default()
        {
            Assert.IsTrue(commands.OfType<MockCommandModel>().All(x => x.CanExecute(null)));
        }
    }

    [TestClass]
    public class when_menu_service_encounters_bad_wizard_type : ContainerContext
    {
        [TestMethod]
        public void then_can_still_resolve_service()
        {
            var commandService = Container.Resolve<MenuCommandService>();
        }
    }

    public class MockCommandModel : WizardCommand
    {
        public MockCommandModel(WizardCommandAttribute attribute, IUIServiceWpf uiService, IResolver<WizardModel> resolver)
            : base(attribute, uiService, resolver)
        {
        }
    }
 }
