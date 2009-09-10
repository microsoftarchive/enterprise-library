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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureSecuritySettings"/> extensions to support configuring <see cref="AuthorizationRuleProvider"/> instances.
    /// </summary>
    /// <seealso cref="AuthorizationRuleProvider"/>
    /// <seealso cref="AuthorizationRuleProviderData"/>
    public static class AuthorizeUsingRuleProviderExtension
    {
        /// <summary>
        /// Adds a new <see cref="AuthorizationRuleProvider"/> to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="ruleBasedProviderName">The name of the <see cref="AuthorizationRuleProvider"/> instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="AuthorizationRuleProviderData"/>. </returns>
        /// <seealso cref="AuthorizationRuleProvider"/>
        /// <seealso cref="AuthorizationRuleProviderData"/>
        public static IAuthorizeUsingRuleProvider AuthorizeUsingRuleProviderNamed(this IConfigureSecuritySettings context, string ruleBasedProviderName)
        {
            if (string.IsNullOrEmpty(ruleBasedProviderName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "ruleBasedProviderName");

            return new AuthorizeRuleBasedBuilder(context, ruleBasedProviderName);
        }

        private class AuthorizeRuleBasedBuilder : ConfigureSecuritySettingsExtension, IAuthorizeUsingRuleProvider
        {
            AuthorizationRuleProviderData ruleAuthZProvider;

            public AuthorizeRuleBasedBuilder(IConfigureSecuritySettings context, string ruleBasedProviderName)
                :base(context)
            {
                ruleAuthZProvider = new AuthorizationRuleProviderData
                {
                    Name = ruleBasedProviderName
                };

                base.SecuritySettings.AuthorizationProviders.Add(ruleAuthZProvider);
            }

            IAuthorizeUsingRuleProvider IAuthorizeUsingRuleProvider.SetAsDefault()
            {
                base.SecuritySettings.DefaultAuthorizationProviderName = ruleAuthZProvider.Name;

                return this;
            }

            IAuthorizeUsingRuleProvider IAuthorizeUsingRuleProvider.SpecifyRule(string ruleName, string ruleExpression)
            {
                if (string.IsNullOrEmpty(ruleName))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "ruleName");

                ruleAuthZProvider.Rules.Add(new AuthorizationRuleData(ruleName, ruleExpression));

                return this;
            }
        }
    }    
}
