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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Represents an object that maps an instance name.
	/// </summary>
	/// <remarks>
	/// The mapping usually consists of getting the default instance name from configuration 
	/// when the requested name is <see langword="null"/>.
	/// </remarks>
	/// <seealso cref="ConfigurationNameMapperAttribute"/>
	/// <seealso cref="ConfigurationNameMappingStrategy"/>
	public interface IConfigurationNameMapper
	{
		/// <summary>
		/// Returns the mapped instance name, usually based on configuration information.
		/// </summary>
		/// <param name="name">The original instance name.</param>
		/// <param name="configSource">The configuration source to use to get configuration information.</param>
		/// <returns>The mapped instance name.</returns>
		string MapName(string name, IConfigurationSource configSource);
	}
}
