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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A data class that maps the information about
    /// a policy from the configuration source.
    /// </summary>
    public partial class PolicyData : NamedConfigurationElement
    {
        private NamedElementCollection<MatchingRuleData> matchingRules = new NamedElementCollection<MatchingRuleData>();

        /// <summary>
        /// Creates a new <see cref="PolicyData"/> with the given name.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        public PolicyData(string policyName)
        {
            Name = policyName;
        }

        /// <summary>
        /// Gets the collection of matching rules from configuration.
        /// </summary>
        /// <value>The matching rule data collection.</value>
        public NamedElementCollection<MatchingRuleData> MatchingRules
        {
            get { return this.matchingRules; }
        }

        private NamedElementCollection<CallHandlerData> handlers = new NamedElementCollection<CallHandlerData>();

        /// <summary>
        /// Get the collection of handlers from configuration.
        /// </summary>
        /// <value>The matching rule data collection.</value>
        public NamedElementCollection<CallHandlerData> Handlers
        {
            get { return this.handlers; }
        }
    }
}
