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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using ParameterKind = Microsoft.Practices.Unity.InterceptionExtension.ParameterKind;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element storing configuration information for an instance of
    /// <see cref="ParameterTypeMatchingRule"/>.
    /// </summary>
    public class ParameterTypeMatchingRuleData : MatchingRuleData
    {
        private const string matchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        public ParameterTypeMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public ParameterTypeMatchingRuleData(string matchingRuleName)
            : base(matchingRuleName, typeof(FakeRules.ParameterTypeMatchingRule))
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="ParameterTypeMatchData"/> to match against.
        /// If any of them match, the rule matches.</param>
        public ParameterTypeMatchingRuleData(string matchingRuleName, IEnumerable<ParameterTypeMatchData> matches)
            : base(matchingRuleName, typeof(FakeRules.ParameterTypeMatchingRule))
        {
            foreach (ParameterTypeMatchData matchData in matches)
            {
                Matches.Add(matchData);
            }
        }

        /// <summary>
        /// The collection of parameter types to match against.
        /// </summary>
        /// <value>The "matches" subelement.</value>
        [ConfigurationProperty(matchesPropertyName)]
        [ConfigurationCollection(typeof(ParameterTypeMatchData))]
        public MatchDataCollection<ParameterTypeMatchData> Matches
        {
            get { return (MatchDataCollection<ParameterTypeMatchData>)base[matchesPropertyName]; }
            set { base[matchesPropertyName] = value; }
        }

        private static IEnumerable<ParameterTypeMatchingInfo> ConvertFromConfigToRuntimeInfo(
            ParameterTypeMatchingRuleData ruleData)
        {
            foreach (ParameterTypeMatchData matchData in ruleData.Matches)
            {
                yield return
                    new ParameterTypeMatchingInfo(matchData.Match, matchData.IgnoreCase, matchData.ParameterKind);
            }
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
                new TypeRegistration<IMatchingRule>(
                    () => new ParameterTypeMatchingRule(ConvertFromConfigToRuntimeInfo(this).ToArray()))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }

    /// <summary>
    /// An extended <see cref="MatchData"/> class that also includes the
    /// <see cref="ParameterKind"/> of the parameter to match.
    /// </summary>
    public class ParameterTypeMatchData : MatchData
    {
        private const string kindPropertyName = "parameterKind";

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        public ParameterTypeMatchData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        /// <param name="match">Parameter type to match. Kind is InputOrOutput.</param>
        public ParameterTypeMatchData(string match)
            : base(match)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        /// <param name="match">Parameter type to match.</param>
        /// <param name="kind"><see cref="ParameterKind"/> to match.</param>
        public ParameterTypeMatchData(string match, ParameterKind kind)
            : base(match)
        {
            ParameterKind = kind;
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        /// <param name="match">Parameter type to match.</param>
        /// <param name="kind"><see cref="ParameterKind"/> to match.</param>
        /// <param name="ignoreCase">If false, type name comparisons are case sensitive. If true, 
        /// comparisons are case insensitive.</param>
        public ParameterTypeMatchData(string match, ParameterKind kind, bool ignoreCase)
            : base(match, ignoreCase)
        {
            ParameterKind = kind;
        }

        /// <summary>
        /// What kind of parameter is this? See <see cref="ParameterKind"/> for available values.
        /// </summary>
        /// <value>The "parameterKind" config attribute.</value>
        [ConfigurationProperty(kindPropertyName, IsRequired = false, DefaultValue = ParameterKind.InputOrOutput)]
        public ParameterKind ParameterKind
        {
            get { return (ParameterKind)base[kindPropertyName]; }
            set { base[kindPropertyName] = value; }
        }
    }
}
