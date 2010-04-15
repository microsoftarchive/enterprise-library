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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_command_service
{
    [TestClass]
    public class when_requesting_wizard_commands : ContainerContext
    {
        private MenuCommandService commandService;
        private IEnumerable<CommandModel> menuCommand;

        protected override void Arrange()
        {
            base.Arrange();

            commandService = Container.Resolve<MenuCommandService>();        
        }

        protected override void Act()
        {
            menuCommand = commandService.GetCommands(CommandPlacement.WizardMenu);
        }

        [TestMethod]
        public void then_contains_log_exceptions_to_database_wizard_command()
        {
            Assert.IsTrue(menuCommand.Where(c => c.GetType() == typeof(LogExceptionsToDatabaseCommand)).Any());
        }

        [TestMethod]
        public void then_wizard_type_is_correct()
        {
            Assert.IsTrue(menuCommand.OfType<LogExceptionsToDatabaseCommand>().All(x => x.WizardType == typeof(LogExceptionsToDatabase)));
        }
    }

}
