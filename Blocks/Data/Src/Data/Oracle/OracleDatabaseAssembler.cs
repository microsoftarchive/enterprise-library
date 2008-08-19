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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
	internal class OracleDatabaseAssembler : IDatabaseAssembler
	{
		public Database Assemble(string name, ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource)
		{
			OracleConnectionSettings oracleConnectionSettings = OracleConnectionSettings.GetSettings(configurationSource);
			if (oracleConnectionSettings != null)
			{
				OracleConnectionData oracleConnectionData = oracleConnectionSettings.OracleConnectionsData.Get(name);
				if (oracleConnectionData != null)
				{
					IOraclePackage[] packages = new IOraclePackage[oracleConnectionData.Packages.Count];
					int i = 0;
					foreach (IOraclePackage package in oracleConnectionData.Packages)
					{
						packages[i++] = package;
					}

					return new OracleDatabase(connectionStringSettings.ConnectionString, packages);
				}
			}

			return new OracleDatabase(connectionStringSettings.ConnectionString);
		}
	}
}
