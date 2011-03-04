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
using System.Collections;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database
{
    /// <summary>
    /// Implementation of a BackingStore to allow CacheItems to be stored through Data Access Application Block.
    /// </summary>
    [ConfigurationElementType(typeof(DataCacheStorageData))]
    public class DataBackingStore : BaseBackingStore
    {
        readonly Data.Database database;
        readonly IStorageEncryptionProvider encryptionProvider;
        readonly string partitionName;

        /// <summary>
        /// This is public purely for unit testing purposes and should never be called by application code
        /// </summary>
        /// <param name="database">Database to use for persistence</param>
        /// <param name="databasePartitionName">Partition name in database</param>
        /// <param name="encryptionProvider">Provider used for encryption</param>
        public DataBackingStore(Data.Database database,
                                string databasePartitionName,
                                IStorageEncryptionProvider encryptionProvider)
        {
            this.database = database;
            partitionName = databasePartitionName;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Returns number of items stored in database
        /// </summary>
        public override int Count
        {
            get
            {
                DbCommand query = database.GetStoredProcCommand("GetItemCount");
                database.AddInParameter(query, "@partitionName", DbType.String, partitionName);
                return (int)database.ExecuteScalar(query);
            }
        }

        /// <summary>
        /// Adds new item to persistence store
        /// </summary>
        /// <param name="storageKey">Unique key for storage item.</param>
        /// <param name="newItem">Item to be added to cache. May not be null.</param>
        protected override void AddNewItem(int storageKey,
                                           CacheItem newItem)
        {
            string key = newItem.Key;
            byte[] valueBytes = SerializationUtility.ToBytes(newItem.Value);
            if (encryptionProvider != null)
            {
                valueBytes = encryptionProvider.Encrypt(valueBytes);
            }

            byte[] expirationBytes = SerializationUtility.ToBytes(newItem.GetExpirations());
            byte[] refreshActionBytes = SerializationUtility.ToBytes(newItem.RefreshAction);
            CacheItemPriority scavengingPriority = newItem.ScavengingPriority;
            DateTime lastAccessedTime = newItem.LastAccessedTime;

            DbCommand insertCommand = database.GetStoredProcCommand("AddItem");
            database.AddInParameter(insertCommand, "@partitionName", DbType.String, partitionName);
            database.AddInParameter(insertCommand, "@storageKey", DbType.Int32, storageKey);
            database.AddInParameter(insertCommand, "@key", DbType.String, key);
            database.AddInParameter(insertCommand, "@value", DbType.Binary, valueBytes);
            database.AddInParameter(insertCommand, "@expirations", DbType.Binary, expirationBytes);
            database.AddInParameter(insertCommand, "@refreshAction", DbType.Binary, refreshActionBytes);
            database.AddInParameter(insertCommand, "@scavengingPriority", DbType.Int32, scavengingPriority);
            database.AddInParameter(insertCommand, "@lastAccessedTime", DbType.DateTime, lastAccessedTime);

            database.ExecuteNonQuery(insertCommand);
        }

        CacheItem CreateCacheItem(DataRow dataToLoad)
        {
            string key = (string)dataToLoad["Key"];
            object value = DeserializeValue(dataToLoad);

            CacheItemPriority scavengingPriority = (CacheItemPriority)dataToLoad["ScavengingPriority"];
            object refreshAction = DeserializeObject(dataToLoad, "RefreshAction");
            object expirations = SerializationUtility.ToObject((byte[])dataToLoad["Expirations"]);
            object timestamp = (DateTime)dataToLoad["LastAccessedTime"];

            CacheItem cacheItem = new CacheItem((DateTime)timestamp, key, value, scavengingPriority, (ICacheItemRefreshAction)refreshAction, (ICacheItemExpiration[])expirations);

            return cacheItem;
        }

        object DeserializeObject(DataRow data,
                                 string columnName)
        {
            object byteArrayAsObject = data[columnName];
            if (byteArrayAsObject == DBNull.Value)
            {
                return null;
            }
            else
            {
                return SerializationUtility.ToObject((byte[])byteArrayAsObject);
            }
        }

        object DeserializeValue(DataRow dataToLoad)
        {
            object value = dataToLoad["Value"];

            if (value == DBNull.Value)
            {
                value = null;
            }
            else
            {
                byte[] valueBytes = (byte[])value;
                if (encryptionProvider != null)
                {
                    valueBytes = encryptionProvider.Decrypt(valueBytes);
                }
                value = SerializationUtility.ToObject(valueBytes);
            }
            return value;
        }

        /// <summary>
        /// Flushes all CacheItems from database. If an exception is thrown during the flush, the database is left unchanged.
        /// </summary>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        public override void Flush()
        {
            DbCommand flushCommand = database.GetStoredProcCommand("Flush");
            database.AddInParameter(flushCommand, "@partitionName", DbType.String, partitionName);
            database.ExecuteNonQuery(flushCommand);
        }

        /// <summary>
        /// Loads data from persistence store.
        /// </summary>
        /// <returns>Unfiltered hashtable of cache items loaded from persistence store.</returns>
        protected override IDictionary LoadDataFromStore()
        {
            DbCommand loadDataCommand = database.GetStoredProcCommand("LoadItems");
            database.AddInParameter(loadDataCommand, "@partitionName", DbType.String, partitionName);
            DataSet dataToLoad = database.ExecuteDataSet(loadDataCommand);

            Hashtable dataToReturn = new Hashtable();
            foreach (DataRow row in dataToLoad.Tables[0].Rows)
            {
                CacheItem cacheItem = CreateCacheItem(row);
                dataToReturn.Add(cacheItem.Key, cacheItem);
            }
            return dataToReturn;
        }

        /// <summary>
        /// Removes the item identified by the key from the database
        /// </summary>
        /// <param name="storageKey">Key of CacheItem to remove.</param>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        protected override void Remove(int storageKey)
        {
            DbCommand deleteCommand = database.GetStoredProcCommand("RemoveItem");
            database.AddInParameter(deleteCommand, "@partitionName", DbType.String, partitionName);
            database.AddInParameter(deleteCommand, "@storageKey", DbType.Int32, storageKey);

            database.ExecuteNonQuery(deleteCommand);
        }

        /// <summary>
        /// Removed existing item stored in persistence store with same key as new item
        /// </summary>
        /// <param name="storageKey">Item being removed from cache.</param>
        protected override void RemoveOldItem(int storageKey)
        {
            Remove(storageKey);
        }

        /// <summary>
        /// Updates the last accessed time for the CacheItem identified by the key
        /// </summary>
        /// <param name="storageKey">Key of item to update</param>
        /// <param name="lastAccessedTime">New timestamp for updated item</param>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        protected override void UpdateLastAccessedTime(int storageKey,
                                                       DateTime lastAccessedTime)
        {
            DbCommand updateCommand = database.GetStoredProcCommand("UpdateLastAccessedTime");
            database.AddInParameter(updateCommand, "@partitionName", DbType.String, partitionName);
            database.AddInParameter(updateCommand, "@lastAccessedTime", DbType.DateTime, lastAccessedTime);
            database.AddInParameter(updateCommand, "@storageKey", DbType.Int32, storageKey);

            database.ExecuteNonQuery(updateCommand);
        }
    }
}
