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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Represents a factory for creating instances of a class which implements <see cref="ISymmetricCryptoProvider"/>.
    /// </summary>
	public class SymmetricCryptoProviderFactory : ContainerBasedInstanceFactory<ISymmetricCryptoProvider>
    {
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="SymmetricCryptoProviderFactory"/> class 
		/// with the default configuration source.</para>
		/// </summary>
		public SymmetricCryptoProviderFactory()
			: base()
		{
		}
 
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="SymmetricCryptoProviderFactory"/> class 
		/// with the given configuration source.</para>
		/// </summary>
		/// <param name="configurationSource">Source for current configuration information.</param>
        public SymmetricCryptoProviderFactory(IConfigurationSource configurationSource)
			: base(configurationSource)
		{
		    
		}

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="SymmetricCryptoProviderFactory"/> class
        /// with the given container (which should have already been configured).</para>
        /// </summary>
        /// <param name="container">Container to resolve object from.</param>
        public SymmetricCryptoProviderFactory(IServiceLocator container)
            : base(container)
        {
        }
	}
}
