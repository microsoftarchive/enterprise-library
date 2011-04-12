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

using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class that stores configuration information about an
    /// instance of <see cref="ReturnTypeMatchingRule"/>.
    /// </summary>
    public partial class ReturnTypeMatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        public ReturnTypeMatchingRuleData()
            :base()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="returnTypeName">Return type to match.</param>
        public ReturnTypeMatchingRuleData(string matchingRuleName, string returnTypeName)
            : base(matchingRuleName, returnTypeName)
        {
        }

        /// <summary>
        /// The pattern to match.
        /// </summary>
        public override string Match
        {
            get;
            set;
        }
    }
}
