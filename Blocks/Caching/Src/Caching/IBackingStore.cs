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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// <P>This interface defines the contract that must be implemented by all backing stores. 
	/// Implementors of this method are responsible for interacting with their underlying
	/// persistence mechanisms to store and retrieve CacheItems. All methods below must guarantee 
	/// Weak Exception Safety. This means that operations must complete entirely, or they must completely
	/// clean up from the failure and leave the cache in a consistent state. The mandatory
	/// cleanup process will remove all traces of the item that caused the failure, causing that item
	/// to be expunged from the cache entirely.
	/// </P>
	/// </summary>
	/// <remarks>
	/// Due to the way the Caching class is implemented, implementations of this class will always be called in 
	/// a thread-safe way. There is no need to make these classes thread-safe on its own.
	/// </remarks>
	[CustomFactory(typeof(BackingStoreCustomFactory))]
	public interface IBackingStore : IDisposable
	{
		/// <summary>
		/// Number of objects stored in the backing store
		/// </summary>
		int Count { get; }

		/// <summary>
		/// <p>
		/// This method is responsible for adding a CacheItem to the BackingStore. This operation must be successful 
		/// even if an item with the same key already exists. This method must also meet the Weak Exception Safety guarantee
		/// and remove the item from the backing store if any part of the Add fails.
		/// </p> 
		/// </summary>
		/// <param name="newCacheItem">CacheItem to be added</param>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Add
		/// </p>
		/// </remarks>
		void Add(CacheItem newCacheItem);

		/// <summary>
		/// Removes an item with the given key from the backing store
		/// </summary>
		/// <param name="key">Key to remove. Must not be null.</param>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Remove
		/// </p>
		/// </remarks>
		void Remove(string key);

		/// <summary>
		/// Updates the last accessed time for a cache item.
		/// </summary>
		/// <param name="key">Key to update</param>
		/// <param name="timestamp">Time at which item updated</param>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during UpdateLastAccessedTime
		/// </p>
		/// </remarks>
		void UpdateLastAccessedTime(string key, DateTime timestamp);

		/// <summary>
		/// Flushes all CacheItems from backing store. This method must meet the Weak Exception Safety guarantee.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Flush
		/// </p>
		/// </remarks>
		void Flush();

		/// <summary>
		/// Loads all CacheItems from backing store. 
		/// </summary>
		/// <returns>Hashtable filled with all existing CacheItems.</returns>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Load
		/// </p>
		/// </remarks>
		Hashtable Load();
	}
}