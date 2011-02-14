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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Unity.given_empty_application_model
{
    [TestClass]
    public class when_accessing_add_application_block_commands : given_empty_application_model_unity
    {
        MenuCommandService commandService;
        IEnumerable<CommandModel> blocksMenu;

        protected override void Arrange()
        {
            base.Arrange();

            commandService = Container.Resolve<MenuCommandService>();
        }

        protected override void Act()
        {
            blocksMenu = commandService.GetCommands(CommandPlacement.BlocksMenu);
        }

        [TestMethod]
        public void then_add_unity_command_is_available()
        {
            Assert.IsTrue(blocksMenu.Any(x => x.Title.IndexOf("unity", StringComparison.OrdinalIgnoreCase) > -1)); 
        }
    }
}
