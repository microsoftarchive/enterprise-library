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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element for the <see cref="AssemblyMatchingRule"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "AssemblyMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "AssemblyMatchingRuleDataDisplayName")]
    public class AssemblyMatchingRuleData : MatchingRuleData
    {
        private const string MatchPropertyName = "match";

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMatchingRuleData"/> class with default settings.
        /// </summary>
        public AssemblyMatchingRuleData()
            : base()
        {
            base.Type = typeof(FakeRules.AssemblyMatchingRule);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMatchingRuleData"/> class with the given
        /// rule name and assembly name pattern to match.
        /// </summary>
        /// <param name="matchingRuleName">The name of rule from the configuration file.</param>
        /// <param name="assemblyName">The assembly name to match.</param>
        public AssemblyMatchingRuleData(string matchingRuleName, string assemblyName)
            : base(matchingRuleName, typeof(FakeRules.AssemblyMatchingRule))
        {
            Match = assemblyName;
        }

        /// <summary>
        /// The assembly name to match.
        /// </summary>
        /// <value>Assembly name to match.</value>
        [ConfigurationProperty(MatchPropertyName)]
        [ResourceDescription(typeof(DesignResources), "AssemblyMatchingRuleDataMatchDescription")]
        [ResourceDisplayName(typeof(DesignResources), "AssemblyMatchingRuleDataMatchDisplayName")]
        public string Match
        {
            get { return (string)base[MatchPropertyName]; }
            set { base[MatchPropertyName] = value; }
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented matching rule by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType<IMatchingRule, AssemblyMatchingRule>(registrationName, new InjectionConstructor(this.Match));
        }
    }
}
