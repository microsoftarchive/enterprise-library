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
    /// Provides data for the <see cref="CachingInstrumentationProvider.cacheScavenged"/> event.
	/// </summary>
	public class CacheScavengedEventArgs : EventArgs
	{
		private long itemsScavenged;

		/// <summary>
        /// Initializes a new instance of the <see cref="CacheScavengedEventArgs"/> class.
		/// </summary>
        /// <param name="itemsScavenged">The number of items scavenged from cache.</param>
		public CacheScavengedEventArgs(long itemsScavenged)
		{
			this.itemsScavenged = itemsScavenged;
		}

		/// <summary>
        /// Gets the number of items scavenged from cache.
		/// </summary>
		public long ItemsScavenged
		{
			get { return itemsScavenged; }
		}
	}
}
