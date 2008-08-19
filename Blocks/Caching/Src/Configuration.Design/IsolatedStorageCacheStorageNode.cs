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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Represents a cache storage using Isolated Storage.
    /// </summary>
    public sealed class IsolatedStorageCacheStorageNode : CacheStorageNode
    {
		private string partitionName;

        /// <summary>
        /// Initialize a new instance of the <see cref="IsolatedStorageCacheStorageNode"/> class.
        /// </summary>
        public IsolatedStorageCacheStorageNode() : this(new IsolatedStorageCacheStorageData(Resources.DefaultIsolatedStorageNodeName, string.Empty, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the<see cref="IsolatedStorageCacheStorageNode"/> class with a <see cref="IsolatedStorageCacheStorageData"/> configuration object.
		/// </summary>
        /// <param name="isolatedStorageCacheStorageData">A <see cref="IsolatedStorageCacheStorageData"/> configuration object.</param>
        public IsolatedStorageCacheStorageNode(IsolatedStorageCacheStorageData isolatedStorageCacheStorageData) 
        {
			if (isolatedStorageCacheStorageData == null) throw new ArgumentNullException("isolatedStorageCacheStorageData");

			Rename(isolatedStorageCacheStorageData.Name);
			this.partitionName = isolatedStorageCacheStorageData.PartitionName;            
        }       

        /// <summary>
        /// Gets or sets the partition name for the storage.
        /// </summary>
		/// <value>
		/// The partition name for the storage.
		/// </value>
        [Required]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("IsolatedStorageAreaNameDescription", typeof(Resources))]
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

		/// <summary>
		/// Gets a <see cref="IsolatedStorageCacheStorageData"/> configuration object using the node data.
		/// </summary>
		/// <value>
		/// A <see cref="IsolatedStorageCacheStorageData"/> configuration object using the node data.
		/// </value>
		public override CacheStorageData CacheStorageData
		{
			get { return new IsolatedStorageCacheStorageData(Name, StorageEncryptionName, partitionName); }
		}
	}
}