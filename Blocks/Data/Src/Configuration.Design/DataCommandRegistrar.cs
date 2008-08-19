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
	sealed class DataCommandRegistrar : CommandRegistrar
	{
		public DataCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}		
		
		public override void Register()
		{
			AddDataCommand();
			AddDefaultCommands(typeof(DatabaseSectionNode));
			
			AddProviderMappingCommand();
			AddDefaultCommands(typeof(ProviderMappingNode));
		}

		private void AddDataCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.DataUICommandText,
				Resources.DataUICommandLongText,
				new AddDatabaseSectionNodeCommand(ServiceProvider),
				typeof(DatabaseSectionNode));
			AddUICommand(item, typeof(ConfigurationApplicationNode));			
		}

		private void AddProviderMappingCommand()
		{
			AddMultipleChildNodeCommand(Resources.ProviderMappingUICommandText,
				Resources.ProviderMappingUICommandLongText,
				typeof(ProviderMappingNode),
				typeof(ProviderMappingsNode));
		}   
	}
}
