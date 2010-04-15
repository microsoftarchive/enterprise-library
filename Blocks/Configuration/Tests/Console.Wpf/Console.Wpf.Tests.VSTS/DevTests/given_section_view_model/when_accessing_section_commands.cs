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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_section_view_model
{
    [TestClass]
    public class when_accessing_section_commands : ContainerContext
    {
        SectionViewModel sectionViewModel;
        IEnumerable<CommandModel> sectionCommands;

        protected override void Arrange()
        {
            base.Arrange();

            sectionViewModel = SectionViewModel.CreateSection(Container, "mocksection", new MockSectionWithUnnamedCollection());
        }

        protected override void Act()
        {
            sectionCommands = sectionViewModel.Commands;
        }

        [TestMethod]
        public void then_section_commands_contain_toggle_expanded_command()
        {
            Assert.IsTrue(sectionCommands.OfType<ToggleExpandedCommand>().Any());
        }

        [TestMethod]
        public void then_delete_command_is_section_delete_command()
        {
            Assert.IsInstanceOfType(sectionViewModel.DeleteCommand, typeof(DefaultSectionDeleteCommandModel));
        }
    }
}
