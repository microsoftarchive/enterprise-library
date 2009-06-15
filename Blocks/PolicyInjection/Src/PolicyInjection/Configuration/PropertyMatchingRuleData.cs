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
using PropertyMatchingOption = Microsoft.Practices.Unity.InterceptionExtension.PropertyMatchingOption;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element class that stores config information for instances
    /// of <see cref="PropertyMatchingRule"/>.
    /// </summary>
    public class PropertyMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        public PropertyMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public PropertyMatchingRuleData(string matchingRuleName)
            : this(matchingRuleName, new List<PropertyMatchData>())
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="PropertyMatchData"/> containing
        /// property patterns to match.</param>
        public PropertyMatchingRuleData(string matchingRuleName, IEnumerable<PropertyMatchData> matches)
            : base(matchingRuleName, typeof(FakeRules.PropertyMatchingRule))
        {
            foreach (PropertyMatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of <see cref="PropertyMatchData"/> containing property names to match.
        /// </summary>
        /// <value>The "matches" config subelement.</value>
        [ConfigurationProperty(MatchesPropertyName)]
        [ConfigurationCollection(typeof(PropertyMatchData))]
        public MatchDataCollection<PropertyMatchData> Matches
        {
            get { return (MatchDataCollection<PropertyMatchData>)base[MatchesPropertyName]; }
            set { base[MatchesPropertyName] = value; }
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
                new TypeRegistration<IMatchingRule>(() =>
                    new PropertyMatchingRule(
                        this.Matches.Select(
                            match => new PropertyMatchingInfo(match.Match, match.MatchOption, match.IgnoreCase)).ToArray()))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }

    /// <summary>
    /// A derived <see cref="MatchData"/> which adds storage for which methods
    /// on the property to match.
    /// </summary>
    public class PropertyMatchData : MatchData
    {
        private const string OptionPropertyName = "matchOption";

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        public PropertyMatchData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        /// <param name="match">Property name pattern to match. The rule will match both getter and setter methods of a property.</param>
        public PropertyMatchData(string match)
            : base(match)
        {
        }

        /// <summary>
        /// Construct a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        /// <param name="match">Property name pattern to match.</param>
        /// <param name="option">Which of the property methods to match. See <see cref="PropertyMatchingOption"/>
        /// for the valid options.</param>
        public PropertyMatchData(string match, PropertyMatchingOption option)
            : base(match)
        {
            MatchOption = option;

        }

        /// <summary>
        /// Construct a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        /// <param name="match">Property name pattern to match.</param>
        /// <param name="option">Which of the property methods to match. See <see cref="PropertyMatchingOption"/>
        /// for the valid options.</param>
        /// <param name="ignoreCase">If false, type name comparisons are case sensitive. If true, 
        /// comparisons are case insensitive.</param>
        public PropertyMatchData(string match, PropertyMatchingOption option, bool ignoreCase)
            : base(match, ignoreCase)
        {
            MatchOption = option;
        }

        /// <summary>
        /// Which methods of the property to match. Default is to match both getters and setters.
        /// </summary>
        /// <value>The "matchOption" config attribute.</value>
        [ConfigurationProperty(OptionPropertyName, DefaultValue = PropertyMatchingOption.GetOrSet, IsRequired = false)]
        public PropertyMatchingOption MatchOption
        {
            get { return (PropertyMatchingOption)base[OptionPropertyName]; }
            set { base[OptionPropertyName] = value; }
        }
    }
}
