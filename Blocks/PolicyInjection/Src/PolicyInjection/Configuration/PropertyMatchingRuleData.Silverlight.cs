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

using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class that stores configuration information for instances
    /// of <see cref="PropertyMatchingRule"/>.
    /// </summary>
    public partial class PropertyMatchingRuleData
    {
        private readonly MatchDataCollection<PropertyMatchData> matches = new MatchDataCollection<PropertyMatchData>();

        /// <summary>
        /// The collection of patterns to match.
        /// </summary>
        public MatchDataCollection<PropertyMatchData> Matches
        {
            get { return this.matches; }
        }
    }

    /// <summary>
    /// A derived <see cref="MatchData"/> which adds storage for which methods
    /// on the property to match.
    /// </summary>
    public partial class PropertyMatchData
    {
        private PropertyMatchingOption matchOption = PropertyMatchingOption.GetOrSet;

        /// <summary>
        /// Which methods of the property to match. Default is to match both getters and setters.
        /// </summary>
        public PropertyMatchingOption MatchOption
        {
            get { return this.matchOption; }
            set { this.matchOption = value; }
        }
    }
}
