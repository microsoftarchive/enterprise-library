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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Represents the common configuration data for all symmetric crypto providers.</para>
    /// </summary>
	public class SymmetricProviderData : NameTypeConfigurationElement
    {
        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricProviderData"/> class.</para>
        /// </summary>
        public SymmetricProviderData()
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="SymmetricProviderData"/> class with a name and a <see cref="ISymmetricCryptoProvider"/>.
		/// </summary>
		/// <param name="name">The name of the configured <see cref="ISymmetricCryptoProvider"/>.</param>
		/// <param name="type">The type of <see cref="ISymmetricCryptoProvider"/>.</param>
		public SymmetricProviderData(string name, Type type)
			: base(name, type)
		{
		}
    }
}