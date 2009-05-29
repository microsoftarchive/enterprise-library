//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Represents a factory for creating instances of a class which implements <see cref="IHashProvider"/>.
    /// </summary>
	public class HashProviderFactory : ContainerBasedInstanceFactory<IHashProvider>
    {
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="HashProviderFactory"/> class 
		/// with the default configuration source.</para>
		/// </summary>
		public HashProviderFactory()
		{
		}
 
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="HashProviderFactory"/> class 
		/// with the given configuration source.</para>
		/// </summary>
		/// <param name="configurationSource"></param>
		public HashProviderFactory(IConfigurationSource configurationSource)
			: base(configurationSource)
        {}

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="HashProviderFactory"/> class that
        /// uses the given <paramref name="container"/> to create instances.</para>
        /// </summary>
        /// <param name="container">Container to use.</param>
        public HashProviderFactory(IServiceLocator container) : base(container)
        {
        }
    }
}
