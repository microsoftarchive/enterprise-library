//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// <para>Represents a factory for creating named instances of <see cref="Database"/> objects.</para>
    /// </summary>
	public class DatabaseProviderFactory : NameTypeFactoryBase<Database>
    {
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class 
		/// with the default configuration source.</para>
		/// </summary>
		protected DatabaseProviderFactory()
			: base()
		{
		}
 
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class 
		/// with the given configuration source.</para>
		/// </summary>
		/// <param name="configurationSource">The source for configuration information.</param>
        public DatabaseProviderFactory(IConfigurationSource configurationSource)
			: base(configurationSource)
        {}

	}
}