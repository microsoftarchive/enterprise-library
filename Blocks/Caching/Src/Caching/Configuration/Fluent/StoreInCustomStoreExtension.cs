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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using System.Globalization;


namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{

    /// <summary>
    /// <see cref="ICachingConfigurationCacheManager"/> extension that allows a custom <see cref="IBackingStore"/> to be configured.
    /// </summary>
    /// <seealso cref="CustomCacheStorageData"/>
    public static class StoreInCustomStoreExtension
    {
        /// <summary>
        /// Specifies that current <see cref="CacheManager"/>'s items should be stored using a custom implementation of <see cref="IBackingStore"/>.
        /// </summary>
        /// <typeparam name="TCustomCacheStorageType">The implementation type of <see cref="IBackingStore"/> that should be used.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="backingStoreName">The name of the <see cref="IBackingStore"/> instance.</param>
        /// <returns>Fluent interface to further configure the custom <see cref="IBackingStore"/> implementation.</returns>
        /// <seealso cref="CustomCacheStorageData"/>
        public static IStoreInCustomStore StoreInCustomStore<TCustomCacheStorageType>(this ICachingConfigurationCacheManager context, string backingStoreName)
            where TCustomCacheStorageType : IBackingStore
        {
            return StoreInCustomStore(context, backingStoreName, typeof(TCustomCacheStorageType), new NameValueCollection());
        }

        /// <summary>
        /// Specifies that current <see cref="CacheManager"/>'s items should be stored using a custom implementation of <see cref="IBackingStore"/>.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="backingStoreName">The name of the <see cref="IBackingStore"/> instance.</param>
        /// <param name="customCacheStoreType">The implementation type of <see cref="IBackingStore"/> that should be used.</param>
        /// <returns>Fluent interface to further configure the custom <see cref="IBackingStore"/> implementation.</returns>
        /// <seealso cref="CustomCacheStorageData"/>
        public static IStoreInCustomStore StoreInCustomStore(this ICachingConfigurationCacheManager context, string backingStoreName, Type customCacheStoreType)
        {
            return StoreInCustomStore(context, backingStoreName, customCacheStoreType, new NameValueCollection());
        }

        /// <summary>
        /// Specifies that current <see cref="CacheManager"/>'s items should be stored using a custom implementation of <see cref="IBackingStore"/>.
        /// </summary>
        /// <typeparam name="TCustomCacheStorageType">The implementation type of <see cref="IBackingStore"/> that should be used.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="backingStoreName">The name of the <see cref="IBackingStore"/> instance.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomCacheStorageType"/> when creating an instance.</param>
        /// <returns>Fluent interface to further configure the custom <see cref="IBackingStore"/> implementation.</returns>
        /// <seealso cref="CustomCacheStorageData"/>
        public static IStoreInCustomStore StoreInCustomStore<TCustomCacheStorageType>(this ICachingConfigurationCacheManager context, string backingStoreName, NameValueCollection attributes)
            where TCustomCacheStorageType : IBackingStore
        {
            return StoreInCustomStore(context, backingStoreName, typeof(TCustomCacheStorageType), attributes);
        }

        /// <summary>
        /// Specifies that current <see cref="CacheManager"/>'s items should be stored using a custom implementation of <see cref="IBackingStore"/>.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="backingStoreName">The name of the <see cref="IBackingStore"/> instance.</param>
        /// <param name="customCacheStoreType">The implementation type of <see cref="IBackingStore"/> that should be used.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customCacheStoreType"/> when creating an instance.</param>
        /// <returns>Fluent interface to further configure the custom <see cref="IBackingStore"/> implementation.</returns>
        /// <seealso cref="CustomCacheStorageData"/>
        public static IStoreInCustomStore StoreInCustomStore(this ICachingConfigurationCacheManager context, string backingStoreName, Type customCacheStoreType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(backingStoreName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);
            if (customCacheStoreType == null) throw new ArgumentNullException("customCacheStoreType");
            if (attributes == null) throw new ArgumentNullException("attributes");
            
            if (!typeof(IBackingStore).IsAssignableFrom(customCacheStoreType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                    Resources.ExceptionTypeMustImplementInterface, typeof(IBackingStore)), "customCacheStoreType");

            return new StoreInCustomStoreBuilder(context, backingStoreName, customCacheStoreType, attributes);
        }

        private class StoreInCustomStoreBuilder : CacheManagerExtension, IStoreInCustomStore, IBackingStoreEncryptItemsUsing
        {
            CustomCacheStorageData customCacheStorageData;
            public StoreInCustomStoreBuilder(ICachingConfigurationCacheManager context, string backingStoreName, Type customCacheStoreType, NameValueCollection attributes)
                :base(context)
            {
                customCacheStorageData = new CustomCacheStorageData
                {
                    Name = backingStoreName,
                    Type = customCacheStoreType
                };

                customCacheStorageData.Attributes.Add(attributes);

                base.AddBackingStoreToCachingConfigurationAndCurrentCacheManager(customCacheStorageData);
            }

            public IBackingStoreEncryptItemsUsing EncryptUsing
            {
                get { return this; }
            }

            public ICachingConfiguration SharedEncryptionProviderNamed(string encryptionProviderName)
            {
                customCacheStorageData.StorageEncryption = encryptionProviderName;
                
                return this;
            }
        }
    }
}
