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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores configuration information about an
    /// instance of <see cref="ReturnTypeMatchingRule"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ReturnTypeMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ReturnTypeMatchingRuleDataDisplayName")]
    public partial class ReturnTypeMatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        public ReturnTypeMatchingRuleData()
        {
            Type = typeof(FakeRules.ReturnTypeMatchingRule);
        }

        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="returnTypeName">Return type to match.</param>
        public ReturnTypeMatchingRuleData(string matchingRuleName, string returnTypeName)
            : base(matchingRuleName, returnTypeName, typeof(FakeRules.ReturnTypeMatchingRule))
        {
        }

        /// <summary>
        /// The pattern to match.
        /// </summary>
        [ResourceDescription(typeof(DesignResources), "ReturnTypeMatchingRuleDataMatchDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ReturnTypeMatchingRuleDataMatchDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        public override string Match
        {
            get{ return base.Match; }
            set{ base.Match = value; }
        }
    }
}
