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

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Properties;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design
{
    /// <summary>
    /// Represents a database cache storage provider.
    /// </summary>
    public sealed class DataCacheStorageNode : CacheStorageNode
    {        
        private ConnectionStringSettingsNode connectionStringSettingsNode;
		private EventHandler<ConfigurationNodeChangedEventArgs> onConnectionNodeRemoved;
		private EventHandler<ConfigurationNodeChangedEventArgs> onConnectionNodeRenamed;
		private string partitionName;
		internal string connectionStringName;

        /// <summary>
        /// Initialize a new instance of the <see cref="DataCacheStorageNode"/> class.
        /// </summary>
        public DataCacheStorageNode() : this(new DataCacheStorageData(Resources.DataCacheStorage, string.Empty, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="DataCacheStorageNode"/> class with a <see cref="DataCacheStorageData"/> configuration object.
		/// </summary>
		/// <param name="dataCacheStorageData">A <see cref="DataCacheStorageData"/> configuration object</param>
        public DataCacheStorageNode(DataCacheStorageData dataCacheStorageData) 
        {
			if (null == dataCacheStorageData) throw new ArgumentNullException("dataCacheStorageData");

			Rename(dataCacheStorageData.Name);
			this.onConnectionNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringSettingsNodeRemoved);
			this.onConnectionNodeRenamed = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringSettingsRenamed);
			this.partitionName = dataCacheStorageData.PartitionName;
			this.connectionStringName = dataCacheStorageData.DatabaseInstanceName;
        }

		/// <summary>
		/// <para>Releases the unmanaged resources used by the <see cref="DataCacheStorageNode "/> and optionally releases the managed resources.</para>
		/// </summary>
		/// <param name="disposing">
		/// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
		/// </param>
		protected override void Dispose(bool disposing)
		{			
			try
			{
				if (disposing)
				{
					if (connectionStringSettingsNode != null)
					{
						connectionStringSettingsNode.Removed -= onConnectionNodeRemoved;
						connectionStringSettingsNode.Renamed -= onConnectionNodeRenamed;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}			
		}

        /// <summary>
        /// Gets or sets the database to use for the cache storage.
        /// </summary>
		/// <value>
		/// The database to use for the cache storage.
		/// </value>
        [Required]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(ConnectionStringSettingsNode))]    
        [SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("DatabaseNameDescription", typeof(Resources))]
        public ConnectionStringSettingsNode DatabaseInstance
        {
            get { return connectionStringSettingsNode; }
            set
            {
				connectionStringSettingsNode = LinkNodeHelper.CreateReference<ConnectionStringSettingsNode>(connectionStringSettingsNode,
																					value,
																					onConnectionNodeRemoved,
																					onConnectionNodeRenamed);
				connectionStringName = null == connectionStringSettingsNode ? string.Empty : connectionStringSettingsNode.Name;
            }
        }

		/// <summary>
		/// Gets or sets the partition name to use for the cache storage.
		/// </summary>
		/// <value>
		/// The partition name to use for the cache storage.
		/// </value>
        [Required]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("DatabasePartitionNameDesciption", typeof(Resources))]
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

		/// <summary>
		/// Gets a <see cref="DataCacheStorageData"/> configuration object using the node data.
		/// </summary>
		/// <value>
		/// A <see cref="DataCacheStorageData"/> configuration object using the node data.
		/// </value>
		public override CacheStorageData CacheStorageData
		{
			get { return new DataCacheStorageData(Name, connectionStringName, partitionName); }
		}

        private void OnConnectionStringSettingsNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            connectionStringSettingsNode = null;
        }

		private void OnConnectionStringSettingsRenamed(object sender, ConfigurationNodeChangedEventArgs e)
		{
			connectionStringName = e.Node.Name;
		}
    }
}
