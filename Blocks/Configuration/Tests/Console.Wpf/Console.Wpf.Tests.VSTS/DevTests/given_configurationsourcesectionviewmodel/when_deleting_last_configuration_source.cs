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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configurationsourcesectionviewmodel
{
    [TestClass]
    public class when_last_configuration_source_remaining : ContainerContext
    {
        private SectionViewModel configSourceSectionViewModel;
        private CommandModel deleteCommand;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ConfigurationSourceSection();
            section.Sources.Add(new SystemConfigurationSourceElement() { Name = "System Source" });

            var configSource = Container.Resolve<ConfigurationSourceModel>();
            configSourceSectionViewModel = configSource.AddSection(ConfigurationSourceSection.SectionName, section);
        }

        protected override void Act()
        {
            var sourceElement = configSourceSectionViewModel.DescendentConfigurationsOfType<ConfigurationSourceElement>().First();
            deleteCommand = sourceElement.DeleteCommand;
        }

        [TestMethod]
        public void then_has_custom_delete_command()
        {
            Assert.IsInstanceOfType(deleteCommand, typeof(ConfigurationSourceElementDeleteCommand));
        }

        [TestMethod]
        public void then_command_cannot_execute()
        {
            Assert.IsFalse(deleteCommand.CanExecute(null));
        }
    }

    [TestClass]
    public class when_multiple_configuration_sources_remaining : ContainerContext
    {
        private SectionViewModel configSourceSectionViewModel;
        private CommandModel deleteCommand;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ConfigurationSourceSection();
            section.Sources.Add(new SystemConfigurationSourceElement() { Name = "System Source" });
            section.Sources.Add(new SystemConfigurationSourceElement() { Name = "System Source 2" });

            var configSource = Container.Resolve<ConfigurationSourceModel>();
            configSourceSectionViewModel = configSource.AddSection(ConfigurationSourceSection.SectionName, section);
        }

        protected override void Act()
        {
            var sourceElement = configSourceSectionViewModel.DescendentConfigurationsOfType<ConfigurationSourceElement>().First();
            deleteCommand = sourceElement.DeleteCommand;
        }

        [TestMethod]
        public void then_has_custom_delete_command()
        {
            Assert.IsInstanceOfType(deleteCommand, typeof(ConfigurationSourceElementDeleteCommand));
        }

        [TestMethod]
        public void then_command_can_execute()
        {
            Assert.IsTrue(deleteCommand.CanExecute(null));
        }
    }
}
