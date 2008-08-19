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
    ///	Specifies the item priority levels.
    /// </summary>
    public enum CacheItemPriority
    {
        /// <summary>
        /// Should never be seen in nature.
        /// </summary>
        None = 0,
        /// <summary>
        /// Low priority for scavenging.
        /// </summary>
        Low = 1,
        /// <summary>
        /// Normal priority for scavenging.
        /// </summary>
        Normal,
        /// <summary>
        /// High priority for scavenging.
        /// </summary>
        High,
        /// <summary>
        /// Non-removable priority for scavenging.
        /// </summary>
        NotRemovable
    }
}