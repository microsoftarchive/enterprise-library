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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores configuration information for
    /// an instance of <see cref="TagAttributeMatchingRule"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "TagAttributeMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TagAttributeMatchingRuleDataDisplayName")]
    public partial class TagAttributeMatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="TagAttributeMatchingRuleData"/> instance.
        /// </summary>
        public TagAttributeMatchingRuleData()
        {
            Type = typeof(FakeRules.TagAttributeMatchingRule);
        }

        /// <summary>
        /// Constructs a new <see cref="TagAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="tagToMatch">Tag string to match.</param>
        public TagAttributeMatchingRuleData(string matchingRuleName, string tagToMatch)
            : base(matchingRuleName, tagToMatch, typeof(FakeRules.TagAttributeMatchingRule))
        {
        }

        /// <summary>
        /// The pattern to match.
        /// </summary>
        [ResourceDescription(typeof(DesignResources), "TagAttributeMatchingRuleDataMatchDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TagAttributeMatchingRuleDataMatchDisplayName")]
        public override string Match
        {
            get{ return base.Match; }
            set{ base.Match =value; }
        }
    }
}
