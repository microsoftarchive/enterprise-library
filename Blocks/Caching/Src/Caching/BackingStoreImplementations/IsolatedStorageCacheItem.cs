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
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
	/// <summary>
	/// Represents a CacheItem as stored in Isolated Storage. This class is responsible for storing and
	/// restoring the item from the underlying file system store.
	/// </summary>
	public class IsolatedStorageCacheItem
	{
		private IsolatedStorageCacheItemField keyField;
		private IsolatedStorageCacheItemField valueField;
		private IsolatedStorageCacheItemField scavengingPriorityField;
		private IsolatedStorageCacheItemField refreshActionField;
		private IsolatedStorageCacheItemField expirationsField;
		private IsolatedStorageCacheItemField lastAccessedField;

		private const int MaxRetries = 3;
		private const int RetryDelayInMilliseconds = 50;

		/// <summary>
		/// Instance constructor. Ensures that the storage location in Isolated Storage is prepared
		/// for reading and writing. This class stores each individual field of the CacheItem into its own
		/// file inside the directory specified by itemDirectoryRoot.
		/// </summary>
		/// <param name="storage">Isolated Storage area to use. May not be null.</param>
		/// <param name="itemDirectoryRoot">Complete path in Isolated Storage where the cache item should be stored. May not be null.</param>
		/// <param name="encryptionProvider">Encryption provider</param>
		public IsolatedStorageCacheItem(IsolatedStorageFile storage, string itemDirectoryRoot, IStorageEncryptionProvider encryptionProvider)
		{
			int retriesLeft = MaxRetries;
			while (true)
			{
				// work around - attempt to write a file in the folder to determine whether delayed io
				// needs to be processed
				// since only a limited number of retries will be attempted, some extreme cases may 
				// still fail if file io is deferred long enough.
				// while it's still possible that the deferred IO is still pending when the item that failed
				// to be added is removed by the cleanup code, thus making the cleanup fail, 
				// the item should eventually be removed (by the original removal)
				try
				{
					storage.CreateDirectory(itemDirectoryRoot);

					// try to write a file
					// if there is a pending operation or the folder is gone, this should find the problem
					// before writing an actual field is attempted
					using (IsolatedStorageFileStream fileStream =
						new IsolatedStorageFileStream(itemDirectoryRoot + @"\sanity-check.txt", FileMode.Create, FileAccess.Write, FileShare.None, storage))
					{ }
					break;
				}
				catch (UnauthorizedAccessException)
				{
					// there are probably pending operations on the directory - retry if allowed
					if (retriesLeft-- > 0)
					{
						Thread.Sleep(RetryDelayInMilliseconds);
						continue;
					}

					throw;
				}
				catch (DirectoryNotFoundException)
				{
					// a pending deletion on the directory was processed before creating the file
					// but after attempting to create it - retry if allowed
					if (retriesLeft-- > 0)
					{
						Thread.Sleep(RetryDelayInMilliseconds);
						continue;
					}

					throw;
				}
			}

			keyField = new IsolatedStorageCacheItemField(storage, "Key", itemDirectoryRoot, encryptionProvider);
			valueField = new IsolatedStorageCacheItemField(storage, "Val", itemDirectoryRoot, encryptionProvider);
			scavengingPriorityField = new IsolatedStorageCacheItemField(storage, "ScPr", itemDirectoryRoot, encryptionProvider);
			refreshActionField = new IsolatedStorageCacheItemField(storage, "RA", itemDirectoryRoot, encryptionProvider);
			expirationsField = new IsolatedStorageCacheItemField(storage, "Exp", itemDirectoryRoot, encryptionProvider);
			lastAccessedField = new IsolatedStorageCacheItemField(storage, "LA", itemDirectoryRoot, encryptionProvider);
		}

		/// <summary>
		/// Stores specified CacheItem into IsolatedStorage at location specified in constructor
		/// </summary>
		/// <param name="itemToStore">The <see cref="CacheItem"/> to store.</param>
		public void Store(CacheItem itemToStore)
		{
			keyField.Write(itemToStore.Key, false);
			valueField.Write(itemToStore.Value, true);
			scavengingPriorityField.Write(itemToStore.ScavengingPriority, false);
			refreshActionField.Write(itemToStore.RefreshAction, false);
			expirationsField.Write(itemToStore.GetExpirations(), false);
			lastAccessedField.Write(itemToStore.LastAccessedTime, false);
		}

		/// <summary>
		/// Loads a CacheItem from IsolatedStorage from the location specified in the constructor
		/// </summary>
		/// <returns>CacheItem loaded from IsolatedStorage</returns>
		public CacheItem Load()
		{
			string key = (string)keyField.Read(false);
			object value = valueField.Read(true);

			CacheItemPriority scavengingPriority = (CacheItemPriority)scavengingPriorityField.Read(false);
			ICacheItemRefreshAction refreshAction = (ICacheItemRefreshAction)refreshActionField.Read(false);
			ICacheItemExpiration[] expirations = (ICacheItemExpiration[])expirationsField.Read(false);
			DateTime lastAccessedTime = (DateTime)lastAccessedField.Read(false);

			return new CacheItem(lastAccessedTime, key, value, scavengingPriority, refreshAction, expirations);
		}

		/// <summary>
		/// Updates the last accessed time for the CacheItem stored at this location in Isolated Storage
		/// </summary>
		/// <param name="newTimestamp">New timestamp</param>
		public void UpdateLastAccessedTime(DateTime newTimestamp)
		{
			lastAccessedField.Overwrite(newTimestamp);
		}
	}
}
