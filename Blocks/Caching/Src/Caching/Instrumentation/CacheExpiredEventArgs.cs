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
    /// Provides data for the <see cref="CachingInstrumentationProvider.cacheExpired"/> event.
	/// </summary>
	public class CacheExpiredEventArgs : EventArgs
	{
		private long itemsExpired;

		/// <summary>
        /// Initializes a new instance of the <see cref="CacheExpiredEventArgs"/> class.
		/// </summary>
        /// <param name="itemsExpired">The number of items that are expired.</param>
		public CacheExpiredEventArgs(long itemsExpired)
		{
			this.itemsExpired = itemsExpired;
		}

		/// <summary>
        /// The number of items that are expired.
		/// </summary>
		public long ItemsExpired
		{
			get { return itemsExpired; }
		}
	}
}