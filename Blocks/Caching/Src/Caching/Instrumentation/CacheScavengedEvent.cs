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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when the cache has been scavanged.
    /// </summary>
    public class CacheScavengedEvent : CacheEvent
	{
		private long itemsScavenged;

		/// <summary>
        /// Initializes a new instance of the <see cref="CacheScavengedEvent"/> class.
        /// </summary>
        /// <param name="instanceName">Name of the <see cref="CacheManager"/> instance in which items have been scavenged.</param>
		/// <param name="itemsScavenged">The number of items scavenged.</param>
		public CacheScavengedEvent(string instanceName, long itemsScavenged)
			: base(instanceName)
		{
			this.itemsScavenged = itemsScavenged;
		}

		/// <summary>
        /// Gets the number of items scavenged.
		/// </summary>
		public long ItemsScavenged
		{
			get { return itemsScavenged; }
		}
	}
}
