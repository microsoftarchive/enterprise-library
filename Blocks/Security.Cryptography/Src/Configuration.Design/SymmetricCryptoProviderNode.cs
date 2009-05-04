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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="SymmetricCryptoProviderData"/>.
    /// </summary>
    [Image(typeof(SymmetricCryptoProviderNode))]
    public abstract class SymmetricCryptoProviderNode : ConfigurationNode
    {

        /// <summary>
        /// Constructs a new instance of the <see cref="SymmetricCryptoProviderNode"/> class
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="symmetricCryptoProviderData">The corresponding runtime configuration data.</param>
        protected SymmetricCryptoProviderNode(SymmetricProviderData symmetricCryptoProviderData)
            : base((symmetricCryptoProviderData == null) ? string.Empty : symmetricCryptoProviderData.Name)
        {
            if (symmetricCryptoProviderData == null)
            {
                throw new ArgumentNullException("symmetricCryptoProviderData");
            }
            Rename(symmetricCryptoProviderData.Name);
        }

        /// <summary>
        /// Gets a <see cref="SymmetricProviderData"/> configuration object using the node data.
        /// </summary>
		/// <value>
        /// A <see cref="SymmetricProviderData"/> configuration object using the node data.
		/// </value>
        [Browsable(false)]
        public abstract SymmetricProviderData SymmetricCryptoProviderData
        {
            get;
        }
    }
}
