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
    /// Provides data for the <see cref="CachingInstrumentationProvider.cacheUpdated"/> event.
	/// </summary>
	public class CacheUpdatedEventArgs : EventArgs
	{
		private long updatedEntriesCount;
		private long totalEntriesCount;

		/// <summary>
        /// Initializes a new instance of the <see cref="CacheUpdatedEventArgs"/> class.
		/// </summary>
        /// <param name="updatedEntriesCount">The number of entries updated.</param>
        /// <param name="totalEntriesCount">The total number of entries in cache.</param>
		public CacheUpdatedEventArgs(long updatedEntriesCount, long totalEntriesCount)
		{
			this.updatedEntriesCount = updatedEntriesCount;
			this.totalEntriesCount = totalEntriesCount;
		}

		/// <summary>
        /// Gets the number of entries updated.
		/// </summary>
		public long UpdatedEntriesCount
		{
			get { return updatedEntriesCount; }
		}

		/// <summary>
        /// Gets the total number of entries in cache.
		/// </summary>
		public long TotalEntriesCount
		{
			get { return totalEntriesCount; }
		}
	}
}
