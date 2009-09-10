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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IBackingStoreEncryptItemsUsing"/> extension that allows an <see cref="IBackingStore"/> to be encrypted using a <see cref="SymmetricStorageEncryptionProvider"/>.
    /// </summary>
    /// <seealso cref="SymmetricStorageEncryptionProvider"/>
    /// <seealso cref="SymmetricStorageEncryptionProviderData"/>
    public static class EncryptItemsWithSharedSymmetricProviderExtension
    {
        /// <summary>
        /// Specifies the currently configured <see cref="IBackingStore"/> instance should be encrypted using a <see cref="SymmetricStorageEncryptionProvider"/>.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheStorageEncryptionProviderName">The name of the <see cref="SymmetricStorageEncryptionProvider"/> instance.</param>
        /// <returns>Fluent interface that can be used to further configure the <see cref="SymmetricStorageEncryptionProvider"/> instance.</returns>
        public static IEncryptItemsWithSharedSymmetricProvider SymmetricEncryptionProviderNamed(this IBackingStoreEncryptItemsUsing context, string cacheStorageEncryptionProviderName)
        {
            if (string.IsNullOrEmpty(cacheStorageEncryptionProviderName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "cacheStorageEncryptionProviderName");
            
            return new EncryptItemsWithSharedSymmetricProviderBuilder(context, cacheStorageEncryptionProviderName);
        }


        private class EncryptItemsWithSharedSymmetricProviderBuilder : CacheStorageExtension, IEncryptItemsWithSharedSymmetricProvider
        {

            SymmetricStorageEncryptionProviderData symmetricCacheStorageEncryption;

            public EncryptItemsWithSharedSymmetricProviderBuilder(IBackingStoreEncryptItemsUsing context, string cacheStorageEncryptionProviderName)
                :base(context)

            {
                symmetricCacheStorageEncryption = new SymmetricStorageEncryptionProviderData
                {
                    Name = cacheStorageEncryptionProviderName
                };

                base.AddEncryptionProviderToCachingConfigurationAndBackingStore(symmetricCacheStorageEncryption);
            }

            public ICachingConfiguration UsingSharedSymmetricEncryptionInstanceNamed(string symmetricEncryptionInstanceName)
            {
                if (string.IsNullOrEmpty(symmetricEncryptionInstanceName))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "symmetricEncryptionInstanceName");
            
                symmetricCacheStorageEncryption.SymmetricInstance = symmetricEncryptionInstanceName;

                return this;
            }
        }
    }
}
