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
        bool ShouldScavenge(IDictionary<string, TCacheEntry> entries);

        bool ShouldScavengeMore(IDictionary<string, TCacheEntry> entries);

        IEnumerable<TCacheEntry> EntriesToScavenge(IEnumerable<TCacheEntry> currentEntries);
    }
}
