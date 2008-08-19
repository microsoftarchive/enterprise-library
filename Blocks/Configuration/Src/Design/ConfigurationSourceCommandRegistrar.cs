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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	sealed class ConfigurationSourceCommandRegistrar : CommandRegistrar
	{
		public ConfigurationSourceCommandRegistrar(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddConfigurationSourceCommand();
			AddFileConfigurationSourceCommand();
			AddSystemConfigurationSourceCommand();
			AddDefaultCommands(typeof(ConfigurationSourceSectionNode));
			AddDefaultCommands(typeof(FileConfigurationSourceElementNode));
			AddDefaultCommands(typeof(SystemConfigurationSourceElementNode));
			AddValidateCommand(typeof(ConfigurationApplicationNode));
			AddCloseConfigurationApplicationCommand();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
		private void AddCloseConfigurationApplicationCommand()
		{
			ConfigurationUICommand item = new ConfigurationUICommand(ServiceProvider, 
				Resources.CloseApplicationUICommandText,
				Resources.CloseApplicationUICommandLongText,
				CommandState.Enabled,
				new CloseConfigurationApplicationCommand(ServiceProvider, true),
				Shortcut.None, InsertionPoint.Action, null);
			AddUICommand(item, typeof(ConfigurationApplicationNode));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
		private void AddConfigurationSourceCommand()
		{
			ConfigurationUICommand item = new ConfigurationUICommand(ServiceProvider, 
				Resources.ConfigurationSourceUICommandText,
				Resources.ConfigurationSourceUICommandLongText,
				CommandState.Enabled, NodeMultiplicity.Disallow,
				new AddConfigurationSourceSectionNodeCommand(ServiceProvider),
				typeof(ConfigurationSourceSectionNode), Shortcut.None, InsertionPoint.New, null);

			AddUICommand(item, typeof(ConfigurationApplicationNode));
		}

		private void AddFileConfigurationSourceCommand()
		{
			AddMultipleChildNodeCommand(Resources.FileConfigurationSourceUICommandText,
				Resources.FileConfigurationSourceUICommandLongText, typeof(FileConfigurationSourceElementNode),
				typeof(ConfigurationSourceSectionNode));
		}

		private void AddSystemConfigurationSourceCommand()
		{
			AddMultipleChildNodeCommand(Resources.SystemConfigurationSourceUICommandText,
				Resources.SystemConfigurationSourceUICommandLongText, typeof(SystemConfigurationSourceElementNode),
				typeof(ConfigurationSourceSectionNode));
		}
	}
}
