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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
    /// <summary>
    /// Configuration node class to handle the ParameterTypeMatchingRule
    /// </summary>
    public class ParameterTypeMatchingRuleNode : MatchingRuleNode
    {
        private List<ParameterTypeMatch> matches;

        /// <summary>
        /// Create a new ParameterTypeMatchingRuleNode with no configured matching information.
        /// </summary>
        public ParameterTypeMatchingRuleNode() 
            : this(new ParameterTypeMatchingRuleData(Resources.ParameterTypeMatchingRuleNodeName, new ParameterTypeMatchData[0]))
        {
        }

        /// <summary>
        /// Construct a new ParameterTypeMatchingRuleNode, given data from the configuration
        /// file.
        /// </summary>
        /// <param name="ruleData">Data configuring this matching rule.</param>
        public ParameterTypeMatchingRuleNode(ParameterTypeMatchingRuleData ruleData)
            : base(ruleData)
        {
            matches = new List<ParameterTypeMatch>(ConvertRuleDataToNodeMatches(ruleData.Matches));
        }

        /// <summary>
        /// Get the list of matches for this matching rule
        /// </summary>
        [SRDescription("ParameterTypeMatchDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public List<ParameterTypeMatch> Matches
        {
            get { return matches; }
        }

        /// <summary>
        /// Get the configuration node data from the design time node.
        /// </summary>
        /// <returns>Configuration data that this node represents.</returns>
        public override MatchingRuleData GetConfigurationData()
        {
            ParameterTypeMatchingRuleData ruleData = new ParameterTypeMatchingRuleData(
                Name,
                ConvertRuleNodeToDataMatches(matches));
            return ruleData;
        }

        private IEnumerable<ParameterTypeMatch> ConvertRuleDataToNodeMatches(MatchDataCollection<ParameterTypeMatchData> matches)
        {
            foreach(ParameterTypeMatchData match in matches)
            {
                yield return new ParameterTypeMatch(match);
            }
        }

        private IEnumerable<ParameterTypeMatchData> ConvertRuleNodeToDataMatches(IEnumerable<ParameterTypeMatch> matches)
        {
            foreach(ParameterTypeMatch match in matches)
            {
                yield return match.ToMatchData();
            }
        }
    }
}
