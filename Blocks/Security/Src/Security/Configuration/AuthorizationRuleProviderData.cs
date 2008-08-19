//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for an
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.AuthorizationRuleProvider"/>.
    /// </summary>
	[Assembler(typeof(AuthorizationRuleProviderAssembler))]
	[ContainerPolicyCreator(typeof(AuthorizationRuleProviderPolicyCreator))]
	public class AuthorizationRuleProviderData : AuthorizationProviderData
    {
        private const string rulesProperty = "rules";

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthorizationRuleProviderData"/> class.
        /// </summary>
        public AuthorizationRuleProviderData()
        {
        }

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="AuthorizationRuleProviderData"/> class.
		/// </summary>
		/// <param name="name">The name of the element.</param>
		public AuthorizationRuleProviderData(string name)
			: base(name, typeof(AuthorizationRuleProvider))
		{
		}							

        /// <summary>
        /// Gets or sets the list of rules associated with
        /// the provider.
        /// </summary>
		/// <value>A collection of <see cref="AuthorizationRuleData"/>.</value>
		[ConfigurationProperty(rulesProperty, IsRequired= false)]
		public NamedElementCollection<AuthorizationRuleData> Rules
		{
			get
			{
				return (NamedElementCollection<AuthorizationRuleData>)base[rulesProperty];
			}
		}
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build an <see cref="AuthorizationRuleProvider"/> described by an <see cref="AuthorizationRuleProviderData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="AuthorizationRuleProviderData"/> type and it is used by the <see cref="AuthorizationProviderCustomFactory"/> 
    /// to build the specific <see cref="IAuthorizationProvider"/> object represented by the configuration object.
    /// </remarks>
	public class AuthorizationRuleProviderAssembler : IAssembler<IAuthorizationProvider, AuthorizationProviderData>
	{
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="AuthorizationRuleProvider"/> based on an instance of <see cref="AuthorizationRuleProviderData"/>.
        /// </summary>
        /// <seealso cref="AuthorizationProviderCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="AuthorizationRuleProviderData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="AuthorizationRuleProvider"/>.</returns>
        public IAuthorizationProvider Assemble(IBuilderContext context, AuthorizationProviderData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			AuthorizationRuleProviderData castedObjectConfiguration
				= (AuthorizationRuleProviderData)objectConfiguration;

			IDictionary<string, IAuthorizationRule> authorizationRules = new Dictionary<string, IAuthorizationRule>();
			foreach(AuthorizationRuleData ruleData in castedObjectConfiguration.Rules)
			{
				authorizationRules.Add(ruleData.Name, ruleData);
			}

			IAuthorizationProvider createdObject
				= new AuthorizationRuleProvider(authorizationRules);

			return createdObject;
		}
	}
}