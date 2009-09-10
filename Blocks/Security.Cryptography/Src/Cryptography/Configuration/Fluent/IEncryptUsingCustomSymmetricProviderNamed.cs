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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a custom <see cref="ISymmetricCryptoProvider"/> instance.
    /// </summary>
    /// <seealso cref="CustomSymmetricCryptoProviderData"/>
    public interface IEncryptUsingCustomSymmetricProviderNamed : IConfigureCryptography, IFluentInterface
    {
        /// <summary>
        /// Specifies this custom <see cref="ISymmetricCryptoProvider"/> should be the cryptography blocks default instance.
        /// </summary>
        /// <returns>Fluent interface to further configure cryptography settings.</returns>
        /// <seealso cref="CustomSymmetricCryptoProviderData"/>
        IConfigureCryptography SetAsDefault();
    }
}
