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
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;
using Container=Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Container;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for an
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.AuthorizationRuleProvider"/>.
    /// </summary>
    [ResourceDisplayName(typeof(Resources), "AddAuthorizationRuleProviderData")]
    [ResourceDescription(typeof(Resources), "AddAuthorizationRuleProviderDataDescription")]
	public class AuthorizationRuleProviderData : AuthorizationProviderData
    {
        private const string rulesProperty = "rules";

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthorizationRuleProviderData"/> class.
        /// </summary>
        public AuthorizationRuleProviderData()
        {
            Type = typeof(AuthorizationRuleProvider);
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
        [Browsable(false)]
		public NamedElementCollection<AuthorizationRuleData> Rules
		{
			get
			{
				return (NamedElementCollection<AuthorizationRuleData>)base[rulesProperty];
			}
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            yield return GetInstrumentationProviderRegistration(configurationSource);
            
            Dictionary<string, IAuthorizationRule> authorizationRules = Rules.ToDictionary(ruleData => ruleData.Name, ruleData => (IAuthorizationRule)ruleData);

            yield return new TypeRegistration<IAuthorizationProvider>(() => new AuthorizationRuleProvider(authorizationRules, Container.Resolved<IAuthorizationProviderInstrumentationProvider>(Name)))
                {
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
