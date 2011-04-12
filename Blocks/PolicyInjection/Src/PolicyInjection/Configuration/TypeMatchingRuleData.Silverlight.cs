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

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class that stores configuration information for
    /// an instance of <see cref="TypeMatchingRule"/>.
    /// </summary>
    public partial class TypeMatchingRuleData
    {
        private NamedElementCollection<MatchData> matches = new NamedElementCollection<MatchData>();
        
        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        public TypeMatchingRuleData()
            : base()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="MatchData"/> containing
        /// types to match. If any one matches, the rule matches.</param>
        public TypeMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base(matchingRuleName)
        {
            foreach (MatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of patterns to match.
        /// </summary>
        public NamedElementCollection<MatchData> Matches
        {
            get { return this.matches; }
        }
    }
}
