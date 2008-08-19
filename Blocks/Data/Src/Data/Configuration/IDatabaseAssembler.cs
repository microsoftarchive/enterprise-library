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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
	/// <summary>
	/// Represents the process to build an instance of a concrete <see cref="Database"/> described by configuration information.
	/// </summary>
	/// <seealso cref="DatabaseCustomFactory"/>
	public interface IDatabaseAssembler
	{
		/// <summary>
		/// Builds an instance of the concrete subtype of <see cref="Database"/> the receiver knows how to build, based on 
		/// the provided connection string and any configuration information that might be contained by the 
		/// <paramref name="configurationSource"/>.
		/// </summary>
		/// <param name="name">The name for the new database instance.</param>
		/// <param name="connectionStringSettings">The connection string for the new database instance.</param>
		/// <param name="configurationSource">The source for any additional configuration information.</param>
		/// <returns>The new database instance.</returns>
		Database Assemble(string name, ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource);
	}
}
