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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_toggle_section_expanded_command
{
    public abstract class given_toggle_section_expanded_command : ContainerContext
    {
        protected ToggleExpandedCommand toggleSectionExpandedCommand;
        protected SectionViewModel sectionViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            sectionViewModel = SectionViewModel.CreateSection(Container, "mocksection", new MockSectionWithUnnamedCollection());
            toggleSectionExpandedCommand = sectionViewModel.Commands.OfType<ToggleExpandedCommand>().First();
        }
    }

    [TestClass]
    public class when_executing_toggle_section_expanded_command : given_toggle_section_expanded_command
    {
        protected override void Arrange()
        {
            base.Arrange();
            base.sectionViewModel.IsExpanded = false;
        }

        protected override void Act()
        {
            base.toggleSectionExpandedCommand.Execute(null);
        }

        [TestMethod]
        public void then_section_view_model_is_expanded()
        {
            Assert.IsTrue(base.sectionViewModel.IsExpanded);
        }
    }
}
