//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ConnectionStringsCommandRegistrar : CommandRegistrar
	{
		public ConnectionStringsCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}				

		public override void Register()
		{
			AddConnectionStringsSectionCommand();
			AddValidateCommand(typeof(ConnectionStringsSectionNode));
			AddConnectionStringSettingsCommand();
			AddDefaultCommands(typeof(ConnectionStringSettingsNode));			
		}

		private void AddConnectionStringsSectionCommand()
		{			
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.ConnectionStringsUICommandText,
				Resources.ConnectionStringsUICommandLongText,
				new AddConnectionStringsSectionNodeCommand(ServiceProvider),
				typeof(ConnectionStringSettingsNode));
			AddUICommand(item, typeof(DatabaseSectionNode));
		}

		private void AddConnectionStringSettingsCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
				Resources.ConnectionStringUICommandText,
				Resources.ConnectionStringUICommandLongText,
				new AddConnectionStringSettingsNodeCommand(ServiceProvider),
				typeof(ConnectionStringSettingsNode));
			AddUICommand(item, typeof(ConnectionStringsSectionNode));
		}        
	}
}
