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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// Configuration data defining CacheStorageData. This configuration section defines the name and type
	/// of the IBackingStore used by a CacheManager
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "CacheStorageDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CacheStorageDataDisplayName")]
    [Browsable(false)]
	public class CacheStorageData : NameTypeConfigurationElement
	{
		private const string encryptionProviderNameProperty = "encryptionProviderName";

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheStorageData"/> class.
		/// </summary>
		public CacheStorageData()
		{
		}

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheStorageData"/> class with a name and the type of <see cref="IBackingStore"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="IBackingStore"/>.</param>
        public CacheStorageData(Type type)
            : this(null, type)
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheStorageData"/> class with a name and the type of <see cref="IBackingStore"/>.
		/// </summary>
		/// <param name="name">The name of the configured <see cref="IBackingStore"/>. </param>
		/// <param name="type">The type of <see cref="IBackingStore"/>.</param>
		public CacheStorageData(string name, Type type)
			: this(name, type, string.Empty)
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerData"/> class with a name, the type of <see cref="IBackingStore"/>, and a reference to a named instance of an <see cref="IStorageEncryptionProvider"/>.
		/// </summary>
		/// <param name="name">The name of the configured <see cref="IBackingStore"/>. </param>
		/// <param name="type">The type of <see cref="IBackingStore"/>.</param>
		/// <param name="storageEncryption">The name of the referenced <see cref="IStorageEncryptionProvider"/>.</param>
		public CacheStorageData(string name, Type type, string storageEncryption)
			: base(name, type)
		{
			this.StorageEncryption = storageEncryption;
		}

		/// <summary>
		/// Gets the name of the referenced <see cref="IStorageEncryptionProvider"/>.
		/// </summary>
        [ConfigurationProperty(encryptionProviderNameProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>), typeof(StorageEncryptionProviderData))]
        [ResourceDescription(typeof(DesignResources), "CacheStorageDataStorageEncryptionDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CacheStorageDataStorageEncryptionDisplayName")]
		public virtual string StorageEncryption
		{
			get { return (string)base[encryptionProviderNameProperty]; }
			set { base[encryptionProviderNameProperty] = value; }
		}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TypeRegistration> GetRegistrations()
        {
            ConstructorInfo constructor = Type.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new NotImplementedException(Caching.Properties.Resources.ExceptionBackingStoresWithNonDefaultConstructorsShouldOverrideGetRegistrations);

            LambdaExpression newExpression =  Expression.Lambda(
                Expression.New(constructor)
                );

            yield return new TypeRegistration(newExpression, typeof(IBackingStore))
            {
                Name = this.Name
            };
        }


    }
}

