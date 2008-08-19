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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration data defining IsolatedStorageCacheStorageData. This configuration section adds the name
    /// of the Isolated Storage area to use to store data.
    /// </summary>    
	[Assembler(typeof(IsolatedStorageBackingStoreAssembler))]
	[ContainerPolicyCreator(typeof(IsolatedStorageBackingStorePolicyCreator))]
	public class IsolatedStorageCacheStorageData : CacheStorageData
    {
		private const string partitionNameProperty = "partitionName";

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheStorageData"/> class.
        /// </summary>
        public IsolatedStorageCacheStorageData() 
        {
        }        

        /// <summary>
		/// Initialize a new instance of the <see cref="IsolatedStorageCacheStorageData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="IsolatedStorageCacheStorageData"/>.
        /// </param>
        /// <param name="storageEncryption">
        /// Storage Encryption data defined in configuration
        /// </param>
        /// <param name="partitionName">
        /// Name of the Isolated Storage area to use.
        /// </param>
        public IsolatedStorageCacheStorageData(string name, string storageEncryption, string partitionName) : base(name, typeof(IsolatedStorageBackingStore), storageEncryption)
        {
            this.PartitionName = partitionName;
        }

        /// <summary>
        /// Name of the Isolated Storage area to use.
        /// </summary>
        [ConfigurationProperty(partitionNameProperty, IsRequired= true)]
        public string PartitionName
        {
            get { return (string)base[partitionNameProperty]; }
            set { base[partitionNameProperty] = value; }
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build an <see cref="IsolatedStorageBackingStore"/> described by a <see cref="IsolatedStorageCacheStorageData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="CacheStorageData"/> type and it is used by the <see cref="BackingStoreCustomFactory"/> 
    /// to build the specific <see cref="IBackingStore"/> object represented by the configuration object.
    /// </remarks>	
    public class IsolatedStorageBackingStoreAssembler : IAssembler<IBackingStore, CacheStorageData>
	{
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="IsolatedStorageBackingStore"/> based on an instance of <see cref="IsolatedStorageCacheStorageData"/>.
        /// </summary>
        /// <seealso cref="BackingStoreCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="IsolatedStorageCacheStorageData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="IsolatedStorageBackingStore"/>.</returns>
        public IBackingStore Assemble(IBuilderContext context, CacheStorageData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			IsolatedStorageCacheStorageData castedObjectConfiguration
				= (IsolatedStorageCacheStorageData)objectConfiguration;

			IStorageEncryptionProvider encryptionProvider
				= GetStorageEncryptionProvider(context, castedObjectConfiguration.StorageEncryption, configurationSource, reflectionCache);

			IBackingStore createdObject
				= new IsolatedStorageBackingStore(
					castedObjectConfiguration.PartitionName,
					encryptionProvider);

			return createdObject;
		}

		private IStorageEncryptionProvider GetStorageEncryptionProvider(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return StorageEncryptionProviderCustomFactory.Instance.Create(context, name, configurationSource, reflectionCache);
			}

			return null;
		}
	}
}