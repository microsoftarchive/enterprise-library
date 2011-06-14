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

using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// This interface encapsulates the logic used to determine when a
    /// scavenging is performed and which items are removed.
    /// </summary>
    /// <typeparam name="TCacheEntry"></typeparam>
    public interface IScavengingStrategy<TCacheEntry>
        where TCacheEntry : CacheEntry
    {
        /// <summary>
        /// Determines whether scavenging is needed for <paramref name="entries"/>.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns><see langword="true"/> if scavenging is needed, otherwise <see langword="false"/>.</returns>
        bool ShouldScavenge(IDictionary<string, TCacheEntry> entries);

        /// <summary>
        /// Determines whether additional scavenging is needed for <paramref name="entries"/>.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns><see langword="true"/> if additional scavenging is needed, otherwise <see langword="false"/>.</returns>
        bool ShouldScavengeMore(IDictionary<string, TCacheEntry> entries);

        /// <summary>
        /// Determines the entries that should be scavenged from <paramref name="currentEntries"/>.
        /// </summary>
        /// <param name="currentEntries">The entries to scavenge.</param>
        /// <returns>A set of the entries that should be scavenged.</returns>
        IEnumerable<TCacheEntry> EntriesToScavenge(IEnumerable<TCacheEntry> currentEntries);
    }
}
