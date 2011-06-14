//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Logging.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Logging.given_new_add_logging_command
{
    public abstract class Context : ContainerContext
    {
        protected TestAddLoggingBlockCommand AddBlockCommand;
        protected ConfigurationSection ConfigurationSection;

        protected override void Arrange()
        {
            base.Arrange();
            
            var configurationModel = Container.Resolve<ConfigurationSourceModel>();
            var attribute = new AddApplicationBlockCommandAttribute(LoggingSettings.SectionName, typeof(AppSettingsSection));
            AddBlockCommand = Container.Resolve<TestAddLoggingBlockCommand>(
                                new DependencyOverride<ConfigurationSourceModel>(configurationModel),
                                new DependencyOverride<AddApplicationBlockCommandAttribute>(attribute));
        }

        protected class TestAddLoggingBlockCommand : AddLoggingBlockCommand
        {
            public TestAddLoggingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute, IUIServiceWpf uiService)
                : base(configurationSourceModel, attribute, uiService)
            {
            }

            public new ConfigurationSection CreateConfigurationSection()
            {
                return base.CreateConfigurationSection();
            }
        }
    }
}
