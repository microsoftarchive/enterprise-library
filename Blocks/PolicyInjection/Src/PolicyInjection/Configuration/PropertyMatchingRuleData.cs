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
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using PropertyMatchingOption = Microsoft.Practices.Unity.InterceptionExtension.PropertyMatchingOption;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element class that stores configuration information for instances
    /// of <see cref="PropertyMatchingRule"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "PropertyMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PropertyMatchingRuleDataDisplayName")]
    public class PropertyMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        public PropertyMatchingRuleData()
        {
            Type = typeof(FakeRules.PropertyMatchingRule);
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
        [ResourceDescription(typeof(DesignResources), "PropertyMatchingRuleDataMatchesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PropertyMatchingRuleDataMatchesDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [Validation(PolicyInjectionDesignTime.Validators.MatchCollectionPopulatedValidationType)]
        public MatchDataCollection<PropertyMatchData> Matches
        {
            get { return (MatchDataCollection<PropertyMatchData>)base[MatchesPropertyName]; }
            set { base[MatchesPropertyName] = value; }
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented matching rule by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType<IMatchingRule, PropertyMatchingRule>(
                registrationName,
                new InjectionConstructor(new InjectionParameter(this.Matches.Select(match => new PropertyMatchingInfo(match.Match, match.MatchOption, match.IgnoreCase)).ToArray())));
        }
    }

    /// <summary>
    /// A derived <see cref="MatchData"/> which adds storage for which methods
    /// on the property to match.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "PropertyMatchDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PropertyMatchDataDisplayName")]
    public class PropertyMatchData : MatchData
    {
        private const string OptionPropertyName = "matchOption";

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMatchData"/> class.
        /// </summary>
        public PropertyMatchData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMatchData"/> class by using the specified pattern.
        /// </summary>
        /// <param name="match">The property name pattern to match. The rule will match both getter and setter methods of a property.</param>
        public PropertyMatchData(string match)
            : base(match)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMatchData"/> class by using the specified pattern and options.
        /// </summary>
        /// <param name="match">The property name pattern to match.</param>
        /// <param name="option">Which of the property methods to match. See <see cref="PropertyMatchingOption"/>
        /// for the valid options.</param>
        public PropertyMatchData(string match, PropertyMatchingOption option)
            : base(match)
        {
            MatchOption = option;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMatchData"/> class by using the specified pattern and options.
        /// </summary>
        /// <param name="match">The property name pattern to match.</param>
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
        [ResourceDescription(typeof(DesignResources), "PropertyMatchDataMatchOptionDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PropertyMatchDataMatchOptionDisplayName")]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementProperty)]
        public PropertyMatchingOption MatchOption
        {
            get { return (PropertyMatchingOption)base[OptionPropertyName]; }
            set { base[OptionPropertyName] = value; }
        }
    }
}
