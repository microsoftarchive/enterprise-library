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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Represents a factory for creating instances of a class which implements <see cref="ISymmetricCryptoProvider"/>.
    /// </summary>
	public class SymmetricCryptoProviderFactory : NameTypeFactoryBase<ISymmetricCryptoProvider>
    {
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="SymmetricCryptoProviderFactory"/> class 
		/// with the default configuration source.</para>
		/// </summary>
		protected SymmetricCryptoProviderFactory()
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
        {}
	}
}