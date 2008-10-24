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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{	
	internal sealed class ManageableConfigurationSourceCommandRegistrar : CommandRegistrar
	{
		public ManageableConfigurationSourceCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{ }

		public override void Register()
		{
			AddManageableConfigurationSourceCommand();
			AddGenerateAdmTemplateCommand();
			AddDefaultCommands(typeof(ManageableConfigurationSourceElementNode));
		}

		private void AddManageableConfigurationSourceCommand()
		{
			AddMultipleChildNodeCommand(Resources.ManageableConfigurationSourceUICommandText,
				Resources.ManageableConfigurationSourceUICommandLongText, typeof(ManageableConfigurationSourceElementNode),
				typeof(ConfigurationSourceSectionNode));
		}

		private void AddGenerateAdmTemplateCommand()
		{
			ConfigurationUICommand command = new ConfigurationUICommand(
				ServiceProvider,
				Resources.GenerateAdmTemplateCommandText,
				Resources.GenerateAdmTemplateLongCommandText,
				CommandState.Enabled,
				new ExportAdmTemplateNodeCommand(ServiceProvider),
				System.Windows.Forms.Shortcut.None,
				InsertionPoint.Action,
				null);
			AddUICommand(command, typeof(ManageableConfigurationSourceElementNode));
		}
	}
}
