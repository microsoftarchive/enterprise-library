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

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// The reason that the cache item was removed.
	/// </summary>
	public enum CacheItemRemovedReason
	{
		/// <summary>
		/// The item has expired.
		/// </summary>
		Expired,

		/// <summary>
		/// The item was manually removed from the cache.
		/// </summary>
        Removed,
        
		/// <summary>
		/// The item was removed by the scavenger because it had a lower priority that any other item in the cache.
		/// </summary>
        Scavenged, 
        
		/// <summary>
		/// Reserved. Do not use.
		/// </summary>
        Unknown = 9999
	}
}
