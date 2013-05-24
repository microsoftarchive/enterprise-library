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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element that stores configuration information for
    /// an instance of <see cref="TypeMatchingRule"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "TypeMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TypeMatchingRuleDataDisplayName")]
    public class TypeMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        public TypeMatchingRuleData()
        {
            Type = typeof(FakeRules.TypeMatchingRule);
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public TypeMatchingRuleData(string matchingRuleName)
            : this(matchingRuleName, new MatchData[0])
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="typeName">Type name to match.</param>
        public TypeMatchingRuleData(string matchingRuleName, string typeName)
            : this(matchingRuleName, new MatchData[] { new MatchData(typeName) })
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="MatchData"/> containing
        /// types to match. If any one matches, the rule matches.</param>
        public TypeMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base(matchingRuleName, typeof(FakeRules.TypeMatchingRule))
        {
            foreach (MatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of <see cref="MatchData"/> giving the types to match.
        /// </summary>
        /// <value>The "matches" configuration subelement.</value>
        [ConfigurationProperty(MatchesPropertyName)]
        [ConfigurationCollection(typeof(MatchData))]
        [ResourceDescription(typeof(DesignResources), "TypeMatchingRuleDataMatchesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TypeMatchingRuleDataMatchesDisplayName")]
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [EnvironmentalOverrides(false)]
        [Validation(PolicyInjectionDesignTime.Validators.MatchCollectionPopulatedValidationType)]
        public MatchDataCollection<MatchData> Matches
        {
            get { return (MatchDataCollection<MatchData>)base[MatchesPropertyName]; }
            set { base[MatchesPropertyName] = value; }
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented matching rule by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType<IMatchingRule, TypeMatchingRule>(
                registrationName, 
                new InjectionConstructor(new InjectionParameter(this.Matches.Select(match => new MatchingInfo(match.Match, match.IgnoreCase)).ToArray())));
        }
    }
}
