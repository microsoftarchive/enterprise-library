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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores configuration information about an
    /// instance of <see cref="ReturnTypeMatchingRule"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ReturnTypeMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ReturnTypeMatchingRuleDataDisplayName")]
    public class ReturnTypeMatchingRuleData : StringBasedMatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        public ReturnTypeMatchingRuleData()
            : base()
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
        /// 
        /// </summary>
        [ResourceDescription(typeof(DesignResources), "ReturnTypeMatchingRuleDataMatchDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ReturnTypeMatchingRuleDataMatchDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        public override string Match
        {
            get{ return base.Match; }
            set{ base.Match = value; }
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the matching rule represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            yield return
                new TypeRegistration<IMatchingRule>(() => new ReturnTypeMatchingRule(this.Match, this.IgnoreCase))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
