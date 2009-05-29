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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// <para>Represents a factory for creating named instances of <see cref="Database"/> objects.</para>
    /// </summary>
	public class DatabaseProviderFactory : ContainerBasedInstanceFactory<Database>
    {
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class 
		/// with the default configuration source.</para>
		/// </summary>
		public DatabaseProviderFactory()
		{
		}

        /// <summary>
        /// Create an instance of <see cref="DatabaseProviderFactory"/> that resolves objects
        /// using the supplied <paramref name="container"/>.
        /// </summary>
        /// <param name="container"><see cref="IServiceLocator"/> to use to resolve objects.</param>
        public DatabaseProviderFactory(IServiceLocator container) : base(container)
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
