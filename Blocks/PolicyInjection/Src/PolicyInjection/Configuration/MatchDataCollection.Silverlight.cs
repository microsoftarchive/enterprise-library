//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="MatchData"/> objects with unique matches.
    /// </summary>
    /// <typeparam name="TMatchData">The type of collection.</typeparam>
    public class MatchDataCollection<TMatchData> : ConfigurationElementCollection<TMatchData>
        where TMatchData : MatchData
    {
        /// <summary>
        /// Determines if the item can be inserted into the collection.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns><see langword="true" /> if the item has a match that is unique in the collection.</returns>
        protected override bool CanInsert(TMatchData item)
        {
            string match = item.Match;
            return !string.IsNullOrEmpty(match) && !this.Any(x => x.Match == match);
        }
    }
}
