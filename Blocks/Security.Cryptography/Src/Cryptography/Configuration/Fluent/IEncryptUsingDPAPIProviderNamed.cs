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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="DpapiSymmetricCryptoProvider"/> instance.
    /// </summary>
    /// <seealso cref="DpapiSymmetricCryptoProvider"/>
    /// <seealso cref="DpapiSymmetricCryptoProviderData"/>
    public interface IEncryptUsingDPAPIProviderNamed : IConfigureCryptography, IFluentInterface
    {

        /// <summary>
        /// Specifies this <see cref="DpapiSymmetricCryptoProvider"/> should be the cryptography blocks default <see cref="ISymmetricCryptoProvider"/> instance.
        /// </summary>
        /// <returns>Fluent interface to further configure the current <see cref="DpapiSymmetricCryptoProvider"/>.</returns>
        /// <seealso cref="DpapiSymmetricCryptoProvider"/>
        /// <seealso cref="DpapiSymmetricCryptoProviderData"/>
        IEncryptUsingDPAPIProviderNamed SetAsDefault();

        /// <summary>
        /// Specifies which <see cref="DataProtectionScope"/> will be used to encrypt and decrypt information.
        /// </summary>
        /// <param name="scope">The <see cref="DataProtectionScope"/> will be used to encrypt and decrypt information.</param>
        /// <returns>Fluent interface to further configure the current <see cref="DpapiSymmetricCryptoProvider"/>.</returns>
        /// <seealso cref="DpapiSymmetricCryptoProvider"/>
        /// <seealso cref="DpapiSymmetricCryptoProviderData"/>
        IEncryptUsingDPAPIProviderNamed SetProtectionScope(DataProtectionScope scope);
    }
}
