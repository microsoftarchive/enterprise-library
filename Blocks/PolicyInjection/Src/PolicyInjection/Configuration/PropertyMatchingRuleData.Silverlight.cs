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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using PropertyMatchingOption = Microsoft.Practices.Unity.InterceptionExtension.PropertyMatchingOption;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class that stores configuration information for instances
    /// of <see cref="PropertyMatchingRule"/>.
    /// </summary>
    public partial class PropertyMatchingRuleData
    {
        private NamedElementCollection<PropertyMatchData> matches = new NamedElementCollection<PropertyMatchData>();

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        public PropertyMatchingRuleData()
            : base()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="PropertyMatchData"/> containing
        /// property patterns to match.</param>
        public PropertyMatchingRuleData(string matchingRuleName, IEnumerable<PropertyMatchData> matches)
            : base(matchingRuleName)
        {
            foreach (PropertyMatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of patterns to match.
        /// </summary>
        public NamedElementCollection<PropertyMatchData> Matches
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
        /// <summary>
        /// Which methods of the property to match. Default is to match both getters and setters.
        /// </summary>
        public PropertyMatchingOption MatchOption
        {
            get; 
            set; 
        }
    }
}
