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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    class EnvironmentalOverridesCommandRegistrar : CommandRegistrar
    {
        public EnvironmentalOverridesCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        public override void Register()
        {
            ConfigurationUICommand addEnvironmentCommand = new ConfigurationUICommand(
                ServiceProvider,
                Resources.EnvironmentUICommandText,
                Resources.EnvironmentUICommandLongText,
                CommandState.Enabled, NodeMultiplicity.Allow,
                new AddEnvironmentNodeCommand(ServiceProvider),
                typeof(EnvironmentNode), Shortcut.None, InsertionPoint.New, null);

            AddUICommand(addEnvironmentCommand, typeof(EnvironmentalOverridesNode));

            ConfigurationUICommand openEnvironmentDelta = new ConfigurationUICommand(ServiceProvider,
                Resources.OpenEnvironmentDeltaCommandText,
                Resources.OpenEnvironmentDeltaCommandLongText,
                CommandState.Enabled,
                new OpenEnvironmentDeltaCommand(ServiceProvider),
                Shortcut.None, InsertionPoint.Action, null);

            AddUICommand(openEnvironmentDelta, typeof(EnvironmentalOverridesNode));

            ConfigurationUICommand saveMergedEnvironment = new ConfigurationUICommand(ServiceProvider,
                Resources.SaveMergedEnvironmentCommandText,
                Resources.SaveMergedEnvironmentCommandLongText,
                CommandState.Enabled,
                new SaveMergedEnvironmentCommand(ServiceProvider),
                Shortcut.None, InsertionPoint.Action, null);

            AddUICommand(saveMergedEnvironment, typeof(EnvironmentNode));

            AddDefaultCommands(typeof(EnvironmentNode));

        }
    }
}
