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
using ParameterKind = Microsoft.Practices.Unity.InterceptionExtension.ParameterKind;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class storing configuration information for an instance of
    /// <see cref="ParameterTypeMatchingRule"/>.
    /// </summary>
    public partial class ParameterTypeMatchingRuleData
    {
        private NamedElementCollection<ParameterTypeMatchData> matches = new NamedElementCollection<ParameterTypeMatchData>();

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        public ParameterTypeMatchingRuleData()
            :base()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public ParameterTypeMatchingRuleData(string matchingRuleName)
            : base(matchingRuleName)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="ParameterTypeMatchData"/> to match against.
        /// If any of them match, the rule matches.</param>
        public ParameterTypeMatchingRuleData(string matchingRuleName, IEnumerable<ParameterTypeMatchData> matches)
            : base(matchingRuleName)
        {
            foreach (ParameterTypeMatchData matchData in matches)
            {
                Matches.Add(matchData);
            }
        }

        /// <summary>
        /// The collection of parameter types to match against.
        /// </summary>
        public NamedElementCollection<ParameterTypeMatchData> Matches
        {
            get { return this.matches; }
        }
    }

    /// <summary>
    /// An extended <see cref="MatchData"/> class that also includes the
    /// <see cref="ParameterKind"/> of the parameter to match.
    /// </summary>
    public partial class ParameterTypeMatchData
    {
        /// <summary>
        /// What kind of parameter is this? See <see cref="ParameterKind"/> for available values.
        /// </summary>
        public ParameterKind ParameterKind
        {
            get; 
            set;
        }
    }
}
