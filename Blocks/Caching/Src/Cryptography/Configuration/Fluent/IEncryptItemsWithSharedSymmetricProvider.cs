//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="SymmetricStorageEncryptionProvider"/> instance.
    /// </summary>
    /// <seealso cref="SymmetricStorageEncryptionProvider"/>
    /// <seealso cref="SymmetricStorageEncryptionProviderData"/>
    public interface IEncryptItemsWithSharedSymmetricProvider : ICachingConfiguration, IFluentInterface
    {
        /// <summary>
        /// Specifies which <see cref="ISymmetricCryptoProvider"/> should be used for encryption.
        /// </summary>
        /// <param name="symmetricEncryptionInstanceName">The name of the <see cref="ISymmetricCryptoProvider"/> that should be used for encryption.</param>
        /// <returns>Fluent interface to further configure caching configuration.</returns>
        /// <seealso cref="SymmetricStorageEncryptionProvider"/>
        /// <seealso cref="SymmetricStorageEncryptionProviderData"/>
        ICachingConfiguration UsingSharedSymmetricEncryptionInstanceNamed(string symmetricEncryptionInstanceName);
    }
}
