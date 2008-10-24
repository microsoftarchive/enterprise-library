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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration
{
	/// <summary>
	/// Configuration information for DataCacheStorageData. This class represents the extra information, over and
	/// above what is defined in <see cref="CacheStorageData" />, needed to connect caching to the Data block.
	/// </summary>
	[Assembler(typeof (DataBackingStoreAssembler))]
    [ContainerPolicyCreator(typeof(DataCacheStoragePolicyCreator))]
	public class DataCacheStorageData : CacheStorageData
	{
		private const string databaseInstanceNameProperty = "databaseInstanceName";
		private const string partitionNameProperty = "partitionName";

		/// <overloads>
		/// Initializes an instance of a <see cref="DataCacheStorageData"/> class.
		/// </overloads>
		/// <summary>
		/// Initializes an instance of a <see cref="DataCacheStorageData"/> class.
		/// </summary>
		public DataCacheStorageData()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="DataCacheStorageData"/> class with a name, database instance name, and partion name.
		/// </summary>
		/// <param name="name">
		/// The name of the provider.
		/// </param>
		/// <param name="databaseInstanceName">
		/// Name of the database instance to use for storage. Instance must be defined in Data configuration.
		/// </param>
		/// <param name="partitionName">
		/// Name of the particular section inside of a database used to store this provider's data. This 
		/// field allows different applications to share the same database safely, preventing any modification of 
		/// one application's data by a provider from another application.
		/// </param>
		public DataCacheStorageData(string name, string databaseInstanceName, string partitionName)
			: base(name, typeof (DataBackingStore))
		{
			this.DatabaseInstanceName = databaseInstanceName;
			this.PartitionName = partitionName;
		}

		/// <summary>
		/// Name of the database instance to use for storage. Instance must be defined in Data configuration.
		/// </summary>
		[ConfigurationProperty(databaseInstanceNameProperty, IsRequired = true)]
		public string DatabaseInstanceName
		{
			get { return (string) base[databaseInstanceNameProperty]; }
			set { base[databaseInstanceNameProperty] = value; }
		}

		/// <summary>
		/// Name of the particular section inside of a database used to store this provider's data. This 
		/// field allows different applications to share the same database safely, preventing any modification of 
		/// one application's data by a provider from another application.
		/// </summary>
		[ConfigurationProperty(partitionNameProperty, IsRequired = true)]
		public string PartitionName
		{
			get { return (string) base[partitionNameProperty]; }
			set { base[partitionNameProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="DataBackingStore"/> described by a <see cref="DataCacheStorageData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="DataCacheStorageData"/> type and it is used by the <see cref="BackingStoreCustomFactory"/> 
	/// to build the specific <see cref="IBackingStore"/> object represented by the configuration object.
	/// </remarks>
	public class DataBackingStoreAssembler : IAssembler<IBackingStore, CacheStorageData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds an <see cref="DataBackingStore"/> based on an instance of <see cref="DataCacheStorageData"/>.
		/// </summary>
		/// <seealso cref="BackingStoreCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="DataCacheStorageData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="DataBackingStore"/>.</returns>
		public IBackingStore Assemble(IBuilderContext context,
		                              CacheStorageData objectConfiguration,
		                              IConfigurationSource configurationSource,
		                              ConfigurationReflectionCache reflectionCache)
		{
			DataCacheStorageData castedObjectConfiguration
				= (DataCacheStorageData) objectConfiguration;

			IBuilderContext databaseContext
				= context.CloneForNewBuild(
					NamedTypeBuildKey.Make<Data.Database>(castedObjectConfiguration.DatabaseInstanceName), null);

			Data.Database database
				= (Data.Database)databaseContext.Strategies.ExecuteBuildUp(databaseContext);

			IStorageEncryptionProvider encryptionProvider
				=
				GetStorageEncryptionProvider(context,
				                             castedObjectConfiguration.StorageEncryption,
				                             configurationSource,
				                             reflectionCache);

			IBackingStore createdObjet
				= new DataBackingStore(
					database,
					castedObjectConfiguration.PartitionName,
					encryptionProvider);

			return createdObjet;
		}


		private IStorageEncryptionProvider GetStorageEncryptionProvider(IBuilderContext context,
		                                                                string name,
		                                                                IConfigurationSource configurationSource,
		                                                                ConfigurationReflectionCache reflectionCache)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return StorageEncryptionProviderCustomFactory.Instance.Create(context, name, configurationSource, reflectionCache);
			}

			return null;
		}
	}
}
