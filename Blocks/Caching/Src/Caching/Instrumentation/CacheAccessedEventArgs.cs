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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	/// <summary>
    /// Provides data for the <see cref="CachingInstrumentationProvider.cacheAccessed"/> event.
	/// </summary>
	public class CacheAccessedEventArgs : EventArgs
	{
		private string key;
		private bool hit;

		/// <summary>
        /// Initializes a new instance of the <see cref="CacheAccessedEventArgs"/> class.
        /// </summary>
		/// <param name="key">The key which was used to access the cache.</param>
		/// <param name="hit"><code>true</code> if an item was found matching <paramref name="key"/>, <code>false</code> otherwise.</param>
		public CacheAccessedEventArgs(string key, bool hit)
		{
			this.key = key;
			this.hit = hit;
		}

		/// <summary>
		/// Gets the key that was used accessing the cache.
		/// </summary>
		public string Key
		{
			get { return key; }
		}

		/// <summary>
        /// Gets <code>true</code> if an item was found matching <paramref name="key"/>, <code>false</code> otherwise.
		/// </summary>
		public bool Hit
		{
			get { return hit; }
		}


	}
}
