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
	sealed class OracleConnectionCommandRegistrar : CommandRegistrar
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public OracleConnectionCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}		
		
		/// <summary>
		/// 
		/// </summary>
		public override void Register()
		{
			AddOracleConnectionElementCommand();
			AddDefaultCommands(typeof(OracleConnectionElementNode));
			AddOraclePacakgeElementCommand();
			AddDefaultCommands(typeof(OraclePackageElementNode));
		}

		private void AddOracleConnectionElementCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.OracleConnectionUICommandText,
				Resources.OracleConnectionUICommandLongText,
				new AddOracleConnectionElementNodeCommand(ServiceProvider),
				typeof(OracleConnectionElementNode));
			AddUICommand(item, typeof(ConnectionStringSettingsNode));			
		}

		private void AddOraclePacakgeElementCommand()
		{
			AddMultipleChildNodeCommand(Resources.OraclePackageUICommandText,
				Resources.OraclePackageUICommandLongText,
				typeof(OraclePackageElementNode),
				typeof(OracleConnectionElementNode));
		}   
	}
}
