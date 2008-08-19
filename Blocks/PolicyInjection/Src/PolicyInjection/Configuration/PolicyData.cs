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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationElement"/> that maps the information about
    /// a policy from the configuration source.
    /// </summary>
    public class PolicyData : NamedConfigurationElement
    {
        private const string HandlersPropertyName = "handlers";
        private const string MatchingRulesPropertyName = "matchingRules";

        /// <summary>
        /// Creates a new <see cref="PolicyData"/> with the given name.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        public PolicyData(string policyName)
            :base(policyName)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PolicyData"/> with no name.
        /// </summary>
        public PolicyData()
            : base()
        {
        }
        
        /// <summary>
        /// Gets or sets the collection of matching rules from configuration.
        /// </summary>
        /// <value>The matching rule data collection.</value>
        [ConfigurationProperty(MatchingRulesPropertyName)]
        public NameTypeConfigurationElementCollection<MatchingRuleData, CustomMatchingRuleData> MatchingRules
        {
            get { return (NameTypeConfigurationElementCollection<MatchingRuleData, CustomMatchingRuleData>)base[MatchingRulesPropertyName]; }
            set { base[MatchingRulesPropertyName] = value; }
        }

        /// <summary>
        /// Get or sets the collection of handlers from configuration.
        /// </summary>
        /// <value>The handler data collection.</value>
        [ConfigurationProperty(HandlersPropertyName)]
        public NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData> Handlers
        {
            get { return (NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData>)base[HandlersPropertyName]; }
            set { base[HandlersPropertyName] = value; }
        }
    }
}
