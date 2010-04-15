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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    public abstract class given_empty_configuration_model : ContainerContext
    {
    }

    [TestClass]
    public class when_executing_add_block_command : given_empty_configuration_model
    {
        AddApplicationBlockCommand addBlockCommand;
        ConfigurationSourceModel configurationModel;
        private CanExecuteChangedListener executeChagnedListener;

        protected override void Arrange()
        {
            base.Arrange();

            configurationModel = Container.Resolve<ConfigurationSourceModel>();
            AddApplicationBlockCommandAttribute attribute = new AddApplicationBlockCommandAttribute("appSettings", typeof(AppSettingsSection));
            addBlockCommand = Container.Resolve<AddApplicationBlockCommand>(
                                new DependencyOverride<ConfigurationSourceModel>(configurationModel),
                                new DependencyOverride<AddApplicationBlockCommandAttribute>(attribute));

            executeChagnedListener = new CanExecuteChangedListener();
            executeChagnedListener.Add(addBlockCommand);
        }

        protected override void Act()
        {
            addBlockCommand.Execute(null);
        }

        [TestMethod]
        public void then_block_is_added_to_configuration_model()
        {
            Assert.IsTrue(configurationModel.Sections.Where(x => x.ConfigurationType == typeof(AppSettingsSection)).Any());
        }

        [TestMethod]
        public void then_configuration_section_has_section()
        {
            Assert.IsTrue(configurationModel.HasSection("appSettings"));
        }

        [TestMethod]
        public void then_command_cannot_be_executed_again()
        {
            Assert.IsFalse(addBlockCommand.CanExecute(null));
        }

        [TestMethod]
        public void then_can_execute_changed_was_called()
        {
            Assert.IsTrue(executeChagnedListener.CanExecuteChangedFired(addBlockCommand));
        }

        [TestMethod]
        public void then_section_is_selected()
        {
            var addedSection = configurationModel.Sections.Where(x => x.ConfigurationType == typeof(AppSettingsSection)).Single();
            Assert.IsTrue(addedSection.IsSelected);
        }
    }
}
