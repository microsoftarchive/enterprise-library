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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql
{
	/// <summary>
	/// Represents the process to build an instance of <see cref="SqlDatabase"/> described by configuration information.
	/// </summary>
	public class SqlDatabaseAssembler : IDatabaseAssembler
	{
		/// <summary>
		/// Builds an instance of <see cref="SqlDatabase"/>, based on the provided connection string.
		/// </summary>
		/// <param name="name">The name for the new database instance.</param>
		/// <param name="connectionStringSettings">The connection string for the new database instance.</param>
		/// <param name="configurationSource">The source for any additional configuration information.</param>
		/// <returns>The new sql database instance.</returns>
		public Database Assemble(string name, ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource)
		{
			return new SqlDatabase(connectionStringSettings.ConnectionString);
		}
	}
}
