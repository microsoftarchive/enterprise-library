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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to add an encryption provider to the <see cref="IBackingStore"/> instance being configured.
    /// </summary>
    public interface IBackingStoreEncryptItemsUsing : IFluentInterface
    {
        /// <summary>
        /// Specifies the <see cref="IBackingStore"/> instance being configured should use the <see cref="IStorageEncryptionProvider"/> by the name of <paramref name="cacheStorageEncryptionProviderName"/>.
        /// </summary>
        /// <param name="cacheStorageEncryptionProviderName">The name od the <see cref="IStorageEncryptionProvider"/> that should be used.</param>
        /// <returns>A fluent interface that can be used to further configure caching settings.</returns>
        ICachingConfiguration SharedEncryptionProviderNamed(string cacheStorageEncryptionProviderName);   
    }
}
